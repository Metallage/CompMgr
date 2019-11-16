using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.DBUtils
{
    /// <summary>
    /// Check database file
    /// </summary>
    interface ICheckDataBase
    {
        /// <summary>
        /// Check if the database correct
        /// </summary>
        /// <returns>Is DB file correct</returns>
        bool CheckDataBase(string connection);
    }
}
