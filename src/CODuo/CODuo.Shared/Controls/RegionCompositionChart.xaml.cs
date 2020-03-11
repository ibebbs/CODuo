using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Controls
{
    public sealed partial class RegionCompositionChart : UserControl
    {
        public double ChartWidth => 1120;
        public double ChartHeight => 320;

        public static readonly DependencyProperty PeriodsProperty = DependencyProperty.Register(nameof(Periods), typeof(IEnumerable<Common.Period>), typeof(RegionCompositionChart), new PropertyMetadata(Enumerable.Empty<Common.Period>(), DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register("SelectedRegion", typeof(int), typeof(RegionCompositionChart), new PropertyMetadata(0, DataPropertyChanged));

        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RegionCompositionChart chart)
            {
                chart.DataChanged();
            }
        }

        private IReadOnlyDictionary<Common.FuelType, Path> _fuelTypePaths;

        public RegionCompositionChart()
        {
            this.InitializeComponent();

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

        private IEnumerable<(Common.FuelType, int, double)> CreateDataPoints(Common.RegionGeneration regionGeneration, int x)
        {
            var generation = regionGeneration.Estimated.ByFuelType;

            return Enum
                .GetValues(typeof(Common.FuelType))
                .OfType<Common.FuelType>()
                //.Where(fuelType => fuelType == Common.FuelType.Coal)
                .GroupJoin(
                    generation,
                    fuelType => fuelType,
                    generationByFuelType => generationByFuelType.FuelType,
                    (fuelType, generationByFuelTypes) => (FuelType: fuelType, Percent: generationByFuelTypes.Select(gft => gft.Percent).FirstOrDefault() ?? 0.0))
                .Aggregate(
                    (Y: 0.0, DataPoints: Enumerable.Empty<(Common.FuelType, int, double)>()),
                    (seed, tuple) =>
                    {
                        var y = tuple.FuelType == Common.FuelType.Wind
                            ? ChartHeight
                            : ChartHeight * tuple.Percent + seed.Y;

                        return (Y: y, DataPoints: seed.DataPoints.Concat(new[] { (tuple.FuelType, x, y) }).ToArray());
                    })
                .DataPoints;
        }

        private (Common.FuelType, PathFigure) AsPath(Common.FuelType fuelType, (int, double)[] source)
        {
            var xStep = ChartWidth / (source.Length - 1);

            var figure = source
                .OrderBy(tuple => tuple.Item1)
                .Select(tuple => new LineSegment { Point = new Point(tuple.Item1 * xStep, ChartHeight - tuple.Item2) })
                .Concat(new [] { new LineSegment { Point = new Point(ChartWidth, ChartHeight) }, new LineSegment { Point = new Point(0.0, ChartHeight) } })
                .Aggregate(
                     new PathFigure
                     {
                         StartPoint = new Point(0.0, ChartHeight),
                         IsClosed = true
                     },
                     (path, segment) =>
                     {
                         path.Segments.Add(segment);

                         return path;
                     });

            return (fuelType, figure);
        }
        private IEnumerable<(Common.FuelType, PathFigure)> CreateFuelTypePaths(IEnumerable<Common.Period> periods)
        {
            return periods
                .Where((period, index) => index % 2 == 0)
                .SelectMany(period => period.Regions.Where(region => region.RegionId == SelectedRegion))
                .SelectMany(CreateDataPoints)
                .GroupBy(tuple => tuple.Item1)
                .Select(group => AsPath(group.Key, group.Select(tuple => (tuple.Item2, tuple.Item3)).ToArray()));
        }

        private async Task DataChanged()
        {
            var periods = Periods;

            var paths = await Task.Run(() => CreateFuelTypePaths(periods));

            var pathPoints = paths
                .Join(
                    _fuelTypePaths,
                    tuple => tuple.Item1,
                    fuelTypePath => fuelTypePath.Key,
                    (tuple, fuelTypePath) => (Path: fuelTypePath.Value, Figure: tuple.Item2));

            foreach (var tuple in pathPoints)
            {
                tuple.Path.Data = new PathGeometry { Figures = { tuple.Figure } };
            }
        }

        public IEnumerable<Common.Period> Periods
        {
            get { return (IEnumerable<Common.Period>)GetValue(PeriodsProperty); }
            set { SetValue(PeriodsProperty, value); }
        }

        public int SelectedRegion
        {
            get { return (int)GetValue(SelectedRegionProperty); }
            set { SetValue(SelectedRegionProperty, value); }
        }
    }
}
