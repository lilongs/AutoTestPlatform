using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoTestDLL.Model;
using AutoTestDLL.Module;
using AutoTestPlatform.Module;
using canlibCLSNET;
using Kvaser.Kvadblib;

namespace AutoTestPlatform
{
    public partial class frmKvaserDBNodeMessageRecevied : Form
    {
        public frmKvaserDBNodeMessageRecevied()
        {
            InitializeComponent();
            //Set up the BackgroundWorker
            transmitter = new BackgroundWorker();
            transmitter.DoWork += ReceiveMessageLoop;
            transmitter.WorkerReportsProgress = true;
            transmitter.ProgressChanged += new ProgressChangedEventHandler(ProcessMessage);
        }
       //KvaserDbcCommunication dBCCan = new KvaserDbcCommunication();
        KvaserDbcMessage dBCCan = new KvaserDbcMessage();
        bool autoTransmit = false;

        private readonly BackgroundWorker transmitter;

        List<AutoTestDLL.Model.Message> messages = new List<AutoTestDLL.Model.Message>();
        

        private void loadDbButton_Click(object sender, EventArgs e)
        {
           OpenFileDialog filedialog=new OpenFileDialog();
            filedialog.DefaultExt = ".dbc";

            DialogResult hasResult=filedialog.ShowDialog();

            if (hasResult == DialogResult.OK)
            {
                string dbFile = filedialog.FileName;
                string safeFile = filedialog.SafeFileName;
                
                dBCCan.loadDb(dbFile);
               
                SetupMessagesBox();
                selectBlock.Text = safeFile;
            }
            UpdateButtons();
        }


        private void SetupMessagesBox()
        {
            List<AutoTestDLL.Model.Message> list = dBCCan.LoadMessages();
            messages = list.Where(x => (x.tx_node == "BCM" || x.tx_node == "Gateway") && x.GenMsgSendType=="Cyclic" ).ToList();

            dataGridView2.DataSource = messages;
            
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            initButton.Enabled = !dBCCan.channelOn;
            closeButton.Enabled = dBCCan.channelOn;
            sendMsgButton.Enabled = dBCCan.channelOn;
            startAutoButton.Enabled = dBCCan.channelOn&& !autoTransmit;
            stopTransmitButton.Enabled = autoTransmit;
        }


        private void initButton_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(this.channelBox.Text);
            dBCCan.initChannel(channel);
            UpdateButtons();
        }

        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            SendAllMessage();
            UpdateButtons();
        }

        private void SendAllMessage()
        {
            foreach (AutoTestDLL.Model.Message message in messages)
            {
                List<Signal> signalList = new List<Signal>();
                signalList = dBCCan.LoadSignalsById(message.id);
                dBCCan.sendMsg(signalList);
            }
        }

        private void startAutoButton_Click(object sender, EventArgs e)
        {
            autoTransmit = true;
            int interval;
            bool parsed = Int32.TryParse(intervalBox.Text, out interval);
            if (!transmitter.IsBusy && parsed)
            {
                transmitter.RunWorkerAsync(interval);
                statusText.Text = "Started auto transmit";
            }
            else if (!parsed)
            {
                MessageBox.Show("Interval must be an integer value");
            }
            UpdateButtons();
        }

        private void stopTransmitButton_Click(object sender, EventArgs e)
        {
            autoTransmit = false;
            UpdateButtons();
        }

        List<AutoTestDLL.Model.Message> message_List = new List<AutoTestDLL.Model.Message>();

        private void ReceiveMessageLoop(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Canlib.canStatus status;
            int id;
            byte[] data = new byte[8];
            int dlc;
            int flags;
            long time;
            bool noError = true;
            string msg;

            //Open up a new handle for reading
            int readHandle = Canlib.canOpenChannel(dBCCan.chanHandle, Canlib.canOPEN_ACCEPT_VIRTUAL);

            status = Canlib.canBusOn(readHandle);

            while (noError &&  readHandle >= 0)
            {
                //status = Canlib.canReadWait(readHandle, out id, data, out dlc, out flags, out time, 50);
                AutoTestDLL.Model.Message message = dBCCan.ReadMessage(readHandle,out status);
                if (status == Canlib.canStatus.canOK)
                {
                    if ((message.flags & Canlib.canMSG_ERROR_FRAME) == Canlib.canMSG_ERROR_FRAME)
                    {
                        msg = "***ERROR FRAME RECEIVED***";
                    }

                    else
                    {
                        msg = String.Format("{0}  {1}  {2:x2} {3:x2} {4:x2} {5:x2} {6:x2} {7:x2} {8:x2} {9:x2}   {10}\r",
                                                 message.id, message.dlc, message.data[0], message.data[1], message.data[2], message.data[3], message.data[4],
                                                 message.data[5], message.data[6], message.data[7], message.time);
                    }
                    //Sends the message to the ProcessMessage method
                    worker.ReportProgress(0, msg);
                }
                else if (status != Canlib.canStatus.canERR_NOMSG)
                {
                    //Sends the error status to the ProcessMessage method and breaks the loop
                    worker.ReportProgress(100, status);
                    noError = false;
                }
            }
            Canlib.canBusOff(readHandle);
        }

        private void ProcessMessage(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage==0)
            {
                string message = (String)e.UserState;
                if (message != null)
                {
                    outputBox.AppendText(message);
                    outputBox.ScrollToCaret();
                }
            }
            
        }

        

        private void closeButton_Click(object sender, EventArgs e)
        {
            autoTransmit = false;
            dBCCan.closeChannel();
            UpdateButtons();
        }
    }
}
