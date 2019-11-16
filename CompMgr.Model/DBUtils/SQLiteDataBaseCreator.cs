using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace CompMgr.Model.DBUtils
{
    public class SQLiteDataBaseCreator : IDataBaseCreator
    {      
        public void CreateDataBase(string filePath)
        {
            throw new NotImplementedException("Создание БД ещё не реализовано");
        }

        private void CteateTable(SQLiteConnection connection, string command) //Нужно ли это?
        {
            if(connection.State == ConnectionState.Open)
            {
                
            }
        }
    }
}
