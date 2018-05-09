using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using qy.config;
namespace qy
{
    public class PlayerData
    {
        public enum RoleState:int
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal,
            /// <summary>
            /// 死亡
            /// </summary>
            Dide,
            /// <summary>
            /// 通关
            /// </summary>
            Pass,
        }

        /// <summary>
        /// id
        /// </summary>
        public string userId = "";
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName = "";
        /// <summary>
        /// //星数
        /// </summary>
        public int starNum = 0;
        /// <summary>
        /// //心数
        /// </summary>
        public int heartNum = 0;
        /// <summary>
        /// //金币
        /// </summary>
        public int coinNum = 0;
        /// <summary>
        /// //消除关卡
        /// </summary>
        public int eliminateLevel = 1;

        private long _hertTimestamp;
        /// <summary>
        /// //下次补充生命的时间戳
        /// </summary>
        public long hertTimestamp
        {
            get
            {
                return _hertTimestamp;
            }
            set
            {
                _hertTimestamp = value;
                if (_hertTimestamp != 0)
                {
                    recoveryLeftTime = (int)(_hertTimestamp - GameUtils.DateTimeToTimestamp(System.DateTime.Now));
                    updataTimeTag = Time.unscaledTime;
                }else
                {
                    recoveryLeftTime = 0;
                    updataTimeTag = 0;
                }
            }
        }
        private int recoveryLeftTime;
        private float updataTimeTag;
        public int countDown
        {
            get
            {
                return Mathf.Max(0,recoveryLeftTime - (int)(Time.unscaledTime - updataTimeTag));
            }
        }
        /// <summary>
        /// //剧情ID
        /// </summary>
        public string questId
        {
            get
            {
                return role==null?"":role.questID;
            }
            set
            {
                if (role != null)
                    role.questID = value;
            }
        }
        /// <summary>
        /// 下一个任务
        /// </summary>
        public string nextQuestId;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level = 1;
        /// <summary>
        /// 总经验值
        /// </summary>
        public int totalExp = 0;
        /// <summary>
        /// 当前经验值
        /// </summary>
        public int currExp = 0;
        /// <summary>
        /// 能力值
        /// </summary>
        public config.Ability ability
        {
            get
            {
                return role==null?new qy.config.Ability(): role.ability;
            }
            set
            {
                if(role!=null)
                    role.ability = value;
            }
        }
        /// <summary>
        /// 是否获得7天奖励信息
        /// </summary>
        public bool isSaveDayInfo = false;
        /// <summary>
        /// 7天登录奖励 0-6表示某天
        /// </summary>
        public int indexDay = 0;
        /// <summary>
        /// 7天奖励 领取状态 0未领取 1已领取 2未开启
        /// </summary>
        public int awardState = 1;
        public bool isPlayScene = false;

        public bool isNeedUpLoadOffLine;
        /// <summary>
        /// 数据是否发生改变
        /// </summary>
        public bool dirty = false;
        /// <summary>
        /// 当前角色
        /// </summary>
        public RoleItem role;
        /// <summary>
        ///  //道具解锁状态, "0"缺省, "1"表示解锁导弹, 所有下关开始的时候, 道具列表要自动勾选导弹; "2"魔方; "3"飞机
        /// </summary>
        public string showUnlockItemStatus = "0";
        /// <summary>
        /// //需要展示了第9关引导
        /// </summary>
        public bool needShow9Help = false;

        public bool isShowedLoginAward = false;
        /// <summary>
        /// 玩家持有的物品列表
        /// </summary>
        public Dictionary<string, PropItem> propsDic = new Dictionary<string, PropItem>();
        /// <summary>
        /// 完成过的任务id
        /// </summary>
        public Dictionary<string, int> complatedQuests = new Dictionary<string, int>();
        /// <summary>
        /// 分支任务中选过的选项 questID_index
        /// </summary>
        public Dictionary<string, int> selectedItems = new Dictionary<string, int>();
        /// <summary>
        /// 角色状态
        /// </summary>
        public Dictionary<string, RoleState> rolesState = new Dictionary<string, RoleState>();

        public RoleState roleState
        {
            get
            {
                if(role!=null)
                {
                    return GetRoleState(role.id);
                }
                return RoleState.Normal;
            }
        }

        public string lang
        {
            get
            {
                return (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified) ? "cn" : "en";
            }
        }
        //生存率计算公式：（纪律、忠诚、智慧的平均值+纪律、忠诚、智慧的最小值）/2
        public int survival
        {
            get
            {
                return ((ability.loyalty + ability.wisdom + ability.discipline) / 3 + Mathf.Min(ability.loyalty, ability.wisdom, ability.discipline)) / 2;
            }
        }

