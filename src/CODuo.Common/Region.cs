using System;
using System.Collections.Generic;

namespace CODuo.Common
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid OperatorId { get; set; }

        public string OperatorEmergencyNumber { get; set; }

        public double PercentOfNationalEnergyConsumption { get; set; }

        public double PercentOfNationalPopulation { get; set; }

        public double PercentOfNationalBusinesses { get; set; }

        public double PercentOfConsumptionBeingDomestic { get; set; }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime? Dawn { get; set; }

        public DateTime? SunRise { get; set; }

        public DateTime? SunSet { get; set; }

        public DateTime? Dusk { get; set; }
    }
}