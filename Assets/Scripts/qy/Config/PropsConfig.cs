using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace qy.config
{
    public class PropsConfig : BaseConfig
    {
        private Dictionary<string, PropItem> dic = new Dictionary<string, PropItem>();
        public override string Name
        {
            get
            {
                return "item.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            PropItem prop = new PropItem();
            prop.id = item.GetAttribute("id");
            prop.name = GetLanguage(item.GetAttribute("name"));
            prop.icon = item.GetAttribute("icon");
            prop.description = GetLanguage(item.GetAttribute("description"));
            prop.type = item.GetAttribute("type");
            prop.lv = item.GetAttribute("lv");
            prop.lv_limit = item.GetAttribute("lv_limit");
            prop.price = item.GetAttribute("price");

            dic.Add(prop.id, prop);
        }


        public PropItem GetItem(string id)
        {
            PropItem value;
            dic.TryGetValue(id, out value);
            return value;
        }

       
    }

    public class PropItem
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 名字
        /// </summary>
        public string name;
        /// <summary>
        /// 图标名称
        /// </summary>
        public string icon;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 类型
        /// </summary>
        public string type;
        /// <summary>
        /// 等级
        /// </summary>
        public string lv;
        /// <summary>
        /// 等级限制
        /// </summary>
        public string lv_limit;
        /// <summary>
        /// 价格
        /// </summary>
        public string price;
        /// <summary>
        /// 数量
        /// </summary>
        public int count;
        /// <summary>
        /// 获得概率
        /// </summary>
        public float rate;
    }
}
