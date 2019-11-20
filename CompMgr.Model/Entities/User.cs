using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model.Entities.Abstract;

namespace CompMgr.Model.Entities
{
    class User: IUser
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public int? PhoneNumber { get; set; }
    }
}
