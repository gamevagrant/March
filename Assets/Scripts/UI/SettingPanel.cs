﻿using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BestHTTP;
using LitJson;

public class SettingPanel : MonoBehaviour
{

	public Button m_linkBtn;
	public Button m_helpBtn;
	public Button m_evaluateBtn;
	public Button m_changeBtn;
	public Button m_notifyBtn;
	public Button m_languageBtn;
	public Button m_closeBtn;

	public Text m_faceBookDes;

	public Image m_effect_01_img;
	public Image m_effect_02_img;
	public Image m_music_01_img;
	public Image m_music_02_img;

	public Text m_titleText;
	private bool is_effect_on;
	private bool is_music_on;

	private GameObject m_modifyNamePanel;

	// Use this for initialization
	void Start ()
	{
		initText ();
	    is_effect_on = PlayerPrefs.GetInt("sound_on") == 1;
	    is_music_on = PlayerPrefs.GetInt("music_on") == 1;

        if (FB.IsLoggedIn) {
			m_linkBtn.GetComponent<Image> ().sprite = Resources.Load ("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
			m_linkBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("210150");
		}

		if (is_effect_on)
		{
			m_effect_02_img.gameObject.SetActive(false);
		}
		else
		{
			m_effect_01_img.gameObject.SetActive(false);
		}
		if (is_music_on)
		{
			m_music_02_img.gameObject.SetActive(false);
		}
		else
		{
			m_music_01_img.gameObject.SetActive(false);
		}
	}

	private void initText()
	{
		m_linkBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200031");
		m_helpBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200033");
		m_evaluateBtn .transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200035");
		m_changeBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200032");
		m_notifyBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200034");
		m_languageBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200036");
		m_faceBookDes.text = LanguageManager.instance.GetValueByKey ("200037");
		m_titleText.text = LanguageManager.instance.GetValueByKey ("200037");
	}
	// Update is called once per frame
	void Update () {

	}

	protected void HandleResult(IResult result)
	{
		if (result == null)
		{
			Debug.Log ("-----------------: " + "Null Response");
			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log ("-----------------: " + "Error Response: " + result.Error);
		}
		else if (result.Cancelled)
		{
			Debug.Log ("-----------------: " + "Cancelled Response: " + result.RawResult);
		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			foreach (KeyValuePair<string, object> pair in result.ResultDictionary)
			{
				Debug.Log ("-----------------Success Response: Key:" + pair.Key + "Value:" + pair.Value.ToString());
			}

			string id = result.ResultDictionary ["user_id"].ToString();
			if (id != null) {
				NetManager.instance.userBind (id, "fbName", userBindInfoRev);
			} else {
				var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
				go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("210147"));//LanguageManager.instance.GetValueByKey ("");
				go.GetComponent<Popup> ().Open ();
			}
			//Debug.Log ("-----------------: " + "Success Response: " + result.ResultDictionary);

		}
		else
		{
			Debug.Log ("-----------------: " + "Empty Response");
		}
	}

	public void onLinkBtn()
	{
		if (FB.IsInitialized) {
			if (!FB.IsLoggedIn) {
				FB.LogInWithReadPermissions (new List<string> () { "public_profile", "email", "user_friends" }, this.HandleResult);
			} else if (FB.IsLoggedIn) {
				FB.LogOut ();
				NetManager.instance.userUnBind (userUnBindInfoRev);
			}
		}
	}

	private void userBindInfoRev(HTTPRequest request,HTTPResponse response)
	{
		Debug.Log ("userBindInfoRev response:" + response.DataAsText);
		JsonData data = JsonMapper.ToObject(response.DataAsText);
		PlayerData.instance.RefreshData(data);
		var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
		go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("210146"));
		go.GetComponent<Popup> ().Open ();

		m_linkBtn.GetComponent<Image> ().sprite = Resources.Load ("Sprites/Cookie/UI/General/buy3btn", typeof(Sprite)) as Sprite;
		m_linkBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("210150");
	}

	private void userUnBindInfoRev(HTTPRequest request,HTTPResponse response)
	{
		Debug.Log ("userUnBindInfoRev response:" + response.DataAsText);
		JsonData data = JsonMapper.ToObject(response.DataAsText);
		//PlayerData.instance.RefreshData(data);
		var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
		go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("210148"));
		go.GetComponent<Popup> ().Open ();

		m_linkBtn.GetComponent<Image> ().sprite = Resources.Load ("Sprites/Cookie/UI/General/buy4btn", typeof(Sprite)) as Sprite;
		m_linkBtn.transform.Find ("Text").GetComponent<Text> ().text = LanguageManager.instance.GetValueByKey ("200031");
	}

	public void onHelpBtn()
	{
		//		if (FB.IsInitialized) {
		//			FB.ShareLink(
		//				new System.Uri("https://developers.facebook.com/"),
		//				"Link Share",
		//				"Look I'm sharing a link",
		//				new System.Uri("http://i.imgur.com/j4M7vCO.jpg"),
		//				callback: this.HandleResult);
		//		}
	}

	public void onEvaluateBtn()
	{


	}

	public void onChangeBtn()
	{
		m_modifyNamePanel = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ModifyNamePanel"));
		GameObject mUICanvas = GameObject.Find("Canvas");
		m_modifyNamePanel.transform.parent = this.transform;
		((RectTransform)m_modifyNamePanel.transform).anchoredPosition = Vector2.zero;
		// m_modifyNamePanel.transform.SetSiblingIndex(mUICanvas.transform.GetSiblingIndex());
	}

	public void onNotifyBtn()
	{

	}

	public void onLanguageBtn()
	{

	}

	public void onCloseBtn()
	{
		MainScene.Instance.m_settingPanel.SetActive(false);
	}

	public void onEffectBtn()
	{
		if (PlayerPrefs.GetInt("sound_on") == 1)
		{
			m_effect_01_img.gameObject.SetActive(false);
			m_effect_02_img.gameObject.SetActive(true);
			PlayerPrefs.SetInt("sound_on",0);
			AudioListener.volume = 0;

		}
		else
		{
			m_effect_01_img.gameObject.SetActive(true);
			m_effect_02_img.gameObject.SetActive(false);
			PlayerPrefs.SetInt("sound_on", 1);
			AudioListener.volume = 1;
		}

	}

	public void onMusicBtn()
	{
		if (PlayerPrefs.GetInt("music_on") == 1)
		{
			m_music_01_img.gameObject.SetActive(false);
			m_music_02_img.gameObject.SetActive(true);
			var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
			backgroundAudioSource.volume = 0;
			PlayerPrefs.SetInt("music_on", 0);
		}
		else
		{
			m_music_01_img.gameObject.SetActive(true);
			m_music_02_img.gameObject.SetActive(false);
			var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
			backgroundAudioSource.volume = 1;
			PlayerPrefs.SetInt("music_on", 1);
		}
	}
}
