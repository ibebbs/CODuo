using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Home.InfoPanels
{
    public sealed partial class ForecastGeneration : UserControl
    {
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container), typeof(Common.Container), typeof(ForecastGeneration), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register(nameof(SelectedRegion), typeof(int), typeof(ForecastGeneration), new PropertyMetadata(0, DataPropertyChanged));
        public static readonly DependencyProperty CurrentPeriodProperty = DependencyProperty.Register(nameof(CurrentPeriod), typeof(Common.Period), typeof(ForecastGeneration), new PropertyMetadata(null));

        public static readonly DependencyProperty PeriodsProperty = DependencyProperty.Register(nameof(Periods), typeof(IEnumerable<Common.Period>), typeof(ForecastGeneration), new PropertyMetadata(Enumerable.Empty<Common.Period>()));

        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ForecastGeneration f)
            {
                f.DataChanged();
            }
        }

        public ForecastGeneration()
        {
            this.InitializeComponent();
        }


        private void DataChanged()
        {
            if (Container != null)
            {
                Periods = Container.Periods;
            }
        }

        public Common.Container Container
        {
            get { return (Common.Container)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public int SelectedRegion
        {
            get { return (int)GetValue(SelectedRegionProperty); }
            set { SetValue(SelectedRegionProperty, value); }
        }

        public Common.Period CurrentPeriod
        {
            get { return (Common.Period)GetValue(CurrentPeriodProperty); }
            set { SetValue(CurrentPeriodProperty, value); }
        }

        public IEnumerable<Common.Period> Periods
        {
            get { return (IEnumerable<Common.Period>)GetValue(PeriodsProperty); }
            set { SetValue(PeriodsProperty, value); }
        }
    }
}
