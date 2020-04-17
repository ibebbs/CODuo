using System;
using Windows.UI.Xaml.Data;

namespace CODuo.Xaml.Converters
{
    public class RegionSelectionToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value, parameter) switch
            {
                (int v, _) when v == 0 => SelectedOpacity,
                (int v, string p) when int.TryParse(p, out int pv) => v == pv ? SelectedOpacity : DeselectedOpactity,
                _ => SelectedOpacity
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public double SelectedOpacity { get; set; } = 1.0;

        public double DeselectedOpactity { get; set; } = 0.5;
    }
}
