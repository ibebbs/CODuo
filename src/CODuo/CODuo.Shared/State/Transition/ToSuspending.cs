namespace CODuo.State.Transition
{
    public class ToSuspending : ITransition 
    {
        public ToSuspending(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
