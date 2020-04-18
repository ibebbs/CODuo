namespace CODuo.Application.State
{
    public interface IFactory
    {
        IState Launching();
        IState Initializing(Aggregate.IRoot aggregateRoot);
        IState Resuming(Aggregate.IRoot aggregateRoot);
        IState Running(Aggregate.IRoot aggregateRoot);
        IState Suspending(Aggregate.IRoot aggregateRoot);
        IState Suspended(Aggregate.IRoot aggregateRoot);
    }

    public class Factory : IFactory
    {
        private readonly Event.IBus _eventBus;
        private readonly Data.IProvider _dataProvider;

        public Factory(
            Event.IBus eventBus, 
            Data.IProvider dataProvider)
        {
            _eventBus = eventBus;
            _dataProvider = dataProvider;
        }

        public IState Launching()
        {
            return new Launching();
        }

        public IState Initializing(Aggregate.IRoot aggregateRoot)
        {
            return new Initializing(aggregateRoot);
        }

        public IState Resuming(Aggregate.IRoot aggregateRoot)
        {
            return new Resuming(aggregateRoot, _dataProvider);
        }

        public IState Running(Aggregate.IRoot aggregateRoot)
        {
            return new Running(aggregateRoot, _eventBus);
        }

        public IState Suspending(Aggregate.IRoot aggregateRoot)
        {
            return new Suspending(aggregateRoot);
        }

        public IState Suspended(Aggregate.IRoot aggregateRoot)
        {
            return new Suspended(aggregateRoot, _eventBus);
        }
    }
}
