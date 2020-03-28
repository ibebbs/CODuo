using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Home.InfoPanels
{
    public class PeriodIntensity
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public double GramsOfCO2PerkWh { get; set; }
    }

    public sealed partial class Planning : UserControl
    {
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container), typeof(Common.Container), typeof(Planning), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty CurrentPeriodProperty = DependencyProperty.Register(nameof(CurrentPeriod), typeof(Common.Period), typeof(Planning), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register(nameof(SelectedRegion), typeof(int), typeof(Planning), new PropertyMetadata(0, DataPropertyChanged));

        public static readonly DependencyProperty GoodPeriodsProperty = DependencyProperty.Register(nameof(GoodPeriods), typeof(IEnumerable<PeriodIntensity>), typeof(Planning), new PropertyMetadata(Enumerable.Empty<PeriodIntensity>()));
        public static readonly DependencyProperty BadPeriodsProperty = DependencyProperty.Register(nameof(BadPeriods), typeof(IEnumerable<PeriodIntensity>), typeof(Planning), new PropertyMetadata(Enumerable.Empty<PeriodIntensity>()));

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
                var periodIntensity = Container.Periods
                    .Where(period => period.From >= CurrentPeriod.From)
                    .SelectMany(period => period.Regions
                        .Where(region => region.RegionId == SelectedRegion)
                        .Where(region => region.Estimated?.GramsOfCO2PerkWh.HasValue ?? false)
                        .Select(region => new PeriodIntensity { From = period.From, To = period.To, GramsOfCO2PerkWh = region.Estimated.GramsOfCO2PerkWh.Value }))
                    .OrderByDescending(periodIntensity => periodIntensity.GramsOfCO2PerkWh)
                    .ToArray();

                GoodPeriods = periodIntensity.Take(3).ToArray();
                BadPeriods = periodIntensity.Reverse().Take(3).ToArray();
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
    }
}
