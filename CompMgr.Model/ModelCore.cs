using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using CompMgr;
using System.Threading.Tasks;


namespace CompMgr.Model
{
    /// <summary>
    /// Класс ядра логики
    /// </summary>
    public class ModelCore
    {
        private DataSet mainDS;
        private DataTable user;
        private DataTable software;
        private DataTable computer;
        private DataTable install;
        private DataTable distribution;

        private DataBaseHelper dbHelper = new DataBaseHelper();

        public delegate void ReadyEventHandler();
        public delegate void ErrorEventHandler(ErrorArgs e);

        public event ReadyEventHandler DataUpdate;
        public event ErrorEventHandler onError;


        public void Start()
        {
            try
            {
                dbHelper.InitialDB();

                mainDS = dbHelper.LogicDataSet;

                software = mainDS.Tables["Software"];
                user = mainDS.Tables["User"];
                computer = mainDS.Tables["Computer"];
                install = mainDS.Tables["Install"];
                distribution = mainDS.Tables["Distribution"];
            }
            catch (Exception e)
            {
                onError?.Invoke(new ErrorArgs("Запуск БД", e.Message));
            }
        }


        public void Save()
        {
            try
            {

                dbHelper.Save();
                dbHelper.Reload();
                foreach (DataTable dt in mainDS.Tables)
                    dt.AcceptChanges();
                DataUpdate?.Invoke();
            }
            catch (Exception e)
            {
                onError?.Invoke(new ErrorArgs("Сохранение в БД", e.Message));
            }
        }

        /// <summary>
        /// Формирует список обновлений для ПО
        /// </summary>
        /// <param name="upSoft">Название ПО</param>
        /// <returns>Список рабочих мест, где нужно обновить данное ПО</returns>
        public List<Update> GetUpdates(string upSoft)
        {

            List<Update> updateList = new List<Update>();

            var updQuery = from comp in computer.AsEnumerable() //Из таблицы компов
                           join dist in distribution.AsEnumerable() on comp.Field<long>("id") equals dist.Field<long>("computerID") //Выбираем те, что разпределены пользователям
                           join usr in user.AsEnumerable() on dist.Field<long>("userID") equals usr.Field<long>("id") //Выбираем пользователей которым распределены компы
                           join inst in install.AsEnumerable() on comp.Field<long>("id") equals inst.Field<long>("computerID") //Выбираем те где есть установленное по
                           join soft in software.AsEnumerable() on inst.Field<long>("softID") equals soft.Field<long>("id") //Выбираем список ПО на этих компах
                           where soft.Field<string>("softName") == upSoft && soft.Field<string>("version") !=inst.Field<string>("version") // Выбираем те, что имеют нужное ПО и неправильную версию
                           select new {NsName = comp.Field<string>("nsName"), Ip=comp.Field<string>("ip"), UserFio = usr.Field<string>("fio"),
                               CurrentVersion =soft.Field<string>("version"), OldVersion = inst.Field<string>("version"), Id = inst.Field<long>("id")}; //Формируем запись

            foreach(dynamic upd in updQuery)
            {
                updateList.Add(new Update(upd.Id, upd.NsName, upd.Ip, upd.UserFio, upd.OldVersion, upd.CurrentVersion)); //Добавляем всё найденное в лист обновлений
            }

            return updateList;
        }



        #region Получение данных для интерфейса



        //public ObservableCollection<User> GetUsersNoComp()
        //{
        //    ObservableCollection<User> users = new ObservableCollection<User>();

        //    foreach (DataRow dru in user.Rows)
        //    {
        //        if (distribution.Select($"userID = {dru.Field<long>("id")}").Count() == 0)
        //        {
        //            User newUser = new User();
        //            newUser.Id = dru.Field<long>("id");
        //            newUser.UserFio = dru.Field<string>("fio");
        //            newUser.UserTel = dru.Field<string>("tel");
        //            users.Add(newUser);
        //        }
        //    }

        //    return users;
        //}


        public List<User> GetUsersNoComp()
        {
            List<User> users = new List<User>();

            foreach (DataRow dru in user.Rows)
            {
                if (distribution.Select($"userID = {dru.Field<long>("id")}").Count() == 0)
                {
                    User newUser = new User();
                    newUser.Id = dru.Field<long>("id");
                    newUser.UserFio = dru.Field<string>("fio");
                    newUser.UserTel = dru.Field<string>("tel");
                    users.Add(newUser);
                }
            }

            return users;
        }

