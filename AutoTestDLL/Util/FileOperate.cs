using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Util
{
    public  class FileOperate
    {
        public  void createFile(string folder,string filename,string info) { 
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filepath = folder  +"\\"+ filename;
            if (File.Exists(filepath))
            {
                FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(info);
                sr.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(info);
                sr.Close();
                fs.Close();
            }
        }

        public static  string ReadFile(string path) { 
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                sb.AppendLine(sr.ReadLine());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 覆盖指定路径下文件的内容
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="info"></param>
        public static void CoverOldFile(string folder, string filename, string info)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filepath = folder + "/" + filename;

            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sr = new StreamWriter(fs);
            sr.WriteLine(info);
            sr.Close();
            fs.Close();
        }
    }
}
