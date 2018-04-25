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

    public qy.config.RoleItem[] roles;
    public StoryData[] stories;
    public string roleUuid="";//当前角色uuid
    public int discipline;
    public int loyaty;
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
    public List<PropItem> items;

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


}
