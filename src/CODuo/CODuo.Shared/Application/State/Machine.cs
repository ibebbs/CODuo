using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CODuo.Application.State
{
    public interface IMachine
    {
        IDisposable Start();
    }

    public class Machine : IMachine
    {
        private readonly IFactory _factory;
        private readonly Subject<IState> _states;

        public Machine(IFactory factory)
        {
            _factory = factory;
            _states = new Subject<IState>();
        }

        public IDisposable Start()
        {
            // First create a stream of transitions by ...
            IObservable<ITransition> transitions = _states
                // ... starting from the initializing state ...
                .StartWith(_factory.Launching())
                // ... enter the current state ...
                .Select(state => state.Enter())
                // ... subscribing to the transition observable ...
                .Switch()
                // ... and ensure only a single shared subscription is made to the transitions observable ...
                .Publish()
                // ... held until there are no more subscribers
                .RefCount();

            // Then, for each transition type, select the new state...
            IObservable<IState> states = Observable.Merge(
                transitions.OfType<Transition.ToInitializing>().Select(transition => _factory.Initializing(transition.AggregateRoot)),
                transitions.OfType<Transition.ToResuming>().Select(transition => _factory.Resuming(transition.AggregateRoot)),
                transitions.OfType<Transition.ToRunning>().Select(transition => _factory.Running(transition.AggregateRoot)),
                transitions.OfType<Transition.ToSuspending>().Select(transition => _factory.Suspending(transition.AggregateRoot)),
                transitions.OfType<Transition.ToSuspended>().Select(transition => _factory.Suspended(transition.AggregateRoot))
            );

            // Finally, subscribe to the state observable ...
            return states
                // ... ensuring all transitions are serialized ...
                .ObserveOn(Scheduler.CurrentThread)
                // ... back onto the source state observable
                .Subscribe(_states);
        }
    }
}
