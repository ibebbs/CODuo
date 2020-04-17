using System;
using System.Reactive.Concurrency;
using System.Threading;

namespace CODuo.Platform
{
    public class Schedulers : ISchedulers
    {
        private static readonly Lazy<IScheduler> DispatchScheduler = new Lazy<IScheduler>(() => new SynchronizationContextScheduler(SynchronizationContext.Current));

        public IScheduler Default => Scheduler.Default;

        public IScheduler Dispatcher => DispatchScheduler.Value;
    }
}
