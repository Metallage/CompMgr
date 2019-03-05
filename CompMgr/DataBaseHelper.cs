using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace CompMgr
{
    /// <summary>
    /// Класс для обмена с sqlite БД
    /// </summary>
    public class DataBaseHelper
    {
        //Путь к БД
        private string dataBaseName = "compMgr.sqlite";

        //Строка подключения поддержки внешних ключей
        private string enableFK = "PRAGMA foreign_keys=on;";

        //Строка соединения с БД
        string connectionString;
        
        private DataTable user;
        private DataTable software;
        private DataTable computer;
        private DataTable install;
        private DataTable distribution;

        public DataBaseHelper()
        {
            connectionString = $"DataSource={dataBaseName};Version=3;";
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
                string createSoftDB = "CREATE TABLE IF NOT EXISTS Software (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "softName TEXT NOT NULL UNIQUE, version TEXT NOT NULL)";
                createDb.CommandText = createSoftDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Users
                string createUserDB = "CREATE TABLE IF NOT EXISTS User (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "fio TEXT NOT NULL, tel TEXT )";
                createDb.CommandText = createUserDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Computer
                string createCompDB = "CREATE TABLE IF NOT EXISTS Computer (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                    "nsName TEXT NOT NULL UNIQUE, ip TEXT)";
                createDb.CommandText = createCompDB;
                createDb.ExecuteNonQuery();

                //Создаём таблицу Install
                string createInstDB = "CREATE TABLE IF NOT EXISTS Install (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "computerID INTEGER, softID INTEGER, version TEXT NOT NULL, " +
                    "FOREIGN KEY (computerId) REFERENCES Computer(id), FOREIGN KEY (softID) REFERENCES Software(id))";
                createDb.CommandText = createInstDB;
                createDb.ExecuteNonQuery();

                string createDistribDB = "CREATE TABLE IF NOT EXISTS Distribution (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "computerID INTEGER NOT NULL, userID INTEGER NOT NULL, " +
                    "FOREIGN KEY (computerID) REFERENCES Computer(id), FOREIGN KEY (userID) REFERENCES user(id))";
                createDb.CommandText = createDistribDB;
                createDb.ExecuteNonQuery();

                transaction.CommandText = "COMMIT";
                transaction.ExecuteNonQuery();

                createConnection.Close();
            }
        }

        private void CreatePK()
        {
            user.PrimaryKey = new DataColumn[] { user.Columns["id"] };
            software.PrimaryKey = new DataColumn[] { software.Columns["id"] };
            computer.PrimaryKey = new DataColumn[] { computer.Columns["id"] };
            install.PrimaryKey = new DataColumn[] { install.Columns["id"] };
            distribution.PrimaryKey = new DataColumn[] { distribution.Columns["id"] };

        }

        /// <summary>
        /// Создаём внешние ключи в датасете
        /// </summary>
        private void CreateFK()
        {

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

            ForeignKeyConstraint compDistr = new ForeignKeyConstraint(computer.Columns["id"], distribution.Columns["computerID"])
            {
                ConstraintName = "comp-distribution",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            distribution.Constraints.Add(compDistr);

            LogicDataSet.Relations.Add("comp-distribution", computer.Columns["id"], distribution.Columns["computerID"]);

            ForeignKeyConstraint userDistrib = new ForeignKeyConstraint(user.Columns["id"], distribution.Columns["userID"])
            {
                ConstraintName = "user-distribution",
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade                
            };

            distribution.Constraints.Add(userDistrib);

            LogicDataSet.Relations.Add("user-distribution", user.Columns["id"], distribution.Columns["userID"]);

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
                    //adapter.FillSchema(LogicDataSet, SchemaType.Source, tableName);
                    adapter.Fill(LogicDataSet, tableName);
                    LogicDataSet.Tables[tableName].Columns["id"].AutoIncrement = true;
                    long maxIndex = 0;
                    foreach (DataRow dr in LogicDataSet.Tables[tableName].Rows)
                        maxIndex = Math.Max(maxIndex, dr.Field<long>("id"));
                    LogicDataSet.Tables[tableName].Columns["id"].AutoIncrementSeed = maxIndex  + 1;
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
                    adapter.DeleteCommand = new SQLiteCommand($"DELETE FROM {tableName} WHERE id = @id", connection);
                    adapter.DeleteCommand.Parameters.Add("@id", DbType.Int32, 0, "id").SourceVersion = DataRowVersion.Original;
                    adapter.Update(LogicDataSet, tableName);
                }
            }
        }


        /// <summary>
        /// Сохраняем таблицы в БД TODO переписать под новое ядро логики
        /// </summary>
        /// <returns>Отчёт об ошибках</returns>
        public void Save()
        {
            using (SQLiteConnection saveConnect = new SQLiteConnection(connectionString))
            {
                saveConnect.Open();

                EnableForeignKeys(saveConnect);

                SaveTable(saveConnect, "Software");
                SaveTable(saveConnect, "User");
                SaveTable(saveConnect, "Computer");
                SaveTable(saveConnect, "Install");
                SaveTable(saveConnect, "Distribution");
                saveConnect.Close();

            }

        }

        public void UpdateInstall()
        {
            using (SQLiteConnection saveConnect = new SQLiteConnection(connectionString))
            {
                saveConnect.Open();
                EnableForeignKeys(saveConnect);

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT id, computerID, softID, version FROM Install" , saveConnect))
                {
                    adapter.DeleteCommand = new SQLiteCommand("DELETE FROM Install WHERE id = @id", saveConnect);
                    adapter.DeleteCommand.Parameters.Add("@id", DbType.Int64, 8, "id").SourceVersion = DataRowVersion.Original;
                    adapter.InsertCommand = new SQLiteCommand("INSERT INTO INSTALL(computerID, softID, version) " +
                        "VALUES (@computerID, @softID, @version)", saveConnect);
                    adapter.InsertCommand.Parameters.Add("@computerID", DbType.Int64, 8, "computerID").SourceVersion = DataRowVersion.Original;
                    adapter.InsertCommand.Parameters.Add("@softID", DbType.Int64, 8, "softID").SourceVersion = DataRowVersion.Original;
                    adapter.InsertCommand.Parameters.Add("@version", DbType.String, 16, "version").SourceVersion = DataRowVersion.Original;
                    adapter.UpdateCommand = new SQLiteCommand("UPDATE Install SET version=@version WHERE id=@id", saveConnect);
                    adapter.UpdateCommand.Parameters.Add("@id", DbType.Int64, 8, "id").SourceVersion = DataRowVersion.Original;
                    adapter.UpdateCommand.Parameters.Add("@version", DbType.String, 16, "version").SourceVersion = DataRowVersion.Original;

                    // DataTable instch = install.GetChanges(DataRowState.Deleted);
                    adapter.Update(install);

                }
                saveConnect.Close();
            }

        }

        public void Reload()
        {
            using (SQLiteConnection reLoadCon = new SQLiteConnection(connectionString))
            {
                reLoadCon.Open();

                EnableForeignKeys(reLoadCon); //Включаем поддержку внешних ключей

                LogicDataSet.Clear();

                Load(reLoadCon);
                //foreach (DataTable dt in LogicDataSet.Tables)
                //    dt.BeginLoadData();

                //foreach (DataTable dt in LogicDataSet.Tables)
                //{
                //    string tablename = dt.TableName;
                //    dt.Clear();
                //    LoadTable(reLoadCon, tablename);

                //}

                //foreach (DataTable dt in LogicDataSet.Tables)
                //{
                //    dt.EndLoadData();
                //    dt.AcceptChanges();
                //}

                reLoadCon.Close();
            }
        }

        /// <summary>
        /// Инизиализация и загрузка таблиц
        /// </summary>
        /// <returns>отчёт об ошибках</returns>
        public void InitialDB()
        {
            //Если файла бд нет, то создаём его и создаём в нем нужные таблицы
            if (!File.Exists(dataBaseName))
            {
                SQLiteConnection.CreateFile(dataBaseName);
                CreateTables();
            }

            //Загружаем таблицы из файла БД
            using (SQLiteConnection loadCon = new SQLiteConnection(connectionString))
            {
                loadCon.Open();

                EnableForeignKeys(loadCon); //Включаем поддержку внешних ключей
                Load(loadCon);


                loadCon.Close();

                //Создаём ключи
                CreateFK();
            }
        }

        

        /// <summary>
        /// Датасет с таблицами связями и ключами
        /// </summary>
        public DataSet LogicDataSet { get; set; } = new DataSet();


        /// <summary>
        /// Загружает все таблицы из БД инициализирует поля, создаёт ключи
        /// </summary>
        /// <param name="loadCon">Установленное соединение к БД</param>
        private void Load(SQLiteConnection loadCon)
        {
            if (loadCon.State == ConnectionState.Open)
            {
                //Загружаем таблицы
                LoadTable(loadCon, "Software");
                LoadTable(loadCon, "User");
                LoadTable(loadCon, "Computer");
                LoadTable(loadCon, "Install");
                LoadTable(loadCon, "Distribution");

                //Инициализируем поля
                software = LogicDataSet.Tables["Software"];
                user = LogicDataSet.Tables["User"];
                computer = LogicDataSet.Tables["Computer"];
                install = LogicDataSet.Tables["Install"];
                distribution = LogicDataSet.Tables["Distribution"];

                CreatePK();

                //разобраться нужно ли
                foreach (DataTable dt in LogicDataSet.Tables)
                    dt.AcceptChanges();
            }
        }

    }
}
