using Microsoft.Extensions.DependencyInjection;
using System;

namespace CODuo.ViewModel
{
    public interface IFactory
    {
        T Create<T>() where T : IViewModel;
    }

    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public Factory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Create<T>() where T : IViewModel
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
