using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
using qy;
using qy.config;
using UnityEngine.UI;
using March.Core.WindowManager;
public class UITaskWindow :  UIWindowBase{

    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UITaskWindow,
                type = qy.ui.UISettings.UIWindowType.PopUp,
            };

            return windowData;
        }
    }

    public Image roleHeadImage;
    public Text roleNameText;
    public Text resurrectionCountText;
    public Slider loyaltySlider;
    public Slider wisdomSlider;
    public Slider discipline;
    public Text levelText;
    public Slider levelSlider;
    public Text levelProgressText;
    public Image taskImg;
    public Text taskTitle;
    public Text taskDesText;
    public GameObjectPool propPool;
    public GameObjectPool selectPool;
    public Text tipsText;
    public Text doBtnText;
    public RectTransform missionTopTF;
    public RectTransform missionBottomTF;
    public RectTransform missionBottomSelectTF;
    public RectTransform taskBgTF;
    public RectTransform taskDesTF;


    private qy.PlayerData playerdata;
    private qy.config.QuestItem questItem;

    protected override void StartShowWindow(object[] data)
    {
        qy.PlayerData player = data[0] as qy.PlayerData;
        playerdata = player;
        
        UpdatePanel();
        
    }

    private void UpdatePanel()
    {
        questItem = playerdata.GetQuest();
        string headUrl = FilePathTools.GetPersonHeadPath(playerdata.role.headIcon);
        AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        {
            roleHeadImage.sprite = sp;
        });
        roleNameText.text = playerdata.role.name;
        resurrectionCountText.text = "";
        loyaltySlider.value = playerdata.ability.loyalty / 100f;
        wisdomSlider.value = playerdata.ability.wisdom / 100f;
        discipline.value = playerdata.ability.discipline / 100f;
        levelText.text = "等级："+playerdata.level.ToString();

        qy.config.LevelItem levelItem = GameMainManager.Instance.configManager.levelConfig.GetItem(playerdata.level);
        if(levelItem!=null)
        {
            float expProgress = playerdata.currExp / (float)levelItem.exp;
            levelSlider.value = expProgress;
            levelProgressText.text = (expProgress*100).ToString("f0")+"%";
        }else
        {
            levelSlider.value = 0;
            levelProgressText.text = "0%";
        }
        

        if(!string.IsNullOrEmpty(questItem.bg))
        {
            taskBgTF.anchorMax = new Vector2(0.49f,1);
            taskDesTF.anchorMin = new Vector2(0.51f,0);
            string taskBGUrl = FilePathTools.GetStorySpritePath(questItem.bg);
            AssetsManager.Instance.LoadAssetAsync<Sprite>(taskBGUrl, (sp) =>
            {
                taskImg.sprite = sp;
            });
        }else
        {
            taskBgTF.anchorMax = new Vector2(0, 1);
            taskDesTF.anchorMin = new Vector2(0, 0);
        }

        
        taskTitle.text = questItem.sectionName;
        taskDesText.text = questItem.sectionDes;



        if (questItem.type == qy.config.QuestItem.QuestType.Branch)
        {
            SetSelectTask();
        }
        else
        {
            SetMainTask();
        }
    }

    private void SetSelectTask()
    {
        missionBottomSelectTF.gameObject.SetActive(true);
        missionBottomTF.gameObject.SetActive(false);
        missionTopTF.anchorMin = new Vector2(0, 0.5f);

        selectPool.resetAllTarget();
        List<SelectItem> selects = questItem.selectList;
        foreach(SelectItem item in selects)
        {
            UISelectItem cell = selectPool.getIdleTarget<UISelectItem>();
            cell.SetData(item);
        }
    }

    private void SetMainTask()
    {
        missionBottomSelectTF.gameObject.SetActive(false);
        missionBottomTF.gameObject.SetActive(true);
        missionTopTF.anchorMin = new Vector2(0,0.35f);

        propPool.resetAllTarget();
        if(!playerdata.ContainsComplateQuest(playerdata.questId))
        {
            List<PropItem> props = new List<PropItem>();
            if(questItem.requireStar>0)
            {
                props.Add(new PropItem()
                {
                    id = "1",
                    icon = "starsp",
                    count = questItem.requireStar,
                });
            }
            
            props.AddRange(questItem.requireItem);
            foreach (PropItem item in props)
            {
                UIPropCell cell = propPool.getIdleTarget<UIPropCell>();
                if (item.id == "1")
                {
                    //设置显示星星数量
                    cell.SetData(item, playerdata.starNum);

                }
                else
                {
                    PropItem haveProp = playerdata.GetPropItem(item.id);
                    cell.SetData(item, haveProp == null ? 0 : haveProp.count);
                }

            }
        }
        
    }

    private void DoTask()
    {
        
        string selectedID = "";
        if (questItem.type == qy.config.QuestItem.QuestType.Branch)
        {
            List<UISelectItem> list = selectPool.getActiveTargets<UISelectItem>();
            foreach(UISelectItem item in list)
            {
                IEnumerable<Toggle> t = item.toggle.group.ActiveToggles();
                if (item.toggle.isOn)
                {
                    selectedID = item.data.id;
                    break;
                }
            }
            if (string.IsNullOrEmpty(selectedID))
            {
                return;
            }
        }
        string storyID;
        PlayerModelErr err = GameMainManager.Instance.playerModel.QuestComplate(out storyID, selectedID);
        if(err == PlayerModelErr.NULL)
        {
            if(!string.IsNullOrEmpty(storyID)&&storyID!="0")
            {
                GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIDialogueWindow, storyID);
                OnClickClose();
            }else
            {
                GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UITaskWindow,playerdata);
                //UpdatePanel();
            }
            
        }else if(err == PlayerModelErr.NOT_ENOUGH_PROP)
        {
            //MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200010"));
            Alert.Show(LanguageManager.instance.GetValueByKey("200010"),Alert.OK,(btn)=>{
                OpenLevelBeginPopupWindow();
            });
            
        }
        else if(err == PlayerModelErr.NOT_ENOUGH_STAR)
        {
            Alert.Show(LanguageManager.instance.GetValueByKey("200011"), Alert.OK, (btn) => {
                OpenLevelBeginPopupWindow();
            });
           
        }
        
    }
    /// <summary>
    /// 打开关卡面板
    /// </summary>
    private void OpenLevelBeginPopupWindow()
    {
        GameMainManager.Instance.netManager.MakePointInEliminateClick((ret, res) => { });
        if (GameMainManager.Instance.playerData.heartNum < 1)
        {
            WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200025"));
        }
        else if (GameMainManager.Instance.playerData.eliminateLevel > GameMainManager.Instance.configManager.settingConfig.max)
        {
            WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("200049"));
        }
        else
        {
            WindowManager.instance.Show<BeginPopupWindow>();
        }
    }

    public void OnClickDoBtnHandle()
    {
        DoTask();
    }
}
