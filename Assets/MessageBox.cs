using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour {

    public static MessageBox Instance { get; private set; }
    private Text m_text;

    // Use this for initialization
	void Awake ()
	{
	    Instance = this;
	    m_text = GetComponent<Text>();
	}

    public void Show(string content)
    {
        m_text.text = content;
        m_text.GetComponent<MaskableGraphic>().CrossFadeAlpha(1,.3f,false);
        transform.SetAsLastSibling();
        Invoke("Hide",1);
    }

    private void Hide()
    {
        m_text.GetComponent<MaskableGraphic>().CrossFadeAlpha(0, 1f, false);
    
    }

}
