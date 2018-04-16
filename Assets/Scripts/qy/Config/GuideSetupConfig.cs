using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace qy.config
{
    public class GuideSetupConfig : BaseConfig
    {
        private Dictionary<string, StoryItem> dic = new Dictionary<string, StoryItem>();

        public override string Name
        {
            get
            {
                return "guidesetup.xml";
            }
        }

        internal override void ReadItem(XmlElement item)
        {
            StoryItem story = new StoryItem();
            story.id = item.GetAttribute("id");
            story.personLocation = item.GetAttribute("npcposition");
            story.personFile = "";
            story.dialogue = GetLanguage(item.GetAttribute("dialogue"));
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

