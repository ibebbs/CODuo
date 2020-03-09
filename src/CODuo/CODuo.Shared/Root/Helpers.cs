using CODuo.Controls;
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
    }
}
