using System;
using PENet;

namespace Protocol {
    [Serializable]
    public class NetMsg : PEMsg {
        public string text;
    }


    public class IPCfg {
        public const string srvIP = "101.132.173.95";
        public const int srvPort = 17666;
    }
}