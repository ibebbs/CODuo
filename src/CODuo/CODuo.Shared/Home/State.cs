using System;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CODuo.Home
{
    public class State : Navigation.IState
    {
        private readonly Event.IBus _eventBus;
        private readonly CODuo.ViewModel.IFactory _viewModelFactory;
        private readonly Platform.ISchedulers _schedulers;
        private readonly IViewModel _initialViewModel;

        public State(Event.IBus eventBus, CODuo.ViewModel.IFactory viewModelFactory, Platform.ISchedulers schedulers, IViewModel initialViewModel)
        {
            _eventBus = eventBus;
            _viewModelFactory = viewModelFactory;
            _schedulers = schedulers;
            _initialViewModel = initialViewModel;
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

        private Root.Layout Tall(IViewModel viewModel)
        {
            viewModel.DetachViews();

            var view = new Tall();

            viewModel.AttachView(view);

            return new Root.Layout(Vector4.Zero, view, 0);
        }

        private Root.Layout AsLayout(IViewModel viewModel, Platform.Layout.Mode mode)
        {
            return mode switch
            {
                Platform.Layout.Mode.LeftRight => LeftRight(viewModel),
                _ => Tall(viewModel) 
            };
        }

        private Event.Layout.Changed AsEvent(Root.Layout layout)
        {
            return new Event.Layout.Changed(layout);
        }

        private (IViewModel, Platform.Layout.Mode, bool, bool) ConstructViewModel(IViewModel viewModel, Platform.Layout.Mode oldMode, Platform.Layout.Mode newMode)
        {
            return viewModel switch
            {
                null => (_viewModelFactory.Create<IViewModel>(), newMode, true, true),
                _ when oldMode != newMode => (viewModel, newMode, false, true),
                _ => (viewModel, newMode, true, false)
            };
        }

        public IObservable<Navigation.State.IChange> Enter()
        {
            return Observable.Create<Navigation.State.IChange>(
                observer =>
                {
                    var tuples = Observable
                        // Listen to layout changes ...
                        .Merge(
                            _eventBus.GetEvent<Event.LayoutModeResponse>().Select(@event => @event.Mode),
                            _eventBus.GetEvent<Event.LayoutModeChanged>().Select(@event => @event.Mode))
                        // ... selecting the view model to use after each layout change ...
                        .Scan((ViewModel: _initialViewModel, Mode: default(Platform.Layout.Mode), RequiresActivation: true, RequiresLayout: false), (tuple, newMode) => ConstructViewModel(tuple.ViewModel, tuple.Mode, newMode))
                        .Publish();

                    var viewModels = tuples
                        .Where(tuple => tuple.RequiresActivation)
                        .Select(tuple => tuple.ViewModel)
                        .Using(viewModel => viewModel.Activate());

                    var layouts = tuples
                        // ... and where it's a new view model ...
                        .Where(tuple => tuple.RequiresLayout)
                        // ... move onto the UI thread ...
                        .ObserveOn(_schedulers.Dispatcher)
                        // ... to create and attach views ...
                        .Select(tuple => AsLayout(tuple.ViewModel, tuple.Mode))
                        // ... then create an event for the layout
                        .Select(AsEvent);

                    var disposable = new CompositeDisposable(
                        layouts.Subscribe(_eventBus.Publish),
                        viewModels.Subscribe(observer),
                        tuples.Connect()
                    );

                    _eventBus.Publish(new Event.LayoutModeRequest());

                    return disposable;
                }
            );
        }
    }
}
