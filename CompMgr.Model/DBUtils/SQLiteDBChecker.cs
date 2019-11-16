using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompMgr.Model.DBUtils
{
    class SQLiteDBChecker : ICheckDataBase
    {
        private readonly string filePath;

        public SQLiteDBChecker(string filePath)
        {
            this.filePath = filePath;
        }
        public bool CheckDataBase() => CheckFile(filePath);
        private bool CheckFile(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
