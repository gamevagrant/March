using Facebook.Unity;
using March.Core.WindowManager;
using qy;
using qy.ui;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private bool isOnApplicationPause;

    private GameObject canvas;

    class LoginInfo
    {
        public string NewDeviceId = "";
        public string GameUid = GameMainManager.Instance.playerData.userId;
        public string AppVersion = Application.version;
        public string GcmRegisterId = "";
        public string Referrer = "";
        public string Lang = GameMainManager.Instance.playerData.lang;
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
    private void Awake()
    {
        mInstance = this;
        Resources.Load<GameObject>("Prefabs/UI/StoryItem");

        //Messenger.AddListener(ELocalMsgID.RefreshBaseData, RefreshGoldData);

        FB.Init();

        GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIMainSceneWindow);
    }
    private void Start()
    {
        canvas = ((MonoBehaviour) UIManager.Instance).gameObject;

        Login();

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

#if GAME_DEBUG
        Instantiate(Resources.Load<GameObject>(Configure.DebugCanvasPath));
#endif
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (isOnApplicationPause == true && GameMainManager.Instance.playerData.heartNum < 5)
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
        bool isNetGood = GameMainManager.Instance.netManager.isNetWorkStatusGood;
        bool isInitPlayer = !string.IsNullOrEmpty(GameMainManager.Instance.playerData.userId);
        if (isNetGood)
        {
            if (!isInitPlayer)
            {
                WaitingPopupManager.instance.Show(canvas);
            }
            GameMainManager.Instance.netManager.Login(new qy.LoginInfo(), (ret, res) =>
            {
                LoadSevenDayInfo();
                WaitingPopupManager.instance.Close();
            });
        }
        else if (!isInitPlayer)
        {
            var alertWindow = WindowManager.instance.Show<UIUnLoginPopupWindow>().GetComponent<UIunLoginPopup>();
            alertWindow.Init(LanguageManager.instance.GetValueByKey("210157"));
        }

        TrySelectRole();
    }

    private void TrySelectRole()
    {
        qy.config.QuestItem quest = GameMainManager.Instance.playerData.GetQuest();
        if (quest == null)
        {
            GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIRoleWindow);
        }
        else if (quest.type == qy.config.QuestItem.QuestType.Ending)
        {
            GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIEndingWindow, quest);
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
        });
    }

    private void ShowDailyLandingPopup()
    {
        //登陆后弹出7日登录活动
        int day = GameMainManager.Instance.playerData.indexDay;
        WindowManager.instance.Show<DailyLandingActivitiesPopupWindow>().GetComponent<DailyLandingActivities>().Init(day + 1);
    }

    private void NewPlayerGuideRefresh()
    {
        if (GameMainManager.Instance.playerData.eliminateLevel == 1)
        {
            if (!PlayerPrefs.HasKey("Welcome"))
            {
                WindowManager.instance.Show<WelcomePopupWindow>();
                PlayerPrefs.SetInt("Welcome", 1);
            }

            // TODO.
        }
    }
}
