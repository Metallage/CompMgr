using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class CompleteTableHelper
    {
        public string SoftName { get; set; }
        public string SoftVersion { get; set; }
        public Dictionary<string, bool> CompNames { get; set; }



        public CompleteTableHelper(string softName, string softVersion)
        {
            SoftName = softName;
            SoftVersion = softVersion;
        }

        public override int GetHashCode()
        {
            if (this.SoftName != null)
                return this.SoftName.GetHashCode();
            else
                return 0;
        }

        public override string ToString()
        {
            if (this.SoftName != null)
                return this.SoftName;
            else
                return "";
        }


    }
}
