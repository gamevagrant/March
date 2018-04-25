using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy.config;
using DG.Tweening;

public class UIRoleWindowHead : BaseItemView {

    public Image head;
    public Image mask;
    public GameObject stateGO;
    public Text stateText;
    public RoleItem data;
    public Sprite[] BGsprites;
    public override void SetData(object data)
    {
        this.data = data as RoleItem;

        string headUrl = FilePathTools.GetPersonHeadPath(this.data.headIcon);
        AssetsManager.Instance.LoadAssetAsync<Sprite>(headUrl, (sp) =>
        {
            head.sprite = sp;
        });

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

    public override void OnSelected(bool isSelected)
    {
        mask.gameObject.SetActive(!isSelected);
        gameObject.GetComponent<Image>().sprite = isSelected?BGsprites[1]: BGsprites[0];
        transform.localScale = isSelected?new Vector3(1.3f,1.3f,1.3f):Vector3.one;
    }

    
}
