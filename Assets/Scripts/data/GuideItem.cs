using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class GuideItem
{
    public string id;
    public string npcid;
    public string npcposition;
    public string dialogue;

    public List<GuideItem> guidesetups = new List<GuideItem>();
}

public class guidesetup : DatabaseConfig
{
    public GuideItem _guidesetup = new GuideItem();

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
            GuideItem _itemBean = new GuideItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.npcid = tempel1.GetAttribute("npcid");
            _itemBean.npcposition = tempel1.GetAttribute("npcposition");
            _itemBean.dialogue = tempel1.GetAttribute("dialogue");
            

            _guidesetup.guidesetups.Add(_itemBean);
        }
        return true;
    }
    public GuideItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _guidesetup.guidesetups)
        {
            if (tempBean.id == id)
            {
                return tempBean;
            }
        }
        return null;
    }

}