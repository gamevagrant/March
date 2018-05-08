using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy.config;
using DG.Tweening;

public class UIRoleWindowHead : BaseItemView {

    public Image head;
    public Toggle toggle
    {
        get
        {
            return GetComponent<Toggle>();
        }
    }
    public GameObject stateGO;
    public Text stateText;
    public RoleItem data;
    public override void SetData(object data)
    {
        this.data = data as RoleItem;

        var sp =
            March.Core.ResourceManager.ResourceManager.instance.Load<Sprite>(Configure.StoryPerson,
                this.data.headIcon);
        head.sprite = sp;
        GameUtils.Scaling(head.transform as RectTransform, new Vector2(sp.texture.width, sp.texture.height));

        //string headUrl = FilePathTools.GetPersonHeadPath(this.data.headIcon);
        //AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        //{
        //    head.sprite = sp;
        //    GameUtils.Scaling(head.transform as RectTransform, new Vector2(sp.texture.width, sp.texture.height));
        //});

        qy.PlayerData.RoleState state = qy.GameMainManager.Instance.playerData.GetRoleState(this.data.id);
        switch (state)
        {
            case qy.PlayerData.RoleState.Dide:
                stateGO.SetActive(true);
                stateText.text = "已死亡";
                break;
            case qy.PlayerData.RoleState.Pass:
                stateGO.SetActive(true);
                stateText.text = "已通关";
                break;
            default:
                stateGO.SetActive(false);
                break;
        }
    }


    
}
