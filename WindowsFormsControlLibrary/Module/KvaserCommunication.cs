using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using canlibCLSNET;
using System.Threading;
using AutoTestDLL.Util;

namespace WindowsFormsControlLibrary.Module
{
    public partial class KvaserCommunication
    {
        FileOperate FileOperation = new FileOperate();

        public int handle = -1;
        private int channel = -1;
        private int readHandle = -1;
        private bool onBus = false;
        private readonly BackgroundWorker dumper;
        private RichTextBox box=null;

        public int DigSendID = 0x714;
        public int DigRecivedID = 0x77E;
        public string LogFilePath = "";
        public string LogFileName = "";

        //Initializes Canlib
        public void init()
        {
            Canlib.canInitializeLibrary();
        }

        //Opens the channel selected in the "Channel" input box
        public  int  openChannel(int channel)
        {
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);

            if (hnd >= 0)
            {
                handle = hnd;
                this.channel = channel;
            }
            return hnd;
        }
        public void initChannel(int channel, string bitrate)
        {
            Canlib.canStatus status;

            Canlib.canInitializeLibrary();
            int hnd = Canlib.canOpenChannel(channel, Canlib.canOPEN_ACCEPT_VIRTUAL);
            if (hnd >= 0)
            {
                handle = hnd;

                Dictionary<string, int> dicBitRates = new Dictionary<string, int>() {
                    { "125 kb/s", Canlib.canBITRATE_125K},
                    { "250 kb/s", Canlib.canBITRATE_250K},
                    { "500 kb/s", Canlib.canBITRATE_500K},
                    { "1 Mb/s", Canlib.BAUD_1M}
                };

                status = Canlib.canSetBusParams(handle, dicBitRates[bitrate], 0, 0, 0, 0, 0);
                status = Canlib.canBusOn(handle);
                if (status == Canlib.canStatus.canOK)
                {
                    onBus = true;
                }
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
            }
        }

        //Reads message data from user input and writes a message to the channel

