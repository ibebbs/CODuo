using System;
using System.Reactive.Linq;

namespace CODuo.State
{
    public class Initializing : IState
    {
        public IObservable<ITransition> Enter()
        {
            return Observable.Return(new Transition.ToHome());
        }
    }
}
