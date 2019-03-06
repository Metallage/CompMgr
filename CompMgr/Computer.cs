using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Computer
    {
        public long Id { get; set; }
        public string NsName { get; set; }
        public string Ip { get; set; }

        public Computer()
        { }

        public Computer(long id, string nsName)
        {
            Id = id;
            NsName = nsName;
        }
    }
}
