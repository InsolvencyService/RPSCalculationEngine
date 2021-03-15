using Newtonsoft.Json;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public static class TraceInfoSerializer
    {
        public static string GetTraceDetails()
        {
            return JsonConvert.SerializeObject(new TraceInfo());
        }

        public static string GetTraceDetails(TraceInfo traceInfo)
        {
            return JsonConvert.SerializeObject(traceInfo);
        }
    }

    public class TraceInfo
    {
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
