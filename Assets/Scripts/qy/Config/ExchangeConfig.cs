using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace qy.config
{
    public class ExchangeConfig : BaseConfig
    {

        private Dictionary<string, ExchangeItem> dic = new Dictionary<string, ExchangeItem>();
        private List<ExchangeItem> list = new List<ExchangeItem>();
        public override string Name
        {
            get
            {
                return "exchange.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            ExchangeItem exchangeItem = new ExchangeItem();
            exchangeItem.id = item.GetAttribute("id");
            exchangeItem.dollar = float.Parse(item.GetAttribute("dollar"));
            exchangeItem.gold = int.Parse(item.GetAttribute("gold_doller"));

            dic.Add(exchangeItem.id, exchangeItem);
            list.Add(exchangeItem);

        }

        public ExchangeItem GetItem(string id)
        {
            ExchangeItem value;
            dic.TryGetValue(id, out value);
            return value;
        }

        public List<ExchangeItem> GetList()
        {
            return list;
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

