using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CODuo.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return parameter switch
            {
                string format => string.Format(format.Replace('[', '{').Replace(']', '}'), value),
                _ => DependencyProperty.UnsetValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
