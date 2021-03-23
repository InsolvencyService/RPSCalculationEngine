using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class TraceInfoDate
    {
        [JsonProperty("start")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end")]
        public DateTime EndDate { get; set; }
    }
}
