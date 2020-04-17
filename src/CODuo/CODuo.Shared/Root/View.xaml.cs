using Microsoft.UI.Xaml.Controls;
using System;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CODuo.Root
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : Windows.UI.Xaml.Controls.Page, IView
    {
        public View()
        {
            this.InitializeComponent();

            CurrentMode = Observable
                .FromEvent<TypedEventHandler<TwoPaneView, object>, TwoPaneView>(
                    action => (s, e) => action(s),
                    handler => twoPaneView.ModeChanged += handler,
                    handler => twoPaneView.ModeChanged -= handler)
                .Select(twoPaneView => twoPaneView.Mode)
                .StartWith(twoPaneView.Mode)
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
            twoPaneView.Pane1 = layout.Pane1Content as UIElement;
            twoPaneView.Pane2 = layout.Pane2Content as UIElement;
        }

        public IObservable<Platform.Layout.Mode> CurrentMode { get; }

        public IObservable<Unit> RefreshData { get; }
    }
}
