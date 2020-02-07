using System;
using System.Numerics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CODuo.Home
{
    public class State : IState
    {
        private readonly Event.IBus _eventBus;
        private readonly CODuo.ViewModel.IFactory _viewModelFactory;
        private readonly Platform.ISchedulers _schedulers;

        public State(Event.IBus eventBus, CODuo.ViewModel.IFactory viewModelFactory, Platform.ISchedulers schedulers)
        {
            _eventBus = eventBus;
            _viewModelFactory = viewModelFactory;
            _schedulers = schedulers;
        }

        private Root.Layout LeftRight(IViewModel viewModel)
        {
            viewModel.DetachViews();

            var left = new Left();
            var right = new Right();

            viewModel.AttachView(left);
            viewModel.AttachView(right);

            return new Root.Layout(Vector4.Zero, left, Vector4.Zero, right, 0);
        }

        private Root.Layout Single(IViewModel viewModel)
        {
            viewModel.DetachViews();

            var view = new Single();

            viewModel.AttachView(view);

            return new Root.Layout(Vector4.Zero, view, 0);
        }

        private Root.Layout AsLayout(IViewModel viewModel, Platform.Layout.Mode mode)
        {
            return mode switch
            {
                Platform.Layout.Mode.LeftRight => LeftRight(viewModel),
                _ => Single(viewModel) 
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

                    var layouts = _eventBus
                        .GetEvent<Event.LayoutModeChanged>()
                        .ObserveOn(_schedulers.Dispatcher)
                        .Select(@event => AsLayout(viewModel, @event.Mode))
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
