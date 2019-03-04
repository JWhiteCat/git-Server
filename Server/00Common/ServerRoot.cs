/****************************************************
	文件：ServerRoot.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 15:08   	
	功能：服务器初始化
*****************************************************/

public class ServerRoot
{
    private static ServerRoot instance;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
                instance = new ServerRoot();
            return instance;
        } 
    }

    public void Init()
    {
        //数据层
        DBMgr.Instance.Init();

        //服务层
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();

        //业务系统层
        CfgSvc.Instance.Init();
        LoginSys.Instance.Init();

        UploadSys.Instance.Init();
        RegisterSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
    }

    public int SessionId = 0;
    public int GetSessionID()
    {
        if(SessionId == int.MaxValue)
        {
            SessionId = 0;
        }
        return SessionId + 1;
    }
}

