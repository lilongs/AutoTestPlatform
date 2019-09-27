using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using canlibCLSNET;

namespace AutoTestPlatform.Module
{
    public class KvaserCommunication
    {
        public int handle = -1;
        private int channel = -1;
        private int readHandle = -1;
        private bool onBus = false;
        private readonly BackgroundWorker dumper;
        private RichTextBox box=null;

        public KvaserCommunication()
        {
            dumper = new BackgroundWorker();
            dumper.DoWork += DumpMessageLoop;
            dumper.WorkerReportsProgress = true;
            dumper.ProgressChanged += new ProgressChangedEventHandler(ProcessMessage);
        }

        /// <summary>
        /// box用于接收显示can读取的信息
        /// </summary>
        /// <param name="box"></param>
        public KvaserCommunication(RichTextBox box)
        {
            dumper = new BackgroundWorker();
            dumper.DoWork += DumpMessageLoop;
            dumper.WorkerReportsProgress = true;
            dumper.ProgressChanged += new ProgressChangedEventHandler(ProcessMessage);
            this.box = box;
        }

        //Initializes Canlib
        public void init()
        {
            Canlib.canInitializeLibrary();
        }

        //Opens the channel selected in the "Channel" input box
        public void openChannel(int channel)
        {
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);

            if (hnd >= 0)
            {
                handle = hnd;
                this.channel = channel;
            }
        }

        //Sets the bitrate
        public void setBitrate(int bitrate)
        {
            Canlib.canStatus status = Canlib.canSetBusParams(handle, bitrate, 0, 0, 0, 0, 0);
        }

        //Goes on bus
        public void busOn()
        {
            Canlib.canStatus status = Canlib.canBusOn(handle);
            if (status == Canlib.canStatus.canOK)
            {
                onBus = true;
                if (!dumper.IsBusy)
                {
                    dumper.RunWorkerAsync();
                }
            }
        }

        //Reads message data from user input and writes a message to the channel

        public void send(int id, int dlc, byte[] data, int flags)
        {
            Canlib.canStatus status = Canlib.canWrite(handle, id, data, dlc, flags);
        }


        public void busOff()
        {
            Canlib.canStatus status = Canlib.canBusOff(handle);
            onBus = false;
        }

        public void closeChannel()
        {
            Canlib.canStatus status = Canlib.canClose(handle);
            handle = -1;
        }

        /*
         * Looks for messages and sends them to the output box. 
         */

        private void DumpMessageLoop(object sender, DoWorkEventArgs e)
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
            readHandle = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);

            status = Canlib.canBusOn(readHandle);

            while (noError && onBus && readHandle >= 0)
            {
                status = Canlib.canReadWait(readHandle, out id, data, out dlc, out flags, out time, 50);

                if (status == Canlib.canStatus.canOK)
                {
                    if ((flags & Canlib.canMSG_ERROR_FRAME) == Canlib.canMSG_ERROR_FRAME)
                    {
                        msg = "***ERROR FRAME RECEIVED***";
                    }
                    else
                    {
                        msg = String.Format("{0}  {1}  {2:x2} {3:x2} {4:x2} {5:x2} {6:x2} {7:x2} {8:x2} {9:x2}   {10}\r",
                                                 id, dlc, data[0], data[1], data[2], data[3], data[4],
                                                 data[5], data[6], data[7], time);
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

        /*
         * Adds the messages to the output box
         */
        private void ProcessMessage(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                string output = (string)e.UserState;
                if(box!=null)
                    AppendText(output);
            }
        }

        private void AppendText(string info)
        {
            box.AppendText(info);
            box.ScrollToCaret();
        }
    }
}
