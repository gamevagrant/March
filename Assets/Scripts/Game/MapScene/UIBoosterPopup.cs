using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBoosterPopup : MonoBehaviour 
{
    public BOOSTER_TYPE booster;
    public Text cost1;
	public Text title;
	public Image itemImage;
	public Text itemNum;
	public Text itemDes;
    public Text cost2;

	private item m_item;
    public item Item { get { if (m_item == null) { m_item = DefaultConfig.getInstance().GetConfigByType<item>(); } return m_item; } }

    public bool clicking;

    public PopupOpener shopPopup;

	BuyBtnCallBcak m_CallBack;

	public void Init(BuyBtnCallBcak callback) {
		m_CallBack = callback;
		Debug.Log ("");
	}

	void Start () 
    {

		title.text = LanguageManager.instance.GetValueByKey ("200017");

		int amount = Configure.instance.package1Amount;
		itemNum.text = string.Format ("×{0}", amount);
	    switch (booster)
        {
            case BOOSTER_TYPE.BEGIN_FIVE_MOVES:
                cost1.text = Configure.instance.beginFiveMovesCost1.ToString();
                cost2.text = Configure.instance.beginFiveMovesCost2.ToString();
                break;
			case BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER:
				cost1.text = string.Format ("{0}", int.Parse (Item.GetItemByID ("200003").price) * amount);
				itemDes.text = LanguageManager.instance.GetValueByKey ("210004");
				itemImage.sprite = Resources.Load ("Sprites/UI/item002", typeof(Sprite)) as Sprite;
				itemImage.SetNativeSize ();
                break;
			case BOOSTER_TYPE.BEGIN_BOMB_BREAKER:
				cost1.text = string.Format ("{0}", int.Parse (Item.GetItemByID ("200004").price) * amount);
				itemDes.text = LanguageManager.instance.GetValueByKey ("210003");
				itemImage.sprite = Resources.Load ("Sprites/UI/item003", typeof(Sprite)) as Sprite;
				itemImage.SetNativeSize ();
                break;
			case BOOSTER_TYPE.BEGIN_PLANE_BREAKER:
				cost1.text = string.Format ("{0}", int.Parse (Item.GetItemByID ("200005").price) * amount);
				itemDes.text = LanguageManager.instance.GetValueByKey ("210005");
				itemImage.sprite = Resources.Load ("Sprites/UI/item004", typeof(Sprite)) as Sprite;
				itemImage.SetNativeSize ();
				break;

            case BOOSTER_TYPE.SINGLE_BREAKER:
				cost1.text = string.Format ("{0}", int.Parse (Item.GetItemByID ("200006").price) * amount);
				itemDes.text = LanguageManager.instance.GetValueByKey ("210002");
				itemImage.sprite = Resources.Load ("Sprites/UI/item005", typeof(Sprite)) as Sprite;
				itemImage.SetNativeSize ();
                break;
            case BOOSTER_TYPE.ROW_BREAKER:
                cost1.text = Configure.instance.rowBreakerCost1.ToString();
                cost2.text = Configure.instance.rowBreakerCost2.ToString();
                break;
            case BOOSTER_TYPE.COLUMN_BREAKER:
                cost1.text = Configure.instance.columnBreakerCost1.ToString();
                cost2.text = Configure.instance.columnBreakerCost2.ToString();
                break;
            case BOOSTER_TYPE.RAINBOW_BREAKER:
                cost1.text = Configure.instance.rainbowBreakerCost1.ToString();
                cost2.text = Configure.instance.rainbowBreakerCost2.ToString();
                break;
            case BOOSTER_TYPE.OVEN_BREAKER:
                cost1.text = Configure.instance.ovenBreakerCost1.ToString();
                cost2.text = Configure.instance.ovenBreakerCost2.ToString();
                break;
        }
	}

    public void BuyButtonClick(int package)
    {
        // avoid multiple click
        if (clicking == true) return;
        
        clicking = true;
        StartCoroutine(ResetButtonClick());

        int cost = 0;
		int amount = Configure.instance.package1Amount;

        if (package == 1)
        {
            switch (booster)
            {
                case BOOSTER_TYPE.BEGIN_FIVE_MOVES:
                    cost = Configure.instance.beginFiveMovesCost1;
                    amount = Configure.instance.package1Amount;
                    break;
                case BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER:
					cost = int.Parse (Item.GetItemByID ("200003").price) * amount;
                    break;
                case BOOSTER_TYPE.BEGIN_BOMB_BREAKER:
					cost = int.Parse (Item.GetItemByID ("200004").price) * amount;
                    break;
				case BOOSTER_TYPE.BEGIN_PLANE_BREAKER:
					cost = int.Parse (Item.GetItemByID ("200005").price) * amount;
					break;

                case BOOSTER_TYPE.SINGLE_BREAKER:
					cost = int.Parse (Item.GetItemByID ("200006").price) * amount;
                    break;
                case BOOSTER_TYPE.ROW_BREAKER:
                    cost = Configure.instance.rowBreakerCost1;
                    amount = Configure.instance.package1Amount;
                    break;
                case BOOSTER_TYPE.COLUMN_BREAKER:
                    cost = Configure.instance.columnBreakerCost1;
                    amount = Configure.instance.package1Amount;
                    break;
                case BOOSTER_TYPE.RAINBOW_BREAKER:
                    cost = Configure.instance.rainbowBreakerCost1;
                    amount = Configure.instance.package1Amount;
                    break;
                case BOOSTER_TYPE.OVEN_BREAKER:
                    cost = Configure.instance.ovenBreakerCost1;
                    amount = Configure.instance.package1Amount;
                    break;
            }
        }
        
		int coin = PlayerData.instance.getCoinNum ();
        // enough coin
		if (cost <= coin)
        {
            // play sound
            AudioManager.instance.CoinPayAudio();

            // add booster amount
            switch (booster)
            {
                case BOOSTER_TYPE.BEGIN_FIVE_MOVES:
                    GameData.instance.SaveBeginFiveMoves(amount);
					NetManager.instance.buyItemToServer("200007", cost.ToString());
                    break;
                case BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER:
					GameData.instance.SaveBeginRainbow(amount);
					NetManager.instance.buyItemToServer("200003", amount.ToString());
                    break;
                case BOOSTER_TYPE.BEGIN_BOMB_BREAKER:
					GameData.instance.SaveBeginBombBreaker(amount);
					NetManager.instance.buyItemToServer("200004", amount.ToString());
                    break;
				case BOOSTER_TYPE.BEGIN_PLANE_BREAKER:
					GameData.instance.SaveBeginBombBreaker(amount);
					NetManager.instance.buyItemToServer("200005", amount.ToString());
					break;

                case BOOSTER_TYPE.SINGLE_BREAKER:
					GameData.instance.SaveSingleBreaker(amount);
					NetManager.instance.buyItemToServer("200006",amount.ToString());
                    break;
                case BOOSTER_TYPE.ROW_BREAKER:
                    GameData.instance.SaveRowBreaker(amount);
                    break;
                case BOOSTER_TYPE.COLUMN_BREAKER:
                    GameData.instance.SaveColumnBreaker(amount);
                    break;
                case BOOSTER_TYPE.RAINBOW_BREAKER:
                    GameData.instance.SaveRainbowBreaker(amount);
                    break;
                case BOOSTER_TYPE.OVEN_BREAKER:
                    GameData.instance.SaveOvenBreaker(amount);
                    break;
            }

			// reduce coin
			int c = coin - cost;
			GameData.instance.SavePlayerCoin(c);
			GameObject coinText_go = GameObject.Find("coinText");
			if (coinText_go != null) {
				coinText_go.GetComponent<Text> ().text = string.Format ("{0}", c);
			}

			if (m_CallBack != null)
			{
				m_CallBack (amount);
			}

            // begin booster
			if (booster == BOOSTER_TYPE.BEGIN_FIVE_MOVES || booster == BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER || booster == BOOSTER_TYPE.BEGIN_BOMB_BREAKER || booster == BOOSTER_TYPE.BEGIN_PLANE_BREAKER)
            {
                // update level popup
                if (GameObject.Find("LevelPopup(Clone)"))
                {
                    var levelPopup = GameObject.Find("LevelPopup(Clone)") as GameObject;

                    switch (booster)
                    {
                        case BOOSTER_TYPE.BEGIN_FIVE_MOVES:
                            levelPopup.GetComponent<UILevelPopup>().number1.text = amount.ToString();
                            levelPopup.GetComponent<UILevelPopup>().add1.gameObject.SetActive(false);
                            levelPopup.GetComponent<UILevelPopup>().BeginBoosterClick(1);
                            break;
                        case BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER:                            
                            levelPopup.GetComponent<UILevelPopup>().number2.text = amount.ToString();
                            levelPopup.GetComponent<UILevelPopup>().add2.gameObject.SetActive(false);
                            levelPopup.GetComponent<UILevelPopup>().BeginBoosterClick(2);
                            break;
                        case BOOSTER_TYPE.BEGIN_BOMB_BREAKER:                            
                            levelPopup.GetComponent<UILevelPopup>().number3.text = amount.ToString();
                            levelPopup.GetComponent<UILevelPopup>().add3.gameObject.SetActive(false);
                            levelPopup.GetComponent<UILevelPopup>().BeginBoosterClick(3);
                            break;
                    }
                }

                // close popup
				if (GameObject.Find("SingleBoosterPopup(Clone)"))
				{
					GameObject.Find("SingleBoosterPopup(Clone)").GetComponent<Popup>().Close();

					GameObject go = GameObject.Find("Board");
					if (go != null) {
						go.GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
					}
				}

//                switch (booster)
//                {
//                    case BOOSTER_TYPE.BEGIN_FIVE_MOVES:
//                        if (GameObject.Find("BeginBooster1Popup(Clone)"))
//                        {
//                            GameObject.Find("BeginBooster1Popup(Clone)").GetComponent<Popup>().Close();
//                        }
//                        break;
//                    case BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER:
//                        if (GameObject.Find("BeginBooster2Popup(Clone)"))
//                        {
//                            GameObject.Find("BeginBooster2Popup(Clone)").GetComponent<Popup>().Close();
//                        }
//                        break;
//                    case BOOSTER_TYPE.BEGIN_BOMB_BREAKER:
//                        if (GameObject.Find("BeginBooster3Popup(Clone)"))
//                        {
//                            GameObject.Find("BeginBooster3Popup(Clone)").GetComponent<Popup>().Close();
//                        }
//                        break;
//                }

                // update coin label
                //GameObject.Find("MapScene").GetComponent<MapScene>().UpdateCoinAmountLabel();
            }
            // in game booster
            else
            {
                // update bottom bar
                switch (booster)
                {
                    case BOOSTER_TYPE.SINGLE_BREAKER:
                        // update amount
                        Booster.instance.singleAmount.text = amount.ToString();
                        // active booster
                        Booster.instance.ActiveBooster(BOOSTER_TYPE.SINGLE_BREAKER);
                        break;
//                    case BOOSTER_TYPE.ROW_BREAKER:                        
////                        Booster.instance.rowAmount.text = amount.ToString();
//                        Booster.instance.ActiveBooster(BOOSTER_TYPE.ROW_BREAKER);
//                        break;
//                    case BOOSTER_TYPE.COLUMN_BREAKER:
////                        Booster.instance.columnAmount.text = amount.ToString();
//                        Booster.instance.ActiveBooster(BOOSTER_TYPE.COLUMN_BREAKER);
//                        break;
//                    case BOOSTER_TYPE.RAINBOW_BREAKER:
////                        Booster.instance.rainbowAmount.text = amount.ToString();
//                        Booster.instance.ActiveBooster(BOOSTER_TYPE.RAINBOW_BREAKER);
//                        break;
//                    case BOOSTER_TYPE.OVEN_BREAKER:
////                        Booster.instance.ovenAmount.text = amount.ToString();
//                        Booster.instance.ActiveBooster(BOOSTER_TYPE.OVEN_BREAKER);
//                        break;
                }

                // close popup
                switch (booster)
                {
                    case BOOSTER_TYPE.SINGLE_BREAKER:
                        if (GameObject.Find("SingleBoosterPopup(Clone)"))
                        {
                            GameObject.Find("SingleBoosterPopup(Clone)").GetComponent<Popup>().Close();

                            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
                        }
                        break;
                    case BOOSTER_TYPE.ROW_BREAKER:
                        if (GameObject.Find("RowBoosterPopup(Clone)"))
                        {
                            GameObject.Find("RowBoosterPopup(Clone)").GetComponent<Popup>().Close();

                            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
                        }
                        break;
                    case BOOSTER_TYPE.COLUMN_BREAKER:
                        if (GameObject.Find("ColumnBoosterPopup(Clone)"))
                        {
                            GameObject.Find("ColumnBoosterPopup(Clone)").GetComponent<Popup>().Close();

                            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
                        }
                        break;
                    case BOOSTER_TYPE.RAINBOW_BREAKER:
                        if (GameObject.Find("RainbowBoosterPopup(Clone)"))
                        {
                            GameObject.Find("RainbowBoosterPopup(Clone)").GetComponent<Popup>().Close();

                            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
                        }
                        break;
                    case BOOSTER_TYPE.OVEN_BREAKER:
                        if (GameObject.Find("OvenBoosterPopup(Clone)"))
                        {
                            GameObject.Find("OvenBoosterPopup(Clone)").GetComponent<Popup>().Close();

                            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
                        }
                        break;
                }
            }
        }
        // not enough coin
        else
        {
            // show shop popup
            shopPopup.OpenPopup();
        }
    }

    IEnumerator ResetButtonClick()
    {
        yield return new WaitForSeconds(1f);

        clicking = false;
    }

    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }

    public void CloseButtonClick()
    {
        AudioManager.instance.ButtonClickAudio();

        // if booster is in game we re-set game state
		GameObject go = GameObject.Find("Board");
		if (go != null) {
			go.GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
		}
    }
}
