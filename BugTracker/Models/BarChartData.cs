using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class BarChartData
    {
        public List<int> Values { get; set; }
        public List<string> Labels { get; set; }

        public BarChartData()
        {
            Values = new List<int>();
            Labels = new List<string>();
        }
    }
}