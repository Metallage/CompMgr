using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using CompMgr.Model.DBUtils;

namespace ForDebug
{
    class Program
    {
        static void Main(string[] args)
        {

            string fileName = "debug.db";
            if (File.Exists(fileName))
                File.Delete(fileName);

            SQLiteDataBaseCreator creator1 = new SQLiteDataBaseCreator();
            creator1.CreateDataBase(fileName);
            GetTables(fileName);



        }

        static void GetTables(string dbPath)
        {
            string connectionString = $"DataSource={dbPath};Version=3;";
            string selectTablesScript = @"SELECT * FROM sqlite_master WHERE type='table'";

            using (SQLiteConnection testSqCon = new SQLiteConnection(connectionString))
            {
                DataTable tables = new DataTable("tables");
                SQLiteCommand selectTables = new SQLiteCommand(testSqCon);
                selectTables.CommandText = selectTablesScript;

                try
                {
                    testSqCon.Open();


                    tables.Load(selectTables.ExecuteReader());
                }
                finally
                {
                    testSqCon.Close();
                }



                foreach (DataColumn dc in tables.Columns)
                    Console.Write("{0} ({1})\t", dc.ColumnName, dc.DataType.Name);
                Console.Write("\n");

                foreach (DataRow dr in tables.Rows)
                    Console.WriteLine("{0}\t{1}\t{2}", dr.Field<string>("type"), dr.Field<string>("name"), dr.Field<string>("tbl_name"), dr.Field<string>("sql"));

                //    Console.WriteLine(,type, name, tbl_name, rootpage(int), sql);
                Console.WriteLine("Таблица Users {0}", tables.Select("name = 'Ololo'").Count() == 1);
                Console.ReadLine();
            }
        }
    }
}
