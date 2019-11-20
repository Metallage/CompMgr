using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using CompMgr.Model.Entities;

namespace CompMgr.Model.DBContext
{
    interface IDBContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Software> Softwares { get; set; }
        DbSet<Division> Divisions { get; set; }
        DbSet<Computer> Computers { get; set; }
        DbSet<Install> Installs { get; set; }

    }
}
