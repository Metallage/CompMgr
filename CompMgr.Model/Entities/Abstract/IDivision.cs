using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.Entities.Abstract
{
    interface IDivision
    {
        int Id { get; set; }
        string DivisionName { get; set; }
    }
}
