using System;
using System.Reactive.Concurrency;
using Windows.UI.Xaml;

namespace CODuo.Platform
{
    public class Schedulers : ISchedulers
    {
        private static readonly Lazy<IScheduler> DispatchScheduler = new Lazy<IScheduler>(() => new CoreDispatcherScheduler(Window.Current.Dispatcher));

        public IScheduler Default => Scheduler.Default;

        public IScheduler Dispatcher => DispatchScheduler.Value;
    }
}