        public void send(int id, int dlc, byte[] data, int flags)
        {
            Canlib.canStatus status = Canlib.canWrite(handle, id, data, dlc, flags);
        }
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public  void CANmsgSend(string datastr)
        {
            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ") + Convert.ToString(DigSendID, 16) + "----"+datastr;
            FileOperation.createFile(LogFilePath, LogFileName, str);

            int collection =0;
            byte[] sendmsg=strToToHexByte(datastr);
            if(datastr.Length==0)
            {
                return;
            }
            decimal a = sendmsg.Length;
            decimal b = 7;
            decimal c = (a-6) / b;
            if (a > 7)
            {
                collection = (int)Math.Ceiling(c)+1;
            }
            else collection = 1;
            Canlib.canFlushReceiveQueue(handle);
            if (collection == 1)
            {
                byte[] Send = new byte[8];
                for (int i = 0; i < 7; i++)
                {
                    if (i + 1 > sendmsg.Length)
                    {
                        Send[i+1] = 0xFF;
                    }
                    else
                    {
                        Send[i+1] = sendmsg[i];
                    }
                }
                Send[0]=(byte)sendmsg.Length;
                send(DigSendID,8,Send,0);             
            }
            else if (collection > 1)
            {
                int length = sendmsg.Length;
                byte[] Send1 = new byte[8];
                Send1[0] = 0x10;
                Send1[1]=(byte)sendmsg.Length;
                for(int i=0;i<6;i++)
                {
                    Send1[i+2]=sendmsg[i];
                }
                send(DigSendID,8,Send1,0);    
                Thread.Sleep(25);
                length = length - 6;
                for (int i = 1; i < collection; i++)
                {
                    byte[] Send = new byte[8];
                    length = length - 7;
                    Send[0]=(byte)(0x20+i);
                    if (length >= 7)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            Send[j+1] = sendmsg[(i-1) * 7 + j + 6];
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (((i-1) * 7 + j + 6) < sendmsg.Length)
                            {
                                Send[j+1] = sendmsg[(i-1) * 7 + j + 6];
                            }
                            else
                            {
                                Send[j+1] = 0xFF;
                            }
                        }
                    }
                    send(DigSendID,8,Send,0);      
                }      
            }
        }
        bool singleframe = true;
        bool multipleframe = false;
        public  byte[]  CANmsgReceived()
        {
            string recvstring = " ";
            int length = 0;
            int cnt = 0;
            string[] multFlame = new string[100];
            string REcv = " ";        
            DumpMessageLoop(singleframe,out multFlame);
            REcv = multFlame[0];
            if (REcv == null)
            {
              REcv = " ";
            }
            if (REcv.Length == 16)
            {              
                if (REcv.Substring(0, 2) == "10")
                {
                    recvstring = REcv.Substring(4);
                    length = Convert.ToByte(REcv.Substring(2, 2), 16) - 6;
                    Send30();
                    DumpMessageLoop(multipleframe,out multFlame);
                    for (int i = 0; i < multFlame.Length; i++)
                    {
                        if (multFlame[i] == null)
                        {
                            break;
                        }
                        if (length > 7)
                        {
                            recvstring += multFlame[i].Substring(2);
                            length -= 7;
                        }
                        else
                        {
                            recvstring += multFlame[i].Substring(2, length * 2);
                        }
                    }
                }
                else
                {
                    if (REcv.Substring(0, 2) != "30")
                    {
                        length = Convert.ToInt16(REcv.Substring(0, 2)) * 2;
                        recvstring = REcv.Substring(2, length);                  
                    }
                }
            }
            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ")+  Convert.ToString(DigRecivedID,16) + "----"+recvstring;
            FileOperation.createFile(LogFilePath, LogFileName, str);

            return strToToHexByte(recvstring);

        }
        public  void Send30()
        {
            byte[] msg = new byte[8] { 0x30,0x0F, 0x05, 0, 0, 0, 0, 0 };
            send(DigSendID,8,msg,0);

            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ") + Convert.ToString(DigSendID, 16)+ "----"+"300F050000000000";
            FileOperation.createFile(LogFilePath, LogFileName, str);

            Thread.Sleep(10);

        }

        public  void CANmsgSend(Int32 id, string datastr)
        {
            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ") + Convert.ToString(id, 16) + datastr + Environment.NewLine;
            FileOperation.createFile(LogFilePath, LogFileName, str);

            int collection = 0;
                byte[] sendmsg = strToToHexByte(datastr);
                if(datastr.Length==0)
                {
                    return;
                }
                decimal a = sendmsg.Length;
                decimal b = 7;
                decimal c = (a - 6) / b;
                if (a > 7)
                {
                    collection = (int) Math.Ceiling(c)+1;
                }
                else collection = 1;
                Canlib.canFlushReceiveQueue(handle);
                // Create an event collection with 2 messages (events)
                if (collection == 1)
                {
                    byte[] Send = new byte[8];
                    for (int i = 0; i< 7; i++)
                    {
                        if (i + 1 > sendmsg.Length)
                        {
                            Send[i + 1] = 0xFF;
                        }
                        else
                        {
                            Send[i + 1] = sendmsg[i];
                        }
                    }
                    Send[0]=(byte) sendmsg.Length;
                    send(id,8, Send,4);             
                }
                else if (collection > 1)
                {
                    int length = sendmsg.Length;
                    byte[] Send1 = new byte[8];
                    Send1[0] = 0x10;
                    Send1[1]=(byte) sendmsg.Length;
                    for(int i=0;i<6;i++)
                    {
                        Send1[i + 2]=sendmsg[i];
                    }
                    send(id,8, Send1,4);
                    Thread.Sleep(30);
                    length = length - 6;
                    for (int i = 1; i<collection; i++)
                    {
                        byte[] Send = new byte[8];
                        length = length - 7;
                        Send[0]=(byte) (0x20+i);
                        if (length >= 7)
                        {
                            for (int j = 0; j< 7; j++)
                            {
                                Send[j + 1] = sendmsg[(i - 1) * 7 + j + 6];
                            }
                        }
                        else
                        {
                            for (int j = 0; j< 7; j++)
                            {
                                if (((i-1) * 7 + j + 6) < sendmsg.Length)
                                {
                                    Send[j + 1] = sendmsg[(i - 1) * 7 + j + 6];
                                }
                                else
                                {
                                    Send[j + 1] = 0xFF;
                                }
                            }
                        }
                        send(id,8, Send,4);
                      
                    }      
                }
            }
        public byte[] CANmsgReceived(Int32 id)
        {
            string recvstring = " ";
            int length = 0;
            int cnt = 0;
            string[] multFlame = new string[100];
            string REcv = " ";

            DumpMessageLoop(singleframe,id,out multFlame);
            REcv = multFlame[0];
            if (REcv == null)
            {
                REcv = " ";
            }

            if (REcv.Length == 16)
            {
                if (REcv.Substring(0, 2) == "10")
                {
                    recvstring = REcv.Substring(4);

                    length = Convert.ToByte(REcv.Substring(2, 2), 16) - 6;

                    //     c_log.Debug(length);
                    Send30(0x17FC0114);
                    DumpMessageLoop(multipleframe,id,out multFlame);
                    for (int i = 0; i < multFlame.Length; i++)
                    {
                        if (multFlame[i] == null)
                        {
                            break;
                        }
                        if (length > 7)
                        {
                            recvstring += multFlame[i].Substring(2);
                            length -= 7;
                        }
                        else
                        {
                            recvstring += multFlame[i].Substring(2, length * 2);
                        }
                    }
                }
                else
                {
                    if (REcv.Substring(0, 2) != "30")
                    {
                        length = Convert.ToInt16(REcv.Substring(0, 2)) * 2;
                        recvstring = REcv.Substring(2, length);
                    }
                }
            }
            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ") + Convert.ToString(id, 16) +"----"+ recvstring ;
            FileOperation.createFile(LogFilePath, LogFileName, str);

            return strToToHexByte(recvstring);

         }
        public void Send30(Int32 id)
        {
            byte[] msg = new byte[8] { 0x30, 0x0F, 0x05, 0, 0, 0, 0, 0 };
            if(id>0x7ff)
            {
                send(id, 8, msg, 4);
            }
            else
            {
                send(id, 8, msg, 0);
            }
            string str = DateTime.Now.ToString("hh:mm:ss:fff---- ") + Convert.ToString(id, 16)+ "----"+"300F050000000000" ;
            FileOperation.createFile(LogFilePath, LogFileName, str);

        }
        public  string ByteToHex(byte[] bcd)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bcd)
            {
                sb.Append(b.ToString("X2"));
                sb.Append(" ");
            }
            return sb.ToString();
        }
        public  byte[] HexStringToBinary(string hexstring)
        {
            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }
        public  string HexstringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }
            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }
            string b = new String(c);
            return b;
        }
        public  int Compare(string st1, string st2, int length)
        {
            if (st1.Length < length || st2.Length < length)
            {
                return -1;
            }
            string ss1 = st1.Substring(0, length);
            string ss2 = st2.Substring(0, length);
            if (ss1 == ss2)
            {
                return 0;
            }
            else
            {
                return -1;
            }

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

        private  void DumpMessageLoop(bool singleframe,out string[] msg)
        {    
            Canlib.canStatus status;
            int id;
            byte[] data = new byte[8];
            int dlc;
            int flags;
            long time;
            bool noError = true;
            int i = 0,j=0;
            msg = new string[30];
            while (j<200)
            {           
                status = Canlib.canReadWait(handle, out id, data, out dlc, out flags, out time, 5);
                if ( id == DigRecivedID)
                {
                    if(data[0]!=0x30)
                    {
                        msg[i] = String.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}",
                                                    data[0], data[1], data[2], data[3], data[4],
                                                    data[5], data[6], data[7]);
                        i++;
                        if(singleframe)
                        {
                            break;
                        }
                     }
                    Thread.Sleep(15);
                }
                else if (status != Canlib.canStatus.canERR_NOMSG)
                {
                    noError = false;
                }              
                j++;
             }
        }
        private void DumpMessageLoop(bool singlefram,Int32 Id,out string[] msg)
        {
            Canlib.canStatus status;
            int id;
            byte[] data = new byte[8];
            int dlc;
            int flags;
            long time;
            bool noError = true;
            int i = 0, j = 0;
            msg = new string[1000];
            while (j < 200)
            {
                status = Canlib.canReadWait(handle, out id, data, out dlc, out flags, out time, 50);
                if (id == Id)
                {
                    if (data[0] != 0x30)
                    {
                        msg[i] = String.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}",
                                                        data[0], data[1], data[2], data[3], data[4],
                                                        data[5], data[6], data[7]);
                        id = 0;
                        i++;
                        if(singlefram)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(15);
                }
                else if (status != Canlib.canStatus.canERR_NOMSG)
                {
                    noError = false;
                }
                j++;
            }
        }

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
