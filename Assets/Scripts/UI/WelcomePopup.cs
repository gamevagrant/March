using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomePopup : MonoBehaviour {

    public Text m_titleText;
    public Text m_des;

    private bool hasSeen;

    void Start()
    {
        m_titleText.text = LanguageManager.instance.GetValueByKey("210143");
        m_des.text = LanguageManager.instance.GetValueByKey("210142");
        hasSeen = false;
        StartCoroutine(WaitForSeen());
    }

    void Update()
    {
        if (hasSeen && Input.GetMouseButtonDown(0))
        {
            GetComponent<Popup>().Close();
        }
    }

    IEnumerator WaitForSeen()
    {
        yield return new WaitForSeconds(1f);
        hasSeen = true;
    }
}
