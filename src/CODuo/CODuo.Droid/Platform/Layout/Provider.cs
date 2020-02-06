using Android.Views;
using CODuo.Droid;
using Microsoft.Device.Display;
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
        private readonly Lazy<ScreenHelper> _screenHelper;
        private readonly IObservable<Size> _sizeChangedEventHandler;
        private readonly IObservable<ILayout> _layouts;

        public Provider()
        {
            _screenHelper = new Lazy<ScreenHelper>(CreateScreenHelper);

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

        private ScreenHelper CreateScreenHelper()
        {
            var screenHelper = new ScreenHelper();

            screenHelper.Initialize(MainActivity.Activity);

            return screenHelper;
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

        private double PixelsToDip(double px) => px / _screenHelper.Value.Activity?.Resources?.DisplayMetrics?.Density ?? 1;

        private double DipScaling => 1 / _screenHelper.Value.Activity?.Resources?.DisplayMetrics?.Density ?? 1;

        private Rect GetHingeBounds()
        {
            var hingeBounds = _screenHelper.Value.GetHingeBounds();
            var dipScaling = DipScaling;

            return new Rect(
                hingeBounds.Left * dipScaling,
                hingeBounds.Top * dipScaling,
                (hingeBounds.Right - hingeBounds.Left) * dipScaling,
                (hingeBounds.Bottom - hingeBounds.Top) * dipScaling
            );
        }

        private ILayout AsDualLayout(Size size)
        {
            var rotation = _screenHelper.Value.GetRotation();
            var hingeBounds = GetHingeBounds();

            return rotation switch
            {
                SurfaceOrientation.Rotation0 => AsLeftRightLayout(size, hingeBounds),
                SurfaceOrientation.Rotation90 => AsTopBottomLayout(size, hingeBounds),
                SurfaceOrientation.Rotation180 => AsRightLeftLayout(size, hingeBounds),
                SurfaceOrientation.Rotation270 => AsBottomTopLayout(size, hingeBounds),
                _ => throw new ArgumentException($"Unknown rotation value: '{rotation}'")
            };
        }

        private ILayout AsSingleLayout(Size size)
        {
            return new Instance(new Vector4(0, 0, Convert.ToSingle(size.Height), Convert.ToSingle(size.Width)), DipScaling);
        }

        private ILayout AsLayout(Size size)
        {
            return _screenHelper.Value.IsDualMode
                ? AsDualLayout(size)
                : AsSingleLayout(size);
        }

        public IObservable<ILayout> Changes => _layouts;
    }
}
