using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    public class InstalledSoft
    {
        public string SoftName { get; set; }
        public string Version { get; set; }

        public InstalledSoft(string softName, string version)
        {
            SoftName = softName;
            Version = version;
        }

        public InstalledSoft()
        { }
    }
}
