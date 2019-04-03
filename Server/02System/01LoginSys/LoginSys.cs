/****************************************************
	文件：LoginSys.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 15:16   	
	功能：
*****************************************************/

using PEProtocol;

public class LoginSys
{
    private static LoginSys instance;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
                instance = new LoginSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("LoginSys Init Done");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;
        //当前账号是否已经上线
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
        };

        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            //已上线：返回错误信息，将原来session踢下线
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            //未上线：
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (DBMgr.isNew)
            {
                msg.err = (int)ErrorCode.AcctNotExist;
            }
            //存在，检测密码
            else if (pd == null)
            {
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };
                cacheSvc.AcctOnline(data.acct, pack.session, pd);
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        //存在？
        if (cacheSvc.IsNameExist(data.name))
        {
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AcctOffLine(session);
    }
}

