using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;

namespace CODuo.State
{
    public class Initializing : IState
    {
        private readonly Aggregate.IRoot _aggregateRoot;

        public Initializing(Aggregate.IRoot aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    var aggregateRoot = _aggregateRoot;

                    var rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;

                    // Do not repeat app initialization when the Window already has content,
                    // just ensure that the window is active
                    if (rootFrame == null)
                    {
                        var rootView = Platform.Services.Service.Provider.GetService<Root.View>();
                        var rootViewModel = Platform.Services.Service.Provider.GetService<Root.IViewModel>();

                        rootViewModel.AttachView(rootView);

                        rootFrame = new Frame();
                        rootFrame.Content = rootView;

                        // Place the frame in the current Window
                        Windows.UI.Xaml.Window.Current.Content = rootFrame;

                        aggregateRoot = _aggregateRoot.ActiveRootViewModel(() => rootViewModel.Activate());
                    }

                    return Observable
                        .Return(new Transition.ToResuming(aggregateRoot))
                        .Subscribe(observer);
                });
            ;
        }
    }
}
