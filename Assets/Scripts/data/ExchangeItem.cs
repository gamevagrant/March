using System.Collections.Generic;
using System.Xml;

public class ExchangeItem
{
    public string ID;
    public string Dollar;
    public string Gold;

    public List<ExchangeItem> ExchangeItemList = new List<ExchangeItem>();
}

public class exchange : DatabaseConfig
{
    public ExchangeItem ExchangeItem { get; set; }

    public override bool Read(string text)
    {
        if (base.Read(text) == false)
        {
            return false;
        }
        var xmldoc = new XmlDocument();
        xmldoc.LoadXml(text);

        ExchangeItem = new ExchangeItem();

        var rootElem = xmldoc.DocumentElement;
        //读取根节点下面子节点的元素  
        var xmllist1 = rootElem.ChildNodes;
        //遍历所有根节点下面子节点的属性  
        foreach (XmlElement tempel1 in xmllist1)
        {
            var item = new ExchangeItem
            {
                ID = tempel1.GetAttribute("id"),
                Dollar = tempel1.GetAttribute("dollar"),
                Gold = tempel1.GetAttribute("gold_doller")
            };

            ExchangeItem.ExchangeItemList.Add(item);
        }
        return true;
    }

    public ExchangeItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var item in ExchangeItem.ExchangeItemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }
}
