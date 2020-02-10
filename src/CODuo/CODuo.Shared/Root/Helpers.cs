using Microsoft.UI.Xaml.Controls;
using System;
#if NETFX_CORE
using TwoPaneViewMode = Windows.UI.Xaml.Controls.TwoPaneViewMode;
#endif

namespace CODuo.Root
{
    public static class Helpers
    {
        public static Platform.Layout.Mode MapMode(TwoPaneViewMode mode)
        {
            return mode switch
            {
                TwoPaneViewMode.SinglePane => Platform.Layout.Mode.Single,
                TwoPaneViewMode.Tall => Platform.Layout.Mode.TopBottom,
                TwoPaneViewMode.Wide => Platform.Layout.Mode.LeftRight,
                _ => throw new ArgumentException($"Unknown TwoPaneViewMode: '{mode}'", nameof(mode))
            };
        }
    }
}
