using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class TempSensorConfiguration
    {
        public string SensorName { get; set; }
        public string CommunicationType { get; set; }
        public string Value { get; set; }
    }
}
