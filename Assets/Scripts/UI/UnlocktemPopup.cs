using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlocktemPopup : MonoBehaviour {

	public Text m_titleText;
	public Image m_itemImg;
	public Text m_itemNum;
	public Text m_itemDes;
	public Text m_btnText;

	// Use this for initialization
	void Start () {
		m_titleText.text = LanguageManager.instance.GetValueByKey ("210001");
		if (PlayerData.instance.getEliminateLevel() == 9) {
			string itemReward = DefaultConfig.getInstance ().GetConfigByType<matchlevel> ().GetItemByID ("1000008").itemReward;
			string[] arr = itemReward.Split (new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in arr) {
				string[] arr1 = s.Split (new string[]{":"}, System.StringSplitOptions.RemoveEmptyEntries);
				if (arr1 [0] == "200006") {
					m_itemNum.text = "×" + arr1 [1];
					break;
				}
			}
			m_itemDes.text = LanguageManager.instance.GetValueByKey ("210002");
			m_itemImg.sprite = Resources.Load ("Sprites/UI/item005", typeof(Sprite)) as Sprite;
			m_itemImg.SetNativeSize ();
		} else if (PlayerData.instance.getEliminateLevel() == 15) {
			string itemReward = DefaultConfig.getInstance ().GetConfigByType<matchlevel> ().GetItemByID ("1000014").itemReward;
			string[] arr = itemReward.Split (new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in arr) {
				string[] arr1 = s.Split (new string[]{":"}, System.StringSplitOptions.RemoveEmptyEntries);
				if (arr1 [0] == "200004") {
					m_itemNum.text = "×" + arr1 [1];
					break;
				}
			}
			m_itemDes.text = LanguageManager.instance.GetValueByKey ("210003");
			m_itemImg.sprite = Resources.Load ("Sprites/UI/item003", typeof(Sprite)) as Sprite;
			m_itemImg.SetNativeSize ();
		} else if (PlayerData.instance.getEliminateLevel() == 17) {
			string itemReward = DefaultConfig.getInstance ().GetConfigByType<matchlevel> ().GetItemByID ("1000016").itemReward;
			string[] arr = itemReward.Split (new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in arr) {
				string[] arr1 = s.Split (new string[]{":"}, System.StringSplitOptions.RemoveEmptyEntries);
				if (arr1 [0] == "200003") {
					m_itemNum.text = "×" + arr1 [1];
					break;
				}
			}
			m_itemDes.text = LanguageManager.instance.GetValueByKey ("210004");
			m_itemImg.sprite = Resources.Load ("Sprites/UI/item002", typeof(Sprite)) as Sprite;
			m_itemImg.SetNativeSize ();
		} else if (PlayerData.instance.getEliminateLevel() == 21) {
			string itemReward = DefaultConfig.getInstance ().GetConfigByType<matchlevel> ().GetItemByID ("1000020").itemReward;
			string[] arr = itemReward.Split (new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in arr) {
				string[] arr1 = s.Split (new string[]{":"}, System.StringSplitOptions.RemoveEmptyEntries);
				if (arr1 [0] == "200005") {
					m_itemNum.text = "×" + arr1 [1];
					break;
				}
			}
			m_itemDes.text = LanguageManager.instance.GetValueByKey ("210005");
			m_itemImg.sprite = Resources.Load ("Sprites/UI/item004", typeof(Sprite)) as Sprite;
			m_itemImg.SetNativeSize ();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onCloseClick() {
		Destroy (gameObject);
	}
}
