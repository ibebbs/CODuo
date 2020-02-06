using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Controls
{
    public sealed partial class CountryByRegion : ContentControl
    {
        private static readonly Brush DefaultBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xAA, 0xAA, 0xAA));

        public static readonly DependencyProperty Region1BrushProperty  = DependencyProperty.Register(nameof(Region1Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region2BrushProperty  = DependencyProperty.Register(nameof(Region2Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region3BrushProperty  = DependencyProperty.Register(nameof(Region3Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region4BrushProperty  = DependencyProperty.Register(nameof(Region4Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region5BrushProperty  = DependencyProperty.Register(nameof(Region5Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region6BrushProperty  = DependencyProperty.Register(nameof(Region6Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region7BrushProperty  = DependencyProperty.Register(nameof(Region7Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region8BrushProperty  = DependencyProperty.Register(nameof(Region8Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region9BrushProperty  = DependencyProperty.Register(nameof(Region9Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region10BrushProperty = DependencyProperty.Register(nameof(Region10Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region11BrushProperty = DependencyProperty.Register(nameof(Region11Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region12BrushProperty = DependencyProperty.Register(nameof(Region12Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region13BrushProperty = DependencyProperty.Register(nameof(Region13Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region14BrushProperty = DependencyProperty.Register(nameof(Region14Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));
        public static readonly DependencyProperty Region15BrushProperty = DependencyProperty.Register(nameof(Region15Brush), typeof(Brush), typeof(CountryByRegion), new PropertyMetadata(DefaultBrush));

        public static readonly DependencyProperty Region1DescriptionProperty = DependencyProperty.Register(nameof(Region1Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region2DescriptionProperty = DependencyProperty.Register(nameof(Region2Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region3DescriptionProperty = DependencyProperty.Register(nameof(Region3Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region4DescriptionProperty = DependencyProperty.Register(nameof(Region4Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region5DescriptionProperty = DependencyProperty.Register(nameof(Region5Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region6DescriptionProperty = DependencyProperty.Register(nameof(Region6Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region7DescriptionProperty = DependencyProperty.Register(nameof(Region7Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region8DescriptionProperty = DependencyProperty.Register(nameof(Region8Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region9DescriptionProperty = DependencyProperty.Register(nameof(Region9Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region10DescriptionProperty = DependencyProperty.Register(nameof(Region10Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region11DescriptionProperty = DependencyProperty.Register(nameof(Region11Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region12DescriptionProperty = DependencyProperty.Register(nameof(Region12Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region13DescriptionProperty = DependencyProperty.Register(nameof(Region13Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region14DescriptionProperty = DependencyProperty.Register(nameof(Region14Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Region15DescriptionProperty = DependencyProperty.Register(nameof(Region15Description), typeof(string), typeof(CountryByRegion), new PropertyMetadata(string.Empty));


        public CountryByRegion()
        {
            this.InitializeComponent();
        }

        private void Region_Tapped(object sender, TappedRoutedEventArgs e)
        {
            System.Diagnostics.Debugger.Break();
        }

        public Brush Region1Brush
        {
            get { return (Brush)GetValue(Region1BrushProperty); }
            set { SetValue(Region1BrushProperty, value); }
        }

        public Brush Region2Brush
        {
            get { return (Brush)GetValue(Region2BrushProperty); }
            set { SetValue(Region2BrushProperty, value); }
        }

        public Brush Region3Brush
        {
            get { return (Brush)GetValue(Region3BrushProperty); }
            set { SetValue(Region3BrushProperty, value); }
        }

        public Brush Region4Brush
        {
            get { return (Brush)GetValue(Region4BrushProperty); }
            set { SetValue(Region4BrushProperty, value); }
        }

        public Brush Region5Brush
        {
            get { return (Brush)GetValue(Region5BrushProperty); }
            set { SetValue(Region5BrushProperty, value); }
        }

        public Brush Region6Brush
        {
            get { return (Brush)GetValue(Region6BrushProperty); }
            set { SetValue(Region6BrushProperty, value); }
        }

        public Brush Region7Brush
        {
            get { return (Brush)GetValue(Region7BrushProperty); }
            set { SetValue(Region7BrushProperty, value); }
        }

        public Brush Region8Brush
        {
            get { return (Brush)GetValue(Region8BrushProperty); }
            set { SetValue(Region8BrushProperty, value); }
        }

        public Brush Region9Brush
        {
            get { return (Brush)GetValue(Region9BrushProperty); }
            set { SetValue(Region9BrushProperty, value); }
        }

        public Brush Region10Brush
        {
            get { return (Brush)GetValue(Region10BrushProperty); }
            set { SetValue(Region10BrushProperty, value); }
        }

        public Brush Region11Brush
        {
            get { return (Brush)GetValue(Region11BrushProperty); }
            set { SetValue(Region11BrushProperty, value); }
        }

        public Brush Region12Brush
        {
            get { return (Brush)GetValue(Region12BrushProperty); }
            set { SetValue(Region12BrushProperty, value); }
        }

        public Brush Region13Brush
        {
            get { return (Brush)GetValue(Region13BrushProperty); }
            set { SetValue(Region13BrushProperty, value); }
        }

        public Brush Region14Brush
        {
            get { return (Brush)GetValue(Region14BrushProperty); }
            set { SetValue(Region14BrushProperty, value); }
        }

        public Brush Region15Brush
        {
            get { return (Brush)GetValue(Region15BrushProperty); }
            set { SetValue(Region15BrushProperty, value); }
        }

        public string Region1Description
        {
            get { return (string)GetValue(Region1DescriptionProperty); }
            set { SetValue(Region1DescriptionProperty, value); }
        }

        public string Region2Description
        {
            get { return (string)GetValue(Region2DescriptionProperty); }
            set { SetValue(Region2DescriptionProperty, value); }
        }

        public string Region3Description
        {
            get { return (string)GetValue(Region3DescriptionProperty); }
            set { SetValue(Region3DescriptionProperty, value); }
        }

        public string Region4Description
        {
            get { return (string)GetValue(Region4DescriptionProperty); }
            set { SetValue(Region4DescriptionProperty, value); }
        }

        public string Region5Description
        {
            get { return (string)GetValue(Region5DescriptionProperty); }
            set { SetValue(Region5DescriptionProperty, value); }
        }

        public string Region6Description
        {
            get { return (string)GetValue(Region6DescriptionProperty); }
            set { SetValue(Region6DescriptionProperty, value); }
        }

        public string Region7Description
        {
            get { return (string)GetValue(Region7DescriptionProperty); }
            set { SetValue(Region7DescriptionProperty, value); }
        }

        public string Region8Description
        {
            get { return (string)GetValue(Region8DescriptionProperty); }
            set { SetValue(Region8DescriptionProperty, value); }
        }

        public string Region9Description
        {
            get { return (string)GetValue(Region9DescriptionProperty); }
            set { SetValue(Region9DescriptionProperty, value); }
        }

        public string Region10Description
        {
            get { return (string)GetValue(Region10DescriptionProperty); }
            set { SetValue(Region10DescriptionProperty, value); }
        }

        public string Region11Description
        {
            get { return (string)GetValue(Region11DescriptionProperty); }
            set { SetValue(Region11DescriptionProperty, value); }
        }

        public string Region12Description
        {
            get { return (string)GetValue(Region12DescriptionProperty); }
            set { SetValue(Region12DescriptionProperty, value); }
        }

        public string Region13Description
        {
            get { return (string)GetValue(Region13DescriptionProperty); }
            set { SetValue(Region13DescriptionProperty, value); }
        }

        public string Region14Description
        {
            get { return (string)GetValue(Region14DescriptionProperty); }
            set { SetValue(Region14DescriptionProperty, value); }
        }

        public string Region15Description
        {
            get { return (string)GetValue(Region15DescriptionProperty); }
            set { SetValue(Region15DescriptionProperty, value); }
        }

    }
}
