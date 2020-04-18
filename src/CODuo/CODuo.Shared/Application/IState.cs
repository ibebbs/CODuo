using System;

namespace CODuo.Application
{
    public interface IState
    {
        IObservable<State.ITransition> Enter();
    }
}
