using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy.ui;
using qy;
public class UIEndingWindow :  UIWindowBase
{
    public Text titleText;
    public Text endingDesText;
    public Button resurrectionBtn;
    public Image bgImg;


    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UIEndingWindow,
                type = qy.ui.UISettings.UIWindowType.PopUp,
            };

            return windowData;
        }
    }

    private qy.config.QuestItem data;

    protected override void StartShowWindow(object[] data)
    {
        this.data = data[0] as qy.config.QuestItem;

        if(this.data.type == qy.config.QuestItem.QuestType.Ending)
        {
            UpdateUI(this.data);
        }

    }

    private void UpdateUI(qy.config.QuestItem quest)
    {
        //string headUrl = FilePathTools.GetStorySpritePath(quest.bg);
        //AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        //{
        //    bgImg.sprite = sp;
        //});

        if(!string.IsNullOrEmpty(quest.bg))
        {
            var sp = March.Core.ResourceManager.ResourceManager.instance.Load<Sprite>(Configure.StoryBackground, quest.bg);
            bgImg.sprite = sp;
        }
        

        if(quest.endingType==1)
        {
            titleText.text = "当前角色光荣牺牲";
            resurrectionBtn.gameObject.SetActive(true);
        }
        else
        {
            titleText.text = "恭喜通关本角色";
            resurrectionBtn.gameObject.SetActive(false);
        }
        endingDesText.text = quest.sectionDes;


    }

    /// <summary>
    /// 新角色
    /// </summary>
    public void OnClickNewRoleBtnHandle()
    {
        GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIRoleWindow);
        OnClickClose();
    }
    /// <summary>
    /// 复活
    /// </summary>
    public void OnClickResurrection()
    {
        GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UICallBackWindow,GameMainManager.Instance.playerData.role.id);
        OnClickClose();
    }
}
