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
        public string sensorName { get; set; }
        public string portName { get; set; }
        public int baudrate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public Handshake handshake { get; set; }
    }
}
