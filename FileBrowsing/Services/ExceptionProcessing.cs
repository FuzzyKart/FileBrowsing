using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace FileBrowsing.Services
{
    public class ExceptionProcessing
    {
        public static void ErrorWriting(string message)
        {
            using (FileStream fs = File.Open(Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.Length - 1),
                    FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(message);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}