using Microsoft.Extensions.DependencyInjection;
using System;

namespace CODuo.State
{
    public interface IFactory
    {
        IState Initializing();
        IState Home();
    }

    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public Factory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IState Home()
        {
            return _serviceProvider.GetService<Home.State>();
        }

        public IState Initializing()
        {
            return _serviceProvider.GetService<Initializing>();
        }
    }
}
