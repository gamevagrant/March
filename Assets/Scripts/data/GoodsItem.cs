using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GoodsItem
{
    public string id;
    public string name;
    public string icon;
    public string description;
    public string type;
    public string lv;
    public string lv_limit;
    public string price;

    public List<GoodsItem> goodsItems = new List<GoodsItem>();
}

public class item : DatabaseConfig
{
    private GoodsItem _item = new GoodsItem();

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
            GoodsItem _itemBean = new GoodsItem();
            _itemBean.id = tempel1.GetAttribute("id");
            _itemBean.name = tempel1.GetAttribute("name");
            _itemBean.icon = tempel1.GetAttribute("icon");
            _itemBean.description = tempel1.GetAttribute("description");
            _itemBean.type = tempel1.GetAttribute("type");
            _itemBean.lv = tempel1.GetAttribute("lv");
            _itemBean.lv_limit = tempel1.GetAttribute("lv_limit");
            _itemBean.price = tempel1.GetAttribute("price");

            _item.goodsItems.Add(_itemBean);
        }
        return true;
    }
    public GoodsItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _item.goodsItems)
        {
            if (tempBean.id == id)
            {
                return tempBean;
            }
        }
        return null;
    }
}
