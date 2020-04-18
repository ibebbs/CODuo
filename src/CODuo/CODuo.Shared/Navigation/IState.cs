using System;

namespace CODuo.Navigation
{
    public interface IState
    {
        IObservable<State.IChange> Enter();
    }
}
