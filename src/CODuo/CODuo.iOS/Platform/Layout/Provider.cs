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
                .FromEvent<WindowSizeChangedEventHandler, WindowSizeChangedEventArgs>(handler => (s, e) => handler(e), handler => Windows.UI.Xaml.Window.Current.SizeChanged += handler, handler => Windows.UI.Xaml.Window.Current.SizeChanged -= handler)
                .Select(arg => arg.Size);

            var initialHeight = Observable.Return(0).Select(_ => new Size(Windows.UI.Xaml.Window.Current.Bounds.Width, Windows.UI.Xaml.Window.Current.Bounds.Height));

            _layouts = Observable
                .Concat(initialHeight, _sizeChangedEventHandler)
                .Select(AsLayout)
                .Replay(1)
                .RefCount();
        }

        private ILayout AsLeftRightLayout(Size size, Rect hinge)
        {
            return new Instance(
                Mode.LeftRight,
                new Vector4(0, 0, Convert.ToSingle(size.Height), Convert.ToSingle(hinge.Left)),
                new Vector4(Convert.ToSingle(hinge.Right), 0, Convert.ToSingle(size.Height), Convert.ToSingle(size.Width - hinge.Right)),
                DipScaling
            );                
        }

        private ILayout AsRightLeftLayout(Size size, Rect hinge)
        {
            return new Instance(
                Mode.LeftRight,
                new Vector4(0, 0, Convert.ToSingle(size.Height), Convert.ToSingle(hinge.Left)),
                new Vector4(Convert.ToSingle(hinge.Right), 0, Convert.ToSingle(size.Height), Convert.ToSingle(size.Width - hinge.Right)),
                DipScaling
            );
        }

        private ILayout AsTopBottomLayout(Size size, Rect hinge)
        {
            return new Instance(
                Mode.LeftRight,
                new Vector4(0, 0, Convert.ToSingle(hinge.Top), Convert.ToSingle(size.Width)),
                new Vector4(0, Convert.ToSingle(hinge.Bottom), Convert.ToSingle(size.Height - hinge.Bottom), Convert.ToSingle(size.Width)),
                DipScaling
            );
        }

        private ILayout AsBottomTopLayout(Size size, Rect hinge)
        {
            return new Instance(
                Mode.LeftRight,
                new Vector4(0, 0, Convert.ToSingle(hinge.Top), Convert.ToSingle(size.Width)),
                new Vector4(0, Convert.ToSingle(hinge.Bottom), Convert.ToSingle(size.Height - hinge.Bottom), Convert.ToSingle(size.Width)),
                DipScaling
            );
        }

        private double PixelsToDip(double px) => 1;

        private double DipScaling => 1;

        private ILayout AsSingleLayout(Size size)
        {
            return new Instance(new Vector4(0, 0, Convert.ToSingle(size.Height), Convert.ToSingle(size.Width)), DipScaling);
        }

        private ILayout AsLayout(Size size)
        {
            return  AsSingleLayout(size);
        }

        public IObservable<ILayout> Changes => _layouts;
    }
}
