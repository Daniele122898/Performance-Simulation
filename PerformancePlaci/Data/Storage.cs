using System.Collections.Generic;

namespace PerformancePlaci.data
{
    public static class Storage
    {
        public static List<Step> PerformanceList = new List<Step> ();
        public static double PerformanceIndexed = 100;
        public static List<double> IndexedHistory { get; set; } = new List<double>();

        static Storage()
        {
            IndexedHistory.Add(100.0);    
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