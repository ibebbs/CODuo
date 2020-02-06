using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CODuo.Root
{
    public static class Attributes
    {
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(double), typeof(Attributes), new PropertyMetadata(0.0));
        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height", typeof(double), typeof(Attributes), new PropertyMetadata(0.0));

        public static double GetWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject obj, double value)
        {
            obj.SetValue(WidthProperty, value);
        }

        public static double GetHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(HeightProperty);
        }

        public static void SetHeight(DependencyObject obj, double value)
        {
            obj.SetValue(HeightProperty, value);
        }

        public static (double, double)? FindSize(DependencyObject source)
        {
            if (source == null) return null;

            var width = GetWidth(source);
            var height = GetHeight(source);

            return (width > 0 && height > 0)
                ? (width, height)
                : FindSize(VisualTreeHelper.GetParent(source));
        }
    }
}
