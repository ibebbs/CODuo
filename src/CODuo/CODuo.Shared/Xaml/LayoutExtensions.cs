using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CODuo.Xaml
{
    public static class LayoutExtensions
    {
        public static readonly DependencyProperty ActualSizeBindingEnabledProperty = DependencyProperty.RegisterAttached("ActualSizeBindingEnabled", typeof(bool), typeof(LayoutExtensions), new PropertyMetadata(false, ActualSizeBindingEnabledPropertyChanged));
        public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.RegisterAttached("ActualWidth", typeof(double), typeof(LayoutExtensions), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.RegisterAttached("ActualHeight", typeof(double), typeof(LayoutExtensions), new PropertyMetadata(0.0));

        private static void SetElementSize(FrameworkElement element, Size size)
        {
            SetActualWidth(element, size.Width);
            SetActualHeight(element, size.Height);
        }

        private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetElementSize(element, e.NewSize);
            }
        }

        private static void EnableActualSizeBindingFor(FrameworkElement element)
        {
            element.SizeChanged += Element_SizeChanged;

            SetElementSize(element, new Size(element.ActualWidth, element.ActualHeight));
        }

        private static void DisableActualSizeBindingFor(FrameworkElement element)
        {
            element.SizeChanged -= Element_SizeChanged;
        }

        private static void ActualSizeBindingEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            switch (args.OldValue, args.NewValue)
            {
                case (false, true) when dependencyObject is FrameworkElement: 
                    EnableActualSizeBindingFor((FrameworkElement)dependencyObject);
                    break;
                case (true, false) when dependencyObject is FrameworkElement:
                    DisableActualSizeBindingFor((FrameworkElement)dependencyObject);
                    break;
            }
        }

        public static bool GetActualSizeBindingEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(ActualSizeBindingEnabledProperty);
        }

        public static void SetActualSizeBindingEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(ActualSizeBindingEnabledProperty, value);
        }

        public static double GetActualWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(ActualWidthProperty);
        }

        public static void SetActualWidth(DependencyObject obj, double value)
        {
            obj.SetValue(ActualWidthProperty, value);
        }

        public static double GetActualHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(ActualHeightProperty);
        }

        public static void SetActualHeight(DependencyObject obj, double value)
        {
            obj.SetValue(ActualHeightProperty, value);
        }

        public static (double, double)? FindSize(DependencyObject source)
        {
            if (source == null) return null;

            var width = GetActualWidth(source);
            var height = GetActualHeight(source);

            return (width > 0 && height > 0)
                ? (width, height)
                : FindSize(VisualTreeHelper.GetParent(source));
        }
    }
}
