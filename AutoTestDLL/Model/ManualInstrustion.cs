using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class ManualInstruction
    {
        public string type { get; set; }
        public string id { get; set; }
        public int dlc { get; set; }
        public string data { get; set; }
        public int cycletime { get; set; }
        public bool enable { get; set; }
        public int cyclecount { get; set; }

        public ManualInstruction()
        {
            this.enable = false;
        }
    }
}
