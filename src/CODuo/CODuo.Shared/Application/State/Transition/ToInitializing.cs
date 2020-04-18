namespace CODuo.Application.State.Transition
{
    public class ToInitializing : ITransition 
    {
        public ToInitializing(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
