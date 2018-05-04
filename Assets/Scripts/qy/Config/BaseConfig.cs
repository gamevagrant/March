using System;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class BaseConfig
    {
        public virtual string Name()
        {
            return "item.xml";
        }
        public virtual bool Read(string xml)
        {
            Debug.Log("正在解析"+Name());
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
                Debug.LogError(e.Message + element.GetAttribute("id") + ", " + Name());
                Debug.LogError(e);
            }

            return true;
        }

        internal virtual void ReadItem(XmlElement item)
        {

        }

        protected string GetLanguage(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return "";
            }
            return ConfigManager.Instance.languageConfig.GetItem(id);
        }
    }
}

