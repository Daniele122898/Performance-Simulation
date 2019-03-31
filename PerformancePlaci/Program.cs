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
            while (true)
            {
                Console.WriteLine("What do you want to do?\n" +
                                  "1. Do Performance Calculations\n" +
                                  "2. Do Risk Calculations");
                string readChar = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readChar) || 
                    !int.TryParse(readChar, out int num) || 
                    num > 2 || 
                    num < 1 )
                {
                    Console.WriteLine("Please enter 1 or 2!");
                    continue;
                }

                if (num == 1)
                    DoPerformanceCalculations();
                else 
                    DoRiskCalculations();
            }
        }

        private static void DoRiskCalculations()
        {
            (List<string> header, List<Line> lines) = (null, null);

            try
            {
                (header, lines) = CsvParser.Parse("./Data/data.csv");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Hit enter to exit.");
                Console.Read();
                Environment.Exit(-1);
            }

            if (header == null || lines == null)
            {
                Console.WriteLine("CSV file could not be Read");
                Console.WriteLine("Hit enter to exit.");
                Console.Read();
                Environment.Exit(-1);
            }

            Storage.Reset();

            int endIndex = 131;
            // calculate performances
            while (true)
            {
                var performances = Performance.Calc(lines, endIndex);
                if (performances == null) break;
                
                float count = 0;
                
                foreach (var performance in performances)
                    if (performance > 0)
                        count++;

                float risk = count / performances.Count;
                
                Storage.RiskHistory.Add(risk);
                
                endIndex++;
            }
            
            CsvParser.WriteRiskFile("./Data/");
            
            Console.WriteLine("Done.");
        }

        private static void DoPerformanceCalculations()
        {
            (List<string> header, List<Line> lines) = (null, null);

            try
            {
                (header, lines) = CsvParser.Parse("./Data/data.csv");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Hit enter to exit.");
                Console.Read();
                Environment.Exit(-1);
            }

            if (header == null || lines == null)
            {
                Console.WriteLine("CSV file could not be Read");
                Console.WriteLine("Hit enter to exit.");
                Console.Read();
                Environment.Exit(-1);
            }
            
            while (true)
            {
                Storage.Reset();
                Console.WriteLine("How many Indexes do you want to choose each iteration: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int indexCount))
                {
                    Console.WriteLine("You stupid. Enter number pls");
                    continue;
                }

                if (indexCount > header.Count-2)
                {
                    Console.WriteLine("Number must be smaller or equal to total amount of indexes. Not counting the first index!");
                    continue;
                }
    
                int endIndex = 131;
                // calculate performances
                while (true)
                {
                    var performances = Performance.Calc(lines, endIndex);
                    if (performances == null) break;

                    if (endIndex + 1 >= lines.Count) break;
                    
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