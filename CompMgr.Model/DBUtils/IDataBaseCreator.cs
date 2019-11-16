using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.DBUtils
{
    /// <summary>
    /// Creates SQLIte Database
    /// </summary>
    interface IDataBaseCreator
    {
        /// <summary>
        /// Creates SQLite database file and initialise tables
        /// </summary>
        /// <param name="connection">Path to database file</param>
        void CreateDataBase(string connection);
    }
}
