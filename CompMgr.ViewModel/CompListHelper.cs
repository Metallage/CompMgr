using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.ViewModel
{
    public struct CompListHelper
    {
        public string Key { get; set; }
        public bool Value { get; set; }

        public CompListHelper(string nsName, bool isUp)
        {
            Key = nsName;
            Value = isUp;
        }
    }
}
