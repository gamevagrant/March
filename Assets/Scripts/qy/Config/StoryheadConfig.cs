using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace qy.config
{
    public class StoryheadConfig:BaseConfig
    {
        private Dictionary<string, StoryItem> dic = new Dictionary<string, StoryItem>();

        public override string Name
        {
            get
            {
                return "storyhead.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            StoryItem story = new StoryItem();
            story.id = item.GetAttribute("id");
            story.bgFile = item.GetAttribute("bgFile");
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
}

