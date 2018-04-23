using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace qy.config
{
    public class MatchLevelConfig : BaseConfig
    {
        private Dictionary<string, MatchLevelItem> dic = new Dictionary<string, MatchLevelItem>();
        public override string Name()
        {
            return "matchlevel.xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            MatchLevelItem matchLevel = new MatchLevelItem();
            matchLevel.id = item.GetAttribute("id");
            matchLevel.level = int.Parse(item.GetAttribute("level"));
            matchLevel.hard = int.Parse(item.GetAttribute("hard"));
            matchLevel.star = int.Parse(item.GetAttribute("star"));
            matchLevel.coin = int.Parse(item.GetAttribute("coin"));
            matchLevel.itemReward = ReadrequireItem(item.GetAttribute("itemReward"));

            dic.Add(matchLevel.id, matchLevel);
        }

        private List<PropItem> ReadrequireItem(string value)
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
                int rate = data.Length > 2 ? int.Parse(data[2])  : 0;
                PropItem prop = ConfigManager.Instance.propsConfig.GetItem(id);
                prop.count = count;
                prop.rate = rate;
                list.Add(prop);
            }

            return list;
        }

        public MatchLevelItem GetItem(string id)
        {
            MatchLevelItem value;
            dic.TryGetValue(id, out value);
            return value;
        }
    }

    public class MatchLevelItem
    {
        public string id;
        /// <summary>
        /// 等级
        /// </summary>
        public int level;
        /// <summary>
        /// 难度
        /// </summary>
        public int hard;
        /// <summary>
        /// 获取星星
        /// </summary>
        public int star;
        /// <summary>
        /// 获取金币
        /// </summary>
        public int coin;
        /// <summary>
        /// 获取物品
        /// </summary>
        public List<PropItem> itemReward;
    }
}

