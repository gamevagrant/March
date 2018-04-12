using BestHTTP;
using Facebook.Unity;
using LitJson;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
	private bool isOnApplicationPause;

    public Text m_coin;
    public Text m_star;
    public Text m_heart;
    public Text m_downTime;

	public Button m_taskBtn;

    public Text m_AllLifeText;

    public Text m_eliminate_text;

    private float totalTime = 100;
    private float intervalTime = 1;

    public GameObject m_storyListLayout;
    public GameObject m_storyListItem;

    public GameObject m_starRecoveryPanel;
    public GameObject m_buyPanel;

    public GameObject m_settingPanel;

    private List<GameObject> m_stroyList = new List<GameObject>();

    public GameObject m_testPopup;

    public GameObject m_Canvas;

    public GameObject m_GuideHand;

    public PopupOpener ShopPopup;

    class LoginInfo
    {
        public string newDeviceId = "";
        public string gameUid = "";
        public string appVersion = "";
        public string gcmRegisterId = "";
        public string referrer = "";
        public string platform = PltformManager.instance.getPlatform();
        public string lang = "";
        public string afUID = "";
        public string pf = "";
        public string pfId = "";
        public string fromCountry = "";
        public string gaid = "";
        public int gmLogin = 1;
        public string terminal = "";
        public string SecurityCode = "";
        public string packageName = "";
        public string isHDLogin = "1";
        public string pfSeeeion = "";
        public string recallId = "";

    }

    private static MainScene m_instance;
    public static MainScene Instance
    {
        get
        {
            return m_instance;
        }
    }
 
    private language_cn m_language_cn;
    public item Item { get { if (m_item == null) { m_item = DefaultConfig.getInstance().GetConfigByType<item>(); } return m_item; } }
    private item m_item;
    public language_cn Language_CN { get { if (m_language_cn == null) { m_language_cn = DefaultConfig.getInstance().GetConfigByType<language_cn>(); } return m_language_cn; } }
  

    private void Awake()
    {
        m_instance = this;
        m_storyListItem = Resources.Load<GameObject>("Prefabs/UI/StoryItem");
        //PlayerPrefs.DeleteAll();

		if (!PlayerPrefs.HasKey("music_on"))
		{
			PlayerPrefs.SetInt("music_on", 1);
			var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
			backgroundAudioSource.volume = 1;
		}

		if (!PlayerPrefs.HasKey("sound_on"))
		{
			PlayerPrefs.SetInt("sound_on", 1);
		}


        Messenger.AddListener(ELocalMsgID.RefreshBaseData, RefreshGoldData);
        
		FB.Init ();
    }

    private void Start()
    {
        TaskManager.Instance.m_panelInfo.SetActive(false);
        TaskManager.Instance.gameObject.SetActive(false);

		Debug.Log ("------------------------------: 语言: " + Application.systemLanguage.ToString ());

        m_testPopup.SetActive(false);
        m_starRecoveryPanel.SetActive(false);
		m_starRecoveryPanel.transform.Find ("title_Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200039");
		m_starRecoveryPanel.transform.Find ("next_Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200039");
        m_buyPanel.SetActive(false);
        login();
        m_star.text = PlayerData.instance.getStarNum().ToString();
		m_taskBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200038");
		if (PlayerData.instance.getPlayScene()) {
			PlayerData.instance.setPlayScene (false);
			int level = PlayerData.instance.getEliminateLevel ();
			if (level == 9 || level == 15 || level == 17 || level == 21) {
				Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UnlocktemPopup"), m_Canvas.transform);
			}
			if (level == 10) {
				saveDayInfo("{}");
			}
		}

        //qy.ui.UIManager.Instance.OpenWindow(qy.ui.UISettings.UIWindowID.UIDialogueWindow, 1000118);
    }


    void Update()
    {
        if (PlayerData.instance.getHeartNum() < 5)
        {
          //  Debug.Log(" m_downTime format data:" + TimeMonoManager.instance.getTotalTime());
            m_downTime.text = string.Format("{0:D2}: {1:D2}", (int)TimeMonoManager.instance.getTotalTime() / 60, (int)TimeMonoManager.instance.getTotalTime() % 60);
            m_starRecoveryPanel.transform.Find("downTime_Text").GetComponent<Text>().text = string.Format("{0:D2}: {1:D2}", (int)TimeMonoManager.instance.getTotalTime() / 60, (int)TimeMonoManager.instance.getTotalTime() % 60);
        }
        else
        {
			m_downTime.text =  LanguageManager.instance.GetValueByKey("200021");
            m_starRecoveryPanel.transform.Find("downTime_Text").GetComponent<Text>().gameObject.SetActive(false);
            m_starRecoveryPanel.transform.Find("next_Text").GetComponent<Text>().gameObject.SetActive(false);
            m_buyPanel.SetActive(false);
            TimeMonoManager.instance.setTotalTime(0);  //心数已满状态的时候totaltime 置为0；
        }

    }

	void OnApplicationFocus(bool hasFocus)
	{
		if (isOnApplicationPause == true && PlayerData.instance.getHeartNum() < 5) {
			login();
			isOnApplicationPause = false;
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		isOnApplicationPause = true;
	}

    private void login()
    {
        if (NetManager.instance.isNetWorkStatusGood())
        {
            HTTPRequest request = new HTTPRequest(new Uri(ServerGlobal.loginUrl), HTTPMethods.Post, LoginRev);
            request.AddField("cmd", ServerGlobal.LOGIN_CMD);
            LoginInfo loginInfo = new LoginInfo();
            string loginInfoJson = JsonUtility.ToJson(loginInfo);
            request.AddField("device", Utils.instance.getDeviceID());//Utils.instance.getDeviceID()
            request.AddField("data", loginInfoJson);
            request.Send();
        }
        else
        {
            Debug.LogError("网络连接错误");
			string localCacheData = SaveDataManager.instance.GetString (SaveDataDefine.serverdata);
			Debug.Log ("本地数据：" + localCacheData);
			if (localCacheData.Equals (""))
				MessageBox.Instance.Show ("数据错误，请联网同步数据");
			else
			{
				JsonData jsonData = JsonMapper.ToObject (SaveDataManager.instance.GetString (SaveDataDefine.serverdata));
				PlayerData.instance.jsonObj = jsonData;
				PlayerData.instance.RefreshData (jsonData);
				RefreshPlayerData();  //离线模式下，用本地数据刷新UI
			}
        }
    }

    private void LoginRev(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("Login Rev :" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        //server 数据合法性验证在RefreshData里面已经统一验证了，这里要判断是否登陆，所以在login接口里面单独判断，先这么处理！！！
        if (null == data)
        {
            Debug.LogError("josn data is null");
            return;
        }
        if (data.Keys.Contains("err"))
        {
            Debug.LogError("recv msg err");
            string errorcode = data["err"].ToString();
            string lang = PlayerData.instance.getMsgByErrorCode(errorcode);
            MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey(lang));
            return;
        }

        NetManager.instance.setIsLogin(true);
        SaveDataManager.instance.SaveString(SaveDataDefine.isLogin, "1");

        PlayerData.instance.jsonObj = data;
        //refresh data
        PlayerData.instance.RefreshData(data);
        RefreshPlayerData();

        if (!PlayerData.instance.getSaveDayInfo())
        {
            int level = PlayerData.instance.getEliminateLevel();
            if (level >= 10)
            {
                saveDayInfo("{}");
            }
        }
    }
    public void saveDayInfo(string data)
    {
       NetManager.instance.httpSend(ServerGlobal.SAVE_DAY_INFO, data, saveDayInfoRev);
    }

    private void saveDayInfoRev(HTTPRequest request, HTTPResponse response)
    {
		PlayerData.instance.setSaveDayInfo (true);
        Debug.Log("saveDayInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        //PlayerData.instance.RefreshData(data);
		PlayerData.instance.setIndexDay(int.Parse(data["sevenDay"]["sevenDayInfo"]["index"].ToString()));
		PlayerData.instance.setAwardState(int.Parse(data["sevenDay"]["sevenDayInfo"]["state"].ToString()));
		if (0 == PlayerData.instance.getAwardState ())
			ShowDailyLandingPopup ();
    }
	private void ShowDailyLandingPopup()
    {
        //登陆后弹出7日登录活动
        var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/DailyLandingActivities"), GameObject.Find("Canvas").transform) as GameObject;
		int day = PlayerData.instance.getIndexDay ();
		go.GetComponent<DailyLandingActivities> ().Init (day + 1);
        go.GetComponent<Popup>().Open();
    }

    public void addStoryListItem(StoryItem storyItem)
    {
        GameObject obj = Instantiate(m_storyListItem, m_storyListLayout.transform);
        obj.GetComponent<StoryListItem>().SetItemContent(storyItem, this);
        m_stroyList.Add(obj);
    }

   public void RefreshPlayerData()
    {
        m_coin.text = PlayerData.instance.getCoinNum().ToString();
        m_heart.text = PlayerData.instance.getHeartNum().ToString();
        m_star.text = PlayerData.instance.getStarNum().ToString();
        m_eliminate_text.text = PlayerData.instance.getEliminateLevel().ToString();  //当前消除关卡
        if (5 == PlayerData.instance.getHeartNum())
			m_downTime.text =  LanguageManager.instance.GetValueByKey("200021");
    }

   //心数恢复的时候刷新心数和状态显示
    private void RefreshGoldData()
    {
        RefreshPlayerData();
        m_heart.text = PlayerData.instance.getHeartNum().ToString();
        if (5 == PlayerData.instance.getHeartNum())
        {
			m_downTime.text =  LanguageManager.instance.GetValueByKey("200021");
            m_starRecoveryPanel.transform.Find("next_Text").GetComponent<Text>().gameObject.SetActive(false);
            m_starRecoveryPanel.transform.Find("downTime_Text").GetComponent<Text>().gameObject.SetActive(false);
            m_starRecoveryPanel.transform.Find("starNum_Text").GetComponent<Text>().gameObject.SetActive(false);
			m_starRecoveryPanel.transform.Find("all_life_text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200022");
        }
        else if (m_starRecoveryPanel.activeSelf)
            m_starRecoveryPanel.transform.Find("starNum_Text").GetComponent<Text>().text =
                PlayerData.instance.getHeartNum().ToString();
    }

    /// <summary>
    /// 生命不足时用金币补满生命
    /// </summary>
    public void onGoldBuyLife()
    {
        int m_leftCoin = PlayerData.instance.getCoinNum() - PlayerData.instance.getLivePrice();
        if (m_leftCoin >= 0)
        {
             NetManager.instance.buyHeart();
            //购买成功之后生命值置满
         /*   PlayerData.instance.setHeartNum(PlayerData.instance.getMaxLives());
            PlayerData.instance.setCoinNum(m_leftCoin);*/
            m_heart.text = PlayerData.instance.getMaxLives().ToString();
            m_coin.text = m_leftCoin.ToString();
            if(m_starRecoveryPanel.activeSelf)
                m_starRecoveryPanel.SetActive(false);
        }
        else
        {
			MessageBox.Instance.Show( LanguageManager.instance.GetValueByKey("200045"));
        }
        
    }

    //倒计时详情面板
    public void onHeartAdd()
    {
        if (m_starRecoveryPanel)
        {
            m_starRecoveryPanel.SetActive(true);
            if (5 == PlayerData.instance.getHeartNum())
            {
                m_starRecoveryPanel.transform.Find("next_Text").GetComponent<Text>().gameObject.SetActive(false);
                m_starRecoveryPanel.transform.Find("downTime_Text").GetComponent<Text>().gameObject.SetActive(false);
                m_starRecoveryPanel.transform.Find("starNum_Text").gameObject.SetActive(false);
				m_starRecoveryPanel.transform.Find("all_life_text").GetComponent<Text>().text =  LanguageManager.instance.GetValueByKey("200022");
            }
            else
            {
                m_starRecoveryPanel.transform.Find("starNum_Text").GetComponent<Text>().text = PlayerData.instance.getHeartNum().ToString();
                if (0 == PlayerData.instance.getHeartNum())
                {
                    //心数为0时，显示出来购买按钮面板
                    m_buyPanel.SetActive(true);
                    m_buyPanel.transform.Find("iconNum_Text").GetComponent<Text>().text = PlayerData.instance.getLivePrice().ToString();
                }
            }
        }
    }

    public void onIconAdd()
    {
        ShopPopup.OpenPopup();
    }

    public void onCloseHeartRecoveryPanel()
    {
        if (m_starRecoveryPanel)
            m_starRecoveryPanel.SetActive(false);
    }

    public void onEliminateTask()
    {
        int eliminateHeartNum = 1;

        //check 心数是否足够
        Debug.Log("当前拥有的心数是:"+PlayerData.instance.getHeartNum().ToString());
#if UNITY_EDITOR
        m_testPopup.SetActive(true);
        m_testPopup.transform.Find("InputField").GetComponent<InputField>().text = PlayerData.instance.getEliminateLevel().ToString();
        return;

#endif
        if (PlayerData.instance.getHeartNum() < eliminateHeartNum)
        {
			MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200025"));
            return;
        }
        if (PlayerData.instance.getEliminateLevel() > Int32.Parse(DefaultConfig.getInstance().GetConfigByType<setting>()
                .GetValueByIDAndKey("maxlevel", "max")))
        {
			//MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200049"));
			var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
			go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("200049"));
			go.GetComponent<Popup> ().Open ();
            return;
        }

        if (m_Canvas != null)
        {
            Instantiate(Resources.Load("Prefabs/PlayScene/Popup/BeginPopup"), m_Canvas.transform);
        }
    }


    public void onSettingBtn()
    {
        if (m_settingPanel == null)
        {
            m_settingPanel = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/SettingPanel"), m_Canvas.transform);
            m_settingPanel.transform.localScale = Vector3.one;
            ((RectTransform)m_settingPanel.transform).anchoredPosition = Vector2.zero;
        }
        m_settingPanel.SetActive(true);
    }

    public void onCloseBtn()
    {
        TaskManager.Instance.gameObject.SetActive(false);
    }

    #region 测试用关卡选择界面

    public void onTestPopupSureBtn()
    {
#if UNITY_EDITOR
        int num = Convert.ToInt32(m_testPopup.transform.Find("InputField").GetComponent<InputField>().textComponent.text);

        PlayerData.instance.setEliminateLevel(num);

        if (m_Canvas != null)
        {
            Instantiate(Resources.Load("Prefabs/PlayScene/Popup/BeginPopup"), m_Canvas.transform);
        }
#endif
    }

    public void onTestPopupCloseBtn()
    {
#if UNITY_EDITOR
        m_testPopup.SetActive(false);
#endif
    }

    #endregion

    public void showStory(StoryItem storyItem)
    {
    }

    public void OnTaskButtonClicked()
    {
        var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
        go.GetComponent<UIAlertPopup>().Init(LanguageManager.instance.GetValueByKey("210140"));
        go.GetComponent<Popup>().Open();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(ELocalMsgID.RefreshBaseData, RefreshGoldData);
    }
}
