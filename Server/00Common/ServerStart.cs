/****************************************************
	文件：ServerStart.cs
	作者：WANJ
	邮箱: 1173653942@qq.com
	日期：2018/12/29 15:07   	
	功能：
*****************************************************/

using System.Threading;

class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();

        while (true) {
            ServerRoot.Instance.Update();
            Thread.Sleep(100);
        }
    }
}

