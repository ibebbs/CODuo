using System;
using System.Reactive.Linq;

namespace CODuo.State
{
    public class Initializing : IState
    {
        private readonly Event.IBus _eventBus;

        public Initializing(Event.IBus eventBus)
        {
            _eventBus = eventBus;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
#if NETFX_CORE
                    Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseVisible);
                    bool result = Windows.UI.ViewManagement.ApplicationViewScaling.TrySetDisableLayoutScaling(true);
#endif

                    return Observable
                        .Return(new Transition.ToResuming(new Aggregate.Root()))
                        .Subscribe(observer);
                });
            ;
        }
    }
}
