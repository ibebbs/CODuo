using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;

namespace CODuo.Home
{
    public interface IViewModel : CODuo.IViewModel, CODuo.IViewAware
    {

    }

    public class ViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly Data.IProvider _dataProvider;
        private readonly Platform.ISchedulers _schedulers;

        private readonly IObservable<Common.Period> _currentPeriod;

        private readonly MVx.Observable.Property<int> _selectedRegion;
        private readonly MVx.Observable.Property<Common.Container> _currentContainer;
        private readonly MVx.Observable.Property<IReadOnlyDictionary<int, double?>> _regionIntensity;
        private readonly MVx.Observable.Property<IReadOnlyDictionary<string, double>> _currentComposition;
        private readonly MVx.Observable.Property<Common.Region> _currentRegion;
        private readonly MVx.Observable.Property<Common.RegionGeneration> _currentRegionGeneration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Data.IProvider dataProvider, Platform.ISchedulers schedulers)
        {
            _dataProvider = dataProvider;
            _schedulers = schedulers;
            
            _currentContainer = new MVx.Observable.Property<Common.Container>(nameof(CurrentContainer), args => PropertyChanged?.Invoke(this, args));
            _selectedRegion = new MVx.Observable.Property<int>(0, nameof(SelectedRegion), args => PropertyChanged?.Invoke(this, args));
            _regionIntensity = new MVx.Observable.Property<IReadOnlyDictionary<int, double?>>(new Dictionary<int, double?>(), nameof(RegionIntensity), args => PropertyChanged?.Invoke(this, args));
            _currentComposition = new MVx.Observable.Property<IReadOnlyDictionary<string, double>>(new Dictionary<string, double>(), nameof(CurrentComposition), args => PropertyChanged?.Invoke(this, args));
            _currentRegion = new MVx.Observable.Property<Common.Region>(nameof(CurrentRegion), args => PropertyChanged?.Invoke(this, args));
            _currentRegionGeneration = new MVx.Observable.Property<Common.RegionGeneration>(nameof(CurrentRegionGeneration), args => PropertyChanged?.Invoke(this, args));

            _currentPeriod = _currentContainer
                .Where(container => !(container is null))
                .Select(container => container.Periods
                    .Where(period => period.From <= _schedulers.Default.Now && period.To >= _schedulers.Default.Now)
                    .FirstOrDefault())
                .Publish()
                .RefCount();
        }

        public IDisposable ShouldRefreshCurrentContainerWhenDataChanges()
        {
            return _dataProvider.Container
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentContainer);
        }

        private IDisposable ShouldRefreshRegionIntensityWhenDataChanges()
        {
            return _currentPeriod
                .Select(period => period.Regions.ToDictionary(region => region.RegionId, region => region.Estimated.GramsOfCO2PerkWh))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_regionIntensity);
        }

        public IDisposable ShouldRefreshCompositionWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(
                    _currentContainer, _currentPeriod, _selectedRegion, 
                    (container, period, regionId) => period.Regions
                        .Where(region => region.RegionId == regionId)
                        .Select(region => (container?.Factors, Region: region))
                        .FirstOrDefault())
                .Where(tuple => !(tuple.Region is null || tuple.Factors is null))
                .Select(tuple => tuple.Factors
                    .GroupJoin(
                        tuple.Region.Estimated.ByFuelType, 
                        factor => factor.FuelType, 
                        generation => generation.FuelType, 
                        (factor, generations) => (factor.FuelType, Percent: generations.Select(generation => generation.Percent ?? 0).FirstOrDefault()))
                    .ToDictionary(tuple => tuple.FuelType.ToString(), tuple => tuple.Percent))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentComposition);
        }

        public IDisposable ShouldRefreshRegionWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(_currentContainer, _selectedRegion, (container, regionId) => container?.Regions.Where(region => region.Id == regionId).FirstOrDefault())
                .Where(region => !(region is null))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentRegion);
        }

        private IDisposable ShouldRefreshRegionGenerationWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(_currentPeriod, _selectedRegion, (period, regionId) => period.Regions.Where(region => region.RegionId == regionId).FirstOrDefault())
                .Where(region => !(region is null))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentRegionGeneration);
        }

        public IDisposable Activate()
        {
            return new CompositeDisposable(
                ShouldRefreshCurrentContainerWhenDataChanges(),
                ShouldRefreshRegionIntensityWhenDataChanges(),
                ShouldRefreshCompositionWhenDataOrSelectedRegionChanges(),
                ShouldRefreshRegionWhenDataOrSelectedRegionChanges(),
                ShouldRefreshRegionGenerationWhenDataOrSelectedRegionChanges()
            );
        }

        public void AttachView(object view)
        {
            if (view is FrameworkElement element)
            {
                element.DataContext = this;
            }
        }

        public void DetachViews()
        {
        }

        public Common.Container CurrentContainer
        {
            get { return _currentContainer.Get(); }
        }

        public IReadOnlyDictionary<int, double?> RegionIntensity
        {
            get { return _regionIntensity.Get(); }
        }

        public IReadOnlyDictionary<string, double> CurrentComposition
        {
            get { return _currentComposition.Get(); }
        }

        public Common.Region CurrentRegion
        {
            get { return _currentRegion.Get(); }
        }

        public Common.RegionGeneration CurrentRegionGeneration
        {
            get { return _currentRegionGeneration.Get(); }
        }

        public int SelectedRegion
        {
            get { return _selectedRegion.Get(); }
            set { _selectedRegion.Set(value); }
        }
    }
}
