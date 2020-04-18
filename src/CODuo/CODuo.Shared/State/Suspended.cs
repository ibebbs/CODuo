using System;
using System.Reactive.Linq;

namespace CODuo.State
{
    public class Suspended : IState
    {
        private readonly Aggregate.IRoot _aggregateRoot;
        private readonly Event.IBus _eventBus;

        public Suspended(Aggregate.IRoot aggregateRoot, Event.IBus eventBus)
        {
            _aggregateRoot = aggregateRoot;
            _eventBus = eventBus;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    _eventBus.Publish(new Event.Application.Suspended());


                    return _eventBus
                        .GetEvent<Event.Application.Resuming>()
                        .Select(_ => new Transition.ToResuming(_aggregateRoot))
                        .Subscribe(observer);
                });
        }
    }
}
