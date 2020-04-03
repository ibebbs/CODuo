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

        public static readonly DependencyProperty WindDirectionProperty = DependencyProperty.Register("WindDirection", typeof(double), typeof(CurrentRegion), new PropertyMetadata(0.0));

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

            WindDirection = Convert.ToDouble(weather?.WindDirection ?? 0.0f);

            WindDirectionArrow.RenderTransform = new RotateTransform { Angle = WindDirection };
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

        public double WindDirection
        {
            get { return (double)GetValue(WindDirectionProperty); }
            set { SetValue(WindDirectionProperty, value); }
        }
    }
}
