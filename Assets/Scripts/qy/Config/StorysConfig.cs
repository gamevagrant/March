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
