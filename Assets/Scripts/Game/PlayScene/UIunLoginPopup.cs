using UnityEngine;
using UnityEngine.UI;

public class UIunLoginPopup : MonoBehaviour
{
    public Text m_titleText;
    public Text m_des;
    public Button m_retryBtn;
    public Text m_retryBtnText;

    void Start()
    {
        m_titleText.text = LanguageManager.instance.GetValueByKey("210133");
        m_retryBtn.onClick.AddListener(() => OnCloseClick());
        m_retryBtnText.text = LanguageManager.instance.GetValueByKey("210156");
    }

    public void Init(string message)
    {
        m_des.text = message;
    }

    public void OnCloseClick()
    {
        MainScene.Instance.m_unLoginPop = null;
        MainScene.Instance.login();
        GetComponent<Popup>().Close();
    }
}
