using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace CompMgr
{
    class Logica
    {
        private SettingsHelper settings;

        private DataSet logicDataSet = new DataSet();

        SQLiteConnection softwareConnection;

        public Logica(SettingsHelper settings)
        {
            this.settings = settings;
            if (!File.Exists(settings.SoftwareBase))
            {
                CreateSettingsBase();
            }
            softwareConnection = new SQLiteConnection($"DataSource={settings.SoftwareBase};Version=3;");
            softwareConnection.Open();
            LoadSoftwareBD();
        }

        /// <summary>
        /// Загружает данные о версиях ПО из БД в рабочий DataSet
        /// </summary>
        private void LoadSoftwareBD()
        {
            //Проверка на наличие соединения
            if (softwareConnection.State == ConnectionState.Open)
            {
                //Загружаем табличку из БД
                using (SQLiteDataAdapter softAdapter = new SQLiteDataAdapter("SELECT * FROM Software", softwareConnection))
                {
                    softAdapter.FillSchema(logicDataSet, SchemaType.Source, "Software");
                    softAdapter.Fill(logicDataSet, "Software");
                }
            }
        }

        /// <summary>
        /// Создает БД для версий ПО
        /// </summary>
        private void CreateSettingsBase()
        {
            //Создаём файл БД
            SQLiteConnection.CreateFile(settings.SoftwareBase);
            //Создаёс соединение с файлом БД
            using (SQLiteConnection createSoftwareConnection = new SQLiteConnection($"DataSource={settings.SoftwareBase};Version=3;"))
            {
                createSoftwareConnection.Open();
                //Создаём табличку Software
                string createString = "CREATE TABLE IF NOT EXISTS Software (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, version TEXT)";
                SQLiteCommand createSoftware = new SQLiteCommand(createString);
                createSoftware.Connection = createSoftwareConnection;
                createSoftware.ExecuteNonQuery();

                //Создаём адаптер для связи таблички в датасете и в БД
                SQLiteDataAdapter softwareAdapter = new SQLiteDataAdapter("SELECT * FROM Software", createSoftwareConnection);
                SQLiteCommandBuilder softSQLbuilt = new SQLiteCommandBuilder(softwareAdapter);

                //Создаём таблицу в датасете
                DataTable softwareTable = new DataTable();
                softwareTable.TableName = "Software";
                //Примари кей
                DataColumn idSoftware = new DataColumn("id", typeof(int));
                idSoftware.AutoIncrement = true;
                idSoftware.AllowDBNull = false;
                idSoftware.Unique = true;
                idSoftware.Caption = "Software ID";
                idSoftware.ReadOnly = true;
                idSoftware.AutoIncrementSeed = 1;
                softwareTable.Columns.Add(idSoftware);
                softwareTable.PrimaryKey = new DataColumn[] { idSoftware };
                //Название
                DataColumn nameSoftware = new DataColumn("name", typeof(string));
                nameSoftware.Caption = "Название ПО";
                softwareTable.Columns.Add(nameSoftware);
                //Версия
                DataColumn currentVersion = new DataColumn("version", typeof(string));
                currentVersion.Caption = "Текущая версия";
                softwareTable.Columns.Add(currentVersion);
                //Добавляем ордер
                DataRow orderRow = softwareTable.NewRow();
                orderRow["name"] = "Ордер";
                orderRow["version"] = "000";
                softwareTable.Rows.Add(orderRow);

                logicDataSet.Tables.Add(softwareTable);

                //Пишем таблицу в БД
                softwareAdapter.Update(logicDataSet, "Software");
                createSoftwareConnection.Close();
            }
        }

        public DataSet LogicDataSet
        {
            get
            {
                return logicDataSet;
            }
        }

    }
}
