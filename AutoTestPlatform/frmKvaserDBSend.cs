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
        DBCCanMessage dBCCan = new DBCCanMessage();
        Kvadblib.Hnd dh;
        Kvadblib.MessageHnd msgHandle;

        int chanHandle;
        int msgId;
        int msgFlags;

        bool hasDb = false;
        bool channelOn = false;
        bool hasMessage = false;
        bool autoTransmit = false;

        List<Signal> list = new List<Signal>();
        private readonly BackgroundWorker transmitter;

        private void loadDbButton_Click(object sender, EventArgs e)
        {
           OpenFileDialog filedialog=new OpenFileDialog();
            filedialog.DefaultExt = ".dbc";

            DialogResult hasResult=filedialog.ShowDialog();

            if (hasResult == DialogResult.OK)
            {
                string dbFile = filedialog.FileName;
                string safeFile = filedialog.SafeFileName;
                
                bool status = dBCCan.LoadDB(dbFile, out dh);

                if (status)
                {
                    hasDb = true;
                    SetupMessagesBox();
                    selectBlock.Text = safeFile;
                }
            }
            UpdateButtons();
        }


        private void SetupMessagesBox()
        {
            Kvadblib.MessageHnd mh;
            Kvadblib.Status status;
            List<AutoTestDLL.Model.Message> list = new List<AutoTestDLL.Model.Message>();
            boxItems.DataSource = null;

            status = Kvadblib.GetFirstMsg(dh, out mh);
            while (status == Kvadblib.Status.OK)
            {
                string name;
                int id;
                Kvadblib.MESSAGE flags;
                status = Kvadblib.GetMsgName(mh, out name);
                status = Kvadblib.GetMsgId(mh, out id, out flags);
                AutoTestDLL.Model.Message m = new AutoTestDLL.Model.Message();
                m.id = id;
                m.name = name;
                list.Add(m);

                status = Kvadblib.GetNextMsg(dh, out mh);
            }
            boxItems.DataSource = list;
            boxItems.DisplayMember = "name";
            boxItems.ValueMember = "id";
            if (status != Kvadblib.Status.Err_NoMsg)
            {
                dBCCan.CheckDBStatus("Reading message", status);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            loadMsgButton.Enabled = hasDb;
            initButton.Enabled = !channelOn;
            closeButton.Enabled = channelOn;
            sendMsgButton.Enabled = channelOn && hasMessage;
            startAutoButton.Enabled = channelOn && hasMessage && !autoTransmit;
            stopTransmitButton.Enabled = autoTransmit;
        }

        private void loadMsgButton_Click(object sender, EventArgs e)
        {
            Kvadblib.MessageHnd mh;
            int id = Convert.ToInt32(boxItems.SelectedValue==null?0: boxItems.SelectedValue);
            Kvadblib.Status status = Kvadblib.GetMsgById(dh, id, out mh);
            this.dataGridView1.DataSource = null;
            if (status == Kvadblib.Status.OK)
            {
                Kvadblib.MESSAGE f;
                msgHandle = mh;
                Kvadblib.GetMsgId(mh, out msgId, out f);
                msgId = ((id & -2147483648) == 0) ? id : id ^ -2147483648;
                msgFlags = ((id & -2147483648) == 0) ? 0 : Canlib.canMSG_EXT;
                msgIdLabel.Text = "Message id: " + msgId;
                hasMessage = true;
                list = dBCCan.LoadSignals(msgHandle);
                this.dataGridView1.DataSource = list;
            }
            dBCCan.CheckDBStatus("Loading message", status);
            UpdateButtons();
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            int channel = Int32.Parse(channelBox.Text);
            Canlib.canStatus status;

            Canlib.canInitializeLibrary();
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);
            if (hnd >= 0)
            {
                chanHandle = hnd;
                status = Canlib.canSetBusParams(chanHandle, Canlib.canBITRATE_250K, 0, 0, 0, 0, 0);
                status = Canlib.canBusOn(chanHandle);
                dBCCan.CheckCANStatus("On bus", status);
                if (status == Canlib.canStatus.canOK)
                {
                    channelOn = true;
                }
            }
            else
            {
                dBCCan.CheckCANStatus("Opening channel", (Canlib.canStatus)hnd);
            }
            UpdateButtons();
        }

        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            if (hasMessage && channelOn)
            {
                dBCCan.SendMessage(list,msgHandle,chanHandle,msgId,msgFlags);
            }
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
            dBCCan.SendMessage(list,msgHandle,chanHandle,msgId,msgFlags);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            autoTransmit = false;
            channelOn = false;
            Canlib.canBusOff(chanHandle);
            Canlib.canClose(chanHandle);
            UpdateButtons();
        }
    }
}
