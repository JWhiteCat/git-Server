/****************************************************
	文件：PETool.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/10/30 11:21   	
	功能：工具类
*****************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace PENet {
    public class PETool {

        public static byte[] PackNetMsg<T>(T msg) where T : PEMsg {
            return PackLenInfo(Serialize(msg));
        }

        /// <summary>
        /// Add length info to package
        /// </summary>
        public static byte[] PackLenInfo(byte[] data) {
            int len = data.Length;

            
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            //string @string = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetString(data);
            //Console.WriteLine(@string);
            

            byte[] pkg = new byte[len + 4];
            byte[] head = BitConverter.GetBytes(len);
            head.CopyTo(pkg, 0);
            data.CopyTo(pkg, 4);

            //
            //FileStream fs = new FileStream("D:\\test.txt", FileMode.Create);
            //fs.Write(pkg, 0, pkg.Length);
            //fs.Close();
            //
            return pkg;
        }

        public static byte[] Serialize<T>(T pkg) where T : PEMsg {
            //using (MemoryStream ms = new MemoryStream()) {
            //    BinaryFormatter bf = new BinaryFormatter();
            //    bf.Serialize(ms, pkg);
            //    ms.Seek(0, SeekOrigin.Begin);
            //    return ms.ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter streamWriter = File.CreateText("seXML.xml"))
            {
                serializer.Serialize(streamWriter, pkg);
                streamWriter.Close();
            }
            return FileTOByte("seXML.xml");
        }

        static byte[] FileTOByte(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] infbytes = new byte[(int)fs.Length];
            fs.Read(infbytes, 0, infbytes.Length);
            fs.Close();
            return infbytes;
        }

        public static T DeSerialize<T>(byte[] bs) where T : PEMsg {
            //using (MemoryStream ms = new MemoryStream(bs)) {
            //    BinaryFormatter bf = new BinaryFormatter();
            //    T pkg = (T)bf.Deserialize(ms);
            //    return pkg;
            //}

            FileStream fs = new FileStream("deXML.xml", FileMode.Create, FileAccess.Write);
            fs.Write(bs, 0, bs.Length);
            fs.Close();

            T pkg;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader streamReader = File.OpenText("deXML.xml"))
            {
                pkg = serializer.Deserialize(streamReader) as T;
                streamReader.Close();
            }
            return pkg;
        }

        #region Log
        public static bool log = true;
        public static Action<string, int> logCB = null;
        public static void LogMsg(string msg, LogLevel lv = LogLevel.None) {
            if (log != true) {
                return;
            }
            //Add Time Stamp
            msg = DateTime.Now.ToLongTimeString() + " >> " + msg;
            if (logCB != null) {
                logCB(msg, (int)lv);
            }
            else {
                if (lv == LogLevel.None) {
                    Console.WriteLine(msg);
                }
                else if (lv == LogLevel.Warn) {
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("//--------------------Warn--------------------//");
                    Console.WriteLine(msg);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (lv == LogLevel.Error) {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("//--------------------Error--------------------//");
                    Console.WriteLine(msg);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (lv == LogLevel.Info) {
                    //Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("//--------------------Info--------------------//");
                    Console.WriteLine(msg);
                    //Console.ForegroundColor = ConsoleColor.Gray;
                }
                else {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("//--------------------Error--------------------//");
                    Console.WriteLine(msg + " >> Unknow Log Type\n");
                    //Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Log Level
    /// </summary>
    public enum LogLevel {
        None = 0,// None
        Warn = 1,//Yellow
        Error = 2,//Red
        Info = 3//Green
    }
}