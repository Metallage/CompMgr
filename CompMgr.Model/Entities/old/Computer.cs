using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    /// <summary>
    /// Класс описания компьютера
    /// </summary>
    public class Computer
    {

        //Поле с доменным именем
        protected string nsName = String.Empty;
        //Поле с ip-адресом
        protected string ip = String.Empty;

        protected Division division;

        protected User user;



        protected  string userFio = String.Empty;

        protected string divisionName = String.Empty;

        public long Id { get; set; }

        /// <summary>
        /// Свойство на доступ к доменному имени
        /// </summary>
        public virtual string NsName {
            get
            {
                return nsName;
            }

            set
            {
                nsName = value;
            }
        }

        /// <summary>
        /// Свойство на доступ к ip-адресу
        /// </summary>
        public virtual string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
         }

        public virtual User CurrentUser
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        public virtual Division CurrentDivision
        {
            get
            {
                return division;
            }
            set
            {
                division = value;
            }
        }


        public virtual string UserFio
        {
            get
            {
                return userFio;
            }
            set
            {
                userFio = value;
            }
        }

        public virtual string DivisionName
        {
            get
            {
                return divisionName;
            }
            set
            {
                divisionName = value;
            }
        }

        public Computer()
        { }

        public Computer(long id, string nsName)
        {
            Id = id;
            NsName = nsName;
        }
    }
}
