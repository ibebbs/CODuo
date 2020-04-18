using System;
using System.Reactive.Linq;

namespace CODuo.Application.State
{
    public class Suspending : IState
    {
        private readonly Aggregate.IRoot _aggregateRoot;

        public Suspending(Aggregate.IRoot aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    var aggregateRoot = _aggregateRoot.UnsubscribeFromDataProvider(disposable => disposable.Dispose());

                    return Observable
                        .Return(new Transition.ToSuspended(aggregateRoot))
                        .Subscribe(observer);
                });
            ;
        }
    }
}
