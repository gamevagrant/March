using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class LevelItem
{
    public string id;
    public string level;
    public string hard;
    public string star;
    public string coin;
    public string itemReward;

    public List<LevelItem> levelItems = new List<LevelItem>(); 

}

public class matchlevel : DatabaseConfig
{
    private LevelItem _item = new LevelItem();

    public override bool Read(string text)
    {
        if (base.Read(text) == false)
        {
            return false;
        }
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(text);

        XmlElement rootElem = xmldoc.DocumentElement;
        //读取根节点下面子节点的元素  
        XmlNodeList xmllist1 = rootElem.ChildNodes;
        //遍历所有根节点下面子节点的属性  
        foreach (XmlElement tempel1 in xmllist1)
        {
            LevelItem _itemBean = new LevelItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.level = tempel1.GetAttribute("level");
            _itemBean.hard = tempel1.GetAttribute("hard");
            _itemBean.star = tempel1.GetAttribute("star");
            _itemBean.coin = tempel1.GetAttribute("coin");
            _itemBean.itemReward = tempel1.GetAttribute("itemReward");
            _item.levelItems.Add(_itemBean);

        }
        return true;
    }

    public Dictionary<string, string> GetRewardsByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        Dictionary<string, string> value = new Dictionary<string, string>();
        foreach (LevelItem tempBean in _item.levelItems)
        {
            if (tempBean.id == id)
            {
                string list = tempBean.itemReward;
                Debug.Log("selectList before split:" + list);
                string[] v = list.Split(',');
                foreach (string va in v)
                {
                    string[] ids = va.Split(':');
                    value[ids[0]] = ids[1];
                }
                if (value.Count != 0)
                    return value;
            }
        }
        return null;
    }
    public LevelItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (LevelItem tempBean in _item.levelItems)
        {

            if (tempBean.id == id)
            {
                Debug.Log("Test:" + tempBean.id);
                return tempBean;
            }
        }
        return null;
    }

}