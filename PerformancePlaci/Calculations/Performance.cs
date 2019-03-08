using System.Collections.Generic;
using PerformancePlaci.data;

namespace PerformancePlaci.Calculations
{
    public static class Performance
    {
        public static List<float> Calc(List<Line> lines, int endIndex)
        {
            // if there aren't enough values to calculate
            if (endIndex > lines.Count) return null;
            // calculate 1, 3, 6 months for each value in line
            List<float> sumOfPerformances = new List<float>();

            int n = lines[0].Values.Count;

            for (int i = 1; i < n; i++)
            {
                float m1 = (lines[endIndex].Values[i]/lines[endIndex-22].Values[i])-1;
                float m2 = (lines[endIndex].Values[i]/lines[endIndex-66].Values[i])-1;
                float m3 = (lines[endIndex].Values[i]/lines[endIndex-132].Values[i])-1;
                sumOfPerformances.Add(m1+m2+m3);
            }
            
            return sumOfPerformances;
        }
    }
}