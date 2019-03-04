/****************************************************
	文件：NetSvr.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 15:14   	
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc instance;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new NetSvc();
            return instance;
        }
    }

    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer("10.22.184.214", SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            //PECommon.Log("PackCount:" + msgPackQue.Count);
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;

            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
            case CMD.ReqUpload:
                UploadSys.Instance.ReqUpload(pack);
                break;
            case CMD.ReqRegister:
                RegisterSys.Instance.ReqRegister(pack);
                break;
        }
    }
}
