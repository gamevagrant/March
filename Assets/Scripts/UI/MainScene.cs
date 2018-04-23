using BestHTTP;
using Facebook.Unity;
using LitJson;
using System;
using System.Collections.Generic;
using Core.March.Config;
using DG.Tweening;
using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    private bool isOnApplicationPause;

    public Text MCoin;
    public Text MStar;
    public Text MHeart;
    public Text MDownTime;

    public Button MTaskBtn;

    public Text MEliminateText;

    public GameObject MStoryListLayout;

    private GameObject MStoryListItem;

    private List<GameObject> mStroyList = new List<GameObject>();

    public GameObject MTestPopup;

    public GameObject MCanvas;

    private GameObject MGuideHand;

    class LoginInfo
    {
        public string NewDeviceId = "";
        public string GameUid = PlayerData.instance.userId;
        public string AppVersion = Application.version;
        public string GcmRegisterId = "";
        public string Referrer = "";
        public string Lang = PlayerData.instance.getLang();
        public string AfUid = "";
        public string Pf = Application.platform.ToString();
        public string PfId = "";
        public string FromCountry = "";
        public string Gaid = "";
        public int GmLogin = 1;
        public string Terminal = "";
        public string SecurityCode = "";
        public string PackageName = "";
        public string IsHdLogin = "1";
        public string PfSeeeion = "";
        public string RecallId = "";
    }

    private static MainScene mInstance;
    public static MainScene Instance
    {
        get
        {
            return mInstance;
        }
    }

    private language_cn mLanguageCn;

    public item Item
    {
        get
        {
            if (mItem == null)
            {
                mItem = DefaultConfig.getInstance().GetConfigByType<item>();
            }
            return mItem;
        }
    }
    private item mItem;

    public language_cn LanguageCn
    {
        get
        {
            if (mLanguageCn == null)
            {
                mLanguageCn = DefaultConfig.getInstance().GetConfigByType<language_cn>();
            }
            return mLanguageCn;
        }
    }


    private void Awake()
    {
        mInstance = this;

        MStoryListItem = Resources.Load<GameObject>("Prefabs/UI/StoryItem");

        Messenger.AddListener(ELocalMsgID.RefreshBaseData, RefreshGoldData);

        FB.Init();
    }

    private void Start()
    {
        Debug.Log("------------------------------: 语言: " + Application.systemLanguage.ToString());

        MTestPopup.SetActive(false);

        Login();
        MStar.text = PlayerData.instance.getStarNum().ToString();

        if (PlayerData.instance.getPlayScene())
        {
            PlayerData.instance.setPlayScene(false);
            int level = PlayerData.instance.getEliminateLevel();
            if (level == 9 || level == 15 || level == 17 || level == 21)
            {
                if (level == 9)
                {
                    PlayerData.instance.setNeedShow9Help(true);
                }
                WindowManager.instance.Show<UnlocktemPopupWindow>();
            }
            if (level == 10)
            {
                SaveDayInfo("{}");
            }
        }

        //NewPlayerGuideRefresh();
    }

    void Update()
    {
        if (PlayerData.instance.getHeartNum() < 5)
        {
            MDownTime.text = string.Format("{0:D2}: {1:D2}", (int)TimeMonoManager.instance.getTotalTime() / 60, (int)TimeMonoManager.instance.getTotalTime() % 60);
        }
        else
        {
            MDownTime.text = LanguageManager.instance.GetValueByKey("200021");
            TimeMonoManager.instance.setTotalTime(0);  //心数已满状态的时候totaltime 置为0；
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (isOnApplicationPause == true && PlayerData.instance.getHeartNum() < 5)
        {
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
        string localCacheData = SaveDataManager.instance.GetString(SaveDataDefine.serverdata);
        if (localCacheData.Equals(""))
        {
            if (NetManager.instance.isNetWorkStatusGood())
            {
                //wait for server data
                WaitingPopupManager.instance.Show(MCanvas);
            }
            else
            {
                var alertWindow = WindowManager.instance.Show<UIUnLoginPopupWindow>().GetComponent<UIunLoginPopup>();
                alertWindow.Init(LanguageManager.instance.GetValueByKey("210157"));
                return;
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
            HTTPRequest request = new HTTPRequest(new Uri(Configure.instance.ServerUrl), HTTPMethods.Post, LoginRev);
            request.AddField("cmd", ServerGlobal.LOGIN_CMD);
            LoginInfo loginInfo = new LoginInfo();
            string loginInfoJson = JsonUtility.ToJson(loginInfo);
            Debug.Log("login data : " + loginInfoJson);
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
        WaitingPopupManager.instance.Close();

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
                SaveDayInfo("{}");
            }
        }
    }
    public void SaveDayInfo(string data)
    {
        PlayerData.instance.setSaveDayInfo(true);
        NetManager.instance.httpSend(ServerGlobal.SAVE_DAY_INFO, data, SaveDayInfoRev);
    }

    private void SaveDayInfoRev(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("saveDayInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        //PlayerData.instance.RefreshData(data);
        PlayerData.instance.setIndexDay(int.Parse(data["sevenDay"]["sevenDayInfo"]["index"].ToString()));
        PlayerData.instance.setAwardState(int.Parse(data["sevenDay"]["sevenDayInfo"]["state"].ToString()));
        if (0 == PlayerData.instance.getAwardState())
            ShowDailyLandingPopup();
    }
    private void ShowDailyLandingPopup()
    {
        //登陆后弹出7日登录活动
        int day = PlayerData.instance.getIndexDay();
        WindowManager.instance.Show<DailyLandingActivitiesPopupWindow>().GetComponent<DailyLandingActivities>().Init(day + 1);
    }

    public void AddStoryListItem(StoryItem storyItem)
    {
        GameObject obj = Instantiate(MStoryListItem, MStoryListLayout.transform);
        obj.GetComponent<StoryListItem>().SetItemContent(storyItem, this);
        mStroyList.Add(obj);
    }

    public void RefreshPlayerData()
    {
        MCoin.text = PlayerData.instance.getCoinNum().ToString();
        MHeart.text = PlayerData.instance.getHeartNum().ToString();
        MStar.text = PlayerData.instance.getStarNum().ToString();
        MEliminateText.text = PlayerData.instance.getEliminateLevel().ToString();  //当前消除关卡
    }

    //心数恢复的时候刷新心数和状态显示
    private void RefreshGoldData()
    {
        RefreshPlayerData();
        NewPlayerGuideRefresh();
        MHeart.text = PlayerData.instance.getHeartNum().ToString();
        if (5 == PlayerData.instance.getHeartNum())
        {
            MDownTime.text = LanguageManager.instance.GetValueByKey("200021");
        }
    }

    /// <summary>
    /// 生命不足时用金币补满生命
    /// </summary>
    public void OnGoldBuyLife()
    {
        int mLeftCoin = PlayerData.instance.getCoinNum() - PlayerData.instance.getLivePrice();
        if (mLeftCoin >= 0)
        {
            NetManager.instance.buyHeart();
            //购买成功之后生命值置满
            /*   PlayerData.instance.setHeartNum(PlayerData.instance.getMaxLives());
               PlayerData.instance.setCoinNum(m_leftCoin);*/
            MHeart.text = PlayerData.instance.getMaxLives().ToString();
            MCoin.text = mLeftCoin.ToString();
        }
        else
        {
            WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200044"));
        }
    }

    //倒计时详情面板
    public void OnHeartAdd()
    {
        var heartPanelController = WindowManager.instance.Show<HeartRecoveryPanelPopupWindow>().GetComponent<HeartRecoverPanelController>();
        heartPanelController.RegisterCallback(OnGoldBuyLife);
    }

    public void OnIconAdd()
    {
        WindowManager.instance.Show<ShopPopupPlayWindow>().GetComponent<HeartRecoverPanelController>();
    }

    public void OnEliminateTask()
    {
        int eliminateHeartNum = 1;
        NetManager.instance.MakePointInEliminateClick();

        //check 心数是否足够
        Debug.Log("当前拥有的心数是:" + PlayerData.instance.getHeartNum());
        if (MGuideHand != null)
        {
            MGuideHand.SetActive(false);
        }

        if (Application.isEditor)
        {
            MTestPopup.SetActive(true);
            MTestPopup.transform.Find("InputField").GetComponent<InputField>().text =
                PlayerData.instance.getEliminateLevel().ToString();
        }
        else
        {
            if (PlayerData.instance.getHeartNum() < eliminateHeartNum)
            {
                WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200025"));
            }
            else if (PlayerData.instance.getEliminateLevel() > Int32.Parse(DefaultConfig.getInstance().GetConfigByType<setting>()
                    .GetValueByIDAndKey("maxlevel", "max")))
            {
                WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200049"));
            }
            else if (MCanvas != null)
            {
                WindowManager.instance.Show<BeginPopupWindow>();
            }
        }
    }


    public void OnSettingBtn()
    {
        WindowManager.instance.Show<SettingPanelPopupWindow>();
    }

    public void OnCloseBtn()
    {
        TaskManager.Instance.gameObject.SetActive(false);
    }

    #region 测试用关卡选择界面

    public void OnTestPopupSureBtn()
    {
#if UNITY_EDITOR
        int num = Convert.ToInt32(MTestPopup.transform.Find("InputField").GetComponent<InputField>().textComponent.text);
        PlayerData.instance.setEliminateLevel(num);

        WindowManager.instance.Show<BeginPopupWindow>();
#endif
    }

    public void OnTestPopupCloseBtn()
    {
#if UNITY_EDITOR
        MTestPopup.SetActive(false);
#endif
    }

    #endregion

    public void ShowStory(StoryItem storyItem)
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
        if (PlayerData.instance.getEliminateLevel() == 1)
        {
            if (!PlayerPrefs.HasKey("Welcome"))
            {
                var welcome = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/WelcomePopup"), MCanvas.transform) as GameObject;
                welcome.GetComponent<Popup>().Open();
                PlayerPrefs.SetInt("Welcome", 1);
            }
            if (MGuideHand == null)
            {
                GameObject guidehand = new GameObject();
                guidehand.name = "GuideHand";
                guidehand.transform.parent = MCanvas.transform;
                guidehand.transform.position = MEliminateText.transform.position + new Vector3(-200, 200, 0);
                Sprite spr = Resources.Load<Sprite>("Sprites/hand");
                guidehand.AddComponent<Image>().sprite = spr;
                guidehand.transform.localRotation = new Quaternion(0, 180, 0, 0);
                guidehand.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
                guidehand.transform.DOMove(guidehand.transform.position + new Vector3(30, -30, 0), 0.5f)
                    .SetLoops(-1, LoopType.Yoyo);
                MGuideHand = guidehand;
            }
            else
            {
                MGuideHand.SetActive(true);
            }
        }
    }
}
