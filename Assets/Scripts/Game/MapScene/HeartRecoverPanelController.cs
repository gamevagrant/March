using System;
using UnityEngine;
using UnityEngine.UI;

public class HeartRecoverPanelController : MonoBehaviour
{
    private Text countDownText;
    private Text tileText;
    private Text nextText;
    private Text starText;
    private Text allLifeText;

    private GameObject buyButton;
    private Text buyText;

    private Action onBuyAction;
    private Popup popup;

    void Awake()
    {
        countDownText = transform.Find("downTime_Text").GetComponent<Text>();
        tileText = transform.Find("title_Text").GetComponent<Text>();
        nextText = transform.Find("next_Text").GetComponent<Text>();
        starText = transform.Find("starNum_Text").GetComponent<Text>();
        allLifeText = transform.Find("all_life_text").GetComponent<Text>();

        buyButton = transform.Find("buyPanel").gameObject;
        buyText = buyButton.transform.Find("iconNum_Text").GetComponent<Text>();

        popup = GetComponent<Popup>();
    }

    void Start()
    {
        tileText.text = LanguageManager.instance.GetValueByKey("200039");
		nextText.text = LanguageManager.instance.GetValueByKey("210151");

        buyButton.SetActive(PlayerData.instance.getHeartNum() == 0);
        buyText.text = PlayerData.instance.getLivePrice().ToString();
    }

    void Update()
    {
		var heartNum = PlayerData.instance.getHeartNum ();
		if (heartNum < 5)
        {
			allLifeText.gameObject.SetActive (false);
			if (heartNum == 0) {
				buyButton.gameObject.SetActive (true);
			}
            countDownText.text = string.Format("{0:D2}: {1:D2}", (int)TimeMonoManager.instance.getTotalTime() / 60, (int)TimeMonoManager.instance.getTotalTime() % 60);
            starText.text = PlayerData.instance.getHeartNum().ToString();
        }
        else
        {
            countDownText.gameObject.SetActive(false);
            nextText.gameObject.SetActive(false);
			allLifeText.gameObject.SetActive (true);

            allLifeText.text = LanguageManager.instance.GetValueByKey("200022");
            TimeMonoManager.instance.setTotalTime(0);  //心数已满状态的时候totaltime 置为0；
        }
    }

    public void OnBuyHeart()
    {
        onBuyAction();
        popup.Close();
    }

    public void RegisterCallback(Action action)
    {
        onBuyAction = action;
    }
}
