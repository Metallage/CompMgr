using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace CompMgr
{
    public class Logica
    {
        string dataBaseName = "compMgr.sqlite";

        private SettingsHelper settings;

        SQLiteConnection dbCon;

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

        public Logica()
        {
            //if(!File.Exists(dataBaseName))
            //{
            //    SQLiteConnection.CreateFile(dataBaseName);
            //}
            //dbCon = new SQLiteConnection($"DataSource={dataBaseName};Version=3;");
        }

        public ErrorMessageHelper InitialDB()
        {
            try
            {
                CreateCompBase();
                CreateDivisionBase();
                CreateInstallBase();
                CreateSoftBase();
                CreateUserBase();
                return new ErrorMessageHelper();
            }
            catch(Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
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

        #region Первичное создание таблиц

        /// <summary>
        /// Создаёт таблицу  ПО и версий
        /// </summary>
        private void CreateSoftBase()
        {
            //Создаём таблицу в датасете
            DataTable softwareTable = new DataTable();
            softwareTable.TableName = "Software";
            
            //Примари кей ID 
            DataColumn idSoftware = new DataColumn("id", typeof(int));
            idSoftware.AutoIncrement = true;
            idSoftware.AllowDBNull = false;
            idSoftware.Unique = true;
            idSoftware.Caption = "Software ID";
            idSoftware.ReadOnly = true;
            idSoftware.AutoIncrementSeed = 1;
            softwareTable.Columns.Add(idSoftware);
            softwareTable.PrimaryKey = new DataColumn[] { idSoftware };
            
            //Название ПО
            DataColumn nameSoftware = new DataColumn("name", typeof(string));
            nameSoftware.Caption = "Название ПО";
            softwareTable.Columns.Add(nameSoftware);
            
            //Версия ПО
            DataColumn currentVersion = new DataColumn("version", typeof(string));
            currentVersion.Caption = "Текущая версия";
            softwareTable.Columns.Add(currentVersion);
            
            
            //Добавляем ордер
            DataRow orderRow = softwareTable.NewRow();
            orderRow["name"] = "Ордер";
            orderRow["version"] = "000";
            softwareTable.Rows.Add(orderRow);

            LogicDataSet.Tables.Add(softwareTable);
        }

        /// <summary>
        /// Создаём таблицу компьютеров
        /// </summary>
        private void CreateCompBase()
        {
            DataTable compTable = new DataTable();
            compTable.TableName = "Computer";

            //Первичный ключ
            DataColumn idComp = new DataColumn("id", typeof(int));
            idComp.AutoIncrement = true;
            idComp.AllowDBNull = false;
            idComp.Unique = true;
            idComp.Caption = "Computer ID";
            idComp.ReadOnly = true;
            idComp.AutoIncrementSeed = 1;
            compTable.Columns.Add(idComp);
            compTable.PrimaryKey = new DataColumn[] { idComp };

            //ДНС имя компа
            DataColumn nsName = new DataColumn("nsName", typeof(string));
            nsName.Caption = "Имя компьютера";
            nsName.AllowDBNull = false;
            compTable.Columns.Add(nsName);

            //IP адрес компа
            DataColumn ipAdr = new DataColumn("ip", typeof(string));
            ipAdr.Caption = "IP адрес";
            ipAdr.AllowDBNull = false;
            compTable.Columns.Add(ipAdr);

            //IP адрес компа
            DataColumn division = new DataColumn("divID", typeof(int));
            division.Caption = "Подразделение";
            compTable.Columns.Add(division);

            DataColumn user = new DataColumn("userID", typeof(int));
            user.Caption = "Пользователь";
            compTable.Columns.Add(user);

            LogicDataSet.Tables.Add(compTable);

            //Для тестов
            DataRow local1 = compTable.NewRow();
            local1["nsName"] = "localhost1"; 
            local1["ip"] = "127.0.0.1";
            compTable.Rows.Add(local1);

            DataRow local2 = compTable.NewRow();
            local2["nsName"] = "localhost2";
            local2["ip"] = "127.0.0.2";
            compTable.Rows.Add(local2);
        }

        private void CreateUserBase()
        {
            DataTable users = new DataTable();
            users.TableName = "Users";

            //Первичный ключ
            DataColumn idUser = new DataColumn("id", typeof(int));
            idUser.AutoIncrement = true;
            idUser.AllowDBNull = false;
            idUser.Unique = true;
            idUser.Caption = "Computer ID";
            idUser.ReadOnly = true;
            idUser.AutoIncrementSeed = 1;
            users.Columns.Add(idUser);
            users.PrimaryKey = new DataColumn[] { idUser };

            //ФИО пользователя
            DataColumn fio = new DataColumn("fio",typeof(string));
            fio.Caption = "ФИО Пользователя";
            fio.AllowDBNull = false;
            users.Columns.Add(fio);

            //Телефон пользователя
            DataColumn telNum = new DataColumn("tel", typeof(int));
            telNum.Caption = "Телефон пользователя";
            users.Columns.Add(telNum);

            LogicDataSet.Tables.Add(users);

            //Для тестов
            DataRow user1 = users.NewRow();
            user1["fio"] = "Тестов Т.Т.";
            users.Rows.Add(user1);

        }

        /// <summary>
        /// Создаёт таблицу подразделений
        /// </summary>
        private void CreateDivisionBase()
        {
            DataTable division = new DataTable();
            division.TableName = "Division";

            //Первичный ключ
            DataColumn idDiv = new DataColumn("id", typeof(int));
            idDiv.AutoIncrement = true;
            idDiv.AllowDBNull = false;
            idDiv.Unique = true;
            idDiv.Caption = "Computer ID";
            idDiv.ReadOnly = true;
            idDiv.AutoIncrementSeed = 1;
            division.Columns.Add(idDiv);
            division.PrimaryKey = new DataColumn[] { idDiv };

            //Наименование подразделения
            DataColumn name = new DataColumn("name", typeof(string));
            name.Caption = "Подразделение";
            name.AllowDBNull = false;
            division.Columns.Add(name);

            LogicDataSet.Tables.Add(division);

            //Для тестов
            DataRow div1 = division.NewRow();
            div1["name"] = "Просто пост";
            division.Rows.Add(div1);

        }

        /// <summary>
        /// Создаёт таблицу установок ПО
        /// </summary>
        private void CreateInstallBase()
        {
            DataTable install = new DataTable();
            install.TableName = "Install";
            
            //Первичный ключ
            DataColumn idInst = new DataColumn("id", typeof(int));
            idInst.AutoIncrement = true;
            idInst.AllowDBNull = false;
            idInst.Unique = true;
            idInst.Caption = "Computer ID";
            idInst.ReadOnly = true;
            idInst.AutoIncrementSeed = 1;
            install.Columns.Add(idInst);
            install.PrimaryKey = new DataColumn[] { idInst };

            //ID компьютера
            DataColumn computer = new DataColumn("computerID", typeof(int));
            computer.Caption = "Компьютер";
            install.Columns.Add(computer);

            //ID ПО
            DataColumn soft = new DataColumn("softID", typeof(int));
            computer.Caption = "ПО";
            install.Columns.Add(soft);


            //Установленная версия ПО
            DataColumn version = new DataColumn("version", typeof(int));
            version.Caption = "Версия ПО";
            install.Columns.Add(version);

            LogicDataSet.Tables.Add(install);

        }


        #endregion

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
                    softAdapter.FillSchema(LogicDataSet, SchemaType.Source, "Software");
                    softAdapter.Fill(LogicDataSet, "Software");
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

                LogicDataSet.Tables.Add(softwareTable);

                //Пишем таблицу в БД
                softwareAdapter.Update(LogicDataSet, "Software");
                createSoftwareConnection.Close();
            }
        }

        public DataSet LogicDataSet { get; set; } = new DataSet();

    }
}
