using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class StorysConfig : BaseConfig
    {

        private Dictionary<string, StoryItem> dic = new Dictionary<string, StoryItem>();

        public override string Name
        {
            get
            {
                return "story.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            StoryItem story = new StoryItem();
            story.id = item.GetAttribute("id");
            story.questId = item.GetAttribute("questId");
            story.bgFile = item.GetAttribute("bgFile");
            story.personFile = item.GetAttribute("personFile");
            story.dialogue = GetLanguage(item.GetAttribute("dialogue"));
            story.nextStoryId = item.GetAttribute("next");
            story.personLocation = item.GetAttribute("personLocation");
            dic.Add(story.id, story);
        }

        public StoryItem GetItem(string id)
        {
            StoryItem value;
            dic.TryGetValue(id, out value);
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
        internal string nextStoryId;
        /// <summary>
        /// 下一个story
        /// </summary>
        public StoryItem nextStory
        {
            get
            {
                return ConfigManager.Instance.storysConfig.GetItem(nextStoryId);
            }
        }
    }
}
