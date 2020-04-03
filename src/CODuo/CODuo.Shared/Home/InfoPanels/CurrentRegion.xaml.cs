using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CODuo.Home.InfoPanels
{
    public sealed partial class CurrentRegion : UserControl
    {
        public static readonly DependencyProperty WeatherProperty = DependencyProperty.Register(nameof(Weather), typeof(Common.Weather), typeof(CurrentRegion), new PropertyMetadata(null, DataProperyChanged));

        public static readonly DependencyProperty TemperatureProperty = DependencyProperty.Register(nameof(Temperature), typeof(float), typeof(CurrentRegion), new PropertyMetadata(0.0f));
        public static readonly DependencyProperty WindSpeedProperty = DependencyProperty.Register(nameof(WindSpeed), typeof(float), typeof(CurrentRegion), new PropertyMetadata(0.0f));
        public static readonly DependencyProperty RainProperty = DependencyProperty.Register(nameof(Rain), typeof(float), typeof(CurrentRegion), new PropertyMetadata(0.0f));
        public static readonly DependencyProperty RainProbabilityProperty = DependencyProperty.Register(nameof(RainProbability), typeof(float), typeof(CurrentRegion), new PropertyMetadata(0.0f));

        private static void DataProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CurrentRegion a)
            {
                a.DataChanged();
            }
        }

        public CurrentRegion()
        {
            this.InitializeComponent();
        }

        private void DataChanged()
        {
            var weather = Weather;

            Temperature = weather?.Temperature ?? 0.0f;
            WindSpeed = weather?.WindSpeed ?? 0.0f;
            Rain = weather?.PrecipitationRate ?? 0.0f;
            RainProbability = (weather?.PrecipitationProbability ?? 0.0f) / 100.0f;

            WindDirectionArrow.RenderTransform = new RotateTransform { Angle = (weather?.WindDirection ?? 0.0f) };
        }

        public Common.Weather Weather
        {
            get { return (Common.Weather)GetValue(WeatherProperty); }
            set { SetValue(WeatherProperty, value); }
        }

        public float Temperature
        {
            get { return (float)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        public float WindSpeed
        {
            get { return (float)GetValue(WindSpeedProperty); }
            set { SetValue(WindSpeedProperty, value); }
        }
        public float Rain
        {
            get { return (float)GetValue(RainProperty); }
            set { SetValue(RainProperty, value); }
        }

        public float RainProbability
        {
            get { return (float)GetValue(RainProbabilityProperty); }
            set { SetValue(RainProbabilityProperty, value); }
        }


    }
}
