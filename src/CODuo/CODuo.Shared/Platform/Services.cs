using Microsoft.Extensions.DependencyInjection;
using System;

namespace CODuo.Platform
{
    public partial class Services
    {
        public static readonly Services Service = new Services();

        private readonly ServiceCollection _serviceCollection;
        private readonly Lazy<IServiceProvider> _serviceProvider;

        private Services() 
        {
            _serviceCollection = new ServiceCollection();
            _serviceProvider = new Lazy<IServiceProvider>(() => _serviceCollection.BuildServiceProvider());
        }

        private void RegisterGlobalServices(IServiceCollection services)
        {
            services.AddSingleton<Event.IBus, Event.Bus>();

            services.AddSingleton<Data.IProvider, Data.Provider>();

            services.AddTransient<State.Initializing>();

            services.AddSingleton<State.IFactory, State.Factory>();
            services.AddSingleton<State.IMachine, State.Machine>();

            services.AddSingleton<ViewModel.IFactory, ViewModel.Factory>();

            services.AddTransient<Root.View>();
            services.AddTransient<Root.IViewModel, Root.ViewModel>();

            services.AddTransient<Home.State>();
            services.AddTransient<Home.IViewModel, Home.ViewModel>();
        }

        partial void RegisterPlatformServices(IServiceCollection services);

        public void PerformRegistration()
        {
            if (_serviceProvider.IsValueCreated) throw new InvalidOperationException("You cannot register services after the service provider has been created");

            RegisterGlobalServices(_serviceCollection);
            RegisterPlatformServices(_serviceCollection);
        }

        public IServiceProvider Provider => _serviceProvider.Value;
    }
}
