﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using qy.config;
namespace qy
{
    public class PlayerData
    {
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
        /// <summary>
        /// //下次补充生命的时间戳
        /// </summary>
        public long hertTimestamp;
        /// <summary>
        /// //下一次生命恢复剩余时间 秒
        /// </summary>
        public long recoveryLeftTime = 0;
        /// <summary>
        /// //剧情ID
        /// </summary>
        public string questId = "10001";

        public config.QuestItem quest
        {
            get
            {
                return GameMainManager.Instance.configManager.questConfig.GetItem(questId);
            }
        }
        /// <summary>
        /// //心数恢复间隔是30min
        /// </summary>
        public int heartRecoverTime = 30;
        /// <summary>
        /// //最大生命数
        /// </summary>
        public int maxLives = 5;
        /// <summary>
        /// //补满生命金币价格
        /// </summary>
        public int livePrice = 900; 
        /// <summary>
        /// 能力值
        /// </summary>
        public config.Ability ability = new config.Ability();
        /// <summary>
        /// 是否获得7天奖励信息
        /// </summary>
        public bool isSaveDayInfo = false;
        /// <summary>
        /// 7天登录奖励 0-6表示某天
        /// </summary>
        public int indexDay = 0;
        /// <summary>
        /// 7天奖励 领取状态 0未领取 1已领取
        /// </summary>
        public int awardState = 1;
        public bool isPlayScene = false;
        /// <summary>
        /// 数据每次改变时自增1，和服务器通信一次行为减1，下次正常通信后超过5进行一次同步，归0
        /// </summary>
        public int dirty = 0;

        //道具解锁状态, "0"缺省, "1"表示解锁导弹, 所有下关开始的时候, 道具列表要自动勾选导弹; "2"魔方; "3"飞机
        public string showUnlockItemStatus = "0";
        //需要展示了第9关引导
        public bool needShow9Help = false;

        public bool isShowedLoginAward = false;

        private Dictionary<string, config.PropItem> propsDic = new Dictionary<string, config.PropItem>();

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
                item = GameMainManager.Instance.configManager.propsConfig.GetItem(id).Clone();
                item.count = num;
                propsDic.Add(id,item);
            }
        }
        public void RemovePropItem(string id,int num)
        {
            PropItem item;
            propsDic.TryGetValue(id, out item);
            if(item!=null && item.count>num)
            {
                item.count -= num;
            }else
            {
                propsDic.Remove(id);
            }
        }

        public void RefreshData(PlayerDataMessage message)
        {
            questId = message.storyid;
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
            }
            if (message.heartTime!=0)
            {
                long temp = hertTimestamp - GameUtils.DateTimeToTimestamp(System.DateTime.Now);
                recoveryLeftTime = temp;
                Debug.Log("系统时间:" + GameUtils.DateTimeToTimestamp(System.DateTime.Now));
                Debug.Log("下一次心数恢复是:" + hertTimestamp);
                Debug.Log("下一次心数恢复还剩余时间是:" + recoveryLeftTime.ToString());

            }
            propsDic = new Dictionary<string, config.PropItem>();
            foreach(PlayerDataMessage.PropItem item in message.items)
            {
                config.PropItem prop = GameMainManager.Instance.configManager.propsConfig.GetItem(item.itemId).Clone();
                prop.count = item.count;
                prop.uuid = item.uuid;
                prop.vanishTime = item.vanishTime;
     
                propsDic.Add(item.itemId,prop);
            }

        }

        public PlayerDataMessage ToPlayerDataMessage()
        {
            PlayerDataMessage message = new PlayerDataMessage();
            message.storyid = questId;
            message.heartTime = hertTimestamp * 1000;
            message.gold = coinNum;
            message.heart = heartNum;
            message.star = starNum;
            message.name = nickName;
            message.uid = userId;
            message.level = eliminateLevel;
            message.items = new List<PlayerDataMessage.PropItem>();
            foreach(PropItem item in propsDic.Values)
            {
                PlayerDataMessage.PropItem prop = new PlayerDataMessage.PropItem();
                prop.itemId = item.id;
                prop.count = item.count;
                prop.uuid = item.uuid;
                prop.vanishTime = item.vanishTime;
            }

            return message;
        }

        public override string ToString()
        {
            var result = string.Format("star count-{0}\nheart count-{1}\ncoin count-{2}\nmain level-{3}\nquest id-{4}", starNum, heartNum, coinNum, eliminateLevel, questId);
            return result;
        }

    }
}

