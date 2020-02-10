using System;
using System.Reactive;
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
        private readonly BehaviorSubject<View> _view;

        public ViewModel(Event.IBus eventBus, Platform.ISchedulers schedulers)
        {
            _eventBus = eventBus;
            _schedulers = schedulers;
            _view = new BehaviorSubject<View>(null);
        }

        private IDisposable ShouldSendLayoutModeChangedEventWhenModeChanges()
        {
            return _view
                .Select(view => view is null ? Observable.Never<Platform.Layout.Mode>() : view.CurrentMode)
                .Switch()
                .Select(mode => new Event.LayoutModeChanged(mode))
                .Subscribe(_eventBus.Publish);
        }

        private IDisposable ShouldSendLayoutModeResponseWhenLayoutModeRequestReceived()
        {
            return _eventBus
                .GetEvent<Event.LayoutModeRequest>()
                .SelectMany(_ => _view.Where(view => view != null).SelectMany(view => view.CurrentMode).Take(1))
                .Select(mode => new Event.LayoutModeChanged(mode))
                .Subscribe(_eventBus.Publish);
        }

        private IDisposable ShouldUpdateLayoutWhenLayoutUpdatedReceivedOrLayoutUpdaterChanges()
        {
            return _eventBus.GetEvent<Event.LayoutChanged>()
                .WithLatestFrom(_view, (@event, view) => (@event.Layout, View: view))
                .Where(tuple => tuple.View != null)
                .ObserveOn(_schedulers.Dispatcher)
                .Subscribe(tuple => tuple.View.PerformLayout(tuple.Layout));
        }

        private IDisposable ShouldSendRefreshDataWhenRefreshDataCommandExecuted()
        {
            return _view
                .Select(view => view is null ? Observable.Never<Unit>() : view.RefreshData)
                .Switch()
                .Select(_ => new Event.RefreshData())
                .Subscribe(_eventBus.Publish);
        }

        public void AttachView(object view)
        {
            switch (view)
            {
                case View rootView: 
                    _view.OnNext(rootView);
                    break;
            }
        }

        public void DetachViews()
        {
            _view.OnNext(null);
        }

        public IDisposable Activate()
        {
            return new CompositeDisposable(
                ShouldSendLayoutModeChangedEventWhenModeChanges(),
                ShouldSendLayoutModeResponseWhenLayoutModeRequestReceived(),
                ShouldUpdateLayoutWhenLayoutUpdatedReceivedOrLayoutUpdaterChanges(),
                ShouldSendRefreshDataWhenRefreshDataCommandExecuted()
            );
        }
    }
}
