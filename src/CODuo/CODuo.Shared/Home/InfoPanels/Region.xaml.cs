using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Home.InfoPanels
{
    public sealed partial class Region : UserControl
    {
        private const string LastUpdateFormat = "hh:mmtt on dddd, dd MMMM yyyy";
        private const string LastUpdateTimeFormat = "hh:mmtt";
        private const string LastUpdateDateFormat = "dddd, dd MMMM yyyy";

        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container), typeof(Common.Container), typeof(Region), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(nameof(Current), typeof(Common.Region), typeof(Region), new PropertyMetadata(null, DataPropertyChanged));

        public static readonly DependencyProperty CityProperty = DependencyProperty.Register(nameof(City), typeof(string), typeof(Region), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty LastUpdateProperty = DependencyProperty.Register(nameof(LastUpdate), typeof(string), typeof(Region), new PropertyMetadata(DateTime.Now.ToString(LastUpdateFormat)));

        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Region a)
            {
                a.DataChanged();
            }
        }

        public Region()
        {
            this.InitializeComponent();
        }

        private string FormatCity(Common.Region region)
        {
            return (region.City, region.Latitude, region.Longitude) switch
            {
                (string city, double north, double east) when !string.IsNullOrEmpty(city) && north >= 0 && east >= 0 => $"{city} [{north}°N, {east}°E]",
                (string city, double north, double east) when !string.IsNullOrEmpty(city) && north >= 0 && east < 0 => $"{city} [{north}°N, {Math.Abs(east)}°W]",
                (string city, double north, double east) when !string.IsNullOrEmpty(city) && north < 0 && east >= 0 => $"{city} [{Math.Abs(north)}°S, {east}°E]",
                (string city, double north, double east) when !string.IsNullOrEmpty(city) && north < 0 && east < 0 => $"{city} [{Math.Abs(north)}°S, {Math.Abs(east)}°W]",
                _ => string.Empty
            };
        }

        private void DataChanged()
        {
            var current = Current;

            City = (current != null) ? FormatCity(current) : string.Empty;

            var lastUpdate = (Container?.LastUpdated ?? DateTime.UtcNow).ToLocalTime();

            LastUpdate = $"{lastUpdate.ToString(LastUpdateTimeFormat).ToLower()} on {lastUpdate.ToString(LastUpdateDateFormat)}";
        }

        public Common.Container Container
        {
            get { return (Common.Container)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public Common.Region Current
        {
            get { return (Common.Region)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }

        public string LastUpdate
        {
            get { return (string)GetValue(LastUpdateProperty); }
            set { SetValue(LastUpdateProperty, value); }
        }
    }
}
