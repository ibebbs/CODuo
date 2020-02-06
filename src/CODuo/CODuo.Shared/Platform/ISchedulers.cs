using System.Reactive.Concurrency;

namespace CODuo.Platform
{
    public interface ISchedulers
    {
        IScheduler Default { get; }

        IScheduler Dispatcher { get; }
    }
}
