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

        public long Id { get; set; }

        /// <summary>
        /// Свойство на доступ к доменному имени
        /// </summary>
        public string NsName {
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
        public string Ip
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

        public Computer()
        { }

        public Computer(long id, string nsName)
        {
            Id = id;
            NsName = nsName;
        }
    }
}
