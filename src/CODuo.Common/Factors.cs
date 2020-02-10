using System;

namespace CODuo.Common
{
    public class Factors
    {
        public const double DutchImports = 474;
        public const double FrenchImports = 53;
        public const double IrishImports = 458;
                     
        public const double GasCombinedCycle = 394;
        public const double GasOpenCycle = 651;
                     
        public const double Imports = (DutchImports + FrenchImports + IrishImports) / 3;
        public const double Biomass = 120;
        public const double Coal = 937;
        public const double Hydro = 0;
        public const double Nuclear = 0;
        public const double Oil = 935;
        public const double Gas = (GasCombinedCycle + GasOpenCycle) / 2;
        public const double Other = 300;
        public const double PumpedStorage = 0;
        public const double Solar = 0;
        public const double Wind = 0;

        public static double CO2PerkWh(FuelType fuelType)
        {
            return fuelType switch
            {
                FuelType.Biomass => Biomass,
                FuelType.Coal => Coal,
                FuelType.Gas => Gas,
                FuelType.Hydro => Hydro,
                FuelType.Import => Imports,
                FuelType.Nuclear => Nuclear,
                FuelType.Oil => Oil,
                FuelType.Other => Other,
                FuelType.Solar => Solar,
                FuelType.Wind => Wind,
                _ => throw new ArgumentException($"Unknown FuelType: '{fuelType}'", nameof(fuelType))
            };
        }
    }
}
