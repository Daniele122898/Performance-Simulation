using System;
using System.Collections.Generic;
using System.Linq;
using PerformancePlaci.data;

namespace PerformancePlaci.Calculations
{
    public static class Performance
    {
        public static List<double> Calc(List<Line> lines, int endIndex)
        {
            // if there aren't enough values to calculate
            if (endIndex > lines.Count) return null;
            // calculate 1, 3, 6 months for each value in line
            List<double> sumOfPerformances = new List<double>();

            int n = lines[0].Values.Count;

            for (int i = 2; i < n; i++)
            {
                // get all the start values
                double t1 = lines[endIndex-22].Values[i];
                double t2 = lines[endIndex - 66].Values[i];
                double t3 = lines[endIndex - 131].Values[i];
                // test if any of them are 0s or 1. If so, discard the performance bcs
                // otherwise it falsifies the entire result
                if (t1 <= 1.1 || t2 <= 1.1 || t3 <= 1.1)
                {
                    sumOfPerformances.Add(0);
                    continue;
                }
                // if not then calculate the performance normally
                double m1 = (lines[endIndex].Values[i]/t1)-1;
                double m2 = (lines[endIndex].Values[i]/t2)-1;
                double m3 = (lines[endIndex].Values[i]/t3)-1;

                sumOfPerformances.Add(m1+m2+m3);
                
            }
            
            return sumOfPerformances;
        }

        private class PerfOrdering
        {
            public double Performance { get; set; }
            public string Header { get; set; }
            public int Index { get; set; }
            public double NewPerformance { get; set; }
        }

        public static void CalcPerformance(int amount, int startIndex, List<Line> lines, List<string> header, List<double> performances)
        {
            int duration = 21;
            if (startIndex + 21 > lines.Count) duration = lines.Count - startIndex-1;

            List<PerfOrdering> ordering = new List<PerfOrdering>();
            for (int i = 0; i < performances.Count; i++)
            {
                ordering.Add(new PerfOrdering()
                {
                    Performance = performances[i],
                    Header = header[i+2],
                    Index = i+1
                });
            }
            
            var sorted = ordering.OrderByDescending(x => x.Performance).Take(amount).ToList();
            
            double totalPerformance = 0;
            
            Step step = new Step();
            
            foreach (var perfOrdering in sorted)
            {
                if (lines[startIndex].Values[perfOrdering.Index] <= 1.1)
                {
                    perfOrdering.NewPerformance = 0;
                }
                else
                {
                    perfOrdering.NewPerformance = lines[startIndex + duration].Values[perfOrdering.Index] /
                                                  lines[startIndex].Values[perfOrdering.Index]-1;
                }
                
                totalPerformance += perfOrdering.NewPerformance;
                step.PerfomanceResults.Add(new PerfomanceResult()
                {
                    Index = perfOrdering.Header,
                    Performance = perfOrdering.NewPerformance
                });

            }
            
            Storage.PerformanceList.Add(step);

            totalPerformance = totalPerformance / amount;
                
            Storage.PerformanceIndexed = Storage.PerformanceIndexed * (1 + totalPerformance);
            
            Storage.IndexedHistory.Add(Storage.PerformanceIndexed);
        }
        
    }
}