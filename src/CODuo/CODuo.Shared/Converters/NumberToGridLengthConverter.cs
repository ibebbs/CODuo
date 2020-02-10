using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CODuo.Converters
{
    public class NumberToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = (value, parameter) switch
            {
                (_, string type) when type.Equals("Auto", StringComparison.OrdinalIgnoreCase) => new GridLength(0, GridUnitType.Auto),
                (int x, string type) when type.Equals("Pixel", StringComparison.OrdinalIgnoreCase) => new GridLength(x, GridUnitType.Pixel),
                (float f, string type) when type.Equals("Pixel", StringComparison.OrdinalIgnoreCase) => new GridLength(f, GridUnitType.Pixel),
                (double d, string type) when type.Equals("Pixel", StringComparison.OrdinalIgnoreCase) => new GridLength(d, GridUnitType.Pixel),
                (int x, string type) when type.Equals("Star", StringComparison.OrdinalIgnoreCase) => new GridLength(x, GridUnitType.Star),
                (float f, string type) when type.Equals("Star", StringComparison.OrdinalIgnoreCase) => new GridLength(f, GridUnitType.Star),
                (double d, string type) when type.Equals("Star", StringComparison.OrdinalIgnoreCase) => new GridLength(d, GridUnitType.Star),
                (int x, _) when x > 0 => new GridLength(x, GridUnitType.Star),
                (float f, _) when f > 0 => new GridLength(f, GridUnitType.Star),
                (double d, _) when d > 0 => new GridLength(d, GridUnitType.Star),
                (int _, _) => new GridLength(0, GridUnitType.Pixel),
                (float _, _) => new GridLength(0, GridUnitType.Pixel),
                (double _, _) => new GridLength(0, GridUnitType.Pixel),
                _ => DependencyProperty.UnsetValue
            };

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
