using System;

namespace CODuo.Navigation.State
{
    public interface IFactory
    {
        IState Home(Home.IViewModel viewModel);
        IState FromData(IData data);
    }

    public class Factory : IFactory
    {
        private readonly Event.IBus _eventBus;
        private readonly ViewModel.IFactory _viewModelFactory;
        private readonly Platform.ISchedulers _platformSchedulers;

        public Factory(
            Event.IBus eventBus,
            ViewModel.IFactory viewModelFactory, 
            Platform.ISchedulers platformSchedulers)
        {
            _eventBus = eventBus;
            _viewModelFactory = viewModelFactory;
            _platformSchedulers = platformSchedulers;
        }

        public IState FromData(IData data)
        {
            return data switch
            {
                null => Home(null),
                Home.IViewModel viewModel => Home(viewModel),
                _ => throw new ArgumentException("Unknown navigation data type")
            };
        }

        public IState Home(Home.IViewModel viewModel)
        {
            return new Home.State(_eventBus, _viewModelFactory, _platformSchedulers, viewModel);
        }
    }
}
