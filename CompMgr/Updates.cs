using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Updates
    {
        public string NsName { get; }
        public string Ip { get; }
        public string User { get; }
        public bool IsUp { get; set; }

        public Updates(string nsName, string ip, string user)
        {
            NsName = nsName;
            Ip = ip;
            User = User;
            IsUp = false;
        }

        public override string ToString()
        {
            return $"{NsName}";
        }

        public override int GetHashCode()
        {
            if (NsName != null)
                return NsName.GetHashCode();
            else
                return 0;
        }
    }
}