        public PropItem GetPropItem(string id)
        {
            PropItem item;
            propsDic.TryGetValue(id,out item);

            return item;
        }
        /// <summary>
        /// 给角色加物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        public void AddPropItem(string id, int num)
        {
            PropItem item;
            propsDic.TryGetValue(id, out item);
            if (item != null)
            {
                item.count += num;
            }
            else
            {
                item = GameMainManager.Instance.configManager.propsConfig.GetItem(id);
                item.count = num;
                propsDic.Add(id,item);
            }
        }
        /// <summary>
        /// 给角色减物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        public void RemovePropItem(string id,int num)
        {
            PropItem item;
            propsDic.TryGetValue(id, out item);
            if(item!=null && item.count>num)
            {
                item.count -= num;
                item.count = Mathf.Max(0,item.count);
            }
            else
            {
                propsDic.Remove(id);
            }
        }
        /// <summary>
        /// 设置角色状态
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="state"></param>
        public void SetRoleState(string id,RoleState state)
        {
            if(rolesState.ContainsKey(id))
            {
                rolesState[id] = state;
            }else
            {
                rolesState.Add(id,state);
            }
        }
        /// <summary>
        /// 获取角色状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleState GetRoleState(string id)
        {
            RoleState state = RoleState.Normal;
            rolesState.TryGetValue(id,out state);
            return state;
        }



        public config.QuestItem GetQuest()
        {
            return string.IsNullOrEmpty(questId)?null:GameMainManager.Instance.configManager.questConfig.GetItem(questId);
        }

        /// <summary>
        ///  分支剧情中 指定的选项是否选择过
        /// </summary>
        /// <param name="questID"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ContainsSelected(string questID,string id)
        {
            return selectedItems.ContainsKey(questID+"_"+ id);
        }
        /// <summary>
        /// 是否完成过此任务
        /// </summary>
        /// <param name="questID"></param>
        /// <returns></returns>
        public bool ContainsComplateQuest(string questID)
        {
            return complatedQuests.ContainsKey(questId);
        }

        public void RefreshData(PlayerDataMessage message)
        {
            
            hertTimestamp = message.heartTime/1000;
            coinNum = message.gold;
            heartNum = message.heart;
            starNum = message.star;
            nickName = message.name;
            userId = message.uid;
            eliminateLevel = message.level;
            

            if(message.sevenDay!=null && message.sevenDay.sevenDayInfo!=null)
            {
                indexDay = message.sevenDay.sevenDayInfo.index;
                awardState = message.sevenDay.sevenDayInfo.state;
            }else
            {
                awardState = 2;
            }
           
            propsDic = new Dictionary<string, config.PropItem>();
            foreach(PlayerDataMessage.PropItem item in message.items)
            {
                PropItem prop = GameMainManager.Instance.configManager.propsConfig.GetItem(item.itemId);
                if(prop!=null)
                {
                    prop.count = item.count;
                    prop.uuid = item.uuid;
                    prop.vanishTime = item.vanishTime;
                }
                
     
                propsDic.Add(item.itemId,prop);
            }

            //---------------------------------------
            if(message.roles!=null)
            {
                foreach (PlayerDataMessage.RoleData item in message.roles)
                {
                    SetRoleState(item.roleId, (RoleState)item.status);
                }
            }
            if(message.stories!=null)
            {
                foreach (PlayerDataMessage.StoryData story in message.stories)
                {
                    if (!complatedQuests.ContainsKey(story.storyId))
                    {
                        complatedQuests.Add(story.storyId, 0);
                    }
                }
            }

            if(!string.IsNullOrEmpty(message.roleUuId))
            {
                string roleID = "101";
                foreach(PlayerDataMessage.RoleData role in message.roles)
                {
                    if(role.uuid == message.roleUuId)
                    {
                        roleID = role.roleId;
                    }
                }
                Debug.Log("============"+roleID.ToString());
                Debug.Log("----------"+GameMainManager.Instance.configManager.roleConfig.ToString());
                role = GameMainManager.Instance.configManager.roleConfig.GetItem(roleID).Clone();
            }
            questId = message.storyid;//任务id存到角色数据中有了角色再保存数据
            if (!string.IsNullOrEmpty(nextQuestId))
            {
                questId = nextQuestId;
            }
            ability = new config.Ability(message.discipline,message.loyalty,message.wisdom);
            totalExp = int.Parse(message.storyExp);
            currExp = int.Parse(message.lvExp);
            level = message.storyLv == 0?1: message.storyLv;
            //-----------------------------------

            dirty = false;

            SaveData();
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
        }

        public override string ToString()
        {
            var result = string.Format("star count-{0}\nheart count-{1}\ncoin count-{2}\nmain level-{3}\nquest id-{4}", starNum, heartNum, coinNum, eliminateLevel, questId);
            return result;
        }


        private void SaveData()
        {
            LocalDatasManager.playerData = this;
        }
    }
}

