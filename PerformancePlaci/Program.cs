using System;
using System.Collections.Generic;
using PerformancePlaci.Calculations;
using PerformancePlaci.data;
using PerformancePlaci.Parser;

namespace PerformancePlaci
{
    class Program
    {
        static void Main(string[] args)
        {

            (List<string> header, List<Line> lines) = CsvParser.Parse("./Data/data.csv");

            while (true)
            {
                Storage.Reset();
                Console.WriteLine("How many Indexes do you want to choose each iteration: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int indexCount))
                {
                    Console.WriteLine("You stupid. Enter number pls");
                    System.Environment.Exit(-1);
                }
                
    
                int endIndex = 131;
                // calculate performances
                while (true)
                {
                    var performances = Performance.Calc(lines, endIndex);
                    if (performances == null) break;
                    
                    Performance.CalcPerformance(indexCount, endIndex+1, lines, header, performances);
    
                    endIndex += 22;
                }
                
                CsvParser.WriteFile("./Data/", indexCount);
                
                Storage.Save("./Data", indexCount);
                
                Console.WriteLine($"Total Indexed Performance: {Storage.PerformanceIndexed}");
                
                if (Console.ReadLine()?.Equals("exit", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Environment.Exit(0);
                }
                
            }

        }
    }
}