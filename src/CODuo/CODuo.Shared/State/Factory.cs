using Microsoft.Extensions.DependencyInjection;
using System;

namespace CODuo.State
{
    public interface IFactory
    {
        IState Initializing();
        IState Resuming(Aggregate.IRoot aggregateRoot);
        IState Suspending(Aggregate.IRoot aggregateRoot);
        IState Suspended(Aggregate.IRoot aggregateRoot);
        IState Home(Aggregate.IRoot aggregateRoot);
    }

    public class Factory : IFactory
    {
        private readonly Event.IBus _eventBus;
        private readonly Data.IProvider _dataProvider;
        private readonly ViewModel.IFactory _viewModelFactory;
        private readonly Platform.ISchedulers _platformSchedulers;

        public Factory(
            Event.IBus eventBus, 
            Data.IProvider dataProvider, 
            ViewModel.IFactory viewModelFactory, 
            Platform.ISchedulers platformSchedulers)
        {
            _eventBus = eventBus;
            _dataProvider = dataProvider;
            _viewModelFactory = viewModelFactory;
            _platformSchedulers = platformSchedulers;
        }

        public IState Home(Aggregate.IRoot aggregateRoot)
        {
            return new Home.State(aggregateRoot, _eventBus, _viewModelFactory, _platformSchedulers);
        }

        public IState Resuming(Aggregate.IRoot aggregateRoot)
        {
            return new Resuming(aggregateRoot, _dataProvider);
        }

        public IState Suspending(Aggregate.IRoot aggregateRoot)
        {
            return new Suspending(aggregateRoot);
        }

        public IState Suspended(Aggregate.IRoot aggregateRoot)
        {
            return new Suspended(aggregateRoot, _eventBus);
        }

        public IState Initializing()
        {
            return new Initializing(_eventBus);
        }
    }
}
