using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void BuyBtnCallBcak(int num);

public class BeginProp : MonoBehaviour
{

	[Header("Material")]
	public Material m_material;

    public Image Icon;
	public Image m_lockImage;
	public Image NumBG;
    public Text Num;
	public Image m_numImage;
    public string ItemId;
    public Button ItemButton;

    private bool isPress;
	private bool isLock = false;

	void Start () {

	}

	private void setGray() {
		//Icon.material = m_material;
		//Num.gameObject.SetActive (false);
		//m_numImage.gameObject.SetActive (false);
		//NumBG.gameObject.SetActive (false);
		m_lockImage.gameObject.SetActive (true);
		//Num.text = "-1";
		isLock = true;
	}

	public void Init(int itemid)
    {
		isPress = false;
		ItemId = string.Format("{0}", itemid);

		int num = PlayerData.instance.getHasItemCountByItemId (ItemId);
		if (num == 0) {
			Num.gameObject.SetActive (false);
			m_numImage.gameObject.SetActive (true);
			m_numImage.GetComponent<Image> ().sprite = Resources.Load ("Sprites/UI/add", typeof(Sprite)) as Sprite;
			Num.text = "+";
		} else {
			Num.gameObject.SetActive (true);
			m_numImage.gameObject.SetActive (false);
			Num.text = num.ToString();
		}

		if (LevelLoader.instance.level < 15) { //全部没有
			setGray();
		} else if (LevelLoader.instance.level < 17) { //炸弹
			if (itemid == 200003 || itemid == 200005) {
				setGray ();
			}
		}  else if (LevelLoader.instance.level < 21) { //飞机
			if (itemid == 200005) {
				setGray ();
			}
		}

		GoodsItem goodsItem = DefaultConfig.getInstance().GetConfigByType<item>().GetItemByID(ItemId); 
		Sprite sp = Resources.Load(string.Format("Sprites/UI/{0}", goodsItem.icon), typeof(Sprite)) as Sprite;
		Icon.sprite = sp;
		Icon.SetNativeSize ();
		Icon.GetComponent<Transform> ().localScale = new Vector3 (0.3f, 0.3f, 0.3f);
    }

    public void onUseBtnClick()
    {
		if (isLock) {
			var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
			go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("210134"));//;
			go.GetComponent<Popup> ().Open ();
			return;
		}
        if (isPress)
        {
            isPress = false;
            //ItemButton.gameObject.GetComponent<Image>().color = Color.white;
			Num.gameObject.SetActive (true);
			m_numImage.gameObject.SetActive (false);

            LevelLoader.instance.beginItemList.Remove(ItemId);
        }
        else
        {
			if (Num.text == "+") {
				PopupOpener go = GetComponent<PopupOpener> ();
				switch (ItemId) {
					case "200003":
						go.popupPrefab.GetComponent<UIBoosterPopup> ().booster = BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER;
						break;
					case "200004":
						go.popupPrefab.GetComponent<UIBoosterPopup> ().booster = BOOSTER_TYPE.BEGIN_BOMB_BREAKER;
						break;
					case "200005":
						go.popupPrefab.GetComponent<UIBoosterPopup> ().booster = BOOSTER_TYPE.BEGIN_PLANE_BREAKER;
						break;
				}

				GameObject popup = go.OpenPopup ();
				popup.GetComponent<UIBoosterPopup> ().Init (bCallBack);
				return;
			}

            isPress = true;
            //ItemButton.gameObject.GetComponent<Image>().color = new Color(0.98f, 0.74f, 0.74f, 1f);
			Num.gameObject.SetActive (false);
			m_numImage.gameObject.SetActive (true);
			if (Num.text == "+") {
				m_numImage.GetComponent<Image> ().sprite = Resources.Load ("Sprites/UI/btn_add_01", typeof(Sprite)) as Sprite;
			} else {
				m_numImage.GetComponent<Image> ().sprite = Resources.Load ("Sprites/UI/sele", typeof(Sprite)) as Sprite;
			}

            LevelLoader.instance.beginItemList.Add(ItemId);
		

        }
    }


	public void bCallBack(int num)
	{
		Num.text = num.ToString();
		Num.gameObject.SetActive (true);
		m_numImage.gameObject.SetActive (false);
	}
}
