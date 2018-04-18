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
