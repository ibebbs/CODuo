using System;

namespace CODuo.Home
{
    public class GenerationData
    {
        public DateTime At { get; set; }

        public double? EstimatedGeneration { get; set; }

        public double? ActualGeneration { get; set; }

        public double? EstimatedGramsOfCO2PerkWh { get; set; }

        public double? ActualGramsOfCO2PerkWh { get; set; }
    }
}
