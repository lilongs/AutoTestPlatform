using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS485;
using System.Threading;
using WindowsFormsControlLibrary;
using System.Configuration;

namespace WindowsFormsControlLibrary.Module
{
    class PCBcontrol
    {
        RS485Control RS485 = new RS485Control();
        string PCBPortName =  GetAppConfig("PCBboardAdd");
      public static string GetAppConfig(string strKey)
        {
            string Path = System.Windows.Forms.Application.StartupPath;// 获取路径
            string FileName = Path + "\\File.config";
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = FileName;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            string key = config.AppSettings.Settings[strKey].Value;
            return key;
        }

        public int SetPowerONOFF(byte channal,bool ONOFF)
        {

            byte[] CMD = new byte[8] { 0xA5, 0x5A, 0x01, 0x0D, channal, 0, 0, 0 };
            if (ONOFF)
            {
                CMD[5] = 0x02;
            }
            else CMD[5] = 0x01;
            RS485.OPenPort(PCBPortName);
            int result = 0;
            int cnt = 0;
            while (result != 1 && cnt<5)
            {
                RS485.Send(PCBPortName, CMD);
                byte[] tt=RS485.Recv(PCBPortName);
                if(tt.Length>2 && tt[0]==0xA5 && tt[1]==0x5A && tt[6]==0x00)
                {
                    result = 1;
                }
                Thread.Sleep(50);
                cnt++;
            }
            RS485.ClosePort(PCBPortName);
            return 0;
        }
       public int SetCurrentMeterChannal(byte channal)
       {
            byte[] CMD = new byte[8] { 0xA5, 0x5A, 0x01, 0x0C, channal, 0, 0, 0 };
            RS485.OPenPort(PCBPortName);
            int result = 0;
            int cnt = 0;
            while (result != 1 && cnt < 5)
            {
                RS485.Send(PCBPortName, CMD);
                byte[] tt = RS485.Recv(PCBPortName);
                if (tt.Length > 2 && tt[0] == 0xA5 && tt[1] == 0x5A && tt[6] == 0x00)
                {
                    result = 1;
                }
                Thread.Sleep(50);
                cnt++;
            }
           RS485.ClosePort(PCBPortName);
           return 0;
       }
        public int PCBboardInit()
        {
            byte[] CMD = new byte[8] { 0xA5, 0x5A, 0x05, 0xC0, 0, 0, 0, 0 };
            byte iicchanal = 0;
            byte iicaddr = 0;
            RS485.OPenPort(PCBPortName);
    
            for (iicchanal = 1; iicchanal <= 4; iicchanal++)
            {
                for (iicaddr = 0; iicaddr < 8; iicaddr++)
                {
                    if (iicchanal == 1 && iicaddr<3)
                    {
                        CMD[2] = 0x01;
                    }
                    else
                    { CMD[2] = 0x05; }
                    if (iicchanal == 2|| iicchanal==4)
                    {
                        if(iicaddr==7)
                        {
                            continue;
                        }
                    }
                    CMD[4] = (byte)(iicchanal * 16 + iicaddr);
                    RS485.Send(PCBPortName, CMD);
                    Thread.Sleep(100);
                }            
            }
            RS485.ClosePort(PCBPortName);
            return 0;
        }
        public int SetLevel(byte channal,bool level)
        {
            byte[] CMD = new byte[8] { 0xA5, 0x5A, 0x00, 0x0E, 0, 0, 0, 0 };
           // byte[] CMDSet = new byte[8] { 0xA5, 0x5A, 0x00, 0xC0, 0, 0, 0, 0 };
            byte[][] bt = new byte[6][];
            bt[0] =new byte[]{0x12};
            bt[1] =new byte[]{0x17};
            bt[2] =new byte[]{0x24};
            bt[3] = new byte[]{0x32 };
            bt[4] = new byte[]{0x37 };
            bt[5] = new byte[]{0x44 };
            if (channal == 1)
            {
                CMD[2] = 0x01;
            }
            else
            {
                CMD[2] = 0x05;
            }
            CMD[4] = bt[channal][0];
            if (level)
            {
                CMD[5] = 0;
            }
            else CMD[5] = 1;

            RS485.OPenPort(PCBPortName);

            RS485.Send(PCBPortName, CMD);
            RS485.ClosePort(PCBPortName);
            return 0;
        }

        public int LEDcontrol(int led, bool ONOFF)
        {
            byte[] CMD_Accurate = new byte[8] { 0xA5, 0x5A, 0x01, 0x0E, 0x12, 0x0F, 0, 0 };
            byte[] CMD_Fault = new byte[8] { 0xA5, 0x5A, 0x01, 0x0E, 0x12, 0x0F, 0, 0 };
            byte[] CMD_Buzzer = new byte[8] { 0xA5, 0x5A, 0x01, 0x0E, 0x12, 0x0F, 0, 0 };
            RS485.OPenPort(PCBPortName);
            if (led == 0)
            {
                if (ONOFF)
                {
                    CMD_Accurate[5] = (byte)(CMD_Accurate[5] & 0x0D);
                }
                RS485.Send(PCBPortName, CMD_Accurate);
            }
            else if (led == 1)
            {
                if (ONOFF)
                {
                    CMD_Fault[5] = (byte)(CMD_Accurate[5] & 0x0B);
                    RS485.Send(PCBPortName, CMD_Fault);
                }
                else if (led == 2)
                {
                    if (ONOFF)
                    {
                        CMD_Buzzer[5] = (byte)(CMD_Accurate[5] & 0x07);
                    }
                    RS485.Send(PCBPortName, CMD_Buzzer);
                }
            }
                RS485.ClosePort(PCBPortName);
                return 0;
        }
       
       public int SetFule(byte channal,Int16 fuel1,Int16 fuel2,Int16 fuel3,Int16 fuel4)
        {
            byte[] CMD = new byte[8] { 0xA5, 0x5A, 0x00, 0xC1, 0, 0, 0, 0 };
         //   byte[] CMDSet = new byte[8] { 0xA5, 0x5A, 0x01, 0xC0, 0x12, 0, 0, 0 };

            byte[][] bt = new byte[6][];
            bt[0] = new byte[] { 0x10,0x11,0x13,0x14 };
            bt[1] = new byte[] { 0x15,0x16,0x20,0x21 };
            bt[2] = new byte[] { 0x22,0x23,0x25,0x26 };
            bt[3] = new byte[] { 0x30,0x31,0x33,0x34 };
            bt[4] = new byte[] { 0x35,0x36,0x40,0x41 };
            bt[5] = new byte[] { 0x42,0x43,0x45,0x46 };

            if (channal == 1)
            {
                CMD[2] = 0x01;
            }
            else
            {
                CMD[2] = 0x05;
            }
           
            RS485.OPenPort(PCBPortName);
            CMD[4] = bt[channal-1][ 0];
            CMD[5] = (byte)((fuel1 & 0xff00) >> 8);
            CMD[6] = (byte)((fuel1 & 0x00ff));
            RS485.Send(PCBPortName, CMD);
            Thread.Sleep(100);
            CMD[4] = bt[channal-1][1];
            CMD[5] = (byte)((fuel2 & 0xff00) >> 8);
            CMD[6] = (byte)((fuel2 & 0x00ff));
            RS485.Send(PCBPortName, CMD);
            Thread.Sleep(100);
            //3
            if (channal == 1)
            {
                CMD[2] = 0x05;
            }
            if (channal == 2)
            {
                Thread.Sleep(1000);
            }
            CMD[4] = bt[channal-1][2];
            CMD[5] = (byte)((fuel3 & 0xff00) >> 8);
            CMD[6] = (byte)((fuel3 & 0x00ff));
            RS485.Send(PCBPortName, CMD);
            Thread.Sleep(100);
            //4
            CMD[4] = bt[channal-1][3];
            CMD[5] = (byte)((fuel4 & 0xff00) >> 8);
            CMD[6] = (byte)((fuel4 & 0x00ff));
            RS485.Send(PCBPortName, CMD);

            Thread.Sleep(100);
            RS485.ClosePort(PCBPortName);
           return 0;

       }
   
    }
}
