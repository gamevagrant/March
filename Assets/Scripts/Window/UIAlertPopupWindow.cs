using UnityEngine.UI;

public class UIAlertPopupWindow : Window {
	public Text m_titleText;
	public Text m_des;

	void Start()
	{
		m_titleText.text = LanguageManager.instance.GetValueByKey("210133");
	}

	public void Init(string message)
	{
		m_des.text = message;
	}
} 