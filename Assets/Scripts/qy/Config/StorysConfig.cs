using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class StorysConfig : BaseConfig
    {

        private Dictionary<string, StoryItem> dic = new Dictionary<string, StoryItem>();

        public override string Name()
        {
            return "story.xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            StoryItem story = new StoryItem();
            story.id = item.GetAttribute("id");
            story.questId = item.GetAttribute("questId");
            story.bgFile = item.GetAttribute("bgFile");
            story.personFile = item.GetAttribute("personFile");
            story.dialogue = GetLanguage(item.GetAttribute("dialogue"));
            story.nextId = item.GetAttribute("next");
            story.personLocation = item.GetAttribute("personLocation");
            string weather = item.GetAttribute("weather");
            story.weather = string.IsNullOrEmpty(weather) ? 0 : int.Parse(weather);
            story.effects = ReadEffects(item.GetAttribute("effect"));

            dic.Add(story.id, story);
        }

        private List<StoryItem.Effect> ReadEffects(string data)
        {
            if(string.IsNullOrEmpty(data))
            {
                return new List<StoryItem.Effect>();
            }
            List<StoryItem.Effect> list = new List<StoryItem.Effect>();
            string[] effectsStr = data.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach(string effectStr in effectsStr)
            {
                string[] strs = effectStr.Split(new char[]{ ':'},System.StringSplitOptions.RemoveEmptyEntries);
                List<int> effectData = new List<int>();
                for(int i =1;i<strs.Length;i++)
                {
                    effectData.Add(int.Parse(strs[i]));
                }
                StoryItem.Effect effect = new StoryItem.Effect()
                {
                    type = (StoryItem.Effect.EffectType)int.Parse(strs[0]),
                    data = effectData,
                };
                list.Add(effect);
            }

            return list;
        }

        public StoryItem GetItem(string id)
        {
            if(string.IsNullOrEmpty(id)||id=="0")
            {
                return null;
            }
            StoryItem value;
            dic.TryGetValue(id, out value);
            if (value == null)
            {
                Debug.LogAssertion(string.Format("{0}表中没有找到id为 {1}的项", Name(), id));
            }
            return value;
        }
    }

    public class StoryItem
    {
       
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 所属任务id
        /// </summary>
        public string questId;
        /// <summary>
        /// 背景图名字
        /// </summary>
        public string bgFile;
        /// <summary>
        /// 对话人物图片名称
        /// </summary>
        public string personFile;
        /// <summary>
        /// 对话人物的位置 0：左边 1：右边
        /// </summary>
        public string personLocation;
        /// <summary>
        /// 对话
        /// </summary>
        public string dialogue;
        /// <summary>
        /// 天气（1：下雨 2：下雪）
        /// </summary>
        public int weather;
        /// <summary>
        /// 特效（1：抖动 2：血屏 3：背景变灰 4:模糊[0:模糊变清晰 1:清晰变模糊] 5:遮罩扩散 6:拉近拉远[0:放大 1:缩小] 7:左右滚动[0:镜头向左移动 1:镜头向右移动]）【特效1id:特效1参数,特效2id:特效2参数】
        /// </summary>
        public List<StoryItem.Effect> effects;

        internal string nextId;
        /// <summary>
        /// 下一个story
        /// </summary>
        public virtual StoryItem next
        {
            get
            {
                return ConfigManager.Instance.storysConfig.GetItem(nextId);
            }
        }

        public class Effect
        {
            public enum EffectType
            {
                Shake = 1,//抖动
                Blood,//血
                Gray,//灰色
                Blur,//模糊
                Mask,//遮罩扩散
                Zoom,//放大缩小
                Scroll,//滚动
            }

            public EffectType type;
            public List<int> data;
        }
    }
}
