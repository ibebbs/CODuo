using Microsoft.Extensions.DependencyInjection;
using System;

namespace CODuo.Platform
{
    public partial class Services
    {
        partial void RegisterPlatformServices(IServiceCollection services)
        {
            services.AddSingleton<ISchedulers, Schedulers>();
            services.AddSingleton<IInformation, Information>();
        }
    }
}
