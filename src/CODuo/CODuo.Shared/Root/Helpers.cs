using CODuo.Controls;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CODuo.Root
{
    public static class Helpers
    {
        public static Platform.Layout.Mode MapMode(DualPaneViewMode mode)
        {
            return mode switch
            {
                DualPaneViewMode.SinglePane => Platform.Layout.Mode.Single,
                DualPaneViewMode.Tall => Platform.Layout.Mode.TopBottom,
                DualPaneViewMode.Wide => Platform.Layout.Mode.LeftRight,
                _ => throw new ArgumentException($"Unknown DualPaneViewMode: '{mode}'", nameof(mode))
            };
        }

        public static Platform.Layout.Mode MapMode(TwoPaneViewMode mode)
        {
            return mode switch
            {
                TwoPaneViewMode.SinglePane => Platform.Layout.Mode.Single,
                TwoPaneViewMode.Tall => Platform.Layout.Mode.TopBottom,
                TwoPaneViewMode.Wide => Platform.Layout.Mode.LeftRight,
                _ => throw new ArgumentException($"Unknown DualPaneViewMode: '{mode}'", nameof(mode))
            };
        }
    }
}
