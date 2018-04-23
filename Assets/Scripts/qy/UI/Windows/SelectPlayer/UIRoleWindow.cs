using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
using UnityEngine.UI;
using qy.config;
using qy;

public class UIRoleWindow : UIWindowBase {

    public AutoScrollView scrollView;
    public Image head;
    public Slider loyaltySlider;
    public Slider wisdomSlider;
    public Slider discipline;
    public Text roleName;
    public Text introduction;

    private string selectedRoleID;

    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UIRoleWindow,
                type = qy.ui.UISettings.UIWindowType.Fixed,
            };

            return windowData;
        }
    }

    private void Awake()
    {
        scrollView.onSelected += OnSelected;
    }

    private void OnDestroy()
    {
        scrollView.onSelected -= OnSelected;
    }

    private void OnSelected(BaseItemView item)
    {
        UIRoleWindowHead roleItem = item as UIRoleWindowHead;
        SetPanel(roleItem.data);
        selectedRoleID = roleItem.data.id;
    }

    protected override void StartShowWindow(object[] data)
    {
        List<RoleItem> roles = GameMainManager.Instance.configManager.roleConfig.GetRoleList();
        scrollView.SetData(roles);
    }

    private void SetPanel(RoleItem item)
    {
        string headUrl = FilePathTools.GetPersonHeadPath(item.headIcon);
        AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        {
            head.sprite = sp;
        });

        loyaltySlider.value = item.ability.loyalty/100f;
        wisdomSlider.value = item.ability.wisdom / 100f;
        discipline.value = item.ability.discipline / 100f;
        roleName.text = item.name;
        introduction.text = item.introduction;
    }

    public void OnClickStartBtnHandle()
    {
        GameMainManager.Instance.playerModel.StartGameWithRole(selectedRoleID);
        OnClickClose(); 
    }
}
