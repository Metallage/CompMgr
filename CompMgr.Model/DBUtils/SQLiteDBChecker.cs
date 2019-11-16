using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompMgr.Model.DBUtils
{
    class SQLiteDBChecker : ICheckDataBase
    {
        public bool CheckDataBase(string filePath) => CheckFile(filePath);
        private bool CheckFile(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
