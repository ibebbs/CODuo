using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CODuo.Controls
{
    public partial class DonutSlice : Path
    {
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(DonutSlice), new PropertyMetadata(default(double), (s, e) => { Changed(s as DonutSlice); }));
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DonutSlice), new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as DonutSlice); }));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(DonutSlice), new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as DonutSlice); }));
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(DonutSlice), new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as DonutSlice); }));

        private bool _isLoaded;

        public DonutSlice()
        {
            Loaded += (s, e) =>
            {
                _isLoaded = true;
                Redraw();
            };
        }

        private void Redraw()
        {
            Width = Height = 2 * (Radius + StrokeThickness);
            var endAngle = StartAngle + Angle;

            var startX = Radius + Math.Sin(StartAngle * Math.PI / 180) * InnerRadius;
            var startY = Radius - Math.Cos(StartAngle * Math.PI / 180) * InnerRadius;

            // path container
            var figure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = true,
            };

            //  start angle line
            var lineX = Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius;
            var lineY = Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius;
            var startline = new LineSegment { Point = new Point(lineX, lineY) };
            figure.Segments.Add(startline);

            // outer arc
            var outerArcX = Radius + Math.Sin(endAngle * Math.PI / 180) * Radius;
            var outerArcY = Radius - Math.Cos(endAngle * Math.PI / 180) * Radius;
            var outerArc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = new Point(outerArcX, outerArcY),
                Size = new Size(Radius, Radius),
                SweepDirection = SweepDirection.Clockwise,
            };
            figure.Segments.Add(outerArc);

            //  end angle line
            var endX = Radius + Math.Sin(endAngle * Math.PI / 180) * InnerRadius;
            var endY = Radius - Math.Cos(endAngle * Math.PI / 180) * InnerRadius;
            var endline = new LineSegment { Point = new Point(endX, endY) };
            figure.Segments.Add(endline);

            // inner arc
            var innerArc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = new Point(startX, startY),
                Size = new Size(InnerRadius, InnerRadius),
                SweepDirection = SweepDirection.Counterclockwise,
            };
            figure.Segments.Add(innerArc);

            Data = new PathGeometry { Figures = { figure } };
            InvalidateArrange();
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        private static void Changed(DonutSlice donutSlice)
        {
            if (donutSlice._isLoaded)
            {
                donutSlice.Redraw();
            }
        }
    }
}
