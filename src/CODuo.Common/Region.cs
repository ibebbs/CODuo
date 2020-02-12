using System.Collections.Generic;

namespace CODuo.Common
{
    public class Region
    {
        public static readonly Region UnitedKingdon = new Region
        {
            Id = 0,
            Name = "United Kingdon",
            Operator = "Various"
        };

        public static readonly IEnumerable<Region> AdditionalRegions = new[] { UnitedKingdon };

        public int Id { get; set; }

        public string Name { get; set; }

        public string Operator { get; set; }

        public string OperatorLogoUri { get; set; }

        public double PercentOfNationalEnergyConsumption { get; set; }

        public double PercentOfNationalPopulation { get; set; }

        public double PercentOfConsumptionBeingDomestic { get; set; }
    }
}