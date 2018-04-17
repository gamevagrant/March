using BestHTTP;
using Facebook.Unity;
using LitJson;
using System.Collections.Generic;
using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Button m_linkBtn;
    public Button m_helpBtn;
    public Button m_evaluateBtn;
    public Button m_changeBtn;
    public Button m_notifyBtn;
    public Button m_languageBtn;

    public Text m_faceBookDes;

    public Image m_effect_01_img;
    public Image m_effect_02_img;
    public Image m_music_01_img;
    public Image m_music_02_img;

    public Text m_titleText;
    private bool is_effect_on;
    private bool is_music_on;

    private GameObject m_modifyNamePanel;

    // Use this for initialization
    void Start()
    {
        initText();
        is_effect_on = PlayerPrefs.GetInt("sound_on") == 1;
        is_music_on = PlayerPrefs.GetInt("music_on") == 1;

        if (FB.IsLoggedIn)
        {
            m_linkBtn.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
            m_linkBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("210150");
        }

        if (is_effect_on)
        {
            m_effect_02_img.gameObject.SetActive(false);
        }
        else
        {
            m_effect_01_img.gameObject.SetActive(false);
        }
        if (is_music_on)
        {
            m_music_02_img.gameObject.SetActive(false);
        }
        else
        {
            m_music_01_img.gameObject.SetActive(false);
        }
    }

    private void initText()
    {
        m_linkBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200031");
        m_helpBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200033");
        m_evaluateBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200035");
        m_changeBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200032");
        m_notifyBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200034");
        m_languageBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200036");
        m_faceBookDes.text = LanguageManager.instance.GetValueByKey("200037");
        m_titleText.text = LanguageManager.instance.GetValueByKey("200037");
    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            Debug.Log("-----------------: " + "Null Response");
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("-----------------: " + "Error Response: " + result.Error);
        }
        else if (result.Cancelled)
        {
            Debug.Log("-----------------: " + "Cancelled Response: " + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            foreach (KeyValuePair<string, object> pair in result.ResultDictionary)
            {
                Debug.Log("-----------------Success Response: Key:" + pair.Key + "Value:" + pair.Value);
            }

            string id = result.ResultDictionary["user_id"].ToString();
            if (!string.IsNullOrEmpty(id))
            {
                NetManager.instance.userBind(id, "fbName", userBindInfoRev);
            }
            else
            {
				WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210147"));
            }
            //Debug.Log ("-----------------: " + "Success Response: " + result.ResultDictionary);

        }
        else
        {
            Debug.Log("-----------------: " + "Empty Response");
        }
    }

    public void onLinkBtn()
    {
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn)
            {
                FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
            }
            else if (FB.IsLoggedIn)
            {
                FB.LogOut();
                NetManager.instance.userUnBind(userUnBindInfoRev);
            }
        }
    }

    private void userBindInfoRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            NetManager.instance.setIsConnected(false);
            return;
        }
        Debug.Log("userBindInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);

		WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210146"));

        m_linkBtn.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
        m_linkBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("210150");
    }

    private void userUnBindInfoRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            NetManager.instance.setIsConnected(false);
            return;
        }
        Debug.Log("userUnBindInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        //PlayerData.instance.RefreshData(data);

		WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210148"));

        m_linkBtn.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy4btn", typeof(Sprite)) as Sprite;
        m_linkBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200031");
    }

    public void onHelpBtn()
    {
    }

    public void onEvaluateBtn()
    {


    }

    public void onChangeBtn()
    {
        WindowManager.instance.Show<ModifyNamePanelPopupWindow>();
    }

    public void onNotifyBtn()
    {

    }

    public void onLanguageBtn()
    {

    }

    public void onEffectBtn()
    {
        if (PlayerPrefs.GetInt("sound_on") == 1)
        {
            m_effect_01_img.gameObject.SetActive(false);
            m_effect_02_img.gameObject.SetActive(true);
            PlayerPrefs.SetInt("sound_on", 0);
            AudioListener.volume = 0;

        }
        else
        {
            m_effect_01_img.gameObject.SetActive(true);
            m_effect_02_img.gameObject.SetActive(false);
            PlayerPrefs.SetInt("sound_on", 1);
            AudioListener.volume = 1;
        }

    }

    public void onMusicBtn()
    {
        if (PlayerPrefs.GetInt("music_on") == 1)
        {
            m_music_01_img.gameObject.SetActive(false);
            m_music_02_img.gameObject.SetActive(true);
            var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
            backgroundAudioSource.volume = 0;
            PlayerPrefs.SetInt("music_on", 0);
        }
        else
        {
            m_music_01_img.gameObject.SetActive(true);
            m_music_02_img.gameObject.SetActive(false);
            var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
            backgroundAudioSource.volume = 1;
            PlayerPrefs.SetInt("music_on", 1);
        }
    }
}
