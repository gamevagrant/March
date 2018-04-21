using UnityEngine;
using UnityEngine.UI;
using March.Core.WindowManager;
using qy;
public class ModifyNamePanel : MonoBehaviour
{
    private Button sureButton;
    private InputField inputField;
    private Text placeholder;
    private Text titleText;

    private Popup popup;

    private void Awake()
    {
        sureButton = transform.Find("SureButton").GetComponent<Button>();
        inputField = transform.Find("InputField").GetComponent<InputField>();
        placeholder = transform.Find("InputField/Placeholder").GetComponent<Text>();
        titleText = transform.Find("Title/Text").GetComponent<Text>();

        popup = GetComponent<Popup>();

        Messenger.AddListener(ELocalMsgID.CloseModifyPanel, OnClosedBtn);
    }

    void Start()
    {
        sureButton.onClick.AddListener(OnSureBtn);
        placeholder.text = GameMainManager.Instance.playerData.nickName;


        titleText.text = LanguageManager.instance.GetValueByKey("200027");
        sureButton.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey("200029");
    }

    private void OnClosedBtn()
    {
        if (popup != null)
        {
            popup.Close();
        }
    }

    private void OnSureBtn()
    {
        if (!qy.net.NetManager.Instance.isNetWorkStatusGood)
        {
            WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210145"));
            return;
        }

        string nickName = GetInputFiledContent();
        if (Utils.instance.isMatchChinese(nickName) || Utils.instance.isMatchNumberOrCharacter(nickName))
        {
            if (Utils.instance.isStrLengthValid(nickName))
            {
                //NetManager.instance.modifyNickName(nickName);
                GameMainManager.Instance.playerModel.ModifyNickName(nickName);
            }
            else
            {
                MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200048"));
            }
        }
        else
        {
            MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200047"));
        }
    }


    private string GetInputFiledContent()
    {
        bool isValid = Utils.instance.isValid(inputField.textComponent.text);
        Debug.Log("输入内容是否有效：" + isValid);
        return inputField.textComponent.text;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(ELocalMsgID.CloseModifyPanel, OnClosedBtn);
    }
}
