using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;

namespace CODuo.Home
{
    public interface IViewModel : CODuo.IViewModel, IViewAware, Navigation.State.IData
    {

    }

    public class ViewModel : IViewModel, INotifyPropertyChanged
    {
        private const double CarbonOffsetCostPerTonne = 13.15;
        private const int UkPopulation = 66435600;
        private const int UkContributingPopulation = 35321599;

        private readonly Data.IProvider _dataProvider;
        private readonly Platform.ISchedulers _schedulers;
        
        private readonly MVx.Observable.Property<int> _selectedRegion;
        private readonly MVx.Observable.Property<Common.Container> _currentContainer;
        private readonly MVx.Observable.Property<long> _sliderMinimum;
        private readonly MVx.Observable.Property<long> _sliderMaximum;
        private readonly MVx.Observable.Property<long> _sliderCurrent;
        private readonly MVx.Observable.Property<IReadOnlyDictionary<int, double?>> _regionIntensity;
        private readonly MVx.Observable.Property<IReadOnlyDictionary<string, double>> _currentComposition;
        private readonly MVx.Observable.Property<Common.Period> _currentPeriod;
        private readonly MVx.Observable.Property<Common.Region> _currentRegion;
        private readonly MVx.Observable.Property<Common.Operator> _currentOperator;
        private readonly MVx.Observable.Property<int> _currentRegionPopulation;
        private readonly MVx.Observable.Property<Common.RegionGeneration> _currentRegionGeneration;
        private readonly MVx.Observable.Property<double> _tonnesOfCO2PerHour;
        private readonly MVx.Observable.Property<double> _domesticConsumption;
        private readonly MVx.Observable.Property<double> _domesticCarbonOffsetCostPerHour;
        private readonly MVx.Observable.Property<double> _domesticCarbonOffsetCostPerPersonPerYear;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Data.IProvider dataProvider, Platform.ISchedulers schedulers)
        {
            _dataProvider = dataProvider;
            _schedulers = schedulers;
            
            _currentContainer = new MVx.Observable.Property<Common.Container>(nameof(CurrentContainer), args => PropertyChanged?.Invoke(this, args));
            _sliderMinimum = new MVx.Observable.Property<long>(nameof(SliderMinimum), args => PropertyChanged?.Invoke(this, args));
            _sliderMaximum = new MVx.Observable.Property<long>(nameof(SliderMaximum), args => PropertyChanged?.Invoke(this, args));
            _sliderCurrent = new MVx.Observable.Property<long>(nameof(SliderCurrent), args => PropertyChanged?.Invoke(this, args));
            _selectedRegion = new MVx.Observable.Property<int>(0, nameof(SelectedRegion), args => PropertyChanged?.Invoke(this, args));
            _regionIntensity = new MVx.Observable.Property<IReadOnlyDictionary<int, double?>>(Enumerable.Range(0, 15).ToDictionary(i => i, _ => default(double?)), nameof(RegionIntensity), args => PropertyChanged?.Invoke(this, args));
            _currentComposition = new MVx.Observable.Property<IReadOnlyDictionary<string, double>>(Enum.GetNames(typeof(Common.FuelType)).ToDictionary(name => name, _ => 0.0), nameof(CurrentComposition), args => PropertyChanged?.Invoke(this, args));
            _currentPeriod = new MVx.Observable.Property<Common.Period>(nameof(CurrentPeriod), args => PropertyChanged?.Invoke(this, args));
            _currentRegion = new MVx.Observable.Property<Common.Region>(nameof(CurrentRegion), args => PropertyChanged?.Invoke(this, args));
            _currentOperator = new MVx.Observable.Property<Common.Operator>(nameof(CurrentOperator), args => PropertyChanged?.Invoke(this, args));
            _currentRegionPopulation = new MVx.Observable.Property<int>(nameof(CurrentRegionPopulation), args => PropertyChanged?.Invoke(this, args));
            _currentRegionGeneration = new MVx.Observable.Property<Common.RegionGeneration>(nameof(CurrentRegionGeneration), args => PropertyChanged?.Invoke(this, args));
            _tonnesOfCO2PerHour = new MVx.Observable.Property<double>(nameof(TonnesOfCO2PerHour), args => PropertyChanged?.Invoke(this, args));
            _domesticConsumption = new MVx.Observable.Property<double>(nameof(DomesticConsumption), args => PropertyChanged?.Invoke(this, args));
            _domesticCarbonOffsetCostPerHour = new MVx.Observable.Property<double>(nameof(DomesticCarbonOffsetCostPerHour), args => PropertyChanged?.Invoke(this, args));
            _domesticCarbonOffsetCostPerPersonPerYear = new MVx.Observable.Property<double>(nameof(DomesticCarbonOffsetCostPerPersonPerYear), args => PropertyChanged?.Invoke(this, args));
        }

