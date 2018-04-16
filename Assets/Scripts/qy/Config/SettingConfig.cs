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
        public string livesPrice;
        /// <summary>
        /// 恢复1条生命时间
        /// </summary>
        public string livesRecoverTime;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public string maxLives;
        public string price;
        public string bonusItems;
        public string addSteps;
        public string functionSwitchOpen;
        /// <summary>
        /// 最大关卡数
        /// </summary>
        public string max;
        /// <summary>
        /// 结算时的道具转化为金币奖励 
        /// </summary>
        public string columnbreaker;
        public string rowbreaker;
        public string planebreaker;
        public string bombbreaker;
        public string rainbow;
        public string maxgold;

        public override string Name
        {
            get
            {
                return "setting.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {

            System.Type t = this.GetType();
            foreach (XmlAttribute attribute in item.Attributes)
            {
                string name = attribute.Name;
                
                foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
                {
                    if (name == pi.Name)
                    {
                        pi.SetValue(this,attribute.Value,null);
                    }
                }
            }


            
        }

    }
}

