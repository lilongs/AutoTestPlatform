using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class TestStep
    {
        public string typename { get; set; }
        public string stepname { get; set; }
        public string modename { get; set; }
        public string voltage { get; set; }
        public string cycletime { get; set; }
        public int repeat { get; set; }
    }
}
