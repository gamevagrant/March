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
            
            TrySelectRole();
        }
        // Use this for initialization
        void Start()
        {
            //Login();
            LoadSevenDayInfo();
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

        // Update is called once per frame
        void Update()
        {

        }

        private void TrySelectRole()
        {
            config.QuestItem quest = GameMainManager.Instance.playerData.GetQuest();
            PlayerData.RoleState state = GameMainManager.Instance.playerData.roleState;
            if (GameMainManager.Instance.playerData.role==null || quest == null || (quest!=null &&  state == PlayerData.RoleState.Pass))
            {
                GameMainManager.Instance.uiManager.OpenWindow(ui.UISettings.UIWindowID.UIRoleWindow);
            }else
            {
                GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIMainSceneWindow);
                if (GameMainManager.Instance.playerData.roleState == PlayerData.RoleState.Dide)
                {

                    GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIEndingWindow, quest);
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

