using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

            XmlNodeList xmlList = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlElement item in xmlList)
            {
                ReadItem(item);

            }
            return true;
        }

        internal virtual void ReadItem(XmlElement item)
        {

        }

        protected string GetLanguage(string id)
        {
            return ConfigManager.Instance.langrageConfig.GetItem(id);
        }
    }
}

