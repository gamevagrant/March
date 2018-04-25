using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class LanguageConfig : BaseConfig
    {

        private Dictionary<string, string> dic = new Dictionary<string, string>();

        public override string Name()
        {
            string ex = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified) ? "cn" : "en";
            return "language_" + ex + ".xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            dic.Add(item.GetAttribute("id"), item.GetAttribute("value"));
        }

        public string GetItem(string id)
        {
            string value;
            dic.TryGetValue(id, out value);
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogAssertion(string.Format("{0}表中没有找到id为 {1} 的多语言项",Name(),id));
            }
            return value;
        }
    }
}

