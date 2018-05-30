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
            string effect = item.GetAttribute("effect");
            story.effect = string.IsNullOrEmpty(effect) ? 0 : int.Parse(effect);

            dic.Add(story.id, story);
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
        /// 特效（1：抖动 2：血屏 3：背景变灰 4:模糊 5:遮罩扩散 6:拉近拉远 7:左右滚动）
        /// </summary>
        public int effect;

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
    }
}
