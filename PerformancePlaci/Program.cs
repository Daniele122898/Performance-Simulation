using System;
using System.Collections.Generic;
using PerformancePlaci.data;
using PerformancePlaci.Parser;

namespace PerformancePlaci
{
    class Program
    {
        static void Main(string[] args)
        {
            (List<string> header, List<Line> lines) = CsvParser.Parse("./Data/data.csv");
            
        }
    }
}