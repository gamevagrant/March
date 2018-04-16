using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace qy.config
{
    public class LangrageConfig : BaseConfig
    {

        private Dictionary<string, string> dic = new Dictionary<string, string>();

        public override string Name
        {
            get
            {
                string ex = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified) ? "cn" : "en";
                return "language_" + ex + ".xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            dic.Add(item.GetAttribute("id"), item.GetAttribute("value"));
        }

        public string GetItem(string id)
        {
            string value = "";
            dic.TryGetValue(id, out value);
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogAssertion(string.Format("没有找到id为 {0} 的多语言项",id));
            }
            return value;
        }
    }
}

