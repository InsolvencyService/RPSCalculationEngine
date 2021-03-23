using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions
{
    public static class TraceInfoSerializer
    {
        private static JsonSerializerSettings jsonSettings { get; set; } = new JsonSerializerSettings
        {
            DateFormatString = "dd/MM/yyy hh:mm:ss",
            Formatting = Formatting.None
            
        };
        public static string ConvertToJson()
        {
            return JsonConvert.SerializeObject(new TraceInfo());
        }

        public static async Task<string> ConvertToJson(this TraceInfo traceInfo)
        {
            return await Task.FromResult(JsonConvert.SerializeObject(traceInfo, jsonSettings));
        }
    }
}
