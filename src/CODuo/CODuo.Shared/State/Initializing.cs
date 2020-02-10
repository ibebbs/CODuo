using System;
using System.Reactive.Linq;

namespace CODuo.State
{
    public class Initializing : IState
    {
        private readonly Data.IProvider _dataProvider;

        public Initializing(Data.IProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                observer =>
                {
                    _dataProvider.Activate();

                    return Observable
                        .Return(new Transition.ToHome())
                        .Subscribe(observer);
                });
            ;
        }
    }
}
