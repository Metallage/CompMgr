using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using CompMgr.Model.Entities;

namespace CompMgr.Model.DBContext
{
    class SQLiteDBContext :DbContext, IDBContext
    {
        
        public SQLiteDBContext():base("sqliteDataBase")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Install> Installs { get; set; }
    }
}
