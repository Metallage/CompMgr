using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model.Entities.Abstract;

namespace CompMgr.Model.Entities
{
    /// <summary>
    /// Класс описания ПО
    /// </summary>
    public class Software : ISoftware
    {

        public int Id { get; set; }

        //Название ПО
        public virtual string Name { get; set;}

        //Версия ПО
        public virtual string ActualVersion { get; set; }

        /// <summary>
        /// Конструктор описания ПО
        /// </summary>
        /// <param name="softName">Название ПО</param>
        /// <param name="version">Версия ПО </param>
        public Software(string softName, string version)
        {
            Name = softName;
            ActualVersion = version;
        }

        /// <summary>
        /// Конструктор ПО
        /// </summary>
        public Software()
        { }
    }
}
