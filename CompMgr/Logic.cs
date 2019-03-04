using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace CompMgr
{
    /// <summary>
    /// Класс ядра логики
    /// </summary>
    public class Logic
    {
        private DataSet mainDS;
        private DataTable user;
       // private DataTable division;
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
               // division = mainDS.Tables["Division"];
                computer = mainDS.Tables["Computer"];
                install = mainDS.Tables["Install"];
                distribution = mainDS.Tables["Distribution"];

                //AddSomeData(); //Для тестов

                return new ErrorMessageHelper();
            }
            catch(Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
        }


        public void Save()
        {
            dbHelper.Save();
            dbHelper.Reload();
            foreach (DataTable dt in mainDS.Tables)
                dt.AcceptChanges();
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



        #region Получение данных для интерфейса

        public DataTable GetUser()
        {
            return user;
        }

        public DataTable GetComputer()
        {
            return computer;
        }



        /// <summary>
        /// Формируем список компьютеров в понятный интерфейсу вид
        /// </summary>
        /// <returns>Лист классов компьютер</returns>
        //public HashSet<Computer> GetComputers()
        //{
        //    HashSet<Computer> computers = new HashSet<Computer>();

        //    //Выбираем все компьютеры с подразделениями
        //    var compQuer = from comp in computer.AsEnumerable()
        //                   join div in division.AsEnumerable() on comp.Field<long>("divisionID") equals div.Field<long>("id")
        //                   select new {Id = comp.Field<long>("id"), NsName=comp.Field<string>("nsName"), Ip=comp.Field<string>("ip"), Division=div.Field<string>("name") };

        //    foreach (dynamic comp in compQuer)
        //    {
        //        Computer newComp = new Computer(comp.NsName, comp.Ip);
        //        newComp.DivisionName = comp.Division;

        //        //Ищем есть ли пользователи которым назначен этот комп
        //        if (distribution.Select($"computerID = {comp.Id}").Count() > 0)
        //        {
        //            var userComp = from dist in distribution.Select($"computerID = {comp.Id}").CopyToDataTable().AsEnumerable()
        //                           join usr in user.AsEnumerable() on dist.Field<long>("userId") equals usr.Field<long>("id")
        //                           select usr.Field<string>("fio");

        //            if (userComp.Count() == 1)
        //                newComp.UserFio = userComp.First();
        //        }
        //        computers.Add(newComp);
        //    }

        //    return computers;
        //}

        /// <summary>
        /// Возвращает список всего софта с версиями в понятном интерфейсу виде
        /// </summary>
        public DataTable GetSoftware()
        {
            return software;
        }


        public List<CompleteTableHelper> GetCompleteTable()
        {
            List<CompleteTableHelper> compData = new List<CompleteTableHelper>();

            foreach(DataRow softdr in software.Select())
            {
                string softName = softdr.Field<string>("softName");
                string currentversion = softdr.Field<string>("version");

                CompleteTableHelper cth = new CompleteTableHelper(softName,currentversion);
                cth.CompNames = GetCompIsUp(softName, currentversion);
                compData.Add(cth);

            }
            return compData;
        }

        public ObservableCollection<Install> GetInstall()
        {
            ObservableCollection<Install> inst = new ObservableCollection<Install>();
            foreach (DataRow drc in computer.Rows)
            {
                Install newInst = new Install();
                newInst.ComputerId = drc.Field<long>("id");
                newInst.Ip = drc.Field<string>("ip");
                newInst.NsName = drc.Field<string>("nsName");

                ObservableCollection<InstallSoft> insS = new ObservableCollection<InstallSoft>();
                foreach (DataRow drs in software.Rows )
                {
                    InstallSoft newInsS = new InstallSoft();
                    newInsS.ComputerId = drc.Field<long>("id");
                    newInsS.SoftName = drs.Field<string>("softName");
                    newInsS.SoftId = drs.Field<long>("id");
                    long idInst = FindInstallID(newInsS.ComputerId, newInsS.SoftId);
                    if(idInst == -1)
                    {
                        newInsS.Installed = false;
                    }
                    else
                    {
                        newInsS.Installed = true;
                    }
                    newInsS.InstallId = idInst;
                    insS.Add(newInsS);
                }

                newInst.IsInstalled = insS;
                inst.Add(newInst);
            }
            return inst;
        }

        #endregion

        #region Парсинг в таблицы из текста

        /// <summary>
        /// Парсинг входной строки для добавления компьютера
        /// </summary>
        /// <param name="data">Входные данные</param>
        public void ParseComputer(string data)
        {

            char[] paramSeparators = { ';', ',' };
            char[] compSeparators = { '\r', '\n' };
            string[] comps =  data.Split(compSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string comp in comps)
            {
                
                string[] param = comp.Split(paramSeparators);
                if(param.Count()>1)
                {
                    DataRow newComp = computer.NewRow();
                    newComp["nsName"] = param[0];
                    newComp["ip"] = param[1];
                    computer.Rows.Add(newComp);
                }
            }
        }

        /// <summary>
        /// Парсинг поточного ввода пользователей
        /// </summary>
        /// <param name="data">Входные данные</param>
        public void ParseUser(string data)
        {
            char[] paramSeparators = { ';', ',' };
            char[] compSeparators = { '\r', '\n' };
            string[] users = data.Split(compSeparators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string usr in users)
            {

                string[] param = usr.Split(paramSeparators);
                if (param.Count() > 1)
                {
                    DataRow newUser = user.NewRow();
                    newUser["fio"] = param[0];
                    newUser["tel"] = param[1];
                    user.Rows.Add(newUser);
                }
            }
        }

        /// <summary>
        /// Парсинг поточного ввода ПО
        /// </summary>
        /// <param name="data">Входные данные</param>
        public void ParseSoftware(string data)
        {
            char[] paramSeparators = { ';', ',' };
            char[] compSeparators = { '\r', '\n' };
            string[] soft = data.Split(compSeparators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string sf in soft)
            {

                string[] param = sf.Split(paramSeparators);
                if (param.Count() > 1)
                {
                    software.BeginLoadData();
                    DataRow newSoft = software.NewRow();
                    newSoft["softName"] = param[0];
                    newSoft["version"] = param[1];
                    software.Rows.Add(newSoft);
                    software.EndLoadData();
                }
            }
        }

        #endregion


        public void SaveInstall(ObservableCollection<Install> installCollection)
        {
            foreach(Install inst in installCollection)
            {
                long compID = inst.ComputerId;
                foreach (InstallSoft sInst in inst.IsInstalled)
                {

                    long softID = sInst.SoftId;
                    long installID = FindInstallID(compID, softID);
                    if (sInst.Installed)
                    {
                        if(installID == -1)
                        {
                            DataRow newInstallRow = install.NewRow();
                            newInstallRow["computerID"] = compID;
                            newInstallRow["softID"] = softID;
                            newInstallRow["version"] = software.Select($"id = {softID}")[0].Field<string>("version");
                            install.Rows.Add(newInstallRow);
                        }
                    }
                    else
                    {
                        if(installID!=-1)
                        {
                            install.Select($"id = {installID}")[0].Delete();
                        }
                    }
                }
            }
            dbHelper.UpdateInstall();
            dbHelper.Reload();

        }

        /// <summary>
        /// Обновляет таблицу компьютеров и связанную с ней таблицу назначений компьютеров пользователю
        /// </summary>
        /// <param name="comps">Обновлённый список компьютеров</param>
        //public void UpdateComp(HashSet<Computer> comps)
        //{
        //    foreach(Computer comp in comps)
        //    {
        //        long id = FindCompID(comp.NsName); //Пытаемся найти ID компьютера
        //        long divID = FindDivisionID(comp.DivisionName); //Пытаемся найти ID подразделения
        //        string userFIO = null;
        //        if ((comp.UserFio != null) && (comp.UserFio != "") && (comp.UserFio != " ")) //Если имя пользователя не пустое
        //            userFIO = comp.UserFio; 
                
        //        long userID = FindUserID(userFIO);//Пытаемся найти имя пользователя

        //        if (id == -1) //Если такого компьютера не найдено
        //        {
        //            DataRow newRow = computer.NewRow(); //создаём новую запись
        //            newRow["nsName"] = comp.NsName;
        //            newRow["ip"] = comp.Ip;

        //            if ((comp.DivisionName != null) && (divID != -1))
        //            {
        //                newRow["divisionID"] = divID;
        //            }
        //            computer.Rows.Add(newRow);
        //        }
        //        else //Если такой уже есть, редактируем
        //        {
        //            DataRow upRow = computer.Rows.Find(id);
        //            upRow["ip"] = comp.Ip;
        //            if ((comp.DivisionName != null) && (divID != -1))
        //            {
        //                upRow["divisionID"] = divID;
        //            }
        //        }

        //        //Если компу назначен пользователь
        //        if ((comp.UserFio != null) && (userID != -1))
        //        {
        //            long distributionID = FindDistributionID(id, userID); //Пытаемся найти связанную запись о назначении
        //            if (distributionID == -1) //Если такой нет
        //            {
        //                DeleteDistributionByUserOrCompID(id, userID); //Снимаем все назначения для выбранных компьютера и пользователя
        //                DataRow newDistrib = distribution.NewRow(); //Создаём новую запись назначения
        //                newDistrib["computerID"] = id;
        //                newDistrib["userID"] = userID;
        //                distribution.Rows.Add(newDistrib);
        //            }
        //        }
        //        else if (comp.UserFio == null) //Если у компа нет пользователя
        //        {
        //            DeleteDistributionByUserOrCompID(id, -1); //Удаляем все записи о назначении для этого компа
        //        }
        //    }
        //}

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
        //private long FindDivisionID(string divisionName)
        //{
        //    if (divisionName == null)
        //        return -1;
        //    else
        //    {
        //        var divQuer = from div in division.Select($"name = '{divisionName}'").AsEnumerable()
        //                      select div.Field<long>("id");
        //        if (divQuer.Count() == 1)
        //            return (long)divQuer.First();
        //        else
        //            return -1;
        //    }
        //}

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


        /// <summary>
        /// Поиск ID установки по ID компа и ID ПО
        /// </summary>
        /// <param name="compId">ID компа</param>
        /// <param name="softId">ID ПО</param>
        /// <returns>ID установки, если -1 то установки нет</returns>
        private long FindInstallID(long compId, long softId)
        {
            DataRow[] ids = install.Select($"computerID = {compId} AND softID = {softId}");
            if (ids.Count() == 1)
                return ids[0].Field<long>("id");
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
                           where (soft.Field<string>("softName") == software) && (ins.Field<string>("version") == version)
                           select comp.Field<string>("nsName");

            foreach(dynamic comp in compQuery)
            {
                comps.Add(comp.ToString(), true);
            }

            var compNotUp = from comp in computer.AsEnumerable() //Выбираем все компьютеры, с версией отличной от актуальной
                            join ins in install.AsEnumerable() on comp.Field<long>("id") equals ins.Field<long>("computerID")
                            join soft in this.software.AsEnumerable() on ins.Field<long>("softID") equals soft.Field<long>("id")
                            where (soft.Field<string>("softName") == software) && (ins.Field<string>("version") != version)
                            select comp.Field<string>("nsName");

            foreach (dynamic comp in compNotUp)
            {
                comps.Add(comp.ToString(), false);
            }

            return comps;
        }

    }
}
