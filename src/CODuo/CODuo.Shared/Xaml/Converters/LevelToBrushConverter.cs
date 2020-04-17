using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CODuo.Xaml.Converters
{
    public class LevelToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value, parameter) switch
            {
                (int x, int level) when x >= level => Filled,
                (float f, float level) when f >= level => Filled,
                (double d, double level) when d >= level => Filled,
                (int x, string level) when x >= double.Parse(level) => Filled,
                (float f, string level) when f >= double.Parse(level) => Filled,
                (double d, string level) when d >= double.Parse(level) => Filled,
                _ => Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public Brush Filled { get; set; }

        public Brush Empty { get; set; }
    }
}
