using BestHTTP;
using Facebook.Unity;
using LitJson;
using System;
using System.Collections.Generic;
using DG.Tweening;
using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.UI;
using qy.net;
using qy.config;
using qy;

public class MainScene : MonoBehaviour
{
    private bool isOnApplicationPause;

    public Text m_coin;
    public Text m_star;
    public Text m_heart;
    public Text m_downTime;

    public Button m_taskBtn;

    public Text m_eliminate_text;

    public GameObject m_storyListLayout;
    public GameObject m_storyListItem;

    private List<GameObject> m_stroyList = new List<GameObject>();

    public GameObject m_testPopup;

    public GameObject m_Canvas;

    public GameObject m_GuideHand;

    public GameObject m_unLoginPop = null;

    class LoginInfo
    {
        public string newDeviceId = "";
        public string gameUid = GameMainManager.Instance.playerData.userId;
        public string appVersion = Application.version;
        public string gcmRegisterId = "";
        public string referrer = "";
        public string lang = qy.GameMainManager.Instance.playerData.lang;
        public string afUID = "";
        public string pf = Application.platform.ToString();
        public string pfId = "";
        public string fromCountry ="";
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
    /*
    public item Item
    {
        get
        {
            if (m_item == null)
            {
                m_item = DefaultConfig.getInstance().GetConfigByType<item>();
            }
            return m_item;
        }
    }
    private item m_item;
    */
    public language_cn Language_CN
    {
        get
        {
            if (m_language_cn == null)
            {
                m_language_cn = DefaultConfig.getInstance().GetConfigByType<language_cn>();
            }
            return m_language_cn;
        }
    }


    private void Awake()
    {
        m_instance = this;
        m_storyListItem = Resources.Load<GameObject>("Prefabs/UI/StoryItem");

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

        FB.Init();
    }
    private void Start()
    {
        m_testPopup.SetActive(false);

        Login();

        m_star.text = qy.GameMainManager.Instance.playerData.starNum.ToString();

        if (GameMainManager.Instance.playerData.isPlayScene)
        {
            GameMainManager.Instance.playerData.isPlayScene = false;
            int level = GameMainManager.Instance.playerData.eliminateLevel;
            if (level == 9 || level == 15 || level == 17 || level == 21)
            {
                if (level == 9)
                {
                    GameMainManager.Instance.playerData.needShow9Help = false;
                }
                WindowManager.instance.Show<UnlocktemPopupWindow>();
            }
            if (level == 10)
            {
                LoadSevenDayInfo();
            }
        }
    }
    /*
    private void Start()
    {

        Debug.Log("------------------------------: 语言: " + Application.systemLanguage.ToString());

        m_testPopup.SetActive(false);

        login();

        m_star.text = PlayerData.instance.getStarNum().ToString();

        if (PlayerData.instance.getPlayScene())
        {
            PlayerData.instance.setPlayScene(false);
            int level = PlayerData.instance.getEliminateLevel();
            if (level == 9 || level == 15 || level == 17 || level == 21)
            {
				if (level == 9) {
					PlayerData.instance.setNeedShow9Help (true);
				}
                WindowManager.instance.Show<UnlocktemPopupWindow>();
            }
            if (level == 10)
            {
                saveDayInfo("{}");
            }
        }

        //NewPlayerGuideRefresh();
    }*/

    void Update()
    {
        if (qy.GameMainManager.Instance.playerData.heartNum < 5)
        {
            m_downTime.text = string.Format("{0:D2}: {1:D2}", (int)TimeMonoManager.instance.getTotalTime() / 60, (int)TimeMonoManager.instance.getTotalTime() % 60);
        }
        else
        {
            m_downTime.text = LanguageManager.instance.GetValueByKey("200021");
            TimeMonoManager.instance.setTotalTime(0);  //心数已满状态的时候totaltime 置为0；
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (isOnApplicationPause == true && qy.GameMainManager.Instance.playerData.heartNum < 5)
        {
            //login();
            Login();
            isOnApplicationPause = false;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isOnApplicationPause = true;
    }

    public void Login()
    {
        bool isNetGood = GameMainManager.Instance.netManager.isNetWorkStatusGood;
        bool isInitPlayer = !string.IsNullOrEmpty(GameMainManager.Instance.playerData.userId);
        if (isNetGood)
        {
            if (!isInitPlayer)
            {
                WaitingPopupManager.instance.show(m_Canvas);
            }
            GameMainManager.Instance.netManager.Login(new qy.LoginInfo(), (ret, res) => 
            {
                LoadSevenDayInfo();
                WaitingPopupManager.instance.close();
            });
        }
        else if (!isInitPlayer)
        {
            if (m_unLoginPop == null)
            {
                var alertWindow = WindowManager.instance.Show<UIUnLoginPopupWindow>().GetComponent<UIunLoginPopup>();
                alertWindow.Init(LanguageManager.instance.GetValueByKey("210157"));
            }
        }
    }

    private void LoadSevenDayInfo()
    {
        //获取7天登录数据
        GameMainManager.Instance.netManager.SevenDayInfo((ret2, res2) =>
        {
            if (GameMainManager.Instance.playerData.awardState == 0)
            {
                ShowDailyLandingPopup();
            }
            ShowDailyLandingPopup();
        });
    }
    /*
    public void login()
    {
        
        string localCacheData = SaveDataManager.instance.GetString(SaveDataDefine.serverdata);
        if (localCacheData.Equals(""))
        {
            if (NetManager.instance.isNetWorkStatusGood())
            {
                //wait for server data
                WaitingPopupManager.instance.show(m_Canvas);
            }
            else
            {
                if (m_unLoginPop == null)
                {
					var alertWindow = WindowManager.instance.Show<UIUnLoginPopupWindow>().GetComponent<UIunLoginPopup>();
                    alertWindow.Init(LanguageManager.instance.GetValueByKey("210157"));
                }
                return;
                //WaitingPopupManager.instance.show(m_Canvas);
                //return;
                //Tips: net error! Check the net or Quit game!
            }
        }
        else
        {
            JsonData jsonData = JsonMapper.ToObject(SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
            PlayerData.instance.jsonObj = jsonData;
            PlayerData.instance.RefreshData(jsonData);
            RefreshPlayerData();  //离线模式下，用本地数据刷新UI
        }
        
       
        
        
        if (NetManager.instance.isNetWorkStatusGood())
        {
            HTTPRequest request = new HTTPRequest(new Uri(ServerGlobal.loginUrl), HTTPMethods.Post, LoginRev);
            request.AddField("cmd", ServerGlobal.LOGIN_CMD);
            LoginInfo loginInfo = new LoginInfo();
            string loginInfoJson = JsonUtility.ToJson(loginInfo);
            Debug.Log("login data : "+ loginInfoJson);
            request.AddField("device", Utils.instance.getDeviceID());//Utils.instance.getDeviceID()
            request.AddField("data", loginInfoJson);
            request.Send();
        }
//        else
//        {
//            Debug.LogError("网络连接错误");
//			string localCacheData = SaveDataManager.instance.GetString (SaveDataDefine.serverdata);
//			Debug.Log ("本地数据：" + localCacheData);
//			if (localCacheData.Equals (""))
//				MessageBox.Instance.Show ("数据错误，请联网同步数据");
//			else
//			{
//				JsonData jsonData = JsonMapper.ToObject (SaveDataManager.instance.GetString (SaveDataDefine.serverdata));
//				PlayerData.instance.jsonObj = jsonData;
//				PlayerData.instance.RefreshData (jsonData);
//				RefreshPlayerData();  //离线模式下，用本地数据刷新UI
//			}
//        }
    }
    */
    /*
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
        WaitingPopupManager.instance.close();

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
		PlayerData.instance.setSaveDayInfo (true);
		NetManager.instance.httpSend(ServerGlobal.SAVE_DAY_INFO, data, saveDayInfoRev);
    } 

    private void saveDayInfoRev(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("saveDayInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        //PlayerData.instance.RefreshData(data);
        PlayerData.instance.setIndexDay(int.Parse(data["sevenDay"]["sevenDayInfo"]["index"].ToString()));
        PlayerData.instance.setAwardState(int.Parse(data["sevenDay"]["sevenDayInfo"]["state"].ToString()));
        if (0 == PlayerData.instance.getAwardState())
            ShowDailyLandingPopup();
    }
    */
    private void ShowDailyLandingPopup()
    {
        //登陆后弹出7日登录活动
        int day = qy.GameMainManager.Instance.playerData.indexDay;
		WindowManager.instance.Show<DailyLandingActivitiesPopupWindow> ().GetComponent<DailyLandingActivities> ().Init (day + 1);
    }
    /*
    public void addStoryListItem(StoryItem storyItem)
    {
        GameObject obj = Instantiate(m_storyListItem, m_storyListLayout.transform);
        obj.GetComponent<StoryListItem>().SetItemContent(storyItem, this);
        m_stroyList.Add(obj);
    }
    */
    public void RefreshPlayerData()
    {
        m_coin.text = qy.GameMainManager.Instance.playerData.coinNum.ToString();
        m_heart.text = qy.GameMainManager.Instance.playerData.heartNum.ToString();
        m_star.text = qy.GameMainManager.Instance.playerData.starNum.ToString();
        m_eliminate_text.text = qy.GameMainManager.Instance.playerData.eliminateLevel.ToString();  //当前消除关卡
    }

    //心数恢复的时候刷新心数和状态显示
    private void RefreshGoldData()
    {
        RefreshPlayerData();
        NewPlayerGuideRefresh();
        m_heart.text = GameMainManager.Instance.playerData.heartNum.ToString();
        if (5 == GameMainManager.Instance.playerData.heartNum)
        {
            m_downTime.text = LanguageManager.instance.GetValueByKey("200021");
        }
    }

    /// <summary>
    /// 生命不足时用金币补满生命
    /// </summary>
    public void onGoldBuyLife()
    {
        int m_leftCoin = GameMainManager.Instance.playerData.coinNum - GameMainManager.Instance.playerData.livePrice;
        if (m_leftCoin >= 0)
        {
            //NetManager.instance.buyHeart();
            GameMainManager.Instance.playerModel.BuyHeart();
            //购买成功之后生命值置满
            /*   PlayerData.instance.setHeartNum(PlayerData.instance.getMaxLives());
               PlayerData.instance.setCoinNum(m_leftCoin);*/
            m_heart.text = GameMainManager.Instance.playerData.maxLives.ToString();
            m_coin.text = m_leftCoin.ToString();
        }
        else
        {
			WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200044"));
        }
    }

    //倒计时详情面板
    public void onHeartAdd()
    {
        var heartPanelController = WindowManager.instance.Show<HeartRecoveryPanelPopupWindow>().GetComponent<HeartRecoverPanelController>();
        heartPanelController.RegisterCallback(onGoldBuyLife);
    }

    public void onIconAdd()
    {
        WindowManager.instance.Show<ShopPopupPlayWindow>().GetComponent<HeartRecoverPanelController>();
    }

    public void onEliminateTask()
    {
        int eliminateHeartNum = 1;
        //NetManager.instance.MakePointInEliminateClick();
        GameMainManager.Instance.netManager.MakePointInEliminateClick((ret,res)=> { });
        //check 心数是否足够
        Debug.Log("当前拥有的心数是:" + qy.GameMainManager.Instance.playerData.heartNum);
        if (m_GuideHand != null)
        {
            m_GuideHand.SetActive(false);
        }

        if (Application.isEditor)
        {
            m_testPopup.SetActive(true);
            m_testPopup.transform.Find("InputField").GetComponent<InputField>().text =
                GameMainManager.Instance.playerData.eliminateLevel.ToString();
        }
        else
        {
            if (GameMainManager.Instance.playerData.heartNum < eliminateHeartNum)
            {
				WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200025"));
            }
            
            else if (GameMainManager.Instance.playerData.eliminateLevel > GameMainManager.Instance.configManager.settingConfig.max)
            {
				WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200049"));
            }
            else if (m_Canvas != null)
            {
                WindowManager.instance.Show<BeginPopupWindow>();
            }
        }
    }


    public void onSettingBtn()
    {
        WindowManager.instance.Show<SettingPanelPopupWindow>();
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
        qy.GameMainManager.Instance.playerData.eliminateLevel = num;

        WindowManager.instance.Show<BeginPopupWindow>();
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
		WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210140"));
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(ELocalMsgID.RefreshBaseData, RefreshGoldData);
    }

    private void NewPlayerGuideRefresh()
    {
        if (qy.GameMainManager.Instance.playerData.eliminateLevel == 1)
        {
            if (!PlayerPrefs.HasKey("Welcome"))
            {
                var welcome = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/WelcomePopup"), m_Canvas.transform) as GameObject;
                welcome.GetComponent<Popup>().Open();
                PlayerPrefs.SetInt("Welcome", 1);
            }
            if (m_GuideHand == null)
            {
                GameObject guidehand = new GameObject();
                guidehand.name = "GuideHand";
                guidehand.transform.parent = m_Canvas.transform;
                guidehand.transform.position = m_eliminate_text.transform.position + new Vector3(-200, 200, 0);
                Sprite spr = Resources.Load<Sprite>("Sprites/hand");
                guidehand.AddComponent<Image>().sprite = spr;
                guidehand.transform.localRotation = new Quaternion(0, 180, 0, 0);
                guidehand.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
                guidehand.transform.DOMove(guidehand.transform.position + new Vector3(30, -30, 0), 0.5f)
                    .SetLoops(-1, LoopType.Yoyo);
                m_GuideHand = guidehand;
            }
            else
            {
                m_GuideHand.SetActive(true);
            }
        }
    }
}
