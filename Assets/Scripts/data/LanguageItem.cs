using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LanguageItem
{

    public string id;
    public string value;

    public  List<LanguageItem> languageItems =  new List<LanguageItem>(); 
}
public class language : DatabaseConfig
{
	private LanguageItem _item = new LanguageItem();

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
			LanguageItem _languageItem = new LanguageItem();
			_languageItem.id = tempel1.GetAttribute("id");
			_languageItem.value = tempel1.GetAttribute("value");

			_item.languageItems.Add(_languageItem);
		}
		return true;
	}
	public LanguageItem GetItemByID(string id)
	{
		if (id.Length == 0)
		{
			return null;
		}
		foreach (var tempBean in _item.languageItems)
		{
			if (tempBean.id == id)
			{
				return tempBean;
			}
		}
		return null;
	}

	public string GetValueByKey(string key)
	{
		if (key.Length == 0)
		{
			return null;
		}
		foreach (var tempBean in _item.languageItems)
		{
			if (tempBean.id == key)
			{
				return tempBean.value;
			}
		}
		return "";
	}
}


public class language_cn : DatabaseConfig
{
    private LanguageItem _item = new LanguageItem();

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
            LanguageItem _languageItem = new LanguageItem();
            _languageItem.id = tempel1.GetAttribute("id");
            _languageItem.value = tempel1.GetAttribute("value");

            _item.languageItems.Add(_languageItem);
        }
        return true;
    }
    public LanguageItem GetItemByID(string id)
    {
        if (id.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _item.languageItems)
        {
            if (tempBean.id == id)
            {
                return tempBean;
            }
        }
        return null;
    }

    public string GetValueByKey(string key)
    {
        if (key.Length == 0)
        {
            return null;
        }
        foreach (var tempBean in _item.languageItems)
        {
            if (tempBean.id == key)
            {
                return tempBean.value;
            }
        }
        return "";
    }
}