        public ObservableCollection<Computer> GetComputersNoUser()
        {
            ObservableCollection<Computer> computers = new ObservableCollection<Computer>();


            foreach (DataRow drc in computer.Rows)
            {
                if (distribution.Select($"computerID = {drc.Field<long>("id")}").Count() == 0)
                {
                    Computer newComp = new Computer();
                    newComp.Id = drc.Field<long>("id");
                    newComp.NsName = drc.Field<string>("nsName");
                    newComp.Ip = drc.Field<string>("ip");
                    computers.Add(newComp);
                }
            }

            return computers;
        }




        /// <summary>
        /// Формирует сводную информацию об установленном ПО
        /// </summary>
        /// <returns>Сводная информация о ПО</returns>
        public List<CompleteTableHelper> GetCompleteTable()
        {
            List<CompleteTableHelper> compData = new List<CompleteTableHelper>();

            foreach (DataRow softdr in software.Select())
            {
                string softName = softdr.Field<string>("softName");
                string currentversion = softdr.Field<string>("version");

                CompleteTableHelper cth = new CompleteTableHelper(softName, currentversion);
                cth.CompNames = GetCompIsUp(softName, currentversion);// Проверка на актуальность версий по компам
                compData.Add(cth);

            }
            return compData;
        }

        public List<Computer> GetComputers()
        {
            List<Computer> compList = new List<Computer>();

            foreach (DataRow cdr in computer.Rows)
            {
                long compId = cdr.Field<long>("id");
                compList.Add(new Computer
                {
                    NsName = cdr.Field<string>("nsName"),
                    Ip = cdr.Field<string>("ip"),
                    UserFio = GetUserFiobyCompID(compId),
                    DivisionName = GetDivisionByCompID(compId)

                });
            }

            return compList;
        }

        private string GetUserFiobyCompID(long compID)
        {
            string userFio = String.Empty;
            var fioQuery = from usr in user.AsEnumerable()
                           join dst in distribution.AsEnumerable() on usr.Field<long>("id") equals dst.Field<long>("userID")
                           where (dst.Field<long>("computerID") == compID)
                           select usr.Field<string>("fio");
            if (fioQuery.Count() > 0)
                userFio = fioQuery.First();
            return userFio;
        }

        private string GetDivisionByCompID(long compID)
        {
            return String.Empty;
        }

        public DataTable GetSoftwareDT()
        {
            return software;
        }

        public List<Soft> GetSoftware()
        {
            List<Soft> softList = new List<Soft>();

            foreach (DataRow sdr in software.Rows)
                softList.Add(new Soft {
                    SoftName = sdr.Field<string>("softName"),
                    SoftVersion = sdr.Field<string>("version") });

            return softList;
        }

        public List<User> GetUsers()
        {
            List<User> userList = new List<User>();

            foreach (DataRow udr in user.Rows)
                userList.Add(new User
                {
                    UserFio = udr.Field<string>("fio"),
                    UserTel = udr.Field<string>("tel")
                });

            return userList;
        }

        public List<string> GetSoftNames()
        {
            List<string> softNames = new List<string>();

            foreach (DataRow dr in software.Rows)
                softNames.Add(dr.Field<string>("softName"));
            softNames.Sort();
            return softNames;
        }

        /// <summary>
        /// Получение списка компов с установленными ПО
        /// </summary>
        /// <returns>Списко компов с установленным ПО</returns>
        public List<Install> GetInstalled()
        {
            List<Install> installed = new List<Install>();

            foreach (DataRow compDr in computer.Rows)
            {
                Install newInst = new Install();
                newInst.NsName = compDr.Field<string>("nsName");

                var userQuery = from usr in user.AsEnumerable()
                                join dst in distribution.AsEnumerable() on usr.Field<long>("id") equals dst.Field<long>("userID")
                                where (dst.Field<long>("computerID") == compDr.Field<long>("id"))
                                select usr.Field<string>("fio");

                if (userQuery.Count() > 0)
                {
                    newInst.UserFio = userQuery.First();
                }


                var softQuery = from inst in install.AsEnumerable()
                                join sft in software.AsEnumerable() on inst.Field<long>("softID") equals sft.Field<long>("id")
                                where (inst.Field<long>("computerID") == compDr.Field<long>("id"))
                                select sft.Field<string>("softName");

                List<string> installedSoft = new List<string>();

                foreach (dynamic sft in softQuery)
                {
                    installedSoft.Add(sft);
                }

                newInst.InstalledSoft = installedSoft;

                installed.Add(newInst);

            }

            return installed;
        }


        

