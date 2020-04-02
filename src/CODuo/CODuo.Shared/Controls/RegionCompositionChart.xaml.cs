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
        private const double ChartWidthValue = 1120;
        private const double ChartHeightValue = 320;

        public static readonly DependencyProperty PeriodsProperty = DependencyProperty.Register(nameof(Periods), typeof(IEnumerable<Common.Period>), typeof(RegionCompositionChart), new PropertyMetadata(Enumerable.Empty<Common.Period>(), DataPropertyChanged));
        public static readonly DependencyProperty SelectedRegionProperty = DependencyProperty.Register(nameof(SelectedRegion), typeof(int), typeof(RegionCompositionChart), new PropertyMetadata(0, DataPropertyChanged));
        public static readonly DependencyProperty CurrentPeriodProperty = DependencyProperty.Register(nameof(CurrentPeriod), typeof(Common.Period), typeof(RegionCompositionChart), new PropertyMetadata(null, DataPropertyChanged));

        public static readonly DependencyProperty ForecastLeftProperty = DependencyProperty.Register(nameof(ForecastLeft), typeof(double), typeof(RegionCompositionChart), new PropertyMetadata(ChartWidthValue));
        public static readonly DependencyProperty ForecastWidthProperty = DependencyProperty.Register(nameof(ForecastWidth), typeof(double), typeof(RegionCompositionChart), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ForecastStartProperty = DependencyProperty.Register(nameof(ForecastStart), typeof(DateTime), typeof(RegionCompositionChart), new PropertyMetadata(DateTime.Now.AddDays(-1)));
        public static readonly DependencyProperty ForecastEndProperty = DependencyProperty.Register(nameof(ForecastEnd), typeof(DateTime), typeof(RegionCompositionChart), new PropertyMetadata(DateTime.Now.AddDays(1)));
        
        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RegionCompositionChart chart)
            {
                _ = chart.DataChanged(e.Property);
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
                        // If this is the last fuel type...
                        var y = tuple.FuelType == Common.FuelType.Wind
                            // ... ensure it is rounded to the top of the chart ...
                            ? ChartHeight
                            // ... otherwise use the percentage height ...
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
                .OrderBy(period => period.From)
                .Where((period, index) => index % 2 == 0)
                .SelectMany(period => period.Regions.Where(region => region.RegionId == SelectedRegion))
                .SelectMany(CreateDataPoints)
                .GroupBy(tuple => tuple.Item1)
                .Select(group => AsPath(group.Key, group.Select(tuple => (tuple.Item2, tuple.Item3)).ToArray()));
        }

        private async Task RefreshPaths()
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

        private ValueTask RefreshDates()
        {
            var periods = (Periods ?? Enumerable.Empty<Common.Period>())
                .OrderBy(period => period.From)
                .ToArray();

            var current = CurrentPeriod;

            if (current != null && periods.Length > 0)
            {
                var xStep = ChartWidth / (periods.Length - 1);

                var currentIndex = periods                    
                    .TakeWhile(period => period.From < current.From)
                    .Select((period, index) => index)
                    .Last();

                var left = currentIndex * xStep;

                ForecastLeft = left;
                ForecastWidth = ChartWidthValue - left;

                ForecastStart = periods.Select(period => period.From).First();
                ForecastEnd = periods.Reverse().Select(period => period.To).First();
            }

            return default;
        }

        private async Task DataChanged(DependencyProperty d)
        {
            if (d.Equals(PeriodsProperty) || d.Equals(SelectedRegionProperty))
            {
                await RefreshPaths();
            }

            if (d.Equals(PeriodsProperty) || d.Equals(CurrentPeriod))
            {
                await RefreshDates();
            }
        }

        public double ChartWidth => ChartWidthValue;
        public double ChartHeight => ChartHeightValue;

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

        public Common.Period CurrentPeriod
        {
            get { return (Common.Period)GetValue(CurrentPeriodProperty); }
            set { SetValue(CurrentPeriodProperty, value); }
        }

        public double ForecastLeft
        {
            get { return (double)GetValue(ForecastLeftProperty); }
            set { SetValue(ForecastLeftProperty, value); }
        }

        public double ForecastWidth
        {
            get { return (double)GetValue(ForecastWidthProperty); }
            set { SetValue(ForecastWidthProperty, value); }
        }

        public DateTime ForecastStart
        {
            get { return (DateTime)GetValue(ForecastStartProperty); }
            set { SetValue(ForecastStartProperty, value); }
        }

        public DateTime ForecastEnd
        {
            get { return (DateTime)GetValue(ForecastEndProperty); }
            set { SetValue(ForecastEndProperty, value); }
        }
    }
}