        public IDisposable ShouldRefreshCurrentContainerWhenDataChanges()
        {
            return _dataProvider.Container
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentContainer);
        }

        private IDisposable ShouldRefreshSliderMinimumWhenCurrentContainerChanges()
        {
            return _currentContainer
                .Where(container => !(container is null))
                .Select(container => container.Periods
                    .OrderBy(period => period.From)
                    .Select(period => period.From.Ticks)
                    .FirstOrDefault())
                .Do(ticks => System.Diagnostics.Debug.WriteLine($"From: '{ticks}'"))
                .Subscribe(_sliderMinimum);
        }

        private IDisposable ShouldRefreshSliderMaximumWhenCurrentContainerChanges()
        {
            return _currentContainer
                .Where(container => !(container is null))
                .Select(container => container.Periods
                    .OrderByDescending(period => period.From)
                    .Select(period => period.From.Ticks)
                    .FirstOrDefault())
                .Do(ticks => System.Diagnostics.Debug.WriteLine($"To: '{ticks}'"))
                .Subscribe(_sliderMaximum);
        }

        private IDisposable ShouldRefreshSliderCurrentWhenCurrentPeriodChanges()
        {
            return _currentPeriod
                .Where(period => !(period is null))
                .Select(period => period.From.Ticks)
                .Do(ticks => System.Diagnostics.Debug.WriteLine($"Current: '{ticks}'"))
                .Subscribe(_sliderCurrent);
        }

        private IDisposable ShouldRefreshRegionIntensityWhenDataChanges()
        {
            return _currentPeriod
                .Where(period => !(period is null))
                .Select(period => period.Regions.ToDictionary(region => region.RegionId, region => region.Estimated.GramsOfCO2PerkWh))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_regionIntensity);
        }

        public IDisposable ShouldRefreshCompositionWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(
                    _currentContainer, _currentPeriod, _selectedRegion, 
                    (container, period, regionId) => period?.Regions
                        .Where(region => region.RegionId == regionId)
                        .Select(region => (container?.Factors, Region: region))
                        .FirstOrDefault() ?? (null, null))
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

        public IDisposable ShouldRefreshCurrentRegionWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(_currentContainer, _selectedRegion, (container, regionId) => container?.Regions.Where(region => region.Id == regionId).FirstOrDefault())
                .Where(region => !(region is null))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentRegion);
        }

        private IDisposable ShouldRefreshCurrentOperatorWhenDataOrCurrentRegionChanges()
        {
            return Observable
                .CombineLatest(_currentContainer, _currentRegion, (container, region) => container?.Operators?.Where(op => op.Id.Equals(region?.OperatorId)).FirstOrDefault())
                .Where(op => !(op is null))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentOperator);
        }

        private IDisposable ShouldRefreshRegionPopulationWhenDataOrSelectedRegionChanges()
        {
            return _currentRegion
                .Where(region => !(region is null))
                .Select(region => Convert.ToInt32(region.PercentOfNationalPopulation * UkPopulation))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentRegionPopulation);
        }

        private IDisposable ShouldRefreshRegionGenerationWhenDataOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(_currentPeriod, _selectedRegion, (period, regionId) => period?.Regions.Where(region => region.RegionId == regionId).FirstOrDefault() ?? null)
                .Where(region => !(region is null))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_currentRegionGeneration);
        }

        private IDisposable ShouldRefreshTonnesOfCO2PerHourWhenPeriodOrSelectedRegionChanges()
        {
            return Observable
                .CombineLatest(_currentPeriod, _selectedRegion, (period, regionId) => period?.Regions
                    .Where(region => region.RegionId == regionId)
                    .Select(region => (region.Estimated.TotalMW * region.Estimated.GramsOfCO2PerkWh) / 1000.0 ?? 0.0)
                    .FirstOrDefault() ?? 0.0)
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_tonnesOfCO2PerHour);
        }

        private IDisposable ShouldRefreshDomesticConsumptionWhenCurrentPeriodOrRegionChanges()
        {
            return Observable
                .CombineLatest(_currentPeriod, _currentRegion, (period, currentRegion) => (Period: period, Region: currentRegion))
                .Where(tuple => !(tuple.Period is null || tuple.Region is null))
                .Select(tuple => tuple.Period.Regions
                    .Where(region => region.RegionId == tuple.Region.Id)
                    .Select(region => region.Estimated.TotalMW * tuple.Region.PercentOfConsumptionBeingDomestic ?? 0.0)
                    .FirstOrDefault())
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_domesticConsumption);
        }

        private IDisposable ShouldRefreshCarbonOffsetCostPerHourWhenTonnesOfCO2PerHourChanges()
        {
            return Observable
                .CombineLatest(_currentPeriod, _currentRegion, (period, currentRegion) => (Period: period, Region: currentRegion))
                .Where(tuple => !(tuple.Period is null || tuple.Region is null))
                .Select(tuple => tuple.Period.Regions
                    .Where(region => region.RegionId == tuple.Region.Id)
                    .Select(region => (region.Estimated.TotalMW * tuple.Region.PercentOfConsumptionBeingDomestic * region.Estimated.GramsOfCO2PerkWh / 1000.0 * CarbonOffsetCostPerTonne) ?? 0.0)
                    .FirstOrDefault())
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_domesticCarbonOffsetCostPerHour);
        }

        private IDisposable ShouldRefreshCarbonOffsetCostPerPersonPerYearWhenCarbonOffsetCostPerPersonPerHourChanges()
        {
            return Observable
                .CombineLatest(_domesticCarbonOffsetCostPerHour, _currentRegion, (cost, currentRegion) => (cost * 24 * 365.25) / (UkContributingPopulation * (currentRegion?.PercentOfNationalPopulation ?? 1)))
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(_domesticCarbonOffsetCostPerPersonPerYear);
        }

        public IDisposable Activate()
        {
            return new CompositeDisposable(
                ShouldRefreshCurrentContainerWhenDataChanges(),
                ShouldRefreshCurrentPeriodWhenCurrentContainerChanges(),
                ShouldRefreshSliderMinimumWhenCurrentContainerChanges(),
                ShouldRefreshSliderMaximumWhenCurrentContainerChanges(),
                ShouldRefreshSliderCurrentWhenCurrentPeriodChanges(),
                ShouldRefreshRegionIntensityWhenDataChanges(),
                ShouldRefreshCompositionWhenDataOrSelectedRegionChanges(),
                ShouldRefreshCurrentRegionWhenDataOrSelectedRegionChanges(),
                ShouldRefreshCurrentOperatorWhenDataOrCurrentRegionChanges(),
                ShouldRefreshRegionPopulationWhenDataOrSelectedRegionChanges(),
                ShouldRefreshRegionGenerationWhenDataOrSelectedRegionChanges(),
                ShouldRefreshTonnesOfCO2PerHourWhenPeriodOrSelectedRegionChanges(),
                ShouldRefreshDomesticConsumptionWhenCurrentPeriodOrRegionChanges(),
                ShouldRefreshCarbonOffsetCostPerHourWhenTonnesOfCO2PerHourChanges(),
                ShouldRefreshCarbonOffsetCostPerPersonPerYearWhenCarbonOffsetCostPerPersonPerHourChanges()
            );
        }

        private IDisposable ShouldRefreshCurrentPeriodWhenCurrentContainerChanges()
        {
            return _currentContainer
                .Where(container => !(container is null))
                .Select(container => container.Periods
                    .Where(period => period.From <= _schedulers.Default.Now && period.To >= _schedulers.Default.Now)
                    .FirstOrDefault())
                .Subscribe(_currentPeriod);
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

        public Common.Period CurrentPeriod
        {
            get { return _currentPeriod.Get(); }
        }

        public Common.Region CurrentRegion
        {
            get { return _currentRegion.Get(); }
        }

        public Common.Operator CurrentOperator
        {
            get { return _currentOperator.Get(); }
        }

        public int CurrentRegionPopulation
        {
            get { return _currentRegionPopulation.Get(); }
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

        public double TonnesOfCO2PerHour
        {
            get { return _tonnesOfCO2PerHour.Get(); }
        }

        public double DomesticConsumption
        {
            get { return _domesticConsumption.Get(); }
        }

        public double DomesticCarbonOffsetCostPerHour
        {
            get { return _domesticCarbonOffsetCostPerHour.Get(); }
        }

        public double DomesticCarbonOffsetCostPerPersonPerYear
        {
            get { return _domesticCarbonOffsetCostPerPersonPerYear.Get(); }
        }

        public long SliderMinimum
        {
            get { return _sliderMinimum.Get(); }
        }

        public long SliderMaximum
        {
            get { return _sliderMaximum.Get(); }
        }

        public long SliderCurrent
        {
            get { return _sliderCurrent.Get(); }
            set { _sliderCurrent.Set(value); }
        }
    }
}
