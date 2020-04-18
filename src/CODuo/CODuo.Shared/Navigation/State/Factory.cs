namespace CODuo.Navigation.State
{
    public interface IFactory
    {
        IState Home();
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

        public IState Home()
        {
            return new Home.State(_eventBus, _viewModelFactory, _platformSchedulers);
        }
    }
}
