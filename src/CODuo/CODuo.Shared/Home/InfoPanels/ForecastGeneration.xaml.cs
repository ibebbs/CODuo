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
    public sealed partial class ForecastGeneration : UserControl
    {
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register("Container", typeof(Common.Container), typeof(ForecastGeneration), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register("SelectedRegion", typeof(int), typeof(ForecastGeneration), new PropertyMetadata(0, DataPropertyChanged));

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

        public IEnumerable<Common.Period> Periods
        {
            get { return (IEnumerable<Common.Period>)GetValue(PeriodsProperty); }
            set { SetValue(PeriodsProperty, value); }
        }
    }
}
