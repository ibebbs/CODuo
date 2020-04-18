using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CODuo.Navigation.State
{
    public interface IMachine
    {
        IObservable<IData> Run(IData data);
    }

    public class Machine : IMachine
    {
        private readonly IFactory _factory;

        public Machine(IFactory factory)
        {
            _factory = factory;
        }

        public IObservable<IData> Run(IData initialData)
        {
            return Observable.Create<IData>(
                observer =>
                {
                    var state = _factory.FromData(initialData);
                    var states = new BehaviorSubject<IState>(state);

                    // First create a stream of changes by ...
                    IConnectableObservable<IChange> changes = states
                        // ... enter the current state ...
                        .Select(state => state.Enter())
                        // ... subscribing to the change observable ...
                        .Switch()
                        // ... and ensure only a single shared subscription is made to the change observable ...
                        .Publish();

                    IObservable<IData> data = changes.OfType<IData>();

                    // Then, for each transition type, select the new state...
                    IObservable<IState> transitions = changes
                        .OfType<ITransition>()
                        .Select(_ => _factory.FromData(initialData))
                        .ObserveOn(Scheduler.CurrentThread);

                    return new CompositeDisposable(
                        transitions.Subscribe(states),
                        data.Subscribe(observer),
                        changes.Connect(),
                        states
                    );
                }
            );
        }
    }
}
