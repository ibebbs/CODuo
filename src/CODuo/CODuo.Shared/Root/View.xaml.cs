using Microsoft.UI.Xaml.Controls;
using System;
using System.Reactive.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Reactive;
using Windows.UI.Xaml.Input;
using CODuo.Controls;
#if NETFX_CORE
using TwoPaneView = Windows.UI.Xaml.Controls.TwoPaneView;
using NavigationView = Windows.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Windows.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewItemInvokedEventArgs = Windows.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;
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
                .FromEvent<TypedEventHandler<DualPaneView, object>, DualPaneView>(
                    action => (s, e) => action(s),
                    handler => dualPaneView.ModeChanged += handler,
                    handler => dualPaneView.ModeChanged -= handler)
                .Select(twoPaneView => twoPaneView.Mode)
                .StartWith(dualPaneView.Mode)
                .Select(mode => Helpers.MapMode(mode));

            RefreshData = Observable
                .FromEvent<TappedEventHandler, TappedRoutedEventArgs>(
                    handler => (s, e) => handler(e),
                    handler => Refresh.Tapped += handler,
                    handler => Refresh.Tapped -= handler)
                .Select(_ => Unit.Default);
        }

        public void PerformLayout(Layout layout)
        {
            dualPaneView.Pane1 = layout.Pane1Content as UIElement;
            dualPaneView.Pane2 = layout.Pane2Content as UIElement;
        }

        public IObservable<Platform.Layout.Mode> CurrentMode { get; }

        public IObservable<Unit> RefreshData { get; }
    }
}
