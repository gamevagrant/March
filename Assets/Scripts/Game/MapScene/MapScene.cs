using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapScene : MonoBehaviour 
{
    public PopupOpener levelPopup;
    public Text coinText;
    public Text starText;
    public PopupOpener shopPopup;
    public GameObject foundTarget;
    public GameObject levels;
    public GameObject scrollContent;
    public PopupOpener lifePopup;
    public PopupOpener dailyRewardsPopup;
    public PopupOpener luckySpinPopup;
    public PopupOpener packagePopup;

    float canvasHeight;

	void Start () 
    {
        canvasHeight = ((float)Screen.height / (float)Screen.width) * 720f;

        coinText.text = GameData.instance.GetPlayerCoin().ToString();

        if (Configure.instance.autoPopup > 0 && Configure.instance.autoPopup <= Configure.instance.maxLevel)
        {
            StartCoroutine(OpenLevelPopup());
        }

        var maxStar = GameData.instance.GetOpendedLevel() * 3;

        int star = 0;

        for (int i = 1; i <= GameData.instance.GetOpendedLevel(); i++)
        {
            star += GameData.instance.GetLevelStar(i);
        }

        starText.text = star.ToString() + "/" + maxStar.ToString();

        var currentPosition = Vector3.zero;

        if (LevelLoader.instance.level == 0)
        {
            currentPosition = TargetPosition();
        }
        else
        {
            currentPosition = levels.transform.GetChild(LevelLoader.instance.level).GetComponent<RectTransform>().localPosition;
        }

        scrollContent.GetComponent<RectTransform>().localPosition = new Vector3(0, canvasHeight / 2 - currentPosition.y, 0);
	}

    void Update()
    {
        #region Scroll

        var position = canvasHeight / 2 - TargetPosition().y;

        var y = scrollContent.GetComponent<RectTransform>().localPosition.y;

        if (position - canvasHeight / 2 + 200f < y && y < position + canvasHeight / 2 - 250f)
        {
            foundTarget.SetActive(false);
        }
        else
        {
            foundTarget.GetComponent<RectTransform>().localScale = Vector3.one;

            foundTarget.SetActive(true);
        }

        #endregion

        #region Button

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // close Level popup
            if (GameObject.Find("LevelPopup(Clone)"))
            {
                GameObject.Find("LevelPopup(Clone)").GetComponent<Popup>().Close();

                // if help popup is open then close it
                if (GameObject.Find("Help").transform.GetChild(0))
                {
                    GameObject.Find("Help").transform.GetChild(0).gameObject.SetActive(false);
                }
                
            }
            else
            {
                Application.Quit();
            }
        }

        #endregion
    }
	
	public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }

    IEnumerator OpenLevelPopup()
    {
        yield return new WaitForSeconds(0.5f);

        LevelLoader.instance.level = Configure.instance.autoPopup;
        LevelLoader.instance.LoadLevel();

        Configure.instance.autoPopup = 0;

        levelPopup.OpenPopup();

        // show help

        yield return new WaitForSeconds(0.5f);

        Help.instance.help = true;
//        Help.instance.ShowOnMap();
    }

    public void CoinButtonClick()
    {
        if (!GameObject.Find("ShopPopupMap(Clone)"))
        {
            shopPopup.OpenPopup();
        }
    }

	public void LifeButtonClick()
	{
		//print ("Show Life Pop Up");

        if (!GameObject.Find("LifePopup(Clone)") && !GameObject.Find("ShopPopupMap(Clone)"))
        {
            lifePopup.OpenPopup();
        }
	}

    public void DailyRewardsButtonClick()
    {
        if (!GameObject.Find("DailyRewardsPopup(Clone)"))
        {
            dailyRewardsPopup.OpenPopup();
        }
    }

    public void LuckySpinButtonClick()
    {
        //print ("Lucky Spin Pop Up");

        if (!GameObject.Find("LuckySpinPopup(Clone)"))
        {
            luckySpinPopup.OpenPopup();
        }
    }

    public void PackageButtonClick()
    {
        //print("Package button click");

        if (!GameObject.Find("PackagePopup(Clone)"))
        {
            packagePopup.OpenPopup();
        }
    }

    public void InviteFriendClick()
    {
//        FBInvite.AppInvite();
    }

    public void RequestFriendClick()
    {
//        FBRequest.RequestChallenge();
    }

    public void FoundTargetButtonClick()
    {
        AudioManager.instance.ButtonClickAudio();

        StartCoroutine(ScrollContent(new Vector3(0, canvasHeight / 2 - TargetPosition().y, 0)));
    }

    IEnumerator ScrollContent(Vector3 target)
    {
        if (target.y > 0) target.y = 0;

        var from = scrollContent.GetComponent<RectTransform>().localPosition;
        float step = Time.fixedDeltaTime;
        float t = 0;

        while (t <= 1.0f)
        {
            t += step;
            scrollContent.GetComponent<RectTransform>().localPosition = Vector3.Lerp(from, target, t);
            yield return new WaitForFixedUpdate();
        }

        scrollContent.GetComponent<RectTransform>().localPosition = target;
    }

    Vector3 TargetPosition()
    {
        var currentPosition = Vector3.zero;

        foreach (Transform level in levels.transform)
        {
            if (level.gameObject.GetComponent<UILevel>().status == MAP_LEVEL_STATUS.CURRENT)
            {
                currentPosition = level.gameObject.GetComponent<RectTransform>().localPosition;
                break;
            }
        }

        return currentPosition;
    }

    public void UpdateCoinAmountLabel()
    {
        coinText.text = GameData.instance.GetPlayerCoin().ToString();
    }
    public void BackBtnClick()
    {
        SceneManager.UnloadScene("map");
        SceneManager.LoadScene("UI");
    }
}
