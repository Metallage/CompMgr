using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CompMgr.Model.DBUtils.Abstractions;

namespace CompMgr.Model.DBUtils
{
    class SQLiteDBChecker : ICheckDataBase
    {
        /// <summary>
        /// Check the SQLite database for correct state
        /// </summary>
        /// <param name="filePath">Path to SQLite database file</param>
        /// <returns>Is database correct</returns>
        public bool CheckDataBase(string filePath) => CheckFile(filePath);
        
        /// <summary>
        /// Check if database file exists
        /// </summary>
        /// <param name="filePath">Path to SQLite database file</param>
        /// <returns>Is database file exists</returns>
        private bool CheckFile(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
