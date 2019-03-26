using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    /// <summary>
    /// Класс описания ПО
    /// </summary>
    public class Soft
    {
        //Название ПО
        protected string softName = String.Empty;
        //Версия ПО
        protected string softVersion = String.Empty;

        //Название ПО
        public string SoftName {
            get
            {
                return softName;
            }

            set
            {
                softName = value;
            }
        }

        //Версия ПО
        public string SoftVersion
        {
            get
            {
                return softVersion;
            }
            set
            {
                softVersion = value;
            }
        }

        /// <summary>
        /// Конструктор описания ПО
        /// </summary>
        /// <param name="softName">Название ПО</param>
        /// <param name="version">Версия ПО </param>
        public Soft(string softName, string version)
        {
            SoftName = softName;
            SoftVersion = version;
        }

        /// <summary>
        /// Конструктор ПО
        /// </summary>
        public Soft()
        { }
    }
}
