using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace CompMgr
{
    public class SettingsHelper
    {

        private Dictionary<string, string> bases = new Dictionary<string, string>();
      


        private string softwareBase;

        public SettingsHelper()
        {
            if(!File.Exists("settings.xml"))
            {
                GenerateNewSettins();
            }
            else
            {
                ParseSettings();
            }
        }

        /// <summary>
        /// Создаёт новый файл настроек
        /// </summary>
        private void GenerateNewSettins()
        {
            XElement rootSettings = new XElement("settings");
            XElement bases = new XElement("dataBases");
            XElement testBase = new XElement("base");
            XAttribute testBaseName = new XAttribute("BaseName","");
            XAttribute testBasePath = new XAttribute("BasePath", "");
            testBase.Add(testBaseName);
            testBase.Add(testBasePath);
            bases.Add(testBase);
            rootSettings.Add(bases);

            XElement softwareBase = new XElement("software");
            XAttribute softDBPath = new XAttribute("SoftwareDBPath", "software.sqlite");
            softwareBase.Add(softDBPath);
            rootSettings.Add(softwareBase);

            rootSettings.Save("settings.xml");
        }


        /// <summary>
        /// Читаем настройки из XML
        /// </summary>
        private void ParseSettings()
        {
            XmlDocument settings = new XmlDocument();
            settings.Load("settings.xml");
            XmlNode rootSettings = settings.DocumentElement;

            XmlNode xmlSoftWare = rootSettings.SelectSingleNode("//software");
            softwareBase = xmlSoftWare.Attributes["SoftwareDBPath"].Value;

            XmlNodeList bases = rootSettings.SelectNodes("//dataBases/base");
            foreach(XmlNode xmlBase in bases)
            {
                this.bases.Add(xmlBase.Attributes["BaseName"].Value, xmlBase.Attributes["BasePath"].Value);
            }

        }

        /// <summary>
        /// Добавляем базу поста к настройкам
        /// </summary>
        /// <param name="newBaseName">Название базы</param>
        /// <param name="newBasePath">Путь к базе</param>
        public void AddBase(string newBaseName, string newBasePath)
        {
            bases.Add(newBaseName, newBasePath);
        }

        /// <summary>
        /// Пишет текущие настройки в файл
        /// </summary>
        public void WriteSettings()
        {
            XElement rootSettings = new XElement("settings");
            XElement bases = new XElement("dataBases");
            for(int i =0; i< this.bases.Count; i++)
            {
                XElement testBase = new XElement("base");
                XAttribute testBaseName = new XAttribute("BaseName",this.bases.ElementAt(i).Key);
                XAttribute testBasePath = new XAttribute("BasePath", this.bases.ElementAt(i).Value);
                testBase.Add(testBaseName);
                testBase.Add(testBasePath);
                bases.Add(testBase);
            }
            rootSettings.Add(bases);

            XElement softwareBase = new XElement("software");
            XAttribute softDBPath = new XAttribute("SoftwareDBPath", this.softwareBase);
            softwareBase.Add(softDBPath);
            rootSettings.Add(softwareBase);

            rootSettings.Save("settings.xml");
        }

        public string SoftwareBase
        { get
            {
                return softwareBase;
            }
        }

        public Dictionary<string, string> Bases
        { get
            {
                return bases;
            }
        }
    }
}
