using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class CurrentElectricValue
    {
        public DateTime now { get; set; }
        public double currentValue { get; set; }
        public double voltageValue { get; set; }
    }
}
