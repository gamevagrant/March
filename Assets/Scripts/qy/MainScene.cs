using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using March.Core.WindowManager;
using qy.ui;
using qy;
namespace qy
{
    public class MainScene : MonoBehaviour
    {
        private void Awake()
        {
            GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIMainSceneWindow);
        }
        // Use this for initialization
        void Start()
        {
            Login();
            TrySelectRole();
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

        // Update is called once per frame
        void Update()
        {

        }

        public void Login()
        {
            GameMainManager.Instance.playerModel.UpdateHeart();
            bool isNetGood = GameMainManager.Instance.netManager.isNetWorkStatusGood;
            bool isInitPlayer = !string.IsNullOrEmpty(GameMainManager.Instance.playerData.userId);
            if (isNetGood)
            {
                if (!isInitPlayer)
                {
                    //WaitingPopupManager.instance.Show(GameMainManager.Instance.uiManager);
                    
                }
                GameMainManager.Instance.netManager.Login(new LoginInfo(), (ret, res) =>
                {
                    LoadSevenDayInfo();
                    WaitingPopupManager.instance.Close();
                });
            }
            else if (!isInitPlayer)
            {
                Alert.Show(LanguageManager.instance.GetValueByKey("210157"));
                
            }

            
        }

        private void TrySelectRole()
        {
            config.QuestItem quest = GameMainManager.Instance.playerData.GetQuest();
            if (quest == null)
            {
                GameMainManager.Instance.uiManager.OpenWindow(ui.UISettings.UIWindowID.UIRoleWindow);
            }
            else if (quest.type == config.QuestItem.QuestType.Ending)
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
    }
}

