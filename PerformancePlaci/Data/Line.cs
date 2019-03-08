using System;
using System.Collections.Generic;

namespace PerformancePlaci.data
{
    public class Line
    {
        public DateTime Date { get; set; }
        public List<float> Values { get; set; } = new List<float>();
    }
}