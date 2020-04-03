using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CODuo.Home.InfoPanels
{
    public class PeriodIntensity
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public double GramsOfCO2PerkWh { get; set; }

        public double PercentOfMaxGramsOfCO2PerkWh { get; set; }

        public double RemainingPercentOfMaxGramsOfCO2PerkWh { get; set; }
    }

    public sealed partial class Planning : UserControl
    {
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container), typeof(Common.Container), typeof(Planning), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty CurrentPeriodProperty = DependencyProperty.Register(nameof(CurrentPeriod), typeof(Common.Period), typeof(Planning), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register(nameof(SelectedRegion), typeof(int), typeof(Planning), new PropertyMetadata(0, DataPropertyChanged));

        public static readonly DependencyProperty GoodPeriodsProperty = DependencyProperty.Register(nameof(GoodPeriods), typeof(IEnumerable<PeriodIntensity>), typeof(Planning), new PropertyMetadata(Enumerable.Empty<PeriodIntensity>()));
        public static readonly DependencyProperty BadPeriodsProperty = DependencyProperty.Register(nameof(BadPeriods), typeof(IEnumerable<PeriodIntensity>), typeof(Planning), new PropertyMetadata(Enumerable.Empty<PeriodIntensity>()));

        public static readonly DependencyProperty BestPeriodProperty = DependencyProperty.Register("BestPeriod", typeof(PeriodIntensity), typeof(Planning), new PropertyMetadata(null));
        public static readonly DependencyProperty WorstPeriodProperty = DependencyProperty.Register("WorstPeriod", typeof(PeriodIntensity), typeof(Planning), new PropertyMetadata(null));
        
        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Planning p)
            {
                p.DataChanged();
            }
        }

        public Planning()
        {
            this.InitializeComponent();
        }

        private void DataChanged()
        {
            if (Container != null && CurrentPeriod != null)
            {
                var periods = Container.Periods
                    .Where(period => period.From >= CurrentPeriod.From)
                    .SelectMany(period => period.Regions
                        .Where(region => region.RegionId == SelectedRegion)
                        .Where(region => region.Estimated?.GramsOfCO2PerkWh.HasValue ?? false)
                        .Select(region => (Period: period, GramsOfCO2PerkWh: region.Estimated.GramsOfCO2PerkWh.Value)))
                    .ToArray();

                var maxGramsOfCO2PerkWh = periods.Max(tuple => tuple.GramsOfCO2PerkWh);

                var periodIntensities = periods
                    .Select(
                        tuple => new PeriodIntensity
                        {
                            From = tuple.Period.From.ToLocalTime(),
                            To = tuple.Period.To.ToLocalTime(),
                            GramsOfCO2PerkWh = tuple.GramsOfCO2PerkWh,
                            PercentOfMaxGramsOfCO2PerkWh = tuple.GramsOfCO2PerkWh / maxGramsOfCO2PerkWh,
                            RemainingPercentOfMaxGramsOfCO2PerkWh = 1 - (tuple.GramsOfCO2PerkWh / maxGramsOfCO2PerkWh)
                        })
                    .OrderBy(periodIntensity => periodIntensity.GramsOfCO2PerkWh)
                    .ToArray();

                GoodPeriods = periodIntensities.Take(3).ToArray();
                BadPeriods = periodIntensities.Reverse().Take(3).Reverse().ToArray();

                BestPeriod = GoodPeriods.First();
                WorstPeriod = BadPeriods.Last();
            }
        }

        public Common.Container Container
        {
            get { return (Common.Container)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public Common.Period CurrentPeriod
        {
            get { return (Common.Period)GetValue(CurrentPeriodProperty); }
            set { SetValue(CurrentPeriodProperty, value); }
        }

        public int SelectedRegion
        {
            get { return (int)GetValue(SelectedRegionProperty); }
            set { SetValue(SelectedRegionProperty, value); }
        }

        public IEnumerable<PeriodIntensity> GoodPeriods
        {
            get { return (IEnumerable<PeriodIntensity>)GetValue(GoodPeriodsProperty); }
            set { SetValue(GoodPeriodsProperty, value); }
        }

        public IEnumerable<PeriodIntensity> BadPeriods
        {
            get { return (IEnumerable<PeriodIntensity>)GetValue(BadPeriodsProperty); }
            set { SetValue(BadPeriodsProperty, value); }
        }

        public PeriodIntensity BestPeriod
        {
            get { return (PeriodIntensity)GetValue(BestPeriodProperty); }
            set { SetValue(BestPeriodProperty, value); }
        }

        public PeriodIntensity WorstPeriod
        {
            get { return (PeriodIntensity)GetValue(WorstPeriodProperty); }
            set { SetValue(WorstPeriodProperty, value); }
        }
    }
}
