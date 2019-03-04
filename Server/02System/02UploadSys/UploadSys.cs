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
        if (data != null)
        {
            string path = "D:\\" + data.acct;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream fs = new FileStream("D:\\" + data.acct + "\\" + data.filename, FileMode.Create);
            fs.Write(data.datas, 0, data.datas.Length);
            fs.Close();

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
