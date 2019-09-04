using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Util
{
    public static class SysLog
    {

        public static void CreateAmmeterLog(string Info)
        {
            DateTime now = DateTime.Now;

            string folder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\Ammeter\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string filename = folder + "/" + now.ToString("yyyyMMdd") + ".txt";
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Info);
                sr.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Info);
                sr.Close();
                fs.Close();
            }
        }


        public static void CreateTemperatureLog(string Info)
        {
            DateTime now = DateTime.Now;

            string folder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"data\\Temperature\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string filename = folder + "/" + now.ToString("yyyyMMdd") + ".txt";
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Info);
                sr.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Info);
                sr.Close();
                fs.Close();
            }
        }
    }
}
