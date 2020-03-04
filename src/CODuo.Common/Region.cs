using System;
using System.Collections.Generic;

namespace CODuo.Common
{
    public class Region
    {
        public static readonly Region UnitedKingdon = new Region
        {
            Id = 0,
            Name = "United Kingdon",
            OperatorId = Guid.Parse("72240edd-7500-49bf-ad61-5645d42d0e81"),
            PercentOfNationalEnergyConsumption = 1,
            PercentOfNationalPopulation = 1,
            PercentOfNationalBusinesses = 1,
            PercentOfConsumptionBeingDomestic = 0.3732408609
        };

        public static readonly IEnumerable<Region> AdditionalRegions = new[] { UnitedKingdon };

        public int Id { get; set; }

        public string Name { get; set; }

        public Guid OperatorId { get; set; }

        public string OperatorEmergencyNumber { get; set; }

        public double PercentOfNationalEnergyConsumption { get; set; }

        public double PercentOfNationalPopulation { get; set; }

        public double PercentOfNationalBusinesses { get; set; }

        public double PercentOfConsumptionBeingDomestic { get; set; }
    }
}