using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CODuo.Xaml
{
    public class IntensityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = value switch
            {
                double d when d < 50 => VeryLow,
                double d when d < 100 => Low,
                double d when d < 200 => Moderate,
                double d when d < 300 => High,
                double d => VeryHigh,
                _ => DependencyProperty.UnsetValue
            };

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public Brush Default { get; set; }
        public Brush VeryLow { get; set; }
        public Brush Low { get; set; }
        public Brush Moderate { get; set; }
        public Brush High { get; set; }
        public Brush VeryHigh { get; set; }
    }
}
