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

        public static void CreateLog(string errInfo)
        {
            DateTime now = DateTime.Now;
            //FileOperate.CreateDirectory("E:\\log");
            //FileOperate.CreateFile("E:\\log\\"+now.ToString("yyyyMMdd") + ".log",new List<string>() { now.ToString("yyyy-MM-dd HH:mm:ss") + "," + errInfo  });


            string folder = "E:\\data ";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string filename = folder + "/" + now.ToString("yyyyMMdd") + ".txt";
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + errInfo);
                sr.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + errInfo);
                sr.Close();
                fs.Close();
            }
        }

    }
}
