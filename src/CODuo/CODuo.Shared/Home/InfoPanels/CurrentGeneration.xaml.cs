using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CODuo.Home.InfoPanels
{
    public sealed partial class CurrentGeneration : UserControl
    {
        private static readonly IDictionary<string, double> DefaultComposition = Enum.GetValues(typeof(Common.FuelType)).OfType<Common.FuelType>().ToDictionary(fuelType => fuelType.ToString(), _ => 0.0);

        public static readonly DependencyProperty CompositionProperty = DependencyProperty.Register("Composition", typeof(IDictionary<string, double>), typeof(CurrentGeneration), new PropertyMetadata(DefaultComposition, CompositionPropertyChanged));

        private static void CompositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CurrentGeneration c)
            {
                c.CompositionChanged();
            }
        }

        public static readonly DependencyProperty CoalStartAngleProperty = DependencyProperty.Register(nameof(CoalStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(0.0));
        public static readonly DependencyProperty OilStartAngleProperty = DependencyProperty.Register(nameof(OilStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty GasStartAngleProperty = DependencyProperty.Register(nameof(GasStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(20.0));
        public static readonly DependencyProperty ImportsStartAngleProperty = DependencyProperty.Register(nameof(ImportsStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(30.0));
        public static readonly DependencyProperty OtherStartAngleProperty = DependencyProperty.Register(nameof(OtherStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(40.0));
        public static readonly DependencyProperty BioMassStartAngleProperty = DependencyProperty.Register(nameof(BioMassStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(50.0));
        public static readonly DependencyProperty NuclearStartAngleProperty = DependencyProperty.Register(nameof(NuclearStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(60.0));
        public static readonly DependencyProperty HydroStartAngleProperty = DependencyProperty.Register(nameof(HydroStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(70.0));
        public static readonly DependencyProperty SolarStartAngleProperty = DependencyProperty.Register(nameof(SolarStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(80.0));
        public static readonly DependencyProperty WindStartAngleProperty = DependencyProperty.Register(nameof(WindStartAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(90.0));

        public static readonly DependencyProperty CoalAngleProperty = DependencyProperty.Register(nameof(CoalAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty OilAngleProperty = DependencyProperty.Register(nameof(OilAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty GasAngleProperty = DependencyProperty.Register(nameof(GasAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty ImportsAngleProperty = DependencyProperty.Register(nameof(ImportsAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty OtherAngleProperty = DependencyProperty.Register(nameof(OtherAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty BioMassAngleProperty = DependencyProperty.Register(nameof(BioMassAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty NuclearAngleProperty = DependencyProperty.Register(nameof(NuclearAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty HydroAngleProperty = DependencyProperty.Register(nameof(HydroAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty SolarAngleProperty = DependencyProperty.Register(nameof(SolarAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(10.0));
        public static readonly DependencyProperty WindAngleProperty = DependencyProperty.Register(nameof(WindAngle), typeof(double), typeof(CurrentGeneration), new PropertyMetadata(270.0));

        public CurrentGeneration()
        {
            this.InitializeComponent();
        }

        private void SetFuelAngle(Common.FuelType fuelType, double startAngle, double angle)
        {
            switch (fuelType)
            {
                case Common.FuelType.Coal:
                    CoalStartAngle = startAngle;
                    CoalAngle = angle;
                    break;
                case Common.FuelType.Oil:
                    OilStartAngle = startAngle;
                    OilAngle = angle;
                    break;
                case Common.FuelType.Gas:
                    GasStartAngle = startAngle;
                    GasAngle = angle;
                    break;
                case Common.FuelType.Import:
                    ImportsStartAngle = startAngle;
                    ImportsAngle = angle;
                    break;
                case Common.FuelType.Other:
                    OtherStartAngle = startAngle;
                    OtherAngle = angle;
                    break;
                case Common.FuelType.Biomass:
                    BioMassStartAngle = startAngle;
                    BioMassAngle = angle;
                    break;
                case Common.FuelType.Nuclear:
                    NuclearStartAngle = startAngle;
                    NuclearAngle = angle;
                    break;
                case Common.FuelType.Solar:
                    SolarStartAngle = startAngle;
                    SolarAngle = angle;
                    break;
                case Common.FuelType.Hydro:
                    HydroStartAngle = startAngle;
                    HydroAngle = angle;
                    break;
                case Common.FuelType.Wind:
                    WindStartAngle = startAngle;
                    WindAngle = 360 - startAngle;
                    break;
            }
        }

        private void CompositionChanged()
        {
            var positions = Enum
                .GetValues(typeof(Common.FuelType))
                .OfType<Common.FuelType>()
                .Join(Composition, fuelType => fuelType.ToString(), kvp => kvp.Key, (fuelType, kvp) => (FuelType: fuelType, Angle: kvp.Value * 360))
                .Aggregate(
                    (Positions: Enumerable.Empty<(Common.FuelType, double, double)>(), StartAngle: 0.0),
                    (seed, tuple) => (Positions: seed.Positions.Concat(new[] { (tuple.FuelType, seed.StartAngle, tuple.Angle) }).ToArray(), StartAngle: seed.StartAngle + tuple.Angle))
                .Positions;

            foreach (var item in positions)
            {
                SetFuelAngle(item.Item1, item.Item2, item.Item3);
            }

            //_ = this.Dispatcher.RunAsync(
            //    CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        foreach (var item in positions)
            //        {
            //            SetFuelAngle(item.Item1, item.Item2, item.Item3);
            //        }
            //    });
        }

        public IDictionary<string, double> Composition
        {
            get { return (IDictionary<string, double>)GetValue(CompositionProperty); }
            set { SetValue(CompositionProperty, value); }
        }

        public double CoalStartAngle
        {
            get { return (double)GetValue(CoalStartAngleProperty); }
            set { SetValue(CoalStartAngleProperty, value); }
        }

        public double OilStartAngle
        {
            get { return (double)GetValue(OilStartAngleProperty); }
            set { SetValue(OilStartAngleProperty, value); }
        }

        public double GasStartAngle
        {
            get { return (double)GetValue(GasStartAngleProperty); }
            set { SetValue(GasStartAngleProperty, value); }
        }

        public double ImportsStartAngle
        {
            get { return (double)GetValue(ImportsStartAngleProperty); }
            set { SetValue(ImportsStartAngleProperty, value); }
        }

        public double OtherStartAngle
        {
            get { return (double)GetValue(OtherStartAngleProperty); }
            set { SetValue(OtherStartAngleProperty, value); }
        }

        public double BioMassStartAngle
        {
            get { return (double)GetValue(BioMassStartAngleProperty); }
            set { SetValue(BioMassStartAngleProperty, value); }
        }

        public double NuclearStartAngle
        {
            get { return (double)GetValue(NuclearStartAngleProperty); }
            set { SetValue(NuclearStartAngleProperty, value); }
        }

        public double HydroStartAngle
        {
            get { return (double)GetValue(HydroStartAngleProperty); }
            set { SetValue(HydroStartAngleProperty, value); }
        }

        public double SolarStartAngle
        {
            get { return (double)GetValue(SolarStartAngleProperty); }
            set { SetValue(SolarStartAngleProperty, value); }
        }

        public double WindStartAngle
        {
            get { return (double)GetValue(WindStartAngleProperty); }
            set { SetValue(WindStartAngleProperty, value); }
        }

        public double CoalAngle
        {
            get { return (double)GetValue(CoalAngleProperty); }
            set { SetValue(CoalAngleProperty, value); }
        }

        public double OilAngle
        {
            get { return (double)GetValue(OilAngleProperty); }
            set { SetValue(OilAngleProperty, value); }
        }

        public double GasAngle
        {
            get { return (double)GetValue(GasAngleProperty); }
            set { SetValue(GasAngleProperty, value); }
        }

        public double ImportsAngle
        {
            get { return (double)GetValue(ImportsAngleProperty); }
            set { SetValue(ImportsAngleProperty, value); }
        }

        public double OtherAngle
        {
            get { return (double)GetValue(OtherAngleProperty); }
            set { SetValue(OtherAngleProperty, value); }
        }

        public double BioMassAngle
        {
            get { return (double)GetValue(BioMassAngleProperty); }
            set { SetValue(BioMassAngleProperty, value); }
        }

        public double NuclearAngle
        {
            get { return (double)GetValue(NuclearAngleProperty); }
            set { SetValue(NuclearAngleProperty, value); }
        }

        public double HydroAngle
        {
            get { return (double)GetValue(HydroAngleProperty); }
            set { SetValue(HydroAngleProperty, value); }
        }

        public double SolarAngle
        {
            get { return (double)GetValue(SolarAngleProperty); }
            set { SetValue(SolarAngleProperty, value); }
        }

        public double WindAngle
        {
            get { return (double)GetValue(WindAngleProperty); }
            set { SetValue(WindAngleProperty, value); }
        }
    }
}
