﻿namespace CODuo.State.Transition
{
    public class ToHome : ITransition
    {
        public ToHome(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
