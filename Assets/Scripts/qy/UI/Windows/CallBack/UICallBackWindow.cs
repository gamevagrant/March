using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
using UnityEngine.UI;
using qy;

public class UICallBackWindow : UIWindowBase
{
    public Text callBackDes;
    public Text callBackCost;
    public Text callBackCardCount;
    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UICallBackWindow,
                type = qy.ui.UISettings.UIWindowType.PopUp,
            };

            return windowData;
        }
    }

    protected override void StartShowWindow(object[] data)
    {
        callBackCost.text = GameMainManager.Instance.configManager.settingConfig.callBackPrice.ToString();
        callBackCardCount.text = "0";
    }

    public void OnClickCallBackCoinHandle()
    {
        PlayerModelErr err = GameMainManager.Instance.playerModel.CallBackRoleWithCoin(GameMainManager.Instance.playerData.role.id);
        if(err == PlayerModelErr.NOT_ENOUGH_COIN)
        {
            Alert.Show("金币不足\n 快去赚金币吧",Alert.OK,(btn)=> {

                Debug.Log("赚金币");

            },"赚金币");
        }
        OnClickClose();
    }

    public void OnClickCallBackCardHandle()
    {

    }
}

