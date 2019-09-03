using AutoTestDLL.Model;
using canlibCLSNET;
using Kvaser.Kvadblib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestPlatform.Module
{
    public class DBCCanMessage
    {

        public bool LoadDB(string filename,out Kvadblib.Hnd dh)
        {
           
            Kvadblib.Hnd hnd = new Kvadblib.Hnd();
            Kvadblib.Status status;

            Kvadblib.Open(out hnd);

            status = Kvadblib.ReadFile(hnd, filename);

            if (status == Kvadblib.Status.OK)
            {
                dh = hnd;
            }
            else
            {
                dh = hnd;
            }

            return status == Kvadblib.Status.OK;
        }

        public bool InitChanel(int channel,out int chanHandle)
        {
            bool channelOn = false;
            Canlib.canStatus status;

            Canlib.canInitializeLibrary();
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);
            if (hnd >= 0)
            {
                chanHandle = hnd;
                status = Canlib.canSetBusParams(chanHandle, Canlib.canBITRATE_250K, 0, 0, 0, 0, 0);
                status = Canlib.canBusOn(chanHandle);
                if (status == Canlib.canStatus.canOK)
                {
                    channelOn = true;
                }
            }
            else
            {
                channelOn = false;
                chanHandle = hnd;
            }
            return channelOn;
        }

        public List<Message> LoadMessages(Kvadblib.Hnd dh)
        {
            Kvadblib.MessageHnd mh;
            Kvadblib.Status status;
            List<Message> list = new List<Message>();

            status = Kvadblib.GetFirstMsg(dh, out mh);
            while (status == Kvadblib.Status.OK)
            {
                string name;
                int id;
                Kvadblib.MESSAGE flags;
                status = Kvadblib.GetMsgName(mh, out name);
                status = Kvadblib.GetMsgId(mh, out id, out flags);
                Message message = new Message();
                message.id = id;
                message.name = name;
                list.Add(message);

                status = Kvadblib.GetNextMsg(dh, out mh);
            }
            if (status != Kvadblib.Status.Err_NoMsg)
            {
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<Signal> LoadSignals(Kvadblib.MessageHnd msgHandle)
        {
            int row = 0;
            Kvadblib.SignalHnd sh;
            Kvadblib.Status status = Kvadblib.GetFirstSignal(msgHandle, out sh);

            List<Signal> signals = new List<Signal>();
            while (status == Kvadblib.Status.OK)
            {
                string name;
                string unit;
                double min, max, scale, offset;

                //Construct the text for the label
                status = Kvadblib.GetSignalName(sh, out name);
                status = Kvadblib.GetSignalUnit(sh, out unit);
                status = Kvadblib.GetSignalValueLimits(sh, out min, out max);
                status = Kvadblib.GetSignalValueScaling(sh, out scale, out offset);
                Signal s = new Signal(name, max, min, min, scale);
                signals.Add(s);

                status = Kvadblib.GetNextSignal(msgHandle, out sh);
                row++;
            }
            if (status == Kvadblib.Status.Err_NoSignal)
            {
                return signals;
            }
            else
            {
                return signals;
            }
        }

        public void SendMessage(List<Signal> signals, Kvadblib.MessageHnd msgHandle,int chanHandle,int msgId,int msgFlags)
        {
            byte[] data = new byte[8];

            Kvadblib.Status status = Kvadblib.Status.OK;
            Kvadblib.SignalHnd sh;
            bool error = false;

            foreach (Signal s in signals)
            {
                double min, max;
                status = Kvadblib.GetSignalByName(msgHandle, s.name, out sh);

                if (status != Kvadblib.Status.OK)
                {
                    error = true;
                    break;
                }

                Kvadblib.GetSignalValueLimits(sh, out min, out max);

                status = Kvadblib.StoreSignalValuePhys(sh, data, 8, s.Value);

                if (status != Kvadblib.Status.OK || s.Value < min || s.Value > max)
                {
                    error = true;
                    break;
                }
            }

            if (!error)
            {
                Canlib.canWriteWait(chanHandle, msgId, data, 8, msgFlags, 50);
            }
            
        }

        public KeyValuePair<long, double> ReadMessage(Kvadblib.MessageHnd msgHandle, int chanHandle, int msgId,out Message message)
        {
            Canlib.canStatus status;
            int id;
            byte[] data = new byte[8];
            int dlc;
            int flags;
            long time;
            bool noError = true;
            message = null;
            KeyValuePair<long, double> result = new KeyValuePair<long, double>();

            status = Canlib.canReadWait(chanHandle, out id, data, out dlc, out flags, out time, 50);
            
            while (noError && chanHandle >= 0)
            {
                status = Canlib.canReadWait(chanHandle, out id, data, out dlc, out flags, out time, 50);

                if (status == Canlib.canStatus.canOK)
                {
                    if (id == msgId)
                    {
                        Message m = new Message(id, data, dlc, flags, time);
                        message = m;
                        result = ProcessMessage(msgHandle,m);
                    }
                }
                else if (status != Canlib.canStatus.canERR_NOMSG)
                {
                    noError = false;
                }
            }
            Canlib.canBusOff(chanHandle);
            return result;
        }

        private KeyValuePair<long, double> ProcessMessage(Kvadblib.MessageHnd msgHandle,Message m)
        {
            KeyValuePair<long, double> result=new KeyValuePair<long, double>();
            Kvadblib.SignalHnd sh;
            Kvadblib.Status status = Kvadblib.GetFirstSignal(msgHandle, out sh);
            int i = 0;
            while (status == Kvadblib.Status.OK)
            {
                double value;
                status = Kvadblib.GetSignalValueFloat(sh, out value, m.data, m.dlc);
                result=new KeyValuePair<long, double>(m.time, value);
                i++;
                status = Kvadblib.GetNextSignal(msgHandle, out sh);
            }
            return result;
        }

        public string CheckCANStatus(String action, Canlib.canStatus status)
        {
            String errorText = "";
            if (status != Canlib.canStatus.canOK)
            {
                Canlib.canGetErrorText(status, out errorText);
            }
            else
            {
                errorText = action + " succeeded";
            }
            return errorText;
        }

        public string CheckDBStatus(String action, Kvadblib.Status status)
        {
            string DBStatus = "";
            if (status != Kvadblib.Status.OK)
            {
                DBStatus = action + " failed: " + status.ToString();
            }
            else
            {
                DBStatus = action + " succeeded";
            }
            return DBStatus;
        }
    }
}
