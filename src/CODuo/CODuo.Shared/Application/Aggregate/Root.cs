using System;

namespace CODuo.Application.Aggregate
{
    public interface IRoot
    {
        IRoot ActiveRootViewModel(Func<IDisposable> activation);

        IRoot DeactivateRootViewModel(Action<IDisposable> deactivation);

        IRoot SubscribeToDataProvider(Func<IDisposable> subscriber);

        IRoot UnsubscribeFromDataProvider(Action<IDisposable> unsubscriber);
    }

    public class Root : IRoot
    {
        private IDisposable _rootViewModelActivation;
        private IDisposable _dataProviderSubscription;

        public IRoot ActiveRootViewModel(Func<IDisposable> activation)
        {
            if (_rootViewModelActivation == null)
            {
                _rootViewModelActivation = activation();

                return this;
            }
            else
            {
                throw new InvalidOperationException("Already activated root view model");
            }
        }

        public IRoot DeactivateRootViewModel(Action<IDisposable> deactivation)
        {
            if (_rootViewModelActivation != null)
            {
                deactivation(_rootViewModelActivation);

                _rootViewModelActivation = null;

                return this;
            }
            else
            {
                throw new InvalidOperationException("Root view model not activated");
            }
        }

        public IRoot SubscribeToDataProvider(Func<IDisposable> subscriber)
        {
            if (_dataProviderSubscription == null)
            {
                _dataProviderSubscription = subscriber();

                return this;
            }
            else
            {
                throw new InvalidOperationException("Already subscribed to data provider");
            }
        }

        public IRoot UnsubscribeFromDataProvider(Action<IDisposable> unsubscriber)
        {
            if (_dataProviderSubscription != null)
            {
                unsubscriber(_dataProviderSubscription);

                _dataProviderSubscription = null;

                return this;
            }
            else
            {
                throw new InvalidOperationException("Not subscribed to data provider");
            }
        }
    }
}
