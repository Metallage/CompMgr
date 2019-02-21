using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Updates
    {
        public string NsName { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public bool IsUp { get; set; }

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
