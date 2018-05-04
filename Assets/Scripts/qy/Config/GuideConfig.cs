using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace qy.config
{
    public class GuideConfig : BaseConfig
    {
        public enum GuideType:int
        {
            Dialog=1,
            Click,
        }
        private Dictionary<string, GuideItem> dic = new Dictionary<string, GuideItem>();
        private Dictionary<string, List<GuideItem>> uiNameDic = new Dictionary<string, List<GuideItem>>();
        public override string Name()
        {
            return "guide.xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            GuideItem guide = new GuideItem();
            guide.id = item.GetAttribute("id");
            guide.type = (GuideType)int.Parse(item.GetAttribute("type"));
            guide.personFile = item.GetAttribute("person");
            guide.dialogue = GetLanguage(item.GetAttribute("dialog"));
            guide.ui = item.GetAttribute("ui");
            guide.highlight = item.GetAttribute("highlight");
            guide.nextId = item.GetAttribute("next");
            guide.action = item.GetAttribute("action");

            string positionStr = item.GetAttribute("position");
            if(!string.IsNullOrEmpty(positionStr))
            {
                string[] p = positionStr.Split(':');
                guide.position = new Vector2(int.Parse(p[0]),int.Parse(p[1]));
            }
            dic.Add(guide.id, guide);

            if(uiNameDic.ContainsKey(guide.ui))
            {
                uiNameDic[guide.ui].Add(guide);
            }else
            {
                uiNameDic[guide.ui] = new List<GuideItem>() {guide };
            }
        }

        public GuideItem GetItem(string id)
        {
            GuideItem value;
            dic.TryGetValue(id, out value);
            return value;
        }

        public List<GuideItem> GetItemWithWindowName(string windowName)
        {
            List<GuideItem> value = new List<GuideItem>();
            uiNameDic.TryGetValue(windowName, out value);
            return value;
        }
    }

    public class GuideItem:StoryItem
    {
        /// <summary>
        /// 引导类型 1对话 2点击
        /// </summary>
        public GuideConfig.GuideType type;
        /// <summary>
        /// 当前UI
        /// </summary>
        public string ui;
        /// <summary>
        /// 需要高亮的区域，type为1时仅仅高亮，type为2时必须强制点击才能进行下一步
        /// </summary>
        public string highlight;
        /// <summary>
        /// 这条引导完成后的行为
        /// </summary>
        public string action;
        /// <summary>
        /// 引导员位置
        /// </summary>
        public Vector2 position = Vector2.zero;

        public override StoryItem next
        {
            get
            {
                return GameMainManager.Instance.configManager.guideConfig.GetItem(nextId);
            }
        }
    }
}

