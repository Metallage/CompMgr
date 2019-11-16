using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using CompMgr.Model.DBUtils.Abstractions;

namespace CompMgr.Model.DBUtils
{
    public class SQLiteDataBaseCreator : IDataBaseCreator
    {
        /// <summary>
        /// Creates SQLite database at selected filepath
        /// </summary>
        /// <param name="filePath">Path to database file</param>
        public void CreateDataBase(string filePath) => CteateSQLiteDatabase(filePath);

        /// <summary>
        /// Enables foreign keys support
        /// </summary>
        /// <param name="connection">Connection to SQLite database</param>
        private void EnableForeignKeys(SQLiteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand enFk = new SQLiteCommand(connection);
                enFk.CommandText = "PRAGMA foreign_keys=on;";
                enFk.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates SQLite database at selected filepath
        /// </summary>
        /// <param name="filePath">Path to database file</param>
        private void CteateSQLiteDatabase(string filePath) 
        {
            SQLiteConnection.CreateFile(filePath);
            string connectionString = $"DataSource={filePath};Version=3;";

            List<string> createTableQueries = GetCreationQueries();

            using (SQLiteConnection mySQLitecon = new SQLiteConnection(connectionString))
            {
                try
                {
                    mySQLitecon.Open();
                    EnableForeignKeys(mySQLitecon);
                    CreateTables(mySQLitecon, createTableQueries);
                }
                finally
                {
                    mySQLitecon.Close();
                }
            }
        }

        /// <summary>
        /// Creates tables in database
        /// </summary>
        /// <param name="sQLiteConnection">Connection to database</param>
        /// <param name="createStrings">Create tables queries</param>
        private void CreateTables(SQLiteConnection sQLiteConnection, List<string> createStrings)
        {

            if (sQLiteConnection.State == ConnectionState.Open)
            {
                SQLiteCommand transaction = new SQLiteCommand(sQLiteConnection);
                transaction.CommandText = "BEGIN TRANSACTION";
                transaction.ExecuteNonQuery();

                foreach (string createString in createStrings)
                {
                    transaction.CommandText = createString;
                    transaction.ExecuteNonQuery();
                }

                transaction.CommandText = "COMMIT";
                transaction.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Creates list of create table queries
        /// </summary>
        /// <returns>List of create table queries</returns>
        private List<string> GetCreationQueries()
        {
            List<string> createStrings = new List<string>();

            string createUserQuery = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "Fio TEXT NOT NULL, PhoneNumber INTEGER)";
            createStrings.Add(createUserQuery);

            string createDivisionQuery = "CREATE TABLE IF NOT EXISTS Divisions (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "DivisionName TEXT NOT NULL)";
            createStrings.Add(createDivisionQuery);

            string createCompQuery = "CREATE TABLE IF NOT EXISTS Computers (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "NsName TEXT NOT NULL UNIQUE, Ip TEXT NOT NULL UNIQUE, UserId INTEGER, DivisionId INTEGER, " +
                    "FOREIGN KEY (UserId) REFERENCES Users(Id) ,FOREIGN KEY (DivisionId) REFERENCES Divisions(Id))";
            createStrings.Add(createCompQuery);

            string createSoftQuery = "CREATE TABLE IF NOT EXISTS Softwares (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "Name TEXT NOT NULL UNIQUE, Version TEXT NOT NULL)";
            createStrings.Add(createSoftQuery);

            string createInstallQuery = "CREATE TABLE IF NOT EXISTS Installs (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "CompId INTEGER NOT NULL, SoftId INTEGER NOT NULL, version TEXT NOT NULL, " +
                    "FOREIGN KEY (CompId) REFERENCES Computers(Id), FOREIGN KEY (SoftId) REFERENCES Softwares(Id))";
            createStrings.Add(createInstallQuery);

            return createStrings;
        }
    }
}
