using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class TraceInfo
    {
        public TraceInfo()
        {
            Dates = new List<TraceInfoDate>();
        }

        [JsonProperty("dates ")]
        public IList<TraceInfoDate> Dates { get; set; }
     
    }
}
