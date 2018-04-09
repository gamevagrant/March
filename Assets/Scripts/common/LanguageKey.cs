using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageKey : MonoBehaviour {



	// Use this for initialization
	void Start () {
	    string key = "";
	    Text m_Text = GetComponent<Text>();
	    if (m_Text != null)
	    {
	        key = m_Text.text;
	    }
	    if (key != "")
	    {
	        m_Text.text = LanguageManager.instance.Language.GetValueByKey(key);
	    }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
