using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.Entities.Abstract
{
    interface IUser
    {
        int Id { get; set; }
        string Fio { get; set; }
        int PhoneNumber { get; set; }
    }
}
