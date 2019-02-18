using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace CompMgr
{
    class SettingsHelper
    {

        private Dictionary<string, string> bases = new Dictionary<string, string>();
      
        private XmlDocument settings = new XmlDocument();

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



        private void ParseSettings()
        {
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

        public void AddBase(string newBaseName, string newBasePath)
        {
            bases.Add(newBaseName, newBasePath);
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
