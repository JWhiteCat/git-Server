/****************************************************
	文件：PECommon.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 18:45   	
	功能：客户端服务器共用工具类
*****************************************************/

using PENet;
using PEProtocol;

public enum LogType
{
    Log=0,
    Warn=1,
    Error=2,
    Info=3
}

public class PECommon
{
    public static void Log(string msg="",LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

}

