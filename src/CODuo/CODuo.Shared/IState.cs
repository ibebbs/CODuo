using System;
using System.Collections.Generic;
using System.Text;

namespace CODuo
{
    public interface IState
    {
        IObservable<State.ITransition> Enter();
    }
}
