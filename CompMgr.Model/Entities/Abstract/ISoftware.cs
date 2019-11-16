using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.Entities.Abstract
{
    interface ISoftware
    {
        int Id { get; set; }
        string Name { get; set; }
        string ActualVersion { get; set; }
    }
}
