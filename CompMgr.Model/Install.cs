using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    public struct Install
    {

        public string NsName { get; set; }
        public string userFio { get; set; }

        public List<string> InstalledSoft { get; set; }
    }
}
