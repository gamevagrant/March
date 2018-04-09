using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class QuestItem
{
    public string id;  //story id 
    public string bg;
    public string type;
    public string sectionName;
    public string sectionDes;
    public string gotoId;
    public string selectList;
    public string requireItem;
    public string storyId;
    public string requireStar;

    public List<QuestItem> questItems = new List<QuestItem>();
}

public class quest : DatabaseConfig
{

    private QuestItem _item = new QuestItem();

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
            QuestItem _itemBean = new QuestItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.bg = tempel1.GetAttribute("bg");
            _itemBean.type = tempel1.GetAttribute("type");
            _itemBean.sectionName = tempel1.GetAttribute("sectionName");
            _itemBean.sectionDes = tempel1.GetAttribute("sectionDes");
            _itemBean.gotoId = tempel1.GetAttribute("gotoId");
            _itemBean.selectList = tempel1.GetAttribute("selectList");
            _itemBean.requireItem = tempel1.GetAttribute("requireItem");
            _itemBean.storyId = tempel1.GetAttribute("storyId");
            _itemBean.requireStar = tempel1.GetAttribute("requireStar");

            _item.questItems.Add(_itemBean);

        }
        return true;
    }

    public Dictionary<string, string> GetQuestSelectListByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        Dictionary<string, string> value = new Dictionary<string, string>();
        foreach (QuestItem tempBean in _item.questItems)
        {
            if (tempBean.id == id)
            {
                string list = tempBean.selectList;
                Debug.Log("selectList before split:" +list);
                string[] v = list.Split(',');
                foreach (string va in v)
                {
                    string[] ids = va.Split(':');
                    value[ids[0]] = ids[1];
                    Debug.Log(" value[ids[0]] :" + value[ids[0]]);
                    Debug.Log("ids[1] :" + ids[1]);
                }
                if (value.Count != 0)
                    return value;
            }
        }
        return null;
    }

    public Dictionary<string, string> GetRequireItemsByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        Dictionary<string, string> value = new Dictionary<string, string>();
        foreach (QuestItem tempBean in _item.questItems)
        {
            if (tempBean.id == id)
            {
                string list = tempBean.requireItem;
                Debug.Log("requireItems before split:" + list);
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

    public QuestItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (QuestItem tempBean in _item.questItems)
        {

            if (tempBean.id == id)
            {
                Debug.Log("Test:" + tempBean.sectionDes);
                Debug.Log("Test:" + tempBean.id);
                return tempBean;
            }
        }
        return null;
    }

}
