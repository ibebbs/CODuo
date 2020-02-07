using Microsoft.UI.Xaml.Controls;
using System;
using System.Numerics;
using System.Reactive.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if NETFX_CORE
using TwoPaneView = Windows.UI.Xaml.Controls.TwoPaneView;
using TwoPaneViewMode = Windows.UI.Xaml.Controls.TwoPaneViewMode;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CODuo.Root
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : Page, IView
    {
        private static Platform.Layout.Mode MapMode(TwoPaneViewMode mode)
        {
            return mode switch
            {
                TwoPaneViewMode.SinglePane => Platform.Layout.Mode.Single,
                TwoPaneViewMode.Tall => Platform.Layout.Mode.TopBottom,
                TwoPaneViewMode.Wide => Platform.Layout.Mode.LeftRight,
                _ => throw new ArgumentException($"Unknown TwoPaneViewMode: '{mode}'", nameof(mode))
            };
        }

        public View()
        {
            this.InitializeComponent();

            CurrentMode = Observable
                .FromEvent<TypedEventHandler<TwoPaneView, object>, TwoPaneView>(
                    handler => (s, e) => handler(s),
                    handler => TwoPaneView.ModeChanged += handler,
                    handler => TwoPaneView.ModeChanged -= handler)
                .Select(twoPaneView => twoPaneView.Mode)
                .Select(MapMode);
        }

        private void LayoutPane(Border pane, bool paneVisible, Vector4 paneLocation, object paneContent, double maxHeight)
        {
            pane.Visibility = Visibility.Collapsed;

            if (paneVisible)
            {
                Canvas.SetLeft(pane, paneLocation.X);
                Canvas.SetTop(pane, paneLocation.Y);

                pane.Width = paneLocation.W;
                pane.Height = Math.Min(paneLocation.Z, maxHeight);

                pane.Child = paneContent as UIElement;
                pane.Visibility = Visibility.Visible;
            }
        }

        public void PerformLayout(Layout layout)
        {
            // Hack to ensure the Canvas element is resized before first being shown
            //LayoutRow1.Height = new GridLength(1, GridUnitType.Star);
            //LayoutRow2.Height = new GridLength(0);

            //var height = Xaml.LayoutExtensions.GetActualHeight(Canvas);

            //LayoutPane(Pane1, layout.Pane1Visible, layout.Pane1Location, layout.Pane1Content, height);
            //LayoutPane(Pane2, layout.Pane2Visible, layout.Pane2Location, layout.Pane2Content, height);

            TwoPaneView.Pane1 = layout.Pane1Content as UIElement;
            TwoPaneView.Pane2 = layout.Pane2Content as UIElement;
        }

        public IObservable<Platform.Layout.Mode> CurrentMode { get; }
    }
}
