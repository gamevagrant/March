using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILevelPopup : MonoBehaviour 
{
    public Text levelText;
    public Image star1;
    public Image star2;
    public Image star3;
    public Image cake;

    public Image tick1;
    public Image tick2;
    public Image tick3;
    public Image add1;
    public Image add2;
    public Image add3;
    public Text number1;
    public Text number2;
    public Text number3;

    public Text targetText;

    public SceneTransition transition;
    public PopupOpener beginBooster1Popup;
    public PopupOpener beginBooster2Popup;
    public PopupOpener beginBooster3Popup;

    public bool avaialbe1;
    public bool avaialbe2;
    public bool avaialbe3;

    public Image booster1;
    public Image booster2;
    public Image booster3;

    public GameObject locked1;
    public GameObject locked2;
    public GameObject locked3;

    public Text lockedText1;
    public Text lockedText2;
    public Text lockedText3;

    // FB Leaderboard
    public GameObject FBLeaderboard;

    void Start()
    {
        levelText.text = "Level " + LevelLoader.instance.level.ToString();

        Configure.instance.beginFiveMoves = false;
        Configure.instance.beginRainbow = false;
        Configure.instance.beginBombBreaker = false;

        var star = GameData.instance.GetLevelStar(LevelLoader.instance.level);

        switch (star)
        {
            case 1:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
                break;
            case 2:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(false);
                break;
            case 3:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(true);
                break;
            default:
                star1.gameObject.SetActive(false);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
                break;
        }

        string name;
        if (LevelLoader.instance.cake > 0)
        {
            name = "cake_" + LevelLoader.instance.cake + "_4";
        }
        else
        {
            // default/test
            name = "cake_1_4";
        }
        
        cake.sprite = Resources.Load<Sprite>(Configure.Cake(name));

        targetText.text = LevelLoader.instance.targetText;

        // begin boosters
        for (int i = 1; i <=3; i++)
        {
            int boosterAmount = 0;
            Image tick = null;
            Image add = null;
            Text number = null;
            bool avaialbe = false;
            Image booster = null;
            GameObject locked = null;
            Text lockedText = null;

            switch (i)
            {
                case 1:
                    boosterAmount = GameData.instance.beginFiveMoves;
                    tick = tick1;
                    add = add1;
                    number = number1;
                    avaialbe1 = (LevelLoader.instance.level < Configure.instance.beginFiveMovesLevel) ? false : true;
                    avaialbe = avaialbe1;
                    booster = booster1;
                    locked = locked1;
                    lockedText = lockedText1;
                    break;
                case 2:
                    boosterAmount = GameData.instance.beginRainbow;
                    tick = tick2;
                    add = add2;
                    number = number2;
                    avaialbe2 = (LevelLoader.instance.level < Configure.instance.beginRainbowLevel) ? false : true;
                    avaialbe = avaialbe2;
                    booster = booster2;
                    locked = locked2;
                    lockedText = lockedText2;
                    break;
                case 3:
                    boosterAmount = GameData.instance.beginBombBreaker;
                    tick = tick3;
                    add = add3;
                    number = number3;
                    avaialbe3 = (LevelLoader.instance.level < Configure.instance.beginBombBreakerLevel) ? false : true;
                    avaialbe = avaialbe3;
                    booster = booster3;
                    locked = locked3;
                    lockedText = lockedText3;
                    break;
            }

            if (avaialbe == true)
            {
                if (boosterAmount > 0)
                {
                    number.text = boosterAmount.ToString();
                    add.gameObject.SetActive(false);
                    tick.gameObject.SetActive(false);
                }
                else
                {
                    number.text = "0";
                    add.gameObject.SetActive(true);
                    tick.gameObject.SetActive(false);
                }
            }
            else
            {
                number.text = "0";
                number.gameObject.transform.parent.gameObject.SetActive(false);
                add.gameObject.SetActive(false);
                tick.gameObject.SetActive(false);
                booster.gameObject.SetActive(false);
                locked.SetActive(true);

                switch (i)
                {
                    case 1:
                        lockedText.text = "Require\nLevel " + Configure.instance.beginFiveMovesLevel;
                        break;
                    case 2:
                        lockedText.text = "Require\nLevel " + Configure.instance.beginRainbowLevel;
                        break;
                    case 3:
                        lockedText.text = "Require\nLevel " + Configure.instance.beginBombBreakerLevel;
                        break;
                }
            }
        }

        // On/Off FB leaderboard
        if (Configure.instance.FBLeaderboard == false)
        {
            FBLeaderboard.SetActive(false);
        }
    }

	public void PlayButtonClick()
    {
        AudioManager.instance.ButtonClickAudio();

        // if enough life
        if (Configure.instance.life > 0)
        {
            // reduce life
            GameObject.Find("LifeBar").GetComponent<Life>().ReduceLife(1);

            // change scene
            transition.PerformTransition();
        }
        else
        {
            //print("Show life popup");
            GameObject.Find("MapScene").GetComponent<MapScene>().LifeButtonClick();
        }
    }

    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }

    public void BeginBoosterClick(int booster)
    {
        var avaiable = false;

        switch (booster)
        {
            case 1:
                avaiable = avaialbe1;
                break;
            case 2:
                avaiable = avaialbe2;
                break;
            case 3:
                avaiable = avaialbe3;
                break;
        }

        if (avaiable == false)
        {            
            return;
        }

        // Help
        if (LevelLoader.instance.level == 10)
        {
            if (Help.instance.step == 1)
            {
                Help.instance.SelfDisactive();
            }
        }

        if (LevelLoader.instance.level == 20)
        {
            if (Help.instance.step == 1)
            {
                Help.instance.SelfDisactive();
            }
        }

        if (LevelLoader.instance.level == 23)
        {
            if (Help.instance.step == 1)
            {
                Help.instance.SelfDisactive();
            }
        }

        AudioManager.instance.ButtonClickAudio();

        int number = 0;

        switch (booster)
        {
            case 1:
                number = GameData.instance.beginFiveMoves;
                break;
            case 2:
                number = GameData.instance.beginRainbow;
                break;
            case 3:
                number = GameData.instance.beginBombBreaker;
                break;
        }

        if (number > 0)
        {
            switch (booster)
            {
                case 1:
                    if (Configure.instance.beginFiveMoves == false)
                    {
                        tick1.gameObject.SetActive(true);
                        number1.gameObject.SetActive(false);
                        Configure.instance.beginFiveMoves = true;
                    }
                    else
                    {
                        tick1.gameObject.SetActive(false);
                        number1.gameObject.SetActive(true);
                        Configure.instance.beginFiveMoves = false;
                    }
                    break;
                case 2:
                    if (Configure.instance.beginRainbow == false)
                    {
                        tick2.gameObject.SetActive(true);
                        number2.gameObject.SetActive(false);
                        Configure.instance.beginRainbow = true;
                    }
                    else
                    {
                        tick2.gameObject.SetActive(false);
                        number2.gameObject.SetActive(true);
                        Configure.instance.beginRainbow = false;
                    }                    
                    break;
                case 3:
                    if (Configure.instance.beginBombBreaker == false)
                    {
                        tick3.gameObject.SetActive(true);
                        number3.gameObject.SetActive(false);
                        Configure.instance.beginBombBreaker = true;
                    }
                    else
                    {
                        tick3.gameObject.SetActive(false);
                        number3.gameObject.SetActive(true);
                        Configure.instance.beginBombBreaker = false;
                    }                    
                    break;
            }
        }
        else
        {
            switch (booster)
            {
                case 1:
                    beginBooster1Popup.OpenPopup();
                    break;
                case 2:
                    beginBooster2Popup.OpenPopup();
                    break;
                case 3:
                    beginBooster3Popup.OpenPopup();
                    break;
            }
        }
    }
}
