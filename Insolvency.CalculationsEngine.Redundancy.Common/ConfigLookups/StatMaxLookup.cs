using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class StatMaxLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StatMax { get; set; }

        public static implicit operator List<object>(StatMaxLookup v)
        {
            throw new NotImplementedException();
        }
    }
}