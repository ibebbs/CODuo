using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Controls
{
    public sealed partial class RegionGenerationChart : UserControl
    {
        public double ChartWidth => 960;
        public double ChartHeight => 320;
        private double XSteps => ChartWidth / 96.0;

        public static readonly DependencyProperty RegionGenerationProperty = DependencyProperty.Register("RegionGeneration", typeof(IEnumerable<Common.RegionGeneration>), typeof(RegionGenerationChart), new PropertyMetadata(null, DataPropertyChanged));

        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RegionGenerationChart chart)
            {
                chart.DataChanged();
            }
        }

        private IReadOnlyDictionary<Common.FuelType, Path> _fuelTypePaths;

        public RegionGenerationChart()
        {
            this.InitializeComponent();
        }

        private IEnumerable<(Common.FuelType, double, double)> CreateDataPoints(Common.RegionGeneration regionGeneration, int xIndex)
        {
            var generation = regionGeneration.Actual.ByFuelType.Any()
                ? regionGeneration.Actual.ByFuelType
                : regionGeneration.Estimated.ByFuelType;

            var x = xIndex * XSteps;

            return Enum
                .GetValues(typeof(Common.FuelType))
                .OfType<Common.FuelType>()
                .GroupJoin(
                    generation,
                    fuelType => fuelType,
                    generationByFuelType => generationByFuelType.FuelType,
                    (fuelType, generationByFuelTypes) => (FuelType: fuelType, Percent: generationByFuelTypes.Select(gft => gft.Percent).FirstOrDefault() ?? 0.0))
                .Aggregate(
                    (Y: 0.0, DataPoints: Enumerable.Empty<(Common.FuelType, double, double)>()),
                    (seed, tuple) =>
                    {
                        var y = ChartHeight * tuple.Percent + seed.Y;

                        return (Y: y, DataPoints: seed.DataPoints.Concat(new[] { (tuple.FuelType, x, y) }).ToArray());
                    })
                .DataPoints;
        }

        private (Common.FuelType, PathFigure) AsPath(Common.FuelType fuelType, IEnumerable<(double, double)> source)
        {
            var figure = source
                .OrderBy(tuple => tuple.Item1)
                .Select(tuple => new LineSegment { Point = new Point(tuple.Item1, tuple.Item2) })
                .Concat(new [] { new LineSegment { Point = new Point(ChartWidth, 0.0) }, new LineSegment { Point = new Point(0.0, 0.0) } })
                .Aggregate(
                     new PathFigure
                     {
                         StartPoint = new Point(0, 0),
                         IsClosed = true
                     },
                     (path, segment) =>
                     {
                         path.Segments.Add(segment);

                         return path;
                     });

            return (fuelType, figure);
        }

        private void DataChanged()
        {
            var pathPoints = RegionGeneration
                .SelectMany(CreateDataPoints)
                .GroupBy(tuple => tuple.Item1)
                .Select(group => AsPath(group.Key, group.Select(tuple => (tuple.Item2, tuple.Item3))))
                .Join(_fuelTypePaths, tuple => tuple.Item1, fuelTypePath => fuelTypePath.Key, (tuple, fuelTypePath) => (Path: fuelTypePath.Value, Figure: tuple.Item2));

            foreach (var tuple in pathPoints)
            {
                tuple.Path.Data = new PathGeometry { Figures = { tuple.Figure } };
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _fuelTypePaths = new Dictionary<Common.FuelType, Path>
            {
                { Common.FuelType.Coal, Coal },
                { Common.FuelType.Oil, Oil },
                { Common.FuelType.Gas, Gas },
                { Common.FuelType.Import, Import },
                { Common.FuelType.Other, Other },
                { Common.FuelType.Biomass, Biomass },
                { Common.FuelType.Nuclear, Nuclear },
                { Common.FuelType.Solar, Solar },
                { Common.FuelType.Hydro, Hydro },
                { Common.FuelType.Wind, Wind },
            };
        }

        public IEnumerable<Common.RegionGeneration> RegionGeneration
        {
            get { return (IEnumerable<Common.RegionGeneration>)GetValue(RegionGenerationProperty); }
            set { SetValue(RegionGenerationProperty, value); }
        }
    }
}
