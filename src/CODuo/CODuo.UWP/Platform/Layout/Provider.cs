using System;
using System.Numerics;
using System.Reactive.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace CODuo.Platform.Layout
{
    public class Provider : IProvider
    {
        private readonly IObservable<Size> _sizeChangedEventHandler;
        private readonly IObservable<ILayout> _layouts;

        public Provider()
        {
            _sizeChangedEventHandler = Observable
                .FromEvent<WindowSizeChangedEventHandler, WindowSizeChangedEventArgs>(handler => (s,e) => handler(e), handler => Window.Current.SizeChanged += handler, handler => Window.Current.SizeChanged -= handler)
                .Select(arg => arg.Size);

            _layouts = _sizeChangedEventHandler
                .StartWith(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height))
                .Select(AsLayout)
                .Replay(1)
                .RefCount();
        }

        private ILayout AsLayout(Size size)
        {
            return new Instance(new Vector4(0, 0, Convert.ToSingle(size.Width), Convert.ToSingle(size.Height)), 1);
        }

        public IObservable<ILayout> Changes => _layouts;
    }
}
