using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Booster : MonoBehaviour 
{
    public static Booster instance = null;

	[Header("Other")]
	public Material m_material;
	public Image icon;
	public Image m_lockImage;
	public Image numBg;

    [Header("Board")]
    public Board board;

    [Header("Booster")]
    public GameObject singleBooster;

    [Header("Active")]
    public GameObject singleActive;

    [Header("Amount")]
    public Text singleAmount;

    [Header("Popup")]
    public PopupOpener singleBoosterPopup;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

	void Start () 
    {
		singleBooster.SetActive(true);
		singleAmount.text = PlayerData.instance.getHasItemCountByItemId("200006").ToString();

        // single breaker
        //todo:从表里读取
        if (LevelLoader.instance.level < 9)
        {
			//icon.material = m_material;
			m_lockImage.gameObject.SetActive (true);
			//numBg.gameObject.SetActive (false);
			//singleAmount.gameObject.SetActive (false);
		}
        Messenger.AddListener(ELocalMsgID.RefreshBaseData,refresh);
//        // change help order in the hierarchy
//        if (LevelLoader.instance.level == 7)
//        {
//            Help.instance.gameObject.transform.SetParent(Booster.instance.gameObject.transform);
//            Help.instance.gameObject.transform.SetSiblingIndex(0);
//        }
//        else if (LevelLoader.instance.level == 12)
//        {
//            Help.instance.gameObject.transform.SetParent(Booster.instance.gameObject.transform);
//            Help.instance.gameObject.transform.SetSiblingIndex(1);
//        }
//        else if (LevelLoader.instance.level == 15)
//        {
//            Help.instance.gameObject.transform.SetParent(Booster.instance.gameObject.transform);
//            Help.instance.gameObject.transform.SetSiblingIndex(2);
//        }
//        else if (LevelLoader.instance.level == 18)
//        {
//            Help.instance.gameObject.transform.SetParent(Booster.instance.gameObject.transform);
//            Help.instance.gameObject.transform.SetSiblingIndex(3);
//        }
//        else if (LevelLoader.instance.level == 25)
//        {
//            Help.instance.gameObject.transform.SetParent(Booster.instance.gameObject.transform);
//            Help.instance.gameObject.transform.SetSiblingIndex(4);
//        }
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(ELocalMsgID.RefreshBaseData,refresh);
    }

    #region Single

    public void SingleBoosterClick()
    {
		if (LevelLoader.instance.level < 9) {
			var go = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UIAlertPopup"), GameObject.Find("Canvas").transform) as GameObject;
			go.GetComponent<UIAlertPopup> ().Init (LanguageManager.instance.GetValueByKey ("210134"));//LanguageManager.instance.GetValueByKey ("");
			go.GetComponent<Popup> ().Open ();
			return;
		}
        if (board.state != GAME_STATE.WAITING_USER_SWAP || board.lockSwap == true)
        {
            return;
        }

        AudioManager.instance.ButtonClickAudio();

        board.dropTime = 1;

        // check amount
        
        if (PlayerData.instance.getHasItemCountByItemId("200006") <= 0)
        {
            // show booster popup
            ShowPopup(BOOSTER_TYPE.SINGLE_BREAKER);

            return;
        }

        if (board.booster == BOOSTER_TYPE.NONE)
        {
            ActiveBooster(BOOSTER_TYPE.SINGLE_BREAKER);
        }
        else
        {
            CancelBooster(BOOSTER_TYPE.SINGLE_BREAKER);
        }
    }

    #endregion

//    #region Row
//
//    public void RowBoosterClick()
//    {
//        if (board.state != GAME_STATE.WAITING_USER_SWAP || board.lockSwap == true)
//        {
//            return;
//        }
//
//        AudioManager.instance.ButtonClickAudio();
//
//        board.dropTime = 1;
//
//        // hide help
//        if (LevelLoader.instance.level == 12)
//        {
//            // hide step 1
//            Help.instance.Hide();
//
//            // show step 2
//            if (Help.instance.step == 1)
//            {
//                var prefab = Instantiate(Resources.Load(Configure.Level12Step2())) as GameObject;
//                prefab.name = "Level 12 Step 2";
//
//                prefab.gameObject.transform.SetParent(Help.instance.gameObject.transform);
//                prefab.GetComponent<RectTransform>().localScale = Vector3.one;
//
//                Help.instance.current = prefab;
//
//                Help.instance.step = 2;
//            }
//        }
//
//        // check amount
//
//        if (GameData.instance.GetRowBreaker() <= 0)
//        {
//            // show booster popup
//            ShowPopup(BOOSTER_TYPE.ROW_BREAKER);
//
//            return;
//        }
//
//        if (board.booster == BOOSTER_TYPE.NONE)
//        {
//            ActiveBooster(BOOSTER_TYPE.ROW_BREAKER);
//        }
//        else
//        {
//            CancelBooster(BOOSTER_TYPE.ROW_BREAKER);
//        }
//    }
//
//    #endregion
//
//    #region Column
//
//    public void ColumnBoosterClick()
//    {
//        if (board.state != GAME_STATE.WAITING_USER_SWAP || board.lockSwap == true)
//        {
//            return;
//        }
//
//        AudioManager.instance.ButtonClickAudio();
//
//        board.dropTime = 1;
//
//        // hide help
//        if (LevelLoader.instance.level == 15)
//        {
//            // hide step 1
//            Help.instance.Hide();
//
//            // show step 2
//            if (Help.instance.step == 1)
//            {
//                var prefab = Instantiate(Resources.Load(Configure.Level15Step2())) as GameObject;
//                prefab.name = "Level 15 Step 2";
//
//                prefab.gameObject.transform.SetParent(Help.instance.gameObject.transform);
//                prefab.GetComponent<RectTransform>().localScale = Vector3.one;
//
//                Help.instance.current = prefab;
//
//                Help.instance.step = 2;
//            }
//        }
//
//        // check amount
//
//        if (GameData.instance.GetColumnBreaker() <= 0)
//        {
//            // show booster popup
//            ShowPopup(BOOSTER_TYPE.COLUMN_BREAKER);
//
//            return;
//        }
//
//        if (board.booster == BOOSTER_TYPE.NONE)
//        {
//            ActiveBooster(BOOSTER_TYPE.COLUMN_BREAKER);
//        }
//        else
//        {
//            CancelBooster(BOOSTER_TYPE.COLUMN_BREAKER);
//        }
//    }
//
//    #endregion
//
//    #region Rainbow
//
//    public void RainbowBoosterClick()
//    {
//        if (board.state != GAME_STATE.WAITING_USER_SWAP || board.lockSwap == true)
//        {
//            return;
//        }
//
//        AudioManager.instance.ButtonClickAudio();
//
//        board.dropTime = 1;
//
//        // hide help
//        if (LevelLoader.instance.level == 18)
//        {
//            // hide step 1
//            Help.instance.Hide();
//
//            // show step 2
//            if (Help.instance.step == 1)
//            {
//                var prefab = Instantiate(Resources.Load(Configure.Level18Step2())) as GameObject;
//                prefab.name = "Level 18 Step 2";
//
//                prefab.gameObject.transform.SetParent(Help.instance.gameObject.transform);
//                prefab.GetComponent<RectTransform>().localScale = Vector3.one;
//
//                Help.instance.current = prefab;
//
//                Help.instance.step = 2;
//            }
//        }
//
//        // check amount
//
//        if (GameData.instance.GetRainbowBreaker() <= 0)
//        {
//            // show booster popup
//            ShowPopup(BOOSTER_TYPE.RAINBOW_BREAKER);
//
//            return;
//        }
//
//        if (board.booster == BOOSTER_TYPE.NONE)
//        {
//            ActiveBooster(BOOSTER_TYPE.RAINBOW_BREAKER);
//        }
//        else
//        {
//            CancelBooster(BOOSTER_TYPE.RAINBOW_BREAKER);
//        }
//    }
//
//    #endregion
//
//    #region Oven
//
//    public void OvenBoosterClick()
//    {
//        if (board.state != GAME_STATE.WAITING_USER_SWAP || board.lockSwap == true)
//        {
//            return;
//        }
//
//        AudioManager.instance.ButtonClickAudio();
//
//        board.dropTime = 0;
//
//        // hide help
//        if (LevelLoader.instance.level == 25)
//        {
//            // hide step 1
//            Help.instance.Hide();
//
//            // show step 2
//            if (Help.instance.step == 1)
//            {
//                var prefab = Instantiate(Resources.Load(Configure.Level25Step2())) as GameObject;
//                prefab.name = "Level 25 Step 2";
//
//                prefab.gameObject.transform.SetParent(Help.instance.gameObject.transform);
//                prefab.GetComponent<RectTransform>().localScale = Vector3.one;
//
//                Help.instance.current = prefab;
//
//                Help.instance.step = 2;
//            }
//        }
//
//        // check amount
//
//        if (GameData.instance.GetOvenBreaker() <= 0)
//        {
//            // show booster popup
//            ShowPopup(BOOSTER_TYPE.OVEN_BREAKER);
//
//            return;
//        }
//
//        if (board.booster == BOOSTER_TYPE.NONE)
//        {
//            ActiveBooster(BOOSTER_TYPE.OVEN_BREAKER);
//        }
//        else
//        {
//            CancelBooster(BOOSTER_TYPE.OVEN_BREAKER);
//        }
//    }
//
//    #endregion

    #region Complete

    public void BoosterComplete()
    {
        if (board.booster == BOOSTER_TYPE.SINGLE_BREAKER)
        {
            CancelBooster(BOOSTER_TYPE.SINGLE_BREAKER);

            // reduce amount

            if (PlayerData.instance.getHasItemCountByItemId("200006") > 0)
            {
                Debug.Log(PlayerData.instance.getHasItemCountByItemId("200006"));

                var amount = PlayerData.instance.getHasItemCountByItemId("200006") - 1;

                NetManager.instance.userToolsToServer("200006", "1");

                // change text

                singleAmount.text = amount.ToString();
            }
        }
    }

    #endregion

    #region Popup

    public void ShowPopup(BOOSTER_TYPE check)
    {
        if (check == BOOSTER_TYPE.SINGLE_BREAKER)
        {
            board.state = GAME_STATE.OPENING_POPUP;

			singleBoosterPopup.popupPrefab.GetComponent<UIBoosterPopup> ().booster = BOOSTER_TYPE.SINGLE_BREAKER;
            singleBoosterPopup.OpenPopup();
        }
    }

    #endregion

    #region Booster

    public void ActiveBooster(BOOSTER_TYPE check)
    {
        if (check == BOOSTER_TYPE.SINGLE_BREAKER)
        {
            board.booster = BOOSTER_TYPE.SINGLE_BREAKER;

            singleActive.SetActive(true);

            // interactable
//            rowActive.transform.parent.GetComponent<AnimatedButton>().interactable = false;
//            columnActive.transform.parent.GetComponent<AnimatedButton>().interactable = false;
//            rainbowActive.transform.parent.GetComponent<AnimatedButton>().interactable = false;
//            ovenActive.transform.parent.GetComponent<AnimatedButton>().interactable = false;
        }
    }

    public void CancelBooster(BOOSTER_TYPE check)
    {
        board.booster = BOOSTER_TYPE.NONE;

        if (check == BOOSTER_TYPE.SINGLE_BREAKER)
        {
            singleActive.SetActive(false);

            // interactable
//            rowActive.transform.parent.GetComponent<AnimatedButton>().interactable = true;
//            columnActive.transform.parent.GetComponent<AnimatedButton>().interactable = true;
//            rainbowActive.transform.parent.GetComponent<AnimatedButton>().interactable = true;
//            ovenActive.transform.parent.GetComponent<AnimatedButton>().interactable = true;
        }
    }

    #endregion

    #region refresh

    public void refresh()
    {
        int x = PlayerData.instance.getHasItemCountByItemId("200006");

        singleAmount.text = PlayerData.instance.getHasItemCountByItemId("200006").ToString();
    }


    #endregion
}
