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
    public class _Lib_Power
    {
        RS485Control RS485 = new RS485Control();
        public string  PortName=GetAppConfig("PowerSouceName");
        public  int PowerSourceControl(int CMD,int vol)
        {
            string errorMsg;


            int result = RS485.OPenPort(PortName);  
            // send command
            if (CMD == 1) //set voltage
            {
                byte[] command = new byte[]{0x53,0x4F,0x55,0x52,0x63,0x65,0x3A,0x56,0x4F,0x4C,0x54,0x61,0x67,0x65,0x3A,0x4C,0x45,0x56,0x65,0x6C,0x3A,
                                        0x49,0x4D,0x4D,0x65,0x64,0x69 ,0x61,0x74 ,0x65 ,0x3A ,0x41,0x4D ,0x50 ,0x4C ,0x69 ,0x74 ,0x75 ,0x64 ,
                                       0x65 ,0x20 ,0x31 ,0x34 ,0x2E,0x30,0x0D ,0x0A};
                command[command.Length - 6] = (byte)(vol % 100 / 10 + 0x30);
                command[command.Length - 5] = (byte)(vol % 10 + 0x30);
                command[command.Length - 3] = (byte)(vol % 1 + 0x30);
                result = RS485.Send(PortName, command);
            }
            if (CMD == 2)
            {
                byte[] command = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x75, 0x74, 0x3A, 0x53, 0x54, 0x41, 0x54, 0x65, 0x20, 0x4F, 0x4E, 0x0D, 0x0A };
                result = RS485.Send(PortName, command);
          
            }
            if (CMD == 3)
            {
                byte[] command = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x75, 0x74, 0x3A, 0x53, 0x54, 0x41, 0x54, 0x65, 0x20, 0x4F, 0x46, 0x46, 0x0D, 0x0A };
                result = RS485.Send(PortName, command);
              
             }
          
            RS485.ClosePort(PortName);
            return 0;
        }

        public static string GetAppConfig(string strKey)
        {
            string Path = System.Windows.Forms.Application.StartupPath;// »ñÈ¡Â·¾¶
            string FileName = Path + "\\File.config";
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = FileName;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            string key = config.AppSettings.Settings[strKey].Value;
            return key;
        }

    }
}