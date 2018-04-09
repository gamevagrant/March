﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class StoryItem
{
    public string id = null;  //story id 
    public string bgmFile = null;
    public string questId = null;
    public string bgFile = null;
    public string personLocation = null;
    public string dialogue = null;
    public string animation = null;
    public string next = null;

    public List<StoryItem> storyItems = new List<StoryItem>(); 

}

public class story : DatabaseConfig
{
     private StoryItem _item = new StoryItem();

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
            StoryItem _itemBean = new StoryItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.bgmFile = tempel1.GetAttribute("bgmFile");
            _itemBean.questId = tempel1.GetAttribute("questId");
            _itemBean.bgFile = tempel1.GetAttribute("bgFile");
            _itemBean.personLocation = tempel1.GetAttribute("personLocation");
            _itemBean.dialogue = tempel1.GetAttribute("dialogue");
            _itemBean.animation = tempel1.GetAttribute("animation");
            _itemBean.next = tempel1.GetAttribute("next");

            _item.storyItems.Add(_itemBean);
        }
        return true;
    }
    public StoryItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _item.storyItems)
        {
            if (tempBean.id == id)
            {
                return tempBean;
            }
        }
        return null;
    }
}