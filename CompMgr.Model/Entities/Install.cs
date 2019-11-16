using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    public class Install
    {

        public string NsName { get; set; }
        public string UserFio { get; set; }

        public List<string> InstalledSoft { get; set; }
    }
}
