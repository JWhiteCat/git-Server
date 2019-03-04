/****************************************************
	文件：CfgSvc.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2019/01/03 19:25   	
	功能：
*****************************************************/


using System;
using System.Collections.Generic;
using System.Xml;

public class CfgSvc
{
    private static CfgSvc instance;
    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new CfgSvc();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("CfgSvc Init Done.");
    }
}

//public class StrongCfg : BaseData<StrongCfg>
//{
//    public int pos;
//    public int startlv;
//    public int addhp;
//    public int addhurt;
//    public int adddef;
//    public int minlv;
//    public int coin;
//    public int crystal;
//}

//public class GuideCfg : BaseData<GuideCfg>
//{
//    public int coin;
//    public int exp;
//}

public class BaseData<T>
{
    public int ID;
}


