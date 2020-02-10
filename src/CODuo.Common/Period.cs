using System;
using System.Collections.Generic;

namespace CODuo.Common
{
    public class Period
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<RegionGeneration> Regions { get; set; }
    }
}
