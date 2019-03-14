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

        public CompleteTableHelper()
        {

        }

        public CompleteTableHelper(string softName, string softVersion)
        {
            SoftName = softName;
            SoftVersion = softVersion;
        }
    }
}
