using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace CompMgr
{
    public class User
    {
        public long Id { get; set; }
        public string UserFio { get; set; }
        public string UserTel { get; set; }

        public User()
        { }

        public User (long id, string fio)
        {
            Id = id;
            UserFio = fio;
        }

    }
}
