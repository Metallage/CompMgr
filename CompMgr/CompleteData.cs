using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class CompleteData:List<CompleteTableHelper>
    {
        public void GetData(List<CompleteTableHelper> data)
        {
            foreach (CompleteTableHelper cth in data)
                this.Add(cth);
        }
    }
}
