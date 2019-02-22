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
        private DataTable users;
        private DataTable division;
        private DataTable software;
        private DataTable computer;
        private DataTable install;

        private DataBaseHelper dbHelper = new DataBaseHelper();

        public ErrorMessageHelper Start()
        {
            try
            {
                dbHelper.InitialDB();
                mainDS = dbHelper.LogicDataSet;

                software = mainDS.Tables["Software"];
                users = mainDS.Tables["Users"];
                division = mainDS.Tables["Division"];
                computer = mainDS.Tables["Computer"];
                install = mainDS.Tables["Install"];

                dbHelper.AddSomeData(); //Для тестов

                return new ErrorMessageHelper();
            }
            catch(Exception e)
            {
                return new ErrorMessageHelper(e.Message);
            }
        }


        //TODO перенести в ядро логики
        public List<Updates> GetUpdates(string upSoft)
        {

            List<Updates> updateList = new List<Updates>();

            var updQuery = from comp in computer.AsEnumerable()
                           join inst in install.AsEnumerable() on comp.Field<long>("id") equals inst.Field<long>("computerID")
                           join soft in software.AsEnumerable() on inst.Field<long>("softID") equals soft.Field<long>("id")
                           join user in users.AsEnumerable() on comp.Field<long>("userID") equals user.Field<long>("id")
                           where soft.Field<string>("name") == upSoft && soft.Field<string>("version") !=inst.Field<string>("version")
                           select new {NsName = comp.Field<string>("nsName"), Ip=comp.Field<string>("ip"), User = user.Field<string>("name")};

            foreach(dynamic upd in updQuery)
            {
                updateList.Add(new Updates(upd.NsName,upd.Ip, upd.User));
            }




            // var updates = from dr in 

            return updateList;
        }


    }
}
