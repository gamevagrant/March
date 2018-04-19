using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Common;
using UnityEngine;

public class XMLDataManager : Singleton<XMLDataManager>
{
    public string getPlatForm(DatabaseConfig configPath)
    {
        string path = String.Format(Application.streamingAssetsPath + "/xml/" + configPath.ToString() + ".xml");
        return path;
    }

	public string getLanguagePath(DatabaseConfig configPath)
	{
		string curLang = PlayerData.instance.getLang();
		string path = String.Format(Application.streamingAssetsPath + "/xml/" + configPath.ToString() + "_" +curLang + ".xml");
		Debug.Log ("当前语言文件读取的是" + path);
		return path;
	}
    public IEnumerator loadXML(DatabaseConfig tempConfig)
    {
        string path = getPlatForm(tempConfig);
        Debug.Log("load xml path is:" + path);
#if UNITY_EDITOR
        path = "file://" + path;
#endif
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            tempConfig.Read(www.text);
            if (!DefaultConfig.getInstance().configDic.ContainsKey(tempConfig.GetType()))
            {
                DefaultConfig.getInstance().configDic.Add(tempConfig.GetType(), tempConfig);
            }
        }

    }

	// language xml read
	public IEnumerator loadLangXML(DatabaseConfig tempConfig)
	{
		string path = getLanguagePath(tempConfig);
		Debug.Log("load xml path is:" + path);
		#if UNITY_EDITOR
		path = "file://" + path;
		#endif
		WWW www = new WWW(path);
		yield return www;
		if (www.isDone)
		{
			tempConfig.Read(www.text);
			if (!DefaultConfig.getInstance().configDic.ContainsKey(tempConfig.GetType()))
			{
				DefaultConfig.getInstance().configDic.Add(tempConfig.GetType(), tempConfig);
			}
		}

	}

}


public class DatabaseConfig
{
    //判断是否为空  
    public virtual bool Read(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        return true;
    }
}

public class DefaultConfig
{
    //单例模式  
    private static DefaultConfig instance;
    public static DefaultConfig getInstance()
    {
        if (instance == null)
        {
            instance = new DefaultConfig();
            return instance;
        }
        else
        {
            return instance;
        }
    }
    public Dictionary<System.Type, DatabaseConfig> configDic = new Dictionary<System.Type, DatabaseConfig>();
    public T GetConfigByType<T>() where T : DatabaseConfig
    {

        //查找字典是否已有T类型的数据源
        if (configDic.ContainsKey(typeof(T)))
        {
//            Debug.Log(configDic == null);
            return configDic[typeof(T)] as T;
        }
        return null;
    }
}

