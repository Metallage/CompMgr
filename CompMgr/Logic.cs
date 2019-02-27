using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompMgr
{
    /// <summary>
    /// Класс ядра логики
    /// </summary>
    public class Logic
    {
        private DataSet mainDS;
        private DataTable user;
        private DataTable division;
        private DataTable software;
        private DataTable computer;
        private DataTable install;
        private DataTable distribution;

        private DataBaseHelper dbHelper = new DataBaseHelper();

        public ErrorMessageHelper Start()
        {
            try
            {
                dbHelper.InitialDB();
                mainDS = dbHelper.LogicDataSet;

                software = mainDS.Tables["Software"];
                user = mainDS.Tables["User"];
                division = mainDS.Tables["Division"];
                computer = mainDS.Tables["Computer"];
                install = mainDS.Tables["Install"];
                distribution = mainDS.Tables["Distribution"];

                AddSomeData(); //Для тестов

                return new ErrorMessageHelper();
            }
            catch(Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
        }


        /// <summary>
        /// Формирует список обновлений для ПО
        /// </summary>
        /// <param name="upSoft">Название ПО</param>
        /// <returns>Список рабочих мест, где нужно обновить данное ПО</returns>
        public List<Updates> GetUpdates(string upSoft)
        {

            List<Updates> updateList = new List<Updates>();

            var updQuery = from comp in computer.AsEnumerable() //Из таблицы компов
                           join dist in distribution.AsEnumerable() on comp.Field<long>("id") equals dist.Field<long>("computerID") //Выбираем те, что разпределены пользователям
                           join usr in user.AsEnumerable() on dist.Field<long>("userID") equals usr.Field<long>("id") //Выбираем пользователей которым распределены компы
                           join inst in install.AsEnumerable() on comp.Field<long>("id") equals inst.Field<long>("computerID") //Выбираем те где есть установленное по
                           join soft in software.AsEnumerable() on inst.Field<long>("softID") equals soft.Field<long>("id") //Выбираем список ПО на этих компах
                           where soft.Field<string>("name") == upSoft && soft.Field<string>("version") !=inst.Field<string>("version") // Выбираем те, что имеют нужное ПО и неправильную версию
                           select new {NsName = comp.Field<string>("nsName"), Ip=comp.Field<string>("ip"), User = usr.Field<string>("fio")}; //Формируем запись

            foreach(dynamic upd in updQuery)
            {
                updateList.Add(new Updates(upd.NsName,upd.Ip, upd.User)); //Добавляем всё найденное в лист обновлений
            }

            return updateList;
        }

        public Dictionary<string, string> GetDistr() //Нужен ли?
        {
            Dictionary<string, string> retDist = new Dictionary<string, string>();

            var distQuery = from comp in computer.AsEnumerable()
                            join dist in distribution.AsEnumerable() on comp.Field<long>("id") equals dist.Field<long>("computerID")
                            join usr in user.AsEnumerable() on dist.Field<long>("userID") equals usr.Field<long>("id")
                            select new {Fio = usr.Field<string>("fio"), NsName = comp.Field<string>("nsName") };

            foreach(dynamic dst in distQuery)
            {
                retDist.Add(dst.Fio, dst.NsName);
            }

            return retDist;
        }

        #region Получение данных для интерфейса

        /// <summary>
        /// Формируем список компьютеров в понятный интерфейсу вид
        /// </summary>
        /// <returns>Лист классов компьютер</returns>
        public HashSet<Computer> GetComputers()
        {
            HashSet<Computer> computers = new HashSet<Computer>();

            //Выбираем все компьютеры с подразделениями
            var compQuer = from comp in computer.AsEnumerable()
                           join div in division.AsEnumerable() on comp.Field<long>("divisionID") equals div.Field<long>("id")
                           select new {Id = comp.Field<long>("id"), NsName=comp.Field<string>("nsName"), Ip=comp.Field<string>("ip"), Division=div.Field<string>("name") };

            foreach (dynamic comp in compQuer)
            {
                Computer newComp = new Computer(comp.NsName, comp.Ip);
                newComp.DivisionName = comp.Division;

                //Ищем есть ли пользователи которым назначен этот комп
                if (distribution.Select($"computerID = {comp.Id}").Count() > 0)
                {
                    var userComp = from dist in distribution.Select($"computerID = {comp.Id}").CopyToDataTable().AsEnumerable()
                                   join usr in user.AsEnumerable() on dist.Field<long>("userId") equals usr.Field<long>("id")
                                   select usr.Field<string>("fio");

                    if (userComp.Count() == 1)
                        newComp.UserFio = userComp.First();
                }
                computers.Add(newComp);
            }

            return computers;
        }

        /// <summary>
        /// Возвращает список всего софта с версиями в понятном интерфейсу виде
        /// </summary>
        /// <returns>Список софта с версиями</returns>
        public HashSet<Software> GetSoftware()
        {
            HashSet<Software> softList = new HashSet<Software>();

            var softQuery = from soft in software.AsEnumerable()
                            select new { Name = soft.Field<string>("name"), Ver = soft.Field<string>("version") };
            foreach(dynamic soft in softQuery)
            {
                Software sf = new Software(soft.Name);
                sf.SoftwareVersion = soft.Ver;
                softList.Add(sf);
            }
            return softList;
        }


        /// <summary>
        /// Возвращает список пользователей в понятном для интерфейса виде
        /// </summary>
        /// <returns>Список пользователей</returns>
        public HashSet<User> GetUsers()
        {
            HashSet<User> users = new HashSet<User>();

            foreach(DataRow usr in user.Select())
            {
                User newUser = new User();
                newUser.UserFio = usr.Field<string>("fio");
                newUser.UserTel = usr.Field<string>("tel");
                users.Add(newUser);
            }

            return users;
        }


        public HashSet<Division> GetDivision()
        {
            HashSet<Division> divisions = new HashSet<Division>();
            foreach(DataRow dr in division.Rows)
            {
                divisions.Add(new Division(dr.Field<string>("name")));
            }

            return divisions;
        }


        public List<CompleteTableHelper> GetCompleteTable()
        {
            List<CompleteTableHelper> compData = new List<CompleteTableHelper>();

            foreach(DataRow softdr in software.Select())
            {
                string softName = softdr.Field<string>("name");
                string currentversion = softdr.Field<string>("version");

                CompleteTableHelper cth = new CompleteTableHelper(softName,currentversion);
                cth.CompNames = GetCompIsUp(softName, currentversion);
                compData.Add(cth);

            }
            return compData;
        }

        #endregion

        /// <summary>
        /// Парсинг входной строки для добавления компьютера
        /// </summary>
        /// <param name="data">Входные данные</param>
        public HashSet<Computer> ParseComp(string data)
        {
            HashSet<Computer> outputComp = new HashSet<Computer>();
            char[] paramSeparators = { ';', ',' };
            char[] compSeparators = { '\r', '\n' };
            string[] comps =  data.Split(compSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string comp in comps)
            {
                
                string[] param = comp.Split(paramSeparators);
                Computer newComp = new Computer(param[0], param[1]);
                if(param.Count()>2)
                {
                    newComp.DivisionName = param[2];
                    if (param.Count() == 4)
                        newComp.UserFio = param[3];
                }

                outputComp.Add(newComp);

            }
            return outputComp;
        }

        /// <summary>
        /// Обновляет таблицу компьютеров и связанную с ней таблицу назначений компьютеров пользователю
        /// </summary>
        /// <param name="comps">Обновлённый список компьютеров</param>
        public void UpdateComp(HashSet<Computer> comps)
        {
            foreach(Computer comp in comps)
            {
                long id = FindCompID(comp.NsName); //Пытаемся найти ID компьютера
                long divID = FindDivisionID(comp.DivisionName); //Пытаемся найти ID подразделения
                string userFIO = null;
                if ((comp.UserFio != null) && (comp.UserFio != "") && (comp.UserFio != " ")) //Если имя пользователя не пустое
                    userFIO = comp.UserFio; 
                
                long userID = FindUserID(userFIO);//Пытаемся найти имя пользователя

                if (id == -1) //Если такого компьютера не найдено
                {
                    DataRow newRow = computer.NewRow(); //создаём новую запись
                    newRow["nsName"] = comp.NsName;
                    newRow["ip"] = comp.Ip;

                    if ((comp.DivisionName != null) && (divID != -1))
                    {
                        newRow["divisionID"] = divID;
                    }
                    computer.Rows.Add(newRow);
                }
                else //Если такой уже есть, редактируем
                {
                    DataRow upRow = computer.Rows.Find(id);
                    upRow["ip"] = comp.Ip;
                    if ((comp.DivisionName != null) && (divID != -1))
                    {
                        upRow["divisionID"] = divID;
                    }
                }

                //Если компу назначен пользователь
                if ((comp.UserFio != null) && (userID != -1))
                {
                    long distributionID = FindDistributionID(id, userID); //Пытаемся найти связанную запись о назначении
                    if (distributionID == -1) //Если такой нет
                    {
                        DeleteDistributionByUserOrCompID(id, userID); //Снимаем все назначения для выбранных компьютера и пользователя
                        DataRow newDistrib = distribution.NewRow(); //Создаём новую запись назначения
                        newDistrib["computerID"] = id;
                        newDistrib["userID"] = userID;
                        distribution.Rows.Add(newDistrib);
                    }
                }
                else if (comp.UserFio == null) //Если у компа нет пользователя
                {
                    DeleteDistributionByUserOrCompID(id, -1); //Удаляем все записи о назначении для этого компа
                }
            }
        }

        #region Поиск ID по таблицам

        /// <summary>
        /// Поиск ID пользователя по имени
        /// </summary>
        /// <param name="userName">Имя</param>
        /// <returns></returns>
        private long FindUserID(string userName)
        {
            if (userName == null)
                return -1;
            else
            {
                var idsQuer = from ids in user.Select($"fio LIKE '{userName}%'").AsEnumerable()
                              select ids.Field<long>("id");
                if (idsQuer.Count() == 1)
                {
                    return (long)idsQuer.First();
                }
                else
                    return -1;
            }          
        }


        /// <summary>
        /// Поиск ID подразделения по названию
        /// </summary>
        /// <param name="divisionName">название подразделения</param>
        /// <returns>ID</returns>
        private long FindDivisionID(string divisionName)
        {
            if (divisionName == null)
                return -1;
            else
            {
                var divQuer = from div in division.Select($"name = '{divisionName}'").AsEnumerable()
                              select div.Field<long>("id");
                if (divQuer.Count() == 1)
                    return (long)divQuer.First();
                else
                    return -1;
            }
        }

        /// <summary>
        /// Поиск ID компа по имени
        /// </summary>
        /// <param name="nsName">имя компа</param>
        /// <returns>ID</returns>
        private long FindCompID(string nsName)
        {
            var compQuery = from comp in computer.Select($"nsName = '{nsName}'").AsEnumerable()
                            select comp.Field<long>("id");
            if (compQuery.Count() == 1)
                return (long)compQuery.First();
            else
                return -1;
        }

        /// <summary>
        /// Поиск ID назначения по ID пользователя или ID компьютера
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private long FindDistributionID(long compID, long userID)
        {
            var distrQuery = from dist in distribution.Select($"(computerID = {compID})AND(userID = {userID})").AsEnumerable()
                             select dist.Field<long>("id");
            if (distrQuery.Count() == 1)
                return distrQuery.First();
            else
                return -1;
        }

        #endregion

        /// <summary>
        /// Удаляет все распреления компов для конкретного пользователя или компа
        /// </summary>
        /// <param name="compID">ID компа/param>
        /// <param name="userID">ID пользователя/param>
        private void DeleteDistributionByUserOrCompID( long compID, long userID)
        {
            var distQuery = from dist in distribution.Select($"(userID = {userID})OR(computerID = {compID})").AsEnumerable()
                            select dist.Field<long>("id");
            if(distQuery.Count()>0)
            {
                foreach(dynamic dist in distQuery)
                {
                    DataRow delRow = distribution.Rows.Find((long)dist);
                    if (delRow != null)
                        distribution.Rows.Remove(delRow);
                }
            }
        }

        /// <summary>
        /// Возвращает массив компьютеров с проверкой состояния обновления
        /// </summary>
        /// <param name="software">ПО</param>
        /// <param name="version">Актуальная версия</param>
        /// <returns></returns>
        private Dictionary<string,bool> GetCompIsUp(string software, string version)
        {
            Dictionary<string, bool> comps = new Dictionary<string, bool>();

            var compQuery = from comp in computer.AsEnumerable() //Выбираем все компьютеры с актуальной версией
                           join ins in install.AsEnumerable() on comp.Field<long>("id") equals ins.Field<long>("computerID")
                           join soft in this.software.AsEnumerable() on ins.Field<long>("softID") equals soft.Field<long>("id")
                           where (soft.Field<string>("name") == software) && (ins.Field<string>("version") == version)
                           select comp.Field<string>("nsName");

            foreach(dynamic comp in compQuery)
            {
                comps.Add(comp.ToString(), true);
            }

            var compNotUp = from comp in computer.AsEnumerable() //Выбираем все компьютеры, с версией отличной от актуальной
                            join ins in install.AsEnumerable() on comp.Field<long>("id") equals ins.Field<long>("computerID")
                            join soft in this.software.AsEnumerable() on ins.Field<long>("softID") equals soft.Field<long>("id")
                            where (soft.Field<string>("name") == software) && (ins.Field<string>("version") != version)
                            select comp.Field<string>("nsName");

            foreach (dynamic comp in compNotUp)
            {
                comps.Add(comp.ToString(), false);
            }

            return comps;
        }


        //Для тестов
        private void AddSomeData()
        {

            DataRow order = software.NewRow();
            order["name"] = "Ордер";
            order["version"] = "001";
            software.Rows.Add(order);

            DataRow test = user.NewRow();
            test["fio"] = "Тестов Тест Тестович";
            test["tel"] = "777777";
            user.Rows.Add(test);

            DataRow localhost1 = computer.NewRow();
            localhost1["nsName"] = "localhost1";
            localhost1["ip"] = "127.0.0.1";
            computer.Rows.Add(localhost1);

            DataRow localhost2 = computer.NewRow();
            localhost2["nsName"] = "localhost2";
            localhost2["ip"] = "127.0.0.2";
            computer.Rows.Add(localhost2);

            DataRow distr1 = distribution.NewRow();
            distr1["userID"] = 0;
            distr1["computerID"] = 1;
            distribution.Rows.Add(distr1);

            DataRow inst1 = install.NewRow();
            inst1["softID"] = 0;
            inst1["computerID"] = 0;
            inst1["version"] = "000";
            install.Rows.Add(inst1);

            DataRow inst2 = install.NewRow();
            inst2["softID"] = 0;
            inst2["computerID"] = 1;
            inst2["version"] = "000";
            install.Rows.Add(inst2);


            //DataRow inst = install.NewRow();
            //inst["softID"] = 1;
            //inst["computerID"] = 1;
            //inst["version"] = "002";
            //install.Rows.Add(inst);
        }

    }
}
