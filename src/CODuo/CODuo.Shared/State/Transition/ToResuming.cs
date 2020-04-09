namespace CODuo.State.Transition
{
    public class ToResuming : ITransition 
    {
        public ToResuming(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
