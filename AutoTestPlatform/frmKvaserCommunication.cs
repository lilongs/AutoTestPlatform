using AutoTestPlatform.Module;
using canlibCLSNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform
{
    public partial class frmKvaserCommunication : Form
    {
        KvaserCommunication can = null;
        public frmKvaserCommunication()
        {
            InitializeComponent();
            can = new KvaserCommunication(this.outputBox);
        }
        
        private void btnInit_Click(object sender, EventArgs e)
        {
            can.init();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(txtChannel.Text);
            can.openChannel(channel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            can.closeChannel();
        }

        private void btnSetBit_Click(object sender, EventArgs e)
        {
            int[] bitrates = new int[4] { Canlib.canBITRATE_125K, Canlib.canBITRATE_250K,
                                            Canlib.canBITRATE_500K, Canlib.BAUD_1M};
            int bitrate = bitrates[combBit.SelectedIndex];
            can.setBitrate(bitrate);
        }

        private void btnBusOn_Click(object sender, EventArgs e)
        {
            can.busOn();
        }

        private void btnBusOff_Click(object sender, EventArgs e)
        {
            can.busOff();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            int dlc = Convert.ToInt32(txtDlc.Text);
            byte[] data = new byte[8];
            TextBox[] textBoxes = {dataBox0, dataBox1, dataBox2, dataBox3, dataBox4,
                                      dataBox5, dataBox6, dataBox7};
            for (int i = 0; i < 8; i++)
            {
                data[i] = textBoxes[i].Text == "" ? (byte)0 : Convert.ToByte(textBoxes[i].Text);
            }

            CheckBox[] boxes = {RTRBox, STDBox, EXTBox, WakeUpBox, NERRBox, errorBox,
                                   TXACKBox, TXRQBox, delayBox, BRSBox, ESIBox};
            int flags = 0;
            foreach (CheckBox box in boxes)
            {
                if (box.Checked)
                {
                    flags += Convert.ToInt32(box.Tag);
                }
            }
           
            can.send(id, dlc, data, flags);          
            
        }        
    }
}
