﻿/****************************************************
	文件：Class1.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 15:22   	
	功能：网络通信协议（客服端服务器共用）
*****************************************************/

using System;
using System.Collections.Generic;
using PENet;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqUpload reqUpload;
        public RspUpload rspUpload;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqRegister reqRegister;
        public RspRegister rspRegister;

        public ReqDownload reqDownload;
        public RspDownload rspDownload;

        public ReqDelete reqDelete;
        public RspDelete rspDelete;
    }

    #region 登陆相关
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }
    #endregion

    #region 注册账号
    [Serializable]
    public class ReqRegister
    {
        public string acct;
        public string pass;
        public string name;
    }

    [Serializable]
    public class RspRegister
    {
        public bool result;
    }
    #endregion

    #region 上传文件
    [Serializable]
    public class ReqUpload
    {
        public string[] filename;
        public byte[][] bytes;

        public string[] keywords;
        public string[] summary;
    }

    [Serializable]
    public class RspUpload
    {
        public bool result;
    }
    #endregion

    #region 下载
    [Serializable]
    public class ReqDownload
    {
        public string[] filename;
    }

    [Serializable]
    public class RspDownload
    {
        public string[] filename;
        public byte[][] bytes;
    }
    #endregion

    #region 删除
    public class ReqDelete
    {
        public string[] filename;
    }

    public class RspDelete
    {
        public bool result;
    } 
    #endregion

    public enum CMD
    {
        None = 0,
        //登陆相关
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        ReqRegister = 105,
        RspRegister = 106,

        ReqUpload = 201,
        RspUpload = 202,

        ReqDownload = 301,
        RspDownload = 302,

        ReqDelete = 401,
        RspDelete = 402,
    }


    public enum ErrorCode
    {
        None = 0,//没有错误
        ServerDataError,
        UpdateDBError,

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        AcctNotExist,

        NameIsExist,

        FileNotExist,
        NotSelect,
    }

    public class SrvCfg
    {
        public const string srvIP = "10.30.28.22";
        //101.132.173.95
        public const int srvPort = 17666;
    }
}
