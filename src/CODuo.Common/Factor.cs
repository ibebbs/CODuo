using System;
using System.Collections.Generic;
using System.Linq;

namespace CODuo.Common
{
    public class Factor
    {
        public static readonly IEnumerable<Factor> Values = Enum
            .GetValues(typeof(FuelType))
            .OfType<FuelType>()
            .Select(fuelType => new Factor { FuelType = fuelType, CO2PerkWh = Factors.CO2PerkWh(fuelType) })
            .ToArray();

        public FuelType FuelType { get; set; }

        public double CO2PerkWh { get; set; }
    }
}
