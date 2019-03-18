using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PerformancePlaci.data
{
    public static class Storage
    {
        static private JsonSerializer _jSerializer = new JsonSerializer();
        
        public static List<Step> PerformanceList = new List<Step> ();
        public static double PerformanceIndexed = 100;
        public static List<double> IndexedHistory { get; set; } = new List<double>();

        static Storage()
        {
            _jSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            _jSerializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public static void Reset()
        {
            PerformanceIndexed = 100;
            PerformanceList.Clear();
            IndexedHistory.Clear();
            IndexedHistory.Add(100.0);    
        }

        public static void Save(string path, int amount)
        {
            using (StreamWriter sw = File.CreateText($"{path}/performanceHistory_{amount}.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    _jSerializer.Serialize(writer, PerformanceList);
                }
            }
        }
    }

    public class Step
    {
        public List<PerfomanceResult> PerfomanceResults { get; set; } = new List<PerfomanceResult>();
    }

    public class PerfomanceResult
    {
        public string Index { get; set; }
        public double Performance { get; set; }
    }
}