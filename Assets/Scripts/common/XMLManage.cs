using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;
using System.Text;

public class XMLManage{

	//返回单个集合
	public static XElement GetXElement(XElement xElement, string rootName)
	{
		if(xElement!=null)
		{   
			IEnumerable<XElement> xmlItems = null;
			xmlItems = xElement.DescendantsAndSelf (rootName);
			if (xmlItems == null)
				return null;
			foreach (var xmlItem in xmlItems)
			{
				return xmlItem;
			}
			return null;
		}
		return null;
	}

	//返回全部集合
	public static List<XElement> GetXElementList(XElement xElement, string rootName)
	{
		List<XElement> xmlElementList = new List<XElement> ();
		if(xElement == null)
		{
			IEnumerable<XElement> xmlItems = null;
			if (rootName == "*") {
				xmlItems = xElement.Elements ();
			} else {
				xmlItems = xElement.DescendantsAndSelf (rootName);
			}
			if (xmlItems != null)
			{
				foreach (var xmlItem in xmlItems)
				{
					xmlElementList.Insert(xmlElementList.Count, xmlItem);
				}
			}
		}
		return xmlElementList;
	}

	public static string GetXElementAttributeStr(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return "";
		return (string)attrib;
	}

	public static int GetXElementAttributeInt(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return -1;
		string str = (string)attrib;
		if (null == str || str == "") return -1;
		int nReturn = 0;
		if(int.TryParse(str,out nReturn))
		{
			return nReturn;
		}
		else
		{
			return -1;
		}
	}

	public static long GetXElementAttributeLong(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return -1;
		string str = (string)attrib;
		if (null == str || str == "") return -1;
		long nReturn = 0;
		if(long.TryParse(str,out nReturn))
		{
			return nReturn;
		}
		else
		{
			return -1;
		}
	}

	public static double GetXElementAttributeDouble(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return -1;
		string str = (string)attrib;
		if (null == str || str == "") return -1;
		double nReturn = 0;
		if(double.TryParse(str,out nReturn))
		{
			return nReturn;
		}
		else
		{
			return -1;
		}
	}

	public static float GetXElementAttributeFloat(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return -1.0f;
		string str = (string)attrib;
		if (null == str || str == "") return -1.0f;
		float nReturn = 0.0f;
		if (float.TryParse(str, out nReturn))
		{
			return nReturn;
		}
		else
		{
			return -1.0f;
		}
	}

	public static Vector3 GetXElementAttributeVector3(XElement XElement, string attributeName)
	{
		XAttribute attrib = GetAttribute(XElement, attributeName);
		if (null == attrib) return Vector3.zero;
		string[] f_slit = attrib.Value.Split (',');
		if (f_slit.Length > 2) {
			return new Vector3 (float.Parse (f_slit [0]), float.Parse (f_slit [1]), float.Parse (f_slit [2]));
		} else {
			return Vector3.zero;
		}
	}

	public static XAttribute GetAttribute(XElement XElement, string attribute)
	{
		if (null == XElement) return null;
		try
		{
			try
			{
				XAttribute attrib = XElement.Attribute(attribute);
				if (null == attrib)
				{
					return null;
				}

				return attrib;
			}
			catch (UnityException e)
			{
				
				return null;
			}
		}
		finally
		{
		}
	}

	public static void SaveXElementToFile(string path, XElement root)
	{
		using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
		{
			sw.Write(root.ToString());
		}
	}
}
