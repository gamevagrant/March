using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SettingItem
{
    //有冗余 setting表写成这样 服务端没时间改 暂时这样 建议：改表
    //目前针对这个表增加了字典的读取方式
    public string id;
    public string livesPrice;
    public string livesRecoverTime;
    public string maxLives;
    public string price;
    public string bonusItems;
    public string addSteps;
    public string functionSwitchOpen;
    public string maxNum;
    public string showDay;
    public string item;
    public string gold;
    public string star;

    public  List<SettingItem> settingItems = new List<SettingItem>(); 
}

public class setting : DatabaseConfig
{
    private SettingItem _item = new SettingItem();
    private Dictionary<string, Dictionary<string, string>> _item2 = new Dictionary<string, Dictionary<string, string>>();

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
            SettingItem _itemBean = new SettingItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.livesPrice = tempel1.GetAttribute("livesPrice");
            _itemBean.livesRecoverTime = tempel1.GetAttribute("livesRecoverTime");
            _itemBean.maxLives = tempel1.GetAttribute("maxLives");
            _itemBean.price = tempel1.GetAttribute("price");
            _itemBean.bonusItems = tempel1.GetAttribute("bonusItems");
            _itemBean.addSteps = tempel1.GetAttribute("addSteps");

            _item.settingItems.Add(_itemBean);


            Dictionary<string, string> _itemBean2 = new Dictionary<string, string>();

            for (int i = 0; i < tempel1.Attributes.Count; i++)
            {
                _itemBean2.Add(tempel1.Attributes[i].Name, tempel1.Attributes[i].Value);
            }

            _item2.Add(tempel1.GetAttribute("id"),_itemBean2);

        }
        return true;
    }

    private void ReadItem(Dictionary<string, string> _itemBean2, XmlElement tempel1, string str)
    {
        if (tempel1.GetAttribute(str) != "")
        {
            _itemBean2.Add(str, tempel1.GetAttribute(str));
        }
    }

    public Dictionary<string,string> GetDictionaryByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        return _item2[id];
    }


    public string GetValueByIDAndKey(string id, string key)
    {
        if (id.Length == 0)
        {
            return null;
        }
        if (_item2[id] != null)
        {
            return _item2[id][key];
        }
        return null;
    }



    public SettingItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _item.settingItems)
        {
            if (tempBean.id == id)
            {
                return tempBean;
            }
        }
        return null;
    }
}

