using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CODuo.Root
{
    public interface IViewModel : CODuo.IViewModel, CODuo.IViewAware
    {
    }

    public class ViewModel : IViewModel
    {
        private readonly Event.IBus _eventBus;
        private readonly Platform.ISchedulers _schedulers;
        private readonly BehaviorSubject<Action<Layout>> _layoutUpdater;

        public ViewModel(Event.IBus eventBus, Platform.ISchedulers schedulers)
        {
            _eventBus = eventBus;
            _schedulers = schedulers;
            _layoutUpdater = new BehaviorSubject<Action<Layout>>(null);
        }

        private IDisposable ShouldDebugAllEvents()
        {
            return _eventBus.GetEvent<object>().Subscribe(@event => System.Diagnostics.Debug.WriteLine($"Event: '{@event}'"));
        }

        private IDisposable ShouldUpdateLayoutWhenLayoutUpdatedReceivedOrLayoutUpdaterChanges()
        {
            return _eventBus.GetEvent<Event.LayoutChanged>()
                .WithLatestFrom(_layoutUpdater, (@event, updater) => (@event.Layout, Updater: updater))
                .Where(tuple => tuple.Updater != null)
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(tuple => tuple.Updater(tuple.Layout));
        }

        public void AttachView(object view)
        {
            switch (view)
            {
                case View rootView: 
                    _layoutUpdater.OnNext(rootView.PerformLayout);
                    break;
            }
        }

        public void DetachViews()
        {
            _layoutUpdater.OnNext(null);
        }

        public IDisposable Activate()
        {
            return new CompositeDisposable(
                ShouldDebugAllEvents(),
                ShouldUpdateLayoutWhenLayoutUpdatedReceivedOrLayoutUpdaterChanges()
            );
        }
    }
}
