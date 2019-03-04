using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PEProtocol;

public class RegisterSys
{
    private static RegisterSys instance;
    public static RegisterSys Instance
    {
        get
        {
            if (instance == null)
                instance = new RegisterSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("RegisterSys Init Done");
    }

    public void ReqRegister(MsgPack pack)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRegister,
        };

        ReqRegister data = pack.msg.reqRegister;

        if(cacheSvc.Register(data.acct, data.pass, data.name))
        {
            msg.rspRegister = new RspRegister
            {
                result = true,
            };
        }
        else
        {
            msg.rspRegister = new RspRegister
            {
                result = false,
            };
        }

        pack.session.SendMsg(msg);
    }
}
