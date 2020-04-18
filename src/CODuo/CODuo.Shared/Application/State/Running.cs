using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CODuo.Application.State
{
    public class Running : IState
    {
        private readonly Aggregate.IRoot _aggregateRoot;
        private readonly Event.IBus _eventBus;

        public Running(Aggregate.IRoot aggregateRoot, Event.IBus eventBus)
        {
            _aggregateRoot = aggregateRoot;
            _eventBus = eventBus;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    var navigation = Platform.Services.Service.Provider
                        .GetService<Navigation.State.IMachine>();

                    var transitions = _eventBus
                        .GetEvent<Event.Application.Suspending>()
                        .Select(_ => new Transition.ToSuspending(_aggregateRoot));

                    return new CompositeDisposable(
                        navigation.Start(),
                        transitions.Subscribe(observer)
                    );
                });
            ;
        }
    }
}
