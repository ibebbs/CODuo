namespace CODuo.State.Transition
{
    public class ToSuspended : ITransition
    {
        public ToSuspended(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
