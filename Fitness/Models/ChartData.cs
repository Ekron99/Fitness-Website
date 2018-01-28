using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class ChartData
    {
        public List<string> labels { get; set; }
        public List<decimal> data { get; set; }
        public List<double> times { get; set; }
    }
}