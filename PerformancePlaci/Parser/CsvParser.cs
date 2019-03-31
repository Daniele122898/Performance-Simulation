using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using PerformancePlaci.data;

namespace PerformancePlaci.Parser
{
    public class CsvParser
    {

        public static void WriteFile(string path, int amount)
        {
            string fileName = $"indexedPerformance_{amount}.csv";

            path += fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer))
            {    
                csv.WriteRecords(Storage.IndexedHistory);
            }
        }
        
        public static void WriteRiskFile(string path)
        {
            string fileName = $"riskHistory.csv";

            path += fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer))
            {    
                csv.WriteRecords(Storage.RiskHistory);
            }
        }
        
        public static (List<string> header, List<Line> lines) Parse(string path)
        {
            using (TextReader reader = new StreamReader(path))
            using (var csvReader = new CsvReader(reader))
            {
                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Read();
                csvReader.Configuration.Delimiter = ";";
                // construct my own header
                int i = 0;
                List<string> header = new List<string>();
                while (csvReader.TryGetField<string>(i, out string field))
                {
                    header.Add(field);
                    i++;
                }
                // parse data
                List<Line> lines = new List<Line>();
                while (csvReader.Read())
                {
                    Line line = new Line();
                    // parse date
                    line.Date = csvReader.GetField<DateTime>(0);
                    // parse values
                    int index = 1;
                    while (csvReader.TryGetField<double>(index, out double value))
                    {
                        line.Values.Add(value);
                        index++;
                    }
                    
                    lines.Add(line);
                }

                return (header, lines);
            }
        }
    }
}