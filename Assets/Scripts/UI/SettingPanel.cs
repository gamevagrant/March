using BestHTTP;
using Facebook.Unity;
using LitJson;
using March.Core.WindowManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy;

public class SettingPanel : MonoBehaviour
{
    private GameObject facebookButton;
    private GameObject helpButton;
    private GameObject evaluateButton;
    private GameObject changeNameButton;
    private GameObject notifyButton;
    private GameObject languageButton;

    private Text facebookText;

    private GameObject soundOnButton;
    private GameObject soundOffButton;
    private GameObject musicOnButton;
    private GameObject musicOffButton;

    private Text tileText;

    void Start()
    {
        facebookButton = transform.Find("ButtonPanel/Facebook").gameObject;
        helpButton = transform.Find("ButtonPanel/Help").gameObject;
        evaluateButton = transform.Find("ButtonPanel/Evaluate").gameObject;
        changeNameButton = transform.Find("ButtonPanel/ChangeName").gameObject;
        notifyButton = transform.Find("ButtonPanel/Notify").gameObject;
        languageButton = transform.Find("ButtonPanel/Language").gameObject;

        facebookText = transform.Find("BottomLeftPanel/FacebookText").GetComponent<Text>();

        soundOnButton = transform.Find("BottomRightPanel/Sound/On").gameObject;
        soundOffButton = transform.Find("BottomRightPanel/Sound/Off").gameObject;
        musicOnButton = transform.Find("BottomRightPanel/Music/On").gameObject;
        musicOffButton = transform.Find("BottomRightPanel/Music/Off").gameObject;

        tileText = transform.Find("Title/bg/Text").GetComponent<Text>();

        InitText();

        if (FB.IsLoggedIn)
        {
            facebookButton.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
            facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("210150");
        }

        OnFlag(soundOnButton, soundOffButton, Configure.instance.SoundOn);
        OnFlag(musicOnButton, musicOffButton, Configure.instance.MusicOn);
    }

    private void OnFlag(GameObject go1, GameObject go2, bool flag)
    {
        go1.gameObject.SetActive(flag);
        go2.gameObject.SetActive(!flag);
    }

    private void InitText()
    {
        facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200031");
        helpButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200033");
        evaluateButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200035");
        changeNameButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200032");
        notifyButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200034");
        languageButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200036");
        facebookText.text = LanguageManager.instance.GetValueByKey("200037");
        tileText.text = LanguageManager.instance.GetValueByKey("200037");
    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            Debug.Log("-----------------: " + "Null Response");
            return;
        }

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
                //NetManager.instance.userBind(id, "fbName", userBindInfoRev);
                GameMainManager.Instance.netManager.UserBind(id, "fbName", (ret,res)=> {
                    WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210146"));

                    facebookButton.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
                    facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("210150");
                });
            }
            else
            {
                WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210147"));
            }
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
                FB.LogInWithReadPermissions(new List<string> { "public_profile", "email", "user_friends" }, HandleResult);
            }
            else if (FB.IsLoggedIn)
            {
                FB.LogOut();
                //NetManager.instance.userUnBind(userUnBindInfoRev);
                GameMainManager.Instance.netManager.UserUnBind((ret,res)=> {
                    WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210148"));

                    facebookButton.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy4btn", typeof(Sprite)) as Sprite;
                    facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200031");
                });
            }
        }
    }
    /*
    private void userBindInfoRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            NetManager.instance.setIsConnected(false);
            return;
        }
        Debug.Log("userBindInfoRev response:" + response.DataAsText);
        PlayerDataMessage data = JsonMapper.ToObject<PlayerDataMessage>(response.DataAsText);
        GameMainManager.Instance.playerData.RefreshData(data);

        WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210146"));

        facebookButton.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
        facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("210150");
    }

    private void userUnBindInfoRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            NetManager.instance.setIsConnected(false);
            return;
        }
        Debug.Log("userUnBindInfoRev response:" + response.DataAsText);

        WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210148"));

        facebookButton.GetComponent<Image>().sprite = Resources.Load("Sprites/Cookie/UI/General/buy4btn", typeof(Sprite)) as Sprite;
        facebookButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200031");
    }
    */
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
        Configure.instance.SoundOn = !Configure.instance.SoundOn;

        PlayerPrefs.SetInt(PlayerPrefEnums.SoundOn, Configure.instance.SoundOn ? 1 : 0);

        OnFlag(soundOnButton, soundOffButton, Configure.instance.SoundOn);
    }

    public void onMusicBtn()
    {
        Configure.instance.MusicOn = !Configure.instance.MusicOn;

        PlayerPrefs.SetInt(PlayerPrefEnums.MusicOn, Configure.instance.MusicOn ? 1 : 0);

        OnFlag(musicOnButton, musicOffButton, Configure.instance.MusicOn);
    }
}
