﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace CompMgr
{
    public class Logica
    {
        string dataBaseName = "compMgr.sqlite";

        string enableFK = "PRAGMA foreign_keys=on;";

        string connectionString;

        DataTable users;
        DataTable division;
        DataTable software;
        DataTable computer;
        DataTable install;

        public Logica()
        {
            connectionString = $"DataSource={dataBaseName};Version=3;";
        }



        public ErrorMessageHelper UpdateTable(string tableName, DataTable newTable)
        {
            try
            {
                if (newTable != null)
                {

                    using (DataTableReader newReader = newTable.CreateDataReader())
                    {
                        LogicDataSet.Tables[$"{tableName}"].Clear();
                        LogicDataSet.Tables[$"{tableName}"].Load(newReader);
                    }

                    return new ErrorMessageHelper();
                }
                else
                {
                    throw new NullReferenceException("Переданной таблицы не существует");
                }
            }
            catch (Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
        }

        public List<Updates> GetUpdates()
        {
            List<Updates> updateList = new List<Updates>();

           // var updates = from dr in 

            return updateList;
        }


        /// <summary>
        /// Включает поддержку внешних ключей для установленного соединения
        /// </summary>
        /// <param name="connection">Установленное соединение</param>
        private void EnableForeignKeys(SQLiteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand enFk = new SQLiteCommand(connection);
                enFk.CommandText = enableFK;
                enFk.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Создаёт таблички в БД
        /// </summary>
        private void CreateTables()
        {
            using (SQLiteConnection createConnection = new SQLiteConnection(connectionString))
            {
                createConnection.Open();

                EnableForeignKeys(createConnection);

                SQLiteCommand transaction = new SQLiteCommand(createConnection);
                transaction.CommandText = "BEGIN TRANSACTION";
                transaction.ExecuteNonQuery();

                //Создаём таблицу Software
                SQLiteCommand createDb = new SQLiteCommand(createConnection);
                string createSoftDB = "CREATE TABLE IF NOT EXISTS Software (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "name TEXT NOT NULL, version TEXT NOT NULL)";
                createDb.CommandText = createSoftDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Users
                string createUserDB = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "fio TEXT NOT NULL, tel INTEGER )";
                createDb.CommandText = createUserDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Division
                string createDivDB = "CREATE TABLE IF NOT EXISTS Division (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "name TEXT NOT NULL )";
                createDb.CommandText = createDivDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Computer
                string createCompDB = "CREATE TABLE IF NOT EXISTS Computer (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "nsName TEXT NOT NULL, ip TEXT, divID INTEGER, userID INTEGER, " +
                    "FOREIGN KEY (divId) REFERENCES Division(id), FOREIGN KEY (userID) REFERENCES Users(id))";
                createDb.CommandText = createCompDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Install
                string createInstDB = "CREATE TABLE IF NOT EXISTS Install (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "computerID INTEGER, softID INTEGER, version TEXT NOT NULL, " +
                    "FOREIGN KEY (computerId) REFERENCES Computer(id) , FOREIGN KEY (softID) REFERENCES Software(id))";
                createDb.CommandText = createInstDB;
                createDb.ExecuteNonQuery();

                transaction.CommandText = "COMMIT";
                transaction.ExecuteNonQuery();

                createConnection.Close();
            }
        }

        /// <summary>
        /// Создаём внешние улючи в датасете
        /// </summary>
        private void CreateFK()
        {
            ForeignKeyConstraint userComp = new ForeignKeyConstraint(users.Columns["id"], computer.Columns["userID"])
            {
                ConstraintName = "user-comp",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.SetNull
            };
            computer.Constraints.Add(userComp);

            LogicDataSet.Relations.Add("user-comp", users.Columns["id"], computer.Columns["userID"]);

            ForeignKeyConstraint divComp = new ForeignKeyConstraint(division.Columns["id"], computer.Columns["divID"])
            {
                ConstraintName = "division-comp",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.SetNull
            };
            computer.Constraints.Add(divComp);

            LogicDataSet.Relations.Add("division-comp", division.Columns["id"], computer.Columns["divID"]);

            ForeignKeyConstraint compInst = new ForeignKeyConstraint(computer.Columns["id"], install.Columns["computerID"])
            {
                ConstraintName = "comp-install",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };
            install.Constraints.Add(compInst);

            LogicDataSet.Relations.Add("comp-install", computer.Columns["id"], install.Columns["computerID"]);

            ForeignKeyConstraint softInst = new ForeignKeyConstraint(software.Columns["id"], install.Columns["softID"])
            {
                ConstraintName = "soft-install",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            install.Constraints.Add(softInst);

            LogicDataSet.Relations.Add("soft-install", software.Columns["id"], install.Columns["softID"]);

            LogicDataSet.EnforceConstraints = true;
        }

        /// <summary>
        /// Загружает таблица из базы данных
        /// </summary>
        /// <param name="connection">Установленное соединение к БД</param>
        /// <param name="tableName">Название таблицы</param>
        private void LoadTable(SQLiteConnection connection, string tableName)
        {
            //Проверка на наличие соединения
            if (connection.State == ConnectionState.Open)
            {
                //Загружаем табличку из БД
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter($"SELECT * FROM {tableName}", connection))
                {
                    adapter.FillSchema(LogicDataSet, SchemaType.Source, tableName);
                    adapter.Fill(LogicDataSet, tableName);
                }
            }
        }


        /// <summary>
        /// Сохраняем таблицу в БД
        /// </summary>
        /// <param name="connection">Открытое соединение с БД</param>
        /// <param name="tableName">Сохраняемая таблица</param>
        private void SaveTable(SQLiteConnection connection, string tableName)
        {
            if(connection.State == ConnectionState.Open)
            {
                //Синхронизируем таблицы в датасете и БД (потом перенести в обновление бд)
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter($"SELECT * FROM {tableName}", connection))
                {
                    SQLiteCommandBuilder commands = new SQLiteCommandBuilder(adapter);
                    adapter.Update(LogicDataSet, tableName);
                }
            }
        }


        /// <summary>
        /// Сохраняем таблицы в БД
        /// </summary>
        /// <returns>Отчёт об ошибках</returns>
        public ErrorMessageHelper Save()
        {
            try
            {
                using (SQLiteConnection saveConnect = new SQLiteConnection(connectionString))
                {
                    saveConnect.Open();

                    EnableForeignKeys(saveConnect);

                    SaveTable(saveConnect, "Software");
                    SaveTable(saveConnect, "Users");
                    SaveTable(saveConnect, "Division");
                    SaveTable(saveConnect, "Computer");
                    SaveTable(saveConnect, "Install");

                    saveConnect.Close();

                }

                return new ErrorMessageHelper();
            }
            catch (Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }

        }

        /// <summary>
        /// Инизиализация и загрузка таблиц
        /// </summary>
        /// <returns>отчёт об ошибках</returns>
        public ErrorMessageHelper InitialDB()
        {
            try
            {
                if (!File.Exists(dataBaseName))
                {
                    SQLiteConnection.CreateFile(dataBaseName);
                    CreateTables();
                }

                using (SQLiteConnection loadCon = new SQLiteConnection(connectionString))
                {
                    loadCon.Open();

                    EnableForeignKeys(loadCon);

                    LoadTable(loadCon, "Software");
                    LoadTable(loadCon, "Users");
                    LoadTable(loadCon, "Division");
                    LoadTable(loadCon, "Computer");
                    LoadTable(loadCon, "Install");

                    loadCon.Close();

                }

                software = LogicDataSet.Tables["Software"];
                users = LogicDataSet.Tables["Users"];
                division = LogicDataSet.Tables["Division"];
                computer = LogicDataSet.Tables["Computer"];
                install = LogicDataSet.Tables["Install"];

                CreateFK();

                return new ErrorMessageHelper();
            }
            catch (Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
        }

        public void AddSomeData()
        {
            DataRow inst = install.NewRow();
            inst["softID"] = 1;
            inst["computerID"] = 1;
            inst["version"] = "000";
            install.Rows.Add(inst);
        }


        public DataSet LogicDataSet { get; set; } = new DataSet();

    }
}
