using UnityEngine;
using UnityEngine.UI;

public class WelcomePopup : MonoBehaviour
{
    public Text m_titleText;
    public Text m_des;

    void Start()
    {
        m_titleText.text = LanguageManager.instance.GetValueByKey("210143");
        m_des.text = LanguageManager.instance.GetValueByKey("210142");
    }
}
