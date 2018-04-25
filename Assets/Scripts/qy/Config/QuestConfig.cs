using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class QuestConfig:BaseConfig
    {
        private Dictionary<string,QuestItem> dic = new Dictionary<string, QuestItem>();

        public override string Name()
        {
            return "quest.xml";
        }

        internal override void ReadItem(XmlElement item)
        {
            QuestItem quest = new QuestItem();
            quest.id = item.GetAttribute("id");
            quest.bg = item.GetAttribute("bg");
            quest.type = (QuestItem.QuestType)int.Parse(item.GetAttribute("type"));
            quest.sectionName = GetLanguage(item.GetAttribute("sectionName"));
            quest.sectionDes = GetLanguage(item.GetAttribute("sectionDes"));
            string star = item.GetAttribute("requireStar");
            quest.requireStar = String.IsNullOrEmpty(star) ? 0 : int.Parse(star);
            string endingType = item.GetAttribute("endingType");
            quest.endingType = String.IsNullOrEmpty(endingType)?0:int.Parse(endingType) ;

            quest.storyID = item.GetAttribute("storyId");
            quest.gotoId = item.GetAttribute("gotoId");

            quest.requireItem = ReadrequireItem(item.GetAttribute("requireItem"));
            quest.prize = ReadrequireItem(item.GetAttribute("prize"));
            quest.selectList = ReadSelectList(item.GetAttribute("selectList"));
            quest.endingPoint = ReadEnding(item.GetAttribute("endingPoint"));
            dic.Add(quest.id,quest);
        }

        private EndingPoint ReadEnding(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }
            EndingPoint endingPoint = new EndingPoint();
            string[] data = value.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            endingPoint.survival = int.Parse(data[0]);
            endingPoint.storyID = data[1];
            endingPoint.questID = data[2];

            return endingPoint;
        }
        
        private List<SelectItem> ReadSelectList(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }

            List<SelectItem> list = new List<SelectItem>();
            string[] items = value.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in items)
            {
                if(!string.IsNullOrEmpty(str))
                {
                    string[] data = str.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    SelectItem selectedItem = new SelectItem();
                    selectedItem.id = data[0];
                    selectedItem.name = GetLanguage(selectedItem.id);
                    selectedItem.storyID = data[1];
                    selectedItem.toQuestId = data[2];
                    string[] ability = data[3].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    selectedItem.ability = new Ability(int.Parse(ability[0]), int.Parse(ability[1]), int.Parse(ability[2]));
                    list.Add(selectedItem);
                }
                
            }
            return list;
        }
        private List<PropItem> ReadrequireItem(string value)
        {
            List<PropItem> list = new List<PropItem>();

            if (!string.IsNullOrEmpty(value) && value != "0")
            {
                string[] items = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in items)
                {
                    string[] data = str.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    string id = data[0];
                    int count = int.Parse(data[1]);
                    int rate = data.Length > 2 ? int.Parse(data[2]) : 0;
                    PropItem prop = ConfigManager.Instance.propsConfig.GetItem(id);
                    if(prop!=null)
                    {
                        prop.count = count;
                        prop.rate = rate;
                        list.Add(prop);
                    }
                    
                }
            }

            return list;
        }

        
        
        public QuestItem GetItem(string id)
        {
            QuestItem value;
            dic.TryGetValue(id, out value);
            if (value == null)
            {
                Debug.LogAssertion(string.Format("{0}表中没有找到id为 {1}的项", Name(), id));
            }
            return value;
        }

        
    }

    public class QuestItem
    {
        public enum QuestType
        {
            /// <summary>
            /// 主线任务
            /// </summary>
            Main = 1,
            /// <summary>
            /// 分支任务
            /// </summary>
            Branch,
            /// <summary>
            /// 关键任务
            /// </summary>
            Important,
            /// <summary>
            /// 结局任务
            /// </summary>
            Ending,
        }
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 背景图片名称
        /// </summary>
        public string bg;
        /// <summary>
        /// 类型
        /// </summary>
        public QuestType type;
        /// <summary>
        /// 任务标题
        /// </summary>
        public string sectionName;
        /// <summary>
        /// 任务描述
        /// </summary>
        public string sectionDes;
        /// <summary>
        /// 下一个任务ID 
        /// </summary>
        internal string gotoId;
        /// <summary>
        /// 下一个任务type!=Branch时起作用
        /// </summary>
        public QuestItem gotoQuest
        {
            get
            {
                return ConfigManager.Instance.questConfig.GetItem(gotoId);
            }
        }
        /// <summary>
        /// 完成任务需要星星数量
        /// </summary>
        public int requireStar;
        /// <summary>
        /// 完成任务需要物品
        /// </summary>
        public List<PropItem> requireItem;
        /// <summary>
        /// 奖励列表
        /// </summary>
        public List<PropItem> prize;

        internal string storyID;
        /// <summary>
        /// 完成任务后播放的剧情
        /// </summary>
        public StoryItem story
        {
            get
            {
                return ConfigManager.Instance.storysConfig.GetItem(storyID);
            }
        }

        /// <summary>
        /// 分支任务中选择项 type==Branch时起作用
        /// </summary>
        public List<SelectItem> selectList;
        /// <summary>
        /// 关键任务中进入结局剧情的条件和剧情id type==EndingPoint时起作用
        /// </summary>
        public EndingPoint endingPoint;
        /// <summary>
        /// 结局任务结果 1:正常结局 2：死亡 
        /// </summary>
        public int endingType;
    }

    public class SelectItem
    {
        public string id;
        /// <summary>
        /// 选项显示的名称
        /// </summary>
        public string name;

        internal string storyID;
        /// <summary>
        /// 选项对应的剧情id
        /// </summary>
        public StoryItem story
        {
            get
            {
                return ConfigManager.Instance.storysConfig.GetItem(storyID);
            }
        }

        /// <summary>
        /// 选项对应的下一个任务
        /// </summary>
        internal string toQuestId;
        public QuestItem toQuest
        {
            get
            {
                return ConfigManager.Instance.questConfig.GetItem(toQuestId);
            }
        }
        /// <summary>
        /// 能力值
        /// </summary>
        public Ability ability;
    }

    public class EndingPoint
    {
        /// <summary>
        /// 进入结局的最高生存率 
        /// </summary>
        public float survival;

        internal string storyID;
        /// <summary>
        /// 剧情
        /// </summary>
        public StoryItem story
        {
            get
            {
                return ConfigManager.Instance.storysConfig.GetItem(storyID);
            }
        }
        /// <summary>
        /// 任务
        /// </summary>
        public string questID;
        public QuestItem quest
        {
            get
            {
                return ConfigManager.Instance.questConfig.GetItem(questID);
            }
        }
    }

    /// <summary>
    /// 能力值
    /// </summary>
    public class Ability:Cloneable<Ability>
    {
        /// <summary>
        /// 纪律
        /// </summary>
        public int discipline = 0;
        /// <summary>
        /// 忠诚
        /// </summary>
        public int loyalty = 0;
        /// <summary>
        /// 智慧
        /// </summary>
        public int wisdom = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discipline">纪律</param>
        /// <param name="loyalty">忠诚</param>
        /// <param name="wisdom">智慧</param>
        public Ability(int discipline, int loyalty, int wisdom)
        {
            this.discipline = discipline;
            this.loyalty = loyalty;
            this.wisdom = wisdom;
        }

        public Ability()
        {

        }

        public static Ability operator +(Ability lhs, Ability rhs)
        {
            Ability abilityNew = new Ability()
            {
                discipline = lhs.discipline + rhs.discipline,
                loyalty = lhs.loyalty + rhs.loyalty,
                wisdom = lhs.wisdom + rhs.wisdom,
            };
            return abilityNew;
        }

        public override string ToString()
        {
            return string.Format("[discipline:{0}, loyalty:{1}, wisdom:{2}]", discipline, loyalty, wisdom);
        }
    }

}

