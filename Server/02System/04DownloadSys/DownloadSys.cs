using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PEProtocol;

public class DownloadSys
{
    private static DownloadSys instance;
    public static DownloadSys Instance
    {
        get
        {
            if (instance == null)
                instance = new DownloadSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("DownloadSys Init Done");
    }

    public void ReqDownload(MsgPack pack)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspDownload
        };

        ReqDownload data = pack.msg.reqDownload;
        string acct = cacheSvc.GetAcctBySession(pack.session);

        if (data.filename.Length > 0 && acct != "")
        {
            string path = "C:\\Research\\" + acct;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            bool flag = true;
            for (int i = 0; i < data.filename.Length; i++)
            {
                string filepath = path + "\\" + data.filename[i];
                if (!File.Exists(filepath))
                {
                    flag = false;
                }
            }

            if (flag)
            {
                msg.rspDownload = new RspDownload
                {
                    filename = data.filename,
                };

                msg.rspDownload.bytes = new byte[data.filename.Length][];

                for (int i = 0; i < data.filename.Length; i++)
                {
                    string filepath = path + "\\" + data.filename[i];

                    FileStream fs = new FileStream(path + "\\" + data.filename[i], FileMode.Open, FileAccess.Read);
                    msg.rspDownload.bytes[i] = new byte[(int)fs.Length];
                    fs.Read(msg.rspDownload.bytes[i], 0, msg.rspDownload.bytes[i].Length);
                    fs.Close();
                }
            }
            else
            {
                msg.err = (int)ErrorCode.FileNotExist;
            }
        }
        else
        {
            msg.err = (int)ErrorCode.NotSelect;
        }

        pack.session.SendMsg(msg);
    }
}