        public List<Distribution> GetDistribution()
        {
            List<Distribution> distrib = new List<Distribution>();

            var distribQuery = from dst in distribution.AsEnumerable()
                               join usr in user.AsEnumerable() on dst.Field<long>("userID") equals usr.Field<long>("id")
                               join cmp in computer.AsEnumerable() on dst.Field<long>("computerID") equals cmp.Field<long>("id")
                               select new
                               {
                                   Id = dst.Field<long>("id"),
                                   ComputerID = dst.Field<long>("computerID"),
                                   UserID = dst.Field<long>("userID"),
                                   NsName = cmp.Field<string>("nsName"),
                                   UserFio = usr.Field<string>("fio")
                               };

            foreach (dynamic distribItem in distribQuery)
            {

                Distribution newDistrib = new Distribution();
                newDistrib.Id = distribItem.Id;
                newDistrib.NsName = distribItem.NsName;
                newDistrib.ComputerID = distribItem.ComputerID;
                newDistrib.UserFio = distribItem.UserFio;
                newDistrib.UserID = distribItem.UserID;
                distrib.Add(newDistrib);
            }

            return distrib;
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

        #region импорт данных

        public void SaveUsers(List<User> newUsers)
        {
            //удаляем удалённых и редактируем изменённых
            foreach(DataRow userDR in user.Rows)
            {
                User editetUser = null;
                bool found = false;
                foreach (User usr in newUsers)
                    if(usr.UserFio == userDR.Field<string>("fio"))
                    {
                        found = true;
                        userDR.BeginEdit();
                        userDR["tel"] = usr.UserTel;
                        userDR.EndEdit();
                        editetUser = usr;
                        break;
                    }
                if(editetUser!=null)
                {
                    newUsers.Remove(editetUser);
                }
                if (!found)
                    userDR.Delete();
            }
            foreach(User usr in newUsers)
            {
                DataRow newUser = user.NewRow();
                newUser["fio"] = usr.UserFio;
                newUser["tel"] = usr.UserTel;
                user.Rows.Add(newUser);
            }
        }


        #endregion


        public void SaveDistribution(List<Distribution> distribs)
        {
            foreach (DataRow drd in distribution.Rows) //Поиск удалённых распределений
            {
                bool found = false;
                foreach (Distribution dst in distribs)
                    if (dst.Id == drd.Field<long>("id"))
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    drd.Delete();
            }

            foreach (Distribution newDistr in distribs)
                if (newDistr.Id == -1)
                {
                    DataRow newDistribution = distribution.NewRow();
                    newDistribution["computerID"] = newDistr.ComputerID;
                    newDistribution["userID"] = newDistr.UserID;
                    distribution.Rows.Add(newDistribution);
                }
            dbHelper.Save();

            DataUpdate?.Invoke();
        }

        public void SaveUpdate(List<long> ids)
        {
            if (ids.Count > 0)
            {
                long softID = install.Rows.Find(ids[0]).Field<long>("softID");
                string version = software.Rows.Find(softID).Field<string>("version");
                foreach (long id in ids)
                {
                    DataRow upRow = install.Rows.Find(id);
                    upRow["version"] = version;
                }
                Save();
                DataUpdate?.Invoke();
            }

        }

        public void UpdateSoft(string softName, string version)
        {
            foreach (DataRow dr in software.Rows)
                if (softName == dr.Field<string>("softName"))
                {
                    dr["version"] = version;
                    break;
                }
            
        }

        public void SaveInstall(List<Install> installs)
        {
            

            foreach (Install inst in installs)
            {
                long compID = FindCompID(inst.NsName);
                UnInstall(compID, inst.InstalledSoft);
                foreach(string insSoft in  inst.InstalledSoft)
                {

                    long softId = FindSoftID(insSoft);
                    if (FindInstallID(compID, softId) < 0)
                    {
                        DataRow idr = install.NewRow();
                        idr["computerID"] = compID;
                        idr["softID"] = softId;
                        idr["version"] = software.Select($"id = {softId}").First().Field<string>("version");
                        install.Rows.Add(idr);
                    }
                }
            }
            dbHelper.UpdateInstall();
            dbHelper.Reload();
            DataUpdate?.Invoke();

        }

        private void UnInstall(long compId, List<string> install)
        {
            foreach (DataRow sdr in software.Rows )
            {
                
                if (!install.Exists(x => x == sdr.Field<string>("softName")))
                {
                    long delID = FindInstallID(compId, sdr.Field<long>("id"));
                    if (delID > 0)
                    {
                        this.install.Rows.Find(delID).Delete();
                       
                    }
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

        private long FindSoftID(string softName)
        {
            DataRow[] ids = software.Select($"softName = '{softName}'");
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
