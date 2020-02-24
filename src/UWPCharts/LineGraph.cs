using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.Media;
using Windows.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace Ailon.QuickCharts
{
    /// <summary>
    /// Facilitates rendering of line graphs.
    /// </summary>
    public partial class LineGraph : SerialGraph
    {
        /// <summary>
        /// Instantiates LineGraph.
        /// </summary>
        public LineGraph()
        {
            this.DefaultStyleKey = typeof(LineGraph);
            _lineGraph = new Polyline();

            BindBrush();
            BindStrokeThickness();
            BindStrokeDashArray();
        }

        private void BindBrush()
        {
            Binding brushBinding = new Binding();
            brushBinding.Path = new PropertyPath("Brush");
            brushBinding.Source = this;
            _lineGraph.SetBinding(Polyline.StrokeProperty, brushBinding);
        }

        private void BindStrokeThickness()
        {
            Binding thicknessBinding = new Binding();
            thicknessBinding.Path = new PropertyPath("StrokeThickness");
            thicknessBinding.Source = this;
            _lineGraph.SetBinding(Polyline.StrokeThicknessProperty, thicknessBinding);
        }

        private void BindStrokeDashArray()
        {
            Binding strokeDashArrayBinding = new Binding();
            strokeDashArrayBinding.Path = new PropertyPath("StrokeDashArray");
            strokeDashArrayBinding.Source = this;
            _lineGraph.SetBinding(Polyline.StrokeDashArrayProperty, strokeDashArrayBinding);
        }

        private Canvas _graphCanvas;
        private Polyline _lineGraph;

        /// <summary>
        /// Applies control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _graphCanvas = (Canvas)TreeHelper.TemplateFindName("PART_GraphCanvas", this);
            _graphCanvas.Children.Add(_lineGraph);
        }

        /// <summary>
        /// Renders line graph.
        /// </summary>
        public override void Render()
        {
            if (Locations?.Any() ?? false)
            {
                _lineGraph.Points = Locations;
                _lineGraph.Visibility = Visibility.Visible;
            }
            else
            {
                _lineGraph.Points = Empty.Value;
                _lineGraph.Visibility = Visibility.Collapsed;
            }
        }

        private static readonly Lazy<PointCollection> Empty = new Lazy<PointCollection>(
            () =>
            {
                var pointCollection = new PointCollection();
                pointCollection.Add(new Point(0, 0));
                return pointCollection;
            }
        );


        /// <summary>
        /// Identifies <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(LineGraph),
            new PropertyMetadata(2.0)
            );

        /// <summary>
        /// Gets or sets stroke thickness for a line graph line.
        /// This is a dependency property.
        /// The default is 2.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(LineGraph.StrokeThicknessProperty); }
            set { SetValue(LineGraph.StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(LineGraph), new PropertyMetadata(null));

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
    }
}
