using Microsoft.UI.Xaml.Controls;
using System;
using System.Reactive.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if NETFX_CORE
using TwoPaneView = Windows.UI.Xaml.Controls.TwoPaneView;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CODuo.Root
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : Page, IView
    {
        public View()
        {
            this.InitializeComponent();

            CurrentMode = Observable
                .FromEvent<TypedEventHandler<TwoPaneView, object>, TwoPaneView>(
                    handler => (s, e) => handler(s),
                    handler => TwoPaneView.ModeChanged += handler,
                    handler => TwoPaneView.ModeChanged -= handler)
                .Select(twoPaneView => twoPaneView.Mode)
                .StartWith(TwoPaneView.Mode)
                .Select(mode => Helpers.MapMode(mode));
        }

        public void PerformLayout(Layout layout)
        {
            TwoPaneView.Pane1 = layout.Pane1Content as UIElement;
            TwoPaneView.Pane2 = layout.Pane2Content as UIElement;
        }

        public IObservable<Platform.Layout.Mode> CurrentMode { get; }
    }
}
