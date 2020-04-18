namespace CODuo.Application.State.Transition
{
    public class ToRunning : ITransition
    {
        public ToRunning(Aggregate.IRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
        }

        public Aggregate.IRoot AggregateRoot { get; }
    }
}
