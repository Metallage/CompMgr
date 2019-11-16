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

        private void EnableForeignKeys(SQLiteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand enFk = new SQLiteCommand(connection);
                enFk.CommandText = "PRAGMA foreign_keys=on;";
                enFk.ExecuteNonQuery();
            }
        }

        private void CteateTables(string filePath) 
        {
            SQLiteConnection.CreateFile(filePath);
            string connectionString = $"DataSource={filePath};Version=3;";
            using (SQLiteConnection mySQLitecon = new SQLiteConnection(connectionString))
            {
                EnableForeignKeys(mySQLitecon);



            }
        }
    }
}
