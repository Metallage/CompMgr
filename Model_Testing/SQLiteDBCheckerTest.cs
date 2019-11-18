using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;

using CompMgr.Model.DBUtils;
using System.Data.SQLite;


namespace Model_Testing
{
    [TestFixture]
    public class SQLiteDBCheckerTest
    {
        SQLiteDataBaseCreator testDBCreator = new SQLiteDataBaseCreator();
        string testFile = "test.db";
        DataTable tables;

        private void GetTables(string dbPath)
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
                this.tables = tables;
            }
        }

        [Test]
        public void DataBaseCreationTest()
        {
            testDBCreator.CreateDataBase(testFile);
            Assert.IsTrue(File.Exists(testFile)&&UsersTableCreationTest()&&ComputersTableCreationTest()&&DivisionsTableCreationTest()&&
                InstallsTableCreationTest()&&SoftwaresTableCreationTest());
        }


       
        private bool UsersTableCreationTest()
        {
            if (tables == null)
                GetTables(testFile);
            return tables.Select("name = 'Users'").Count()==1;
        }


        private bool ComputersTableCreationTest()
        {
            if (tables == null)
                GetTables(testFile);
            return tables.Select("name = 'Computers'").Count() == 1;
        }

        private bool DivisionsTableCreationTest()
        {
            if (tables == null)
                GetTables(testFile);
            return tables.Select("name = 'Divisions'").Count() == 1;
        }

        private bool InstallsTableCreationTest()
        {
            if (tables == null)
                GetTables(testFile);
            return tables.Select("name = 'Installs'").Count() == 1;
        }

        private bool SoftwaresTableCreationTest()
        {
            if (tables == null)
                GetTables(testFile);
            return tables.Select("name = 'Softwares'").Count() == 1;
        }

    }
}
