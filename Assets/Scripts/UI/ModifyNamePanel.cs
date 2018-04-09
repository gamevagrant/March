using UnityEngine;
using UnityEngine.UI;

public class ModifyNamePanel : MonoBehaviour
{

    public Button m_closeBtn;
    public Button m_sureBtn;
    public InputField m_inputField;
    public Text m_Placeholder;
	public Text m_name;
	public Text m_titleText;

    private void Awake()
    {
        Messenger.AddListener(ELocalMsgID.CloseModifyPanel, onClosedBtn);
    }
	// Use this for initialization
	void Start () {
        m_closeBtn.onClick.AddListener(onClosedBtn);
        m_sureBtn.onClick.AddListener(onSureBtn);
//		m_Placeholder.text = LanguageManager.instance.GetValueByKey ("200028");
//		m_name.text = PlayerData.instance.getNickName();
		m_Placeholder.text = PlayerData.instance.getNickName();

		m_titleText.text = LanguageManager.instance.GetValueByKey ("200027");
		m_sureBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200029");
	}

    private void onClosedBtn()
    {
        this.gameObject.SetActive(false);
    }

    private void onSureBtn()
    {
        if (!NetManager.instance.isNetWorkStatusGood())
        {
            var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
            go.GetComponent<UIAlertPopup>().Init(LanguageManager.instance.GetValueByKey("210145"));
            go.GetComponent<Popup>().Open();
            return;
        }
        
        string nickName = getInputFiledContent();
        if (Utils.instance.isMatchChinese(nickName) || Utils.instance.isMatchNumberOrCharacter(nickName))
        {
            if (Utils.instance.isStrLengthValid(nickName))
            {
                NetManager.instance.modifyNickName(nickName);
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

    
    private string getInputFiledContent()
    {
       bool  _isValid =  Utils.instance.isValid(m_inputField.textComponent.text);
       Debug.Log("输入内容是否有效：" + _isValid);
       return m_inputField.textComponent.text;
    }

    // Update is called once per frame
	void Update () {
		
	}


    private void OnDestroy()
    {
        Messenger.RemoveListener(ELocalMsgID.CloseModifyPanel, onClosedBtn);
    }
}
