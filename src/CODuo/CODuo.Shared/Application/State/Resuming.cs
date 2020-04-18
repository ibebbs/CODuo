using System;
using System.Reactive.Linq;

namespace CODuo.Application.State
{
    public class Resuming : IState
    {
        private readonly Aggregate.IRoot _aggregateRoot;
        private readonly Data.IProvider _dataProvider;

        public Resuming(Aggregate.IRoot aggregateRoot, Data.IProvider dataProvider)
        {
            _aggregateRoot = aggregateRoot;
            _dataProvider = dataProvider;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    var aggregateRoot = _aggregateRoot.SubscribeToDataProvider(() => _dataProvider.Activate());

                    return Observable
                        .Return(new Transition.ToRunning(aggregateRoot))
                        .Subscribe(observer);
                });
            ;
        }
    }
}
