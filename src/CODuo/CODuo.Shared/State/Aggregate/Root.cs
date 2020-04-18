using System;

namespace CODuo.State.Aggregate
{
    public interface IRoot
    {
        IRoot SubscribeToDataProvider(Func<IDisposable> subscriber);

        IRoot UnsubscribeFromDataProvider(Action<IDisposable> unsubscriber);
    }

    public class Root : IRoot
    {
        private IDisposable _dataProviderSubscription;

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
