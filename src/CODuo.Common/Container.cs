using System.Collections.Generic;

namespace CODuo.Common
{
    public class Container
    {
        public IEnumerable<Region> Regions { get; set; }

        public IEnumerable<Factor> Factors { get; set; }

        public IEnumerable<Period> Periods { get; set; }
    }
}
