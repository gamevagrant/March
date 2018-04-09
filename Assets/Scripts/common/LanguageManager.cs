using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;

public class LanguageManager : MonoSingleton<LanguageManager> 
{

	private language m_language;
	public language Language
	{ 
		get
		{
			if (m_language == null) 
			{ 
				m_language = DefaultConfig.getInstance().GetConfigByType<language>(); 
			
			}
			return m_language; 
		}
	}

	
	public void initConfig()
	{
		AddLanguageonfig<language>();

	}

	public string GetValueByKey(string key)
	{
		return DefaultConfig.getInstance ().GetConfigByType<language> ().GetValueByKey(key);
	}



	private void AddLanguageonfig<T>() where T : DatabaseConfig, new()
	{
		T config = new T();
		StartCoroutine(XMLDataManager.instance.loadLangXML(config));
	}
}
