using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class Temperature_humidity
    {
        public DateTime now { get; set; }
        public double temperatureValue { get; set; }
        public double humidtyValue { get; set; }
    }
}
