using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Util
{
    public static class JsonOperate
    {
        /// <summary>
        ///     IO读取本地json
        /// </summary>
        /// <param name="desktopPath"></param>
        /// <returns></returns>
        public static string GetJson(string Path,string filename)
        {
            using (FileStream fsRead = new FileStream(string.Format("{0}\\{1}", Path, filename), FileMode.Open))
            {
                //读取加转换
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                return System.Text.Encoding.UTF8.GetString(heByte);
            }
        }

        /// <summary>
        ///     将我们的json保存到本地
        /// </summary>
        /// <param name="desktopPath"></param>
        /// <param name="json"></param>
        public static void SaveJson(string Path, string filename, string json)
        {
            using (FileStream fs = new FileStream(string.Format("{0}\\{1}", Path, filename), FileMode.Create))
            {
                //写入
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(json);
                }

            }

        }
    }
}
