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
using AutoTestPlatform.Module;
using canlibCLSNET;
using Kvaser.Kvadblib;

namespace AutoTestPlatform
{
    public partial class frmKvaserDBNodeMessageSend : Form
    {
        public frmKvaserDBNodeMessageSend()
        {
            InitializeComponent();
            //Set up the BackgroundWorker
            transmitter = new BackgroundWorker();
            transmitter.DoWork += SendMessageLoop;
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
            messages = list.Where(x => x.tx_node == "BCM" || x.tx_node == "Gateway").ToList();
            dataGridView2.DataSource = messages;
            
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            initButton.Enabled = !dBCCan.channelOn;
            closeButton.Enabled = dBCCan.channelOn;
            sendMsgButton.Enabled = dBCCan.channelOn;
            startAutoButton.Enabled = dBCCan.channelOn && dBCCan.hasMessage && !autoTransmit;
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

        private void SendMessageLoop(object sender, DoWorkEventArgs e)
        {
            int interval = (int)e.Argument;
            BackgroundWorker worker = sender as BackgroundWorker;

            while (autoTransmit)
            {
                System.Threading.Thread.Sleep(interval);
                worker.ReportProgress(0);
            }
        }

        private void ProcessMessage(object sender, ProgressChangedEventArgs e)
        {
            SendAllMessage();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            autoTransmit = false;
            dBCCan.closeChannel();
            UpdateButtons();
        }
    }
}
