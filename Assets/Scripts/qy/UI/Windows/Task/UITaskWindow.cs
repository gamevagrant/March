using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
using qy;
using qy.config;
using UnityEngine.UI;
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
        string headUrl = FilePathTools.GetPersonHeadPath("Person1");
        AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        {
            roleHeadImage.sprite = sp;
        });
        roleNameText.text = "帅哥齐";
        resurrectionCountText.text = "死亡次数：6";
        loyaltySlider.value = playerdata.ability.loyalty / 100f;
        wisdomSlider.value = playerdata.ability.wisdom / 100f;
        discipline.value = playerdata.ability.discipline / 100f;
        levelText.text = "等级：30";
        levelSlider.value = 0.6f;
        levelProgressText.text = "60%";

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
        List<PropItem> props = questItem.requireItem;
        foreach(PropItem item in props)
        {
            UIPropCell cell = propPool.getIdleTarget<UIPropCell>();
            PropItem haveProp = playerdata.GetPropItem(item.id);
            cell.SetData(item, haveProp==null?0: haveProp.count);
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
        }
        
        string storyID;
        int err = GameMainManager.Instance.playerModel.QuestComplate(out storyID, selectedID);
        if((PlayerModel.ErrType)err == PlayerModel.ErrType.NULL)
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
            
        }else
        {
            MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200010"));
        }

    }

    public void OnClickDoBtnHandle()
    {
        DoTask();
    }
}
