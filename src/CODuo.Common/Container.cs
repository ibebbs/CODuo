using System;
using System.Collections.Generic;

namespace CODuo.Common
{
    public class Container
    {
        public DateTime LastUpdated { get; set; }

        public string Version { get; set; }

        public Static Static { get; set; }

        public IEnumerable<Operator> Operators { get; set; }

        public IEnumerable<Region> Regions { get; set; }

        public IEnumerable<Factor> Factors { get; set; }

        public IEnumerable<Period> Periods { get; set; }
    }
}
