using System.Collections.Generic;

namespace CODuo.Common
{
    public class Generation
    {
        public double? TotalMW { get; set; }

        public double? GramsOfCO2PerkWh { get; set; }

        public IEnumerable<FuelTypeGeneration> ByFuelType { get; set; }
    }
}