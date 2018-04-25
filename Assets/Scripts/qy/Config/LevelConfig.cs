using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class LevelConfig : BaseConfig
    {
        private Dictionary<int, LevelItem> dic = new Dictionary<int, LevelItem>();
        public override string Name()
        {
            return "level.xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            LevelItem levelItem = new LevelItem();
            levelItem.level = int.Parse(item.GetAttribute("level"));
            levelItem.exp = int.Parse(item.GetAttribute("exp"));
            levelItem.heart = int.Parse(item.GetAttribute("heart"));
            levelItem.star = int.Parse(item.GetAttribute("star"));
            levelItem.coin = int.Parse(item.GetAttribute("coin"));
            levelItem.props = ReadProps(item.GetAttribute("props"));


            dic.Add(levelItem.level, levelItem);
        }

        private List<PropItem>ReadProps(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }
            List<PropItem> list = new List<PropItem>();
            string[] items = value.Split(',');
            foreach (string str in items)
            {
                string[] data = str.Split(':');
                string id = data[0];
                int count = int.Parse(data[1]);
                PropItem prop = ConfigManager.Instance.propsConfig.GetItem(id);
                if(prop!=null)
                {
                    prop.count = count;
                    list.Add(prop);
                }
            }
            return list;
        }

        public LevelItem GetItem(int level)
        {
            LevelItem value;
            dic.TryGetValue(level, out value);
            if (value == null)
            {
                Debug.LogAssertion(string.Format("{0}表中没有找到id为 {1}的项", Name(), level));
            }
            return value;
        }
    }

    public class LevelItem
    {
        public int level;
        public int exp;
        public int heart;
        public int star;
        public int coin;
        /// <summary>
        /// 奖励物品
        /// </summary>
        public List<PropItem> props;
    }
}

