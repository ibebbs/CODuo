using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CODuo.Xaml.Converters
{
    public class ValueToMinHeightConverter : IValueConverter
    {
        private double? Convert(object value, object parameter)
        {
            double? height = (value, parameter) switch
            {
                (int x, int y) when x < y => 0.0,
                (int x, int y) => 36.0,
                (int x, _) when x <= 0 => 0.0,
                (int x, _) => 36.0,
                (double x, double y) when x < y => 0.0,
                (double x, double y) => 36.0,
                (double x, _) when x <= 0 => 0.0,
                (double x, _) => 36.0,
                (float x, float y) when x < y => 0.0,
                (float x, float y) => 36.0,
                (float x, _) when x <= 0 => 0.0,
                (float x, _) => 36.0,
                _ => default(double?)
            };

            return height;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = Convert(value, parameter) ?? DependencyProperty.UnsetValue;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public double MinHeight { get; set; }
    }
}
