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
   public  class TemperatureSensor
    {
        RS485Control RS485 = new RS485Control();
        public string PortName = GetAppConfig("TempSouceName");

        public void ReadTemperature(byte add)
        {
            int result = RS485.OPenPort(PortName);
            byte[] command = new byte[] { 0x23,0x30,(byte)(0x30+add),0x0D};
            result = RS485.Send(PortName, command);
            byte[] tt = RS485.Recv(PortName);
            RS485.ClosePort(PortName);
        }
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
    }
}
