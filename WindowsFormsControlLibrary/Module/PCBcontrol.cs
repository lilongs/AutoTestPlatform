using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS485;
namespace WindowsFormsControlLibrary.Module
{
    class PCBcontrol
    {
        RS485Control RS485 = new RS485Control();
       string PCBPortName = "";
       public const byte MCUaddr = 1;
       public const byte CMD_CurrentMeterChannal = 0x0C;
       public const byte CMD_Seting = 0x0E;
       public const byte CMD_IICAddr = 0x0D;
       public const byte CMD_Fule = 0x0F;
       byte[] CMD = new byte[8] { 0xA5, 0x5A, MCUaddr, 0, 0, 0, 0, 0 }; 
       public int SetCurrentMeterChannal(byte channal)
       {
           CMD[3] = CMD_CurrentMeterChannal;
           CMD[4] = channal;
           RS485.OPenPort(PCBPortName);
           RS485.Send(PCBPortName,CMD);
           RS485.ClosePort(PCBPortName);
           return 0;
       }
       public int SetLevelandLED(byte channal,byte  Level,byte LEDA,byte LEDF,byte Buzzer)
       {
           CMD[3] = CMD_Seting;
           CMD[4] =(byte)( Level+LEDA*2+LEDF*4+Buzzer*8);
           RS485.OPenPort(PCBPortName);
           RS485.Send(PCBPortName, CMD);
           RS485.ClosePort(PCBPortName);
           return 0;
       }
       public int SetFule(byte channal,Int16 fuel)
       {
           CMD[3] = CMD_Fule;
           CMD[4] = (byte)((fuel&0xff00)>>8);
           CMD[5] = (byte)((fuel & 0x00ff));
           RS485.OPenPort(PCBPortName);
           RS485.Send(PCBPortName, CMD);
           RS485.ClosePort(PCBPortName);
           return 0;

       }
    }
}
