using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifePopup : MonoBehaviour 
{
    public Text lifeRemain;
    public Text recoveryCost;
    public GameObject recoveryButton;

    int cost;

	// Use this for initialization
	void Start () 
    {
        if (Configure.instance.life < Configure.instance.maxLife)
        {
            lifeRemain.text = "Life: " + Configure.instance.life.ToString() + "/" + Configure.instance.maxLife.ToString();

            cost = Configure.instance.recoveryCostPerLife * (Configure.instance.maxLife - Configure.instance.life);

            recoveryCost.text = cost.ToString();;
        }
        else
        {
            lifeRemain.text = "Life: " + Configure.instance.maxLife.ToString() + "/" + Configure.instance.maxLife.ToString();
            recoveryButton.SetActive(false);
            recoveryCost.gameObject.transform.parent.gameObject.SetActive(false);
        }
	}
	
    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }

    public void RecoveryButtonClick()
    {
        // check coin
        if (GameData.instance.playerCoin < cost)
        {
            // open shop popup
            GameObject.Find("MapScene").GetComponent<MapScene>().CoinButtonClick();
        }
        else
        {
            // reduce coin and refill life
            GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() - cost);

            // play add coin sound
            AudioManager.instance.CoinPayAudio();

            // update text label
            GameObject.Find("MapScene").GetComponent<MapScene>().UpdateCoinAmountLabel();

            // update life text
            GameObject.Find("LifeBar").GetComponent<Life>().AddLife(Configure.instance.maxLife);

            // update life pupup text
            lifeRemain.text = "Life: " + Configure.instance.maxLife.ToString() + "/" + Configure.instance.maxLife.ToString();
            recoveryButton.SetActive(false);
            recoveryCost.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
