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

        private readonly MVx.Observable.Property<int> _selectedRegion;
        private readonly MVx.Observable.Property<IReadOnlyDictionary<int, double?>> _regionIntensity;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Data.IProvider dataProvider, Platform.ISchedulers schedulers)
        {
            _dataProvider = dataProvider;
            _schedulers = schedulers;

            _selectedRegion = new MVx.Observable.Property<int>(0, nameof(SelectedRegion), args => PropertyChanged?.Invoke(this, args));
            _regionIntensity = new MVx.Observable.Property<IReadOnlyDictionary<int, double?>>(new Dictionary<int, double?>(), nameof(RegionIntensity), args => PropertyChanged?.Invoke(this, args));
        }

        private IDisposable ShouldRefreshRegionIntensityWhenDataChanges()
        {
            return _dataProvider.Container
                .Select(container => container.Periods
                    .Where(period => period.From <= _schedulers.Default.Now && period.To >= _schedulers.Default.Now)
                    .FirstOrDefault())
                .Select(period => period.Regions.ToDictionary(region => region.RegionId, region => region.Estimated.GramsOfCO2PerkWh))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_regionIntensity);
        }

        public IDisposable Activate()
        {
            return new CompositeDisposable(
                ShouldRefreshRegionIntensityWhenDataChanges()
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

        public IReadOnlyDictionary<int, double?> RegionIntensity
        {
            get { return _regionIntensity.Get(); }
        }

        public int SelectedRegion
        {
            get { return _selectedRegion.Get(); }
            set { _selectedRegion.Set(value); }
        }
    }
}
