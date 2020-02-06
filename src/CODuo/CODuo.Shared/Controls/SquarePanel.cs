using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CODuo.Controls
{
    public partial class SquarePanel : Panel
    {
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(double), typeof(SquarePanel), new PropertyMetadata(double.NaN));
               
        protected override Size ArrangeOverride(Size finalSize)
        {
            var length = Capped(Math.Min(finalSize.Width, finalSize.Height));

            var rect = new Rect(0, 0, length, length);

            foreach (UIElement child in Children)
            {
                child.Arrange(rect);
            }

            return finalSize;
        }

        protected double Capped(double size)
        {
            return double.IsNaN(MaxLength) ? size : Math.Min(MaxLength, size);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var length = Capped(Math.Min(availableSize.Width, availableSize.Height));

            var size = new Size(length, length);

            foreach (UIElement child in Children)
            {
                child.Measure(size);
            }

            return size;
        }

        public double MaxLength
        {
            get { return (double)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
    }
}
