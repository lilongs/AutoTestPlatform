using AutoTestDLL.Model;
using canlibCLSNET;
using Kvaser.Kvadblib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestDLL.Module
{
    public class KvaserDbcMessage
    {
        //Handle for the channel to listen for messages on
        public int chanHandle;
        //Handle to the database and the message to send
        Kvadblib.Hnd dh;
        Kvadblib.MessageHnd msgHandle;
        //ID of the message
        int msgId;
        int msgFlags;


        //Used to determine which actions are possible
        //e.g. we can't send a message unless we've opened a channel
        public bool channelOn = false;
        public bool hasMessage = false;
        public bool hasDb = false;

        /*
         * Loads a database from the file selected by the user.
         */
        public void loadDb(string dbFile)
        {
            if (LoadDB(dbFile))
            {
                hasDb = true;
            }
        }

        /*
         * Loads the selected message's signals to construct a form
         */
        public List<Signal> LoadSignalsById(int MsgId)
        {
            List<Signal> result = new List<Signal>();
            Kvadblib.MessageHnd mh;

            Kvadblib.Status status = Kvadblib.GetMsgById(dh, MsgId, out mh);
            if (status == Kvadblib.Status.OK)
            {
                Kvadblib.MESSAGE f;
                int dlc;
                msgHandle = mh;
                Kvadblib.GetMsgId(mh, out msgId, out f);
                Kvadblib.GetMsgDlc(mh,out dlc);
                
                msgId = ((MsgId & -2147483648) == 0) ? MsgId : MsgId ^ -2147483648;
                msgFlags = ((MsgId & -2147483648) == 0) ? 0 : Canlib.canMSG_EXT;
                hasMessage = true;
                result = LoadSignals(dlc);
            }
            return result;
        }

        /*
         * Initiates the channel
         */
        public void initChannel(int channel,string bitrate)
        {
            Canlib.canStatus status;

            Canlib.canInitializeLibrary();
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);
            if (hnd >= 0)
            {
                chanHandle = hnd;

                Dictionary<string, int> dicBitRates = new Dictionary<string, int>() {
                    { "125 kb/s", Canlib.canBITRATE_125K},
                    { "250 kb/s", Canlib.canBITRATE_250K},
                    { "500 kb/s", Canlib.canBITRATE_500K},
                    { "1 Mb/s", Canlib.BAUD_1M}
                };

                status = Canlib.canSetBusParams(chanHandle, dicBitRates[bitrate], 0, 0, 0, 0, 0);
                status = Canlib.canBusOn(chanHandle);
                if (status == Canlib.canStatus.canOK)
                {
                    channelOn = true;
                }
            }
        }

        /*
         * Sends a message if one is constructed and we are on bus
         */
        public bool sendMsg(int dlc,List<Signal> signals)
        {
            bool result = false;
            if (hasMessage && channelOn)
            {
                result = SendMessage(dlc,signals);
            }
            return result;
        }

        /*
         * Loads the selected database
         * returns true if OK.
         */
        private bool LoadDB(string filename)
        {
            Kvadblib.Hnd hnd = new Kvadblib.Hnd();
            Kvadblib.Status status;

            Kvadblib.Open(out hnd);

            status = Kvadblib.ReadFile(hnd, filename);

            if (status == Kvadblib.Status.OK)
            {
                dh = hnd;
            }

            return status == Kvadblib.Status.OK;
        }

        /*
         * Once a database is loaded, this method will be called
         * to add all the messages to the ComboBox.
         */
        public List<Message> LoadMessages()
        {
            Kvadblib.MessageHnd mh;
            Kvadblib.Status status;
            Kvadblib.NodeHnd nh;

            List<Message> messages = new List<Message>();
            status = Kvadblib.GetFirstMsg(dh, out mh);
            while (status == Kvadblib.Status.OK)
            {
                string name;
                int id;
                int dlc;
                string nodeName;
                Kvadblib.MESSAGE flags;
                status = Kvadblib.GetMsgName(mh, out name);
                status = Kvadblib.GetMsgId(mh, out id, out flags);
                status = Kvadblib.GetMsgDlc(mh,out dlc);
                status = Kvadblib.GetMsgSendNode(mh, out nh);
                status = Kvadblib.GetNodeName(nh, out nodeName);

                Kvadblib.AttributeHnd ah;
                int attEunmVal=8;
                status = Kvadblib.GetAttributeByName(dh, "GenMsgSendType", out ah);//获取ah用于存储对应message的ah
                status = Kvadblib.GetMsgAttributeByName(mh, "GenMsgSendType", ref ah);//获取对应message的attribute信息
                Kvadblib.GetAttributeValueEnumeration(ah, out attEunmVal);

                int GenMsgCycleTime = 0;
                status = Kvadblib.GetAttributeByName(dh, "GenMsgCycleTime", out ah);//获取ah用于存储对应message的ah
                status = Kvadblib.GetMsgAttributeByName(mh, "GenMsgCycleTime", ref ah);//获取对应message的attribute信息
                Kvadblib.GetAttributeValueInt(ah, out GenMsgCycleTime);


                Message message = new Message();
                message.id = id;
                message.name = name;
                message.dlc = dlc;
                message.tx_node = nodeName;
                message.GenMsgSendType = this.GenMsgSendType[attEunmVal];
                message.GenMsgCycleTime = GenMsgCycleTime;
                message.CycleCount = 0;

                messages.Add(message);


                status = Kvadblib.GetNextMsg(dh, out mh);
            }
            return messages;
        }


        /*
         * Constructs a form for creating messages.
         * Consists of one TextBox for every signal in the loaded message.
         */
        private List<Signal> LoadSignals(int dlc)
        {
            Kvadblib.SignalHnd sh;
            Kvadblib.Status status = Kvadblib.GetFirstSignal(msgHandle, out sh);
            List<Signal> signals = new List<Signal>();

            while (status == Kvadblib.Status.OK)
            {
                string name;
                string unit;
                double min, max, scale, offset,val;
                //byte[] data = new byte[dlc];

                //Construct the text for the label
                status = Kvadblib.GetSignalName(sh, out name);
                status = Kvadblib.GetSignalUnit(sh, out unit);
                status = Kvadblib.GetSignalValueLimits(sh, out min, out max);
                status = Kvadblib.GetSignalValueScaling(sh, out scale, out offset);
                //status = Kvadblib.GetSignalValueFloat(sh, out val, data, dlc);

                //Signal s = new Signal(name, max, min, min, scale);
                Signal s = new Signal(name,max,min,min,scale);
                signals.Add(s);


                status = Kvadblib.GetNextSignal(msgHandle, out sh);
            }
            return signals;
        }


        /*
         * Constructs a message from the form and sends it
         * to the channel
         */
        private bool SendMessage(int dlc,List<Signal> signals)
        {
            byte[] data = new byte[dlc];

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

                status = Kvadblib.StoreSignalValuePhys(sh, data, dlc, s.Value);

                //Check if the signal value was successfully stored and that it's in the correct interval
                if (status != Kvadblib.Status.OK || s.Value < min || s.Value > max)
                {
                    error = true;
                    break;
                }
            }

            if (!error)
            {
                Canlib.canWriteWait(chanHandle, msgId, data, dlc, msgFlags, 50);
            }
            return error;
        }

        public Message ReadMessage(int readHandle, out Canlib.canStatus status)
        {
            status = Canlib.canStatus.canOK;
            Message m = null;
            int id;
            byte[] data = new byte[8];
            int dlc;
            int flags;
            long time;

            status = Canlib.canReadWait(readHandle, out id, data, out dlc, out flags, out time, 50);
            m = new Message(id, data, dlc, flags, time);

            return m;
        }


        /*
         * Closes the channel. Also stops any automatic transmission.
         */
        public void closeChannel()
        {
            channelOn = false;
            Canlib.canBusOff(chanHandle);
            Canlib.canClose(chanHandle);
        }

        private List<string> GenMsgSendType = new List<string>()
        {
            "Cyclic",
            "not_used",
            "not_used",
            "not_used",
            "not_used",
            "not_used",
            "not_used",
            "IfActive",
            "NoMsgSendType"
        };


    }

}
