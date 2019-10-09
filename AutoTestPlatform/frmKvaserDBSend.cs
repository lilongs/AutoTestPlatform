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
    public partial class frmKvaserDBSend : Form
    {
        public frmKvaserDBSend()
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

        List<Signal> signalList = new List<Signal>();

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
            boxItems.DataSource = list;
            boxItems.DisplayMember = "name";
            boxItems.ValueMember = "id";
            
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            loadMsgButton.Enabled = dBCCan.hasDb;
            initButton.Enabled = !dBCCan.channelOn;
            closeButton.Enabled = dBCCan.channelOn;
            sendMsgButton.Enabled = dBCCan.channelOn && dBCCan.hasMessage;
            startAutoButton.Enabled = dBCCan.channelOn && dBCCan.hasMessage && !autoTransmit;
            stopTransmitButton.Enabled = autoTransmit;
        }

        private void loadMsgButton_Click(object sender, EventArgs e)
        {
            int messageID = Convert.ToInt32(this.boxItems.SelectedValue);
            this.dataGridView1.DataSource = null;
            signalList = dBCCan.LoadSignalsById(messageID);
            this.dataGridView1.DataSource = signalList;
            UpdateButtons();
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(this.channelBox.Text);
            dBCCan.initChannel(channel, "250 kb/s");
            UpdateButtons();
        }

        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            dBCCan.sendMsg(8,signalList);
            UpdateButtons();
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
            dBCCan.sendMsg(8,signalList);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            autoTransmit = false;
            dBCCan.closeChannel();
            UpdateButtons();
        }
    }
}
