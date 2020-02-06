using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CODuo.Controls
{
    public sealed partial class CompositionGrid : UserControl
    {
        public static readonly DependencyProperty CoalPercentProperty = DependencyProperty.Register(nameof(CoalPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty OilPercentProperty = DependencyProperty.Register(nameof(OilPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty GasPercentProperty = DependencyProperty.Register(nameof(GasPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty ImportsPercentProperty = DependencyProperty.Register(nameof(ImportsPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty OtherPercentProperty = DependencyProperty.Register(nameof(OtherPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty BioMassPercentProperty = DependencyProperty.Register(nameof(BioMassPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty NuclearPercentProperty = DependencyProperty.Register(nameof(NuclearPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty HydroPercentProperty = DependencyProperty.Register(nameof(HydroPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty SolarPercentProperty = DependencyProperty.Register(nameof(SolarPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));
        public static readonly DependencyProperty WindPercentProperty = DependencyProperty.Register(nameof(WindPercent), typeof(double), typeof(CompositionGrid), new PropertyMetadata(0.1));

        public static readonly DependencyProperty SimpleModeProperty = DependencyProperty.Register("SimpleMode", typeof(bool), typeof(CompositionGrid), new PropertyMetadata(false));

        public CompositionGrid()
        {
            this.InitializeComponent();
        }

        public Visibility LabelVisibility
        {
            get { return SimpleMode ? Visibility.Visible : Visibility.Collapsed; }
        }

        public double CoalPercent
        {
            get { return (double)GetValue(CoalPercentProperty); }
            set { SetValue(CoalPercentProperty, value); }
        }

        public double OilPercent
        {
            get { return (double)GetValue(OilPercentProperty); }
            set { SetValue(OilPercentProperty, value); }
        }

        public double GasPercent
        {
            get { return (double)GetValue(GasPercentProperty); }
            set { SetValue(GasPercentProperty, value); }
        }

        public double ImportsPercent
        {
            get { return (double)GetValue(ImportsPercentProperty); }
            set { SetValue(ImportsPercentProperty, value); }
        }

        public double OtherPercent
        {
            get { return (double)GetValue(OtherPercentProperty); }
            set { SetValue(OtherPercentProperty, value); }
        }

        public double BioMassPercent
        {
            get { return (double)GetValue(BioMassPercentProperty); }
            set { SetValue(BioMassPercentProperty, value); }
        }

        public double NuclearPercent
        {
            get { return (double)GetValue(NuclearPercentProperty); }
            set { SetValue(NuclearPercentProperty, value); }
        }

        public double HydroPercent
        {
            get { return (double)GetValue(HydroPercentProperty); }
            set { SetValue(HydroPercentProperty, value); }
        }

        public double SolarPercent
        {
            get { return (double)GetValue(SolarPercentProperty); }
            set { SetValue(SolarPercentProperty, value); }
        }

        public double WindPercent
        {
            get { return (double)GetValue(WindPercentProperty); }
            set { SetValue(WindPercentProperty, value); }
        }

        public bool SimpleMode
        {
            get { return (bool)GetValue(SimpleModeProperty); }
            set { SetValue(SimpleModeProperty, value); }
        }
    }
}
