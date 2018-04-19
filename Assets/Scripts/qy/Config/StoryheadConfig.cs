using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace qy.config
{
    public class StoryheadConfig:BaseConfig
    {
        private Dictionary<string, StoryHeadItem> dic = new Dictionary<string, StoryHeadItem>();

        public override string Name
        {
            get
            {
                return "storyhead.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            StoryHeadItem story = new StoryHeadItem();
            story.id = item.GetAttribute("id");
            story.bgFile = item.GetAttribute("bgFile");
            story.dialogue = GetLanguage(item.GetAttribute("dialogue"));
            story.nextStoryId = item.GetAttribute("next");
            story.personLocation = item.GetAttribute("personLocation");
            dic.Add(story.id, story);
        }

        public StoryHeadItem GetItem(string id)
        {
            StoryHeadItem value;
            dic.TryGetValue(id, out value);
            return value;
        }
    }

    public class StoryHeadItem
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
        public StoryHeadItem nextStory
        {
            get
            {
                return ConfigManager.Instance.StoryHeadConfig.GetItem(nextStoryId);
            }
        }
    }
}

