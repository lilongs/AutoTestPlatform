using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class Message
    {
        public int id { get; set; }
        public string name { get; set; }
        public byte[] data { get; set; }
        public int dlc { get; set; }
        public int flags { get; set; }
        public long time { get; set; }
        public string tx_node { get; set; }
        public string GenMsgSendType { get; set; }
        public int GenMsgCycleTime { get; set; }

        public Message()
        {

        }

        public Message(int id, byte[] data, int dlc, int flags, long time)
        {
            this.id = id;
            this.data = data;
            this.dlc = dlc;
            this.flags = flags;
            this.time = time;
        }
    }
}
