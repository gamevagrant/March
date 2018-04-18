using UnityEngine.UI;

public class UIFlashPopupWindow : Window {
	public Text m_des;

	// Use this for initialization
	void Start () {

	}

	public void Init(string message)
	{
		m_des.text = message;
	}
} 