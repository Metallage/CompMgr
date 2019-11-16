using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.Entities.Abstract
{
    interface IComputer
    {
        int Id { get; set; }
        string NsName { get; set; }
        string Ip { get; set; }
        int UserId { get; set; }
    }
}
