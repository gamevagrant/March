using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace qy.config
{
    public class ExchangeConfig : BaseConfig
    {

        private Dictionary<string, ExchangeItem> dic = new Dictionary<string, ExchangeItem>();
        public override string Name
        {
            get
            {
                return "exchange.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            ExchangeItem matchLevel = new ExchangeItem();
            matchLevel.id = item.GetAttribute("id");
            matchLevel.dollar = float.Parse(item.GetAttribute("dollar"));
            matchLevel.gold = int.Parse(item.GetAttribute("gold"));

            dic.Add(matchLevel.id, matchLevel);
        }

        public ExchangeItem GetItem(string id)
        {
            ExchangeItem value;
            dic.TryGetValue(id, out value);
            return value;
        }
    }

    public class ExchangeItem
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 价格
        /// </summary>
        public float dollar;
        /// <summary>
        /// 金币
        /// </summary>
        public int gold;
    }
}

