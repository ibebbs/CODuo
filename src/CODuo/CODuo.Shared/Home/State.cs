using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CODuo.Home
{
    public class State : IState
    {
        private readonly Event.IBus _eventBus;
        private readonly Platform.Layout.IProvider _layoutProvider;
        private readonly CODuo.ViewModel.IFactory _viewModelFactory;
        private readonly Platform.ISchedulers _schedulers;

        public State(Event.IBus eventBus, Platform.Layout.IProvider layoutProvider, CODuo.ViewModel.IFactory viewModelFactory, Platform.ISchedulers schedulers)
        {
            _eventBus = eventBus;
            _layoutProvider = layoutProvider;
            _viewModelFactory = viewModelFactory;
            _schedulers = schedulers;
        }

        private Root.Layout LeftRight(IViewModel viewModel, Platform.ILayout layout)
        {
            viewModel.DetachViews();

            var left = new Left();
            var right = new Right();

            viewModel.AttachView(left);
            viewModel.AttachView(right);

            return new Root.Layout(layout.Pane1Location, left, layout.Pane2Location, right, layout.DipScaling);
        }

        private Root.Layout Single(IViewModel viewModel, Platform.ILayout layout)
        {
            viewModel.DetachViews();

            var view = new Single();

            viewModel.AttachView(view);

            return new Root.Layout(layout.Pane1Location, view, layout.DipScaling);
        }

        private Root.Layout AsLayout(IViewModel viewModel, Platform.ILayout layout)
        {
            return layout.Mode switch
            {
                Platform.Layout.Mode.LeftRight => LeftRight(viewModel, layout),
                _ => Single(viewModel, layout) 
            };
        }

        private Event.LayoutChanged AsEvent(Root.Layout layout)
        {
            return new Event.LayoutChanged(layout);
        }

        public IObservable<CODuo.State.ITransition> Enter()
        {
            return Observable.Create<CODuo.State.ITransition>(
                observer =>
                {
                    var viewModel = _viewModelFactory.Create<IViewModel>();

                    var layouts = _layoutProvider.Changes
                        .ObserveOn(_schedulers.Dispatcher)
                        .Select(layout => AsLayout(viewModel, layout))
                        .Select(AsEvent)
                        .Subscribe(_eventBus.Publish);

                    return new CompositeDisposable(
                        viewModel.Activate(),
                        layouts
                    );
                }
            );
        }
    }
}
