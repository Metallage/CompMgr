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

        public Dictionary<string, string> GetDistr()
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


        public DataTable GetDiv()
        {
            return install;
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



        public void ParseComp(string data)
        {

            char[] paramSeparators = { ';', ',' };
            string[] comps =  data.Split('\n');
            foreach (string comp in comps)
            {
                string[] param = comp.Split(paramSeparators);


                DataRow newComp = computer.NewRow();
                newComp["nsName"] = param[0];
                newComp["ip"] = param[1];
                if(param.Count() > 2)
                {
                    long div = FindDivisionID(param[2]);
                    if (div >= 0)
                        newComp["divID"] = div;
                    long usr = FindUserID(param[3]);
                    if (usr >= 0)
                        newComp["userID"] = usr;
                    computer.Rows.Add(newComp);
                }

            }
        }

        //Поиск ID пользователя по имени
        private long FindUserID(string userName)
        {
            var idsQuer = from ids in user.Select($"fio LIKE %{userName}%").CopyToDataTable().AsEnumerable()
                          select ids.Field<long>("id");
            if (idsQuer.Count() == 1)
            {
                return (long)idsQuer.First();
            }
            else
                return -1;                   
        }

        //Поиск ID подразделения по части названия
        private long FindDivisionID(string divisionName)
        {
            var divQuer = from div in division.Select($"name LIKE %{divisionName}").CopyToDataTable().AsEnumerable()
                          select div.Field<long>("id");
            if (divQuer.Count() == 1)
                return (long)divQuer.First();
            else
                return -1;
        }

        private Dictionary<string,bool> GetCompIsUp(string software, string version)
        {
            Dictionary<string, bool> comps = new Dictionary<string, bool>();

            var compQuery = from comp in computer.AsEnumerable()
                           join ins in install.AsEnumerable() on comp.Field<long>("id") equals ins.Field<long>("computerID")
                           join soft in this.software.AsEnumerable() on ins.Field<long>("softID") equals soft.Field<long>("id")
                           where (soft.Field<string>("name") == software) && (ins.Field<string>("version") == version)
                           select comp.Field<string>("nsName");

            foreach(dynamic comp in compQuery)
            {
                comps.Add(comp.ToString(), true);
            }

            var compNotUp = from comp in computer.AsEnumerable()
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
