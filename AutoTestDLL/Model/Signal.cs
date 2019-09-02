using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class Signal
    {
        public string name { get; private set; }
        public double max { get; private set; }
        public double min { get; private set; }
        public double scale { get; private set; }
        private double val;
        public double Value
        {
            get { return val; }
            set
            {
                if (value >= min && value <= max)
                {
                    val = value;
                }
            }
        }

        public Signal(string name, double max, double min, double val, double scale)
        {
            this.name = name;
            this.max = max;
            this.min = min;
            this.val = val;
            this.scale = scale;
        }
    }
}
