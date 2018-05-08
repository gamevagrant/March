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
    private string _gotoId;
    public string selectList;
    public string requireItem;
    public string storyId;
    public string requireStar;
    public string endingPoint;//剧情结局关键点

    public List<QuestItem> questItems = new List<QuestItem>();

    private string selectedID;//分支剧情的选择

    /// <summary>
    ///type==2 时设置选择的分支
    /// </summary>
    public void SetSelectedBranch(string id)
    {
        selectedID = id;
    }
    /// <summary>
    /// 设置当前的生还率
    /// </summary>
    public void SetSurvival(int survival)
    {
    }

    public Ability GetAbility(string id)
    {
        if (type == "2")
        {
            string[] branchs = selectList.Split(',');
            foreach (string branch in branchs)
            {
                string[] data = branch.Split(':');
                if (data[0] == id && data.Length > 3)
                {
                    string[] abilityStr = data[3].Split('|');
                    Ability ability = new Ability()
                    {
                        discipline = int.Parse(abilityStr[0]),
                        loyalty = int.Parse(abilityStr[1]),
                        wisdom = int.Parse(abilityStr[2]),
                    };
                    return ability;
                }

            }
        }
        return null;
    }

    public string gotoId
    {
        get
        {
            if (type == "2")
            {
                string[] branchs = selectList.Split(',');
                foreach (string branch in branchs)
                {
                    string[] data = branch.Split(':');
                    if (data[0] == selectedID && data.Length > 2)
                    {
                        return data[2];
                    }

                }

            }

            return _gotoId;
        }
        set
        {
            _gotoId = value;
        }
    }
}
/// <summary>
/// 能力值
/// </summary>
public class Ability
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
        return string.Format("[discipline:{0} loyalty:{1} wisdom:{2}]", discipline, loyalty, wisdom);
    }
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
            _itemBean.endingPoint = tempel1.GetAttribute("endingPoint");

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
                Debug.Log("selectList before split:" + list);
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
