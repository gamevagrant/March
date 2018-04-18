using System;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class BaseConfig
    {
        public virtual string Name
        {
            get
            {
                return "item.xml";
            }
        }
        public bool Read(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return false;
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);

            XmlElement element = null;
            try
            {
                XmlNodeList xmlList = xmldoc.DocumentElement.ChildNodes;
                foreach (XmlElement item in xmlList)
                {
                    element = item;
                    ReadItem(item);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + element.GetAttribute("id") + ", " + Name);
            }

            return true;
        }

        internal virtual void ReadItem(XmlElement item)
        {

        }

        protected string GetLanguage(string id)
        {
            return ConfigManager.Instance.languageConfig.GetItem(id);
        }
    }
}

