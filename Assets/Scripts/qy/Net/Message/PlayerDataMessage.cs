using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataMessage : NetMessage
{
    public string storyid;
    public long heartTime;
    public string country;
    public int fivemore;
    public int star;
    public int level;
    public int heart;
    public int gold;
    public string uid;
    public string pf;
    public string name;
    public string lang;
    public List<PropItem> items;
    public SevenDayData sevenDay;

    public RoleData[] roles;
    public StoryData[] stories;
    public string roleUuId="";//当前角色uuid
    public int discipline;
    public int loyalty;
    public int wisdom;
    public string storyExp="0";//总经验
    public string lvExp="0";//当前等级
    public int storyLv=1;//等级
    

    public class PropItem
    {
        public string itemId;
        public int count;
        public string uuid;
        /// <summary>
        /// 消失时间
        /// </summary>
        public int vanishTime;
    }

    public class SevenDayData
    {
        public SevenDayInfo sevenDayInfo;
        public int functionSwitch;
        public long endTime;
    }

    public class SevenDayInfo
    {
        public string uid;
        public long rewardTime;
        public long regTime;
        public int index;
        public int state;
        public long endTime;
    }

    public class StoryData
    {
        public string storyId;
    }

    public class RoleData
    {
        public string roleId;
        public string uuid;
        /// <summary>
        ///  1死亡 ，2通关    
        /// </summary>
        public int status;
    }
}

public class PlayerDataServerMessage : NetMessage
{
    public string storyid;
    public string heartTime;
    public string country;
    public int fivemore;
    public int star;
    public int level;
    public int heart;
    public int gold;
    public string uid;
    public string pf;
    public string name;
    public string lang;
    public List<PlayerDataMessage.PropItem> items;

    public List<PlayerDataMessage.RoleData> roles;
    public List<PlayerDataMessage.StoryData> stories;
    public string roleUuId = "";//当前角色uuid
    public int discipline;
    public int loyaty;
    public int wisdom;
    public string storyExp = "0";//总经验
    public string lvExp = "0";//当前等级
    public int storyLv = 1;//等级

    public PlayerDataServerMessage(qy.PlayerData playerData)
    {
        storyid = playerData.questId;
        heartTime = (playerData.hertTimestamp * 1000).ToString();
        gold = playerData.coinNum;
        heart = playerData.heartNum;
        star = playerData.starNum;
        name = playerData.nickName;
        uid = playerData.userId;
        level = playerData.eliminateLevel;
        items = new List<PlayerDataMessage.PropItem>();
        foreach (qy.config.PropItem item in playerData.propsDic.Values)
        {
            PlayerDataMessage.PropItem prop = new PlayerDataMessage.PropItem();
            prop.itemId = item.id;
            prop.count = item.count;
            prop.uuid = item.uuid;
            prop.vanishTime = item.vanishTime;
            items.Add(prop);
        }

        discipline = playerData.ability.discipline;
        loyaty = playerData.ability.loyalty;
        wisdom = playerData.ability.wisdom;
        storyExp = playerData.totalExp.ToString();
        lvExp = playerData.currExp.ToString();
        storyLv = playerData.level;
        roles = new List<PlayerDataMessage.RoleData>();
        foreach(string id in playerData.rolesState.Keys)
        {
            qy.PlayerData.RoleState state = playerData.rolesState[id];
            PlayerDataMessage.RoleData role = new PlayerDataMessage.RoleData();
            role.roleId = id;
            role.status = (int)state;
            roles.Add(role);
        }
        stories = new List<PlayerDataMessage.StoryData>();
        foreach(string id in playerData.complatedQuests.Keys)
        {
            PlayerDataMessage.StoryData storyData = new PlayerDataMessage.StoryData();
            storyData.storyId = id;
            stories.Add(storyData);
        }
    }
}
