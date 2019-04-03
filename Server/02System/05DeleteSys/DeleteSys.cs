using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PEProtocol;

public class DeleteSys
{
    private static DeleteSys instance;
    public static DeleteSys Instance
    {
        get
        {
            if (instance == null)
                instance = new DeleteSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("DeleteSys Init Done");
    }

    public void ReqDelete(MsgPack pack)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspDelete
        };

        ReqDelete data = pack.msg.reqDelete;
        string acct = cacheSvc.GetAcctBySession(pack.session);

        if (data.filename.Length > 0 && acct != "")
        {
            string path = "C:\\Research\\" + acct;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            for (int i = 0; i < data.filename.Length; i++)
            {
                string filepath = path + "\\" + data.filename[i];
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                else
                {
                    msg.err = (int)ErrorCode.FileNotExist;
                    break;
                }

                if(i==data.filename.Length - 1)
                {
                    msg.rspDelete = new RspDelete
                    {
                        result = true,
                    };
                }
            }
        }
        else
        {
            msg.err = (int)ErrorCode.NotSelect;
        }

        pack.session.SendMsg(msg);
    }
}
