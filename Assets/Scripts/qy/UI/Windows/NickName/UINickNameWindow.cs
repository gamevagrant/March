using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy.ui;
public class UINickNameWindow : UIWindowBase
{
    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UINickNameWindow,
                type = qy.ui.UISettings.UIWindowType.PopUp,
            };

            return windowData;
        }
    }

    public InputField inputField;

    protected override void StartShowWindow(object[] data)
    {
        base.StartShowWindow(data);
    }

    private void ChangeNickName(string name)
    {
        string nickName = name;
        if (Utils.instance.isMatchChinese(nickName) || Utils.instance.isMatchNumberOrCharacter(nickName))
        {
            if (Utils.instance.isStrLengthValid(nickName))
            {
                qy.GameMainManager.Instance.playerModel.ModifyNickName(nickName);
                OnClickClose();
            }
            else
            {
                Alert.Show(LanguageManager.instance.GetValueByKey("200048"));
            }
        }
        else
        {
            Alert.Show(LanguageManager.instance.GetValueByKey("200047"));
        }
    }

    public void OnClickOKBtn()
    {
        ChangeNickName(inputField.text);
    }

}
