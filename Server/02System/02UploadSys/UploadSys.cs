using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PEProtocol;

public class UploadSys
{
    private static UploadSys instance;
    public static UploadSys Instance
    {
        get
        {
            if (instance == null)
                instance = new UploadSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("UploadSys Init Done");
    }

    public void ReqUpload(MsgPack pack)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspUpload
        };

        ReqUpload data = pack.msg.reqUpload;
        if (data.filename.Length > 0)
        {
            string path = "C:\\Research\\" + data.acct;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            for(int i = 0; i < data.filename.Length; i++)
            {
                FileStream fs = new FileStream(path + "\\" + data.filename[i], FileMode.Create);
                fs.Write(data.bytes[i], 0, data.bytes[i].Length);
                fs.Close();
            }

            msg.rspUpload = new RspUpload
            {
                result = true,
            };
        }
        else
        {
            msg.rspUpload = new RspUpload
            {
                result = false,
            };
        }

        pack.session.SendMsg(msg);
    }
}
