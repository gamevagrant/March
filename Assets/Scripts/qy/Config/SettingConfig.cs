using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace qy.config
{
    public class SettingConfig : BaseConfig
    {
        /// <summary>
        /// 补满生命金币价格
        /// </summary>
        public int livesPrice;
        /// <summary>
        /// 恢复1条生命时间 分钟
        /// </summary>
        public int livesRecoverTime;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public int maxLives;



        /// <summary>
        /// 购买五步的价格 同一关 每买一次价格升一次
        /// </summary>
        private List<int> price;
        private List<List<PropItem>> bonusItems;
        private List<List<PropItem>> bonusItemsBag;
        public int addSteps;

        /// <summary>
        /// 最大关卡数
        /// </summary>
        public int max;


        /// <summary>
        /// 结算时的道具转化为金币奖励 
        /// </summary>
        public int columnbreaker;
        public int rowbreaker;
        public int planebreaker;
        public int bombbreaker;
        public int rainbow;
        public int maxgold;

        public override string Name
        {
            get
            {
                Debug.Log(Network.player.ipAddress);
                return "setting.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {

            System.Type t = this.GetType();
            foreach (XmlAttribute attribute in item.Attributes)
            {
                string name = attribute.Name;
                if(name == "bonusItems")
                {
                    bonusItems = ReadBonusItems(attribute.Value);
                }
                else if(name == "bonusItemsBag")
                {
                    bonusItemsBag = ReadBonusItems(attribute.Value);
                }
                else if(name == "price")
                {
                    price = ReadPrice(attribute.Value);
                }else
                {
                    foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
                    {
                        if (name == pi.Name)
                        {
                            if (pi.PropertyType == typeof(int))
                            {
                                pi.SetValue(this, int.Parse(attribute.Value), null);
                            }
                            else
                            {
                                pi.SetValue(this, attribute.Value, null);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 获取关卡中购买5步功能的价格
        /// </summary>
        /// <param name="step">本关卡已经使用次数</param>
        /// <returns></returns>
        public int GetPriceWithStep(int step)
        {
            step = Mathf.Min(step, price.Count);
            return price[step];
        }
        /// <summary>
        /// 获取购买5步后立即使用的物品
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public List<PropItem> GetBonusItemWithStep(int step)
        {
            step = Mathf.Min(step, bonusItems.Count);
            return bonusItems[step];
        }
         /// <summary>
         /// 获取购买5步后加入背包的物品
         /// </summary>
         /// <param name="step"></param>
         /// <returns></returns>
        public List<PropItem> GetBonusItemBagWithStep(int step)
        {
            step = Mathf.Min(step, bonusItemsBag.Count);
            return bonusItemsBag[step];
        }

        private List<List<PropItem>> ReadBonusItems(string value)
        {
            List<List<PropItem>> list = new List<List<PropItem>>();
            string[] steps = value.Split('|');
            for(int i =0;i<steps.Length;i++)
            {
                string step = steps[i];
                if (string.IsNullOrEmpty(step) || step == "0")
                {
                    list.Add(null);
                }
                else
                {
                    List<PropItem> props = new List<PropItem>();
                    string[] items = step.Split(',');
                    foreach (string item in items)
                    {
                        string[] itemVlues = item.Split(':');
                        PropItem propItem = ConfigManager.Instance.propsConfig.GetItem(itemVlues[0]).Clone();
                        propItem.count = int.Parse(itemVlues[1]);
                        props.Add(propItem);
                    }
                    list.Add(props);
                }
            }

            return list;
        }

        private List<int> ReadPrice(string value)
        {
            List<int> list = new List<int>();
            string[] strs = value.Split('|');
            for(int i =0;i<strs.Length;i++)
            {
                list.Add(int.Parse(strs[i]));
            }
            return list;
            
        }

    }
}

