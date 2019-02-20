using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Computer
    {
        public string NsName { get; set; }
        public string Ip { get; set; }
        public Dictionary<int, string> User { get; set; }
        public Dictionary<int, string> Division { get; set; }
    }
}
