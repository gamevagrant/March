//using System;

using System;
using UnityEngine;
//using System.Collections;
//using BestHTTP;
//using Boo.Lang;
//using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILosePopup : MonoBehaviour
{

	public Text m_titleText;
	public Text m_des;

    public Text keepCost;
    public Text headText;
    public Text btnText;

    public Board board;

    public GameObject loseTargetLayout;
    public GameObject loseSBILayout;


    private int cost;
    private int addsteps;
    private int times;
    private StepsBuyConfig stepsbuyconfig;

	public PopupOpener shopPopup;

    void Start()
    {
		m_titleText.text = LanguageManager.instance.GetValueByKey ("200019");
		m_des.text = LanguageManager.instance.GetValueByKey ("210010");
		//btnText.text = LanguageManager.instance.GetValueByKey ("200016");

        board = GameObject.Find("Board").GetComponent<Board>();

        stepsbuyconfig = new StepsBuyConfig();

        if (board != null)
        {
            times = board.FiveMoreTimes;
            cost = stepsbuyconfig.GetPriceByTimes(times);
            keepCost.text = cost.ToString();
            addsteps = stepsbuyconfig.GetAddSteps();

            for (int i = 0; i < board.targetLeftList.Count; i++)
            {
                if (board.targetLeftList[i] > 0)
                {
                    if (loseTargetLayout != null)
                    {
                        GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseTargetCell"), loseTargetLayout.transform) as GameObject;
						cell.GetComponent<UILoseTargetCell>().Init(LevelLoader.instance.targetList[i].Type, board.targetLeftList[i], LevelLoader.instance.targetList[i].color);
                    }
                }
            }
            var dic = stepsbuyconfig.GetEffectDicByTimes(times);
			if (dic.Count > 0) {
				GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseStepBuyItemCell"), loseSBILayout.transform) as GameObject;

				cell.GetComponent<UILoseSBICell>().Init();
			}
            foreach (var tmp in dic)
            {
                if (loseSBILayout != null)
                {
                    GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseStepBuyItemCell"), loseSBILayout.transform) as GameObject;

                    cell.GetComponent<UILoseSBICell>().Init(tmp.Key,tmp.Value);
                }
            }
            dic = stepsbuyconfig.GetItemBagDicByTimes(times);
            foreach (var tmp in dic)
            {
                if (loseSBILayout != null)
                {
                    GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseStepBuyItemCell"), loseSBILayout.transform) as GameObject;

                    cell.GetComponent<UILoseSBICell>().Init(tmp.Key, tmp.Value);
                }
            }




        }

    }

  

//    public void ExitButtonClick()
//    {
//        AudioManager.instance.ButtonClickAudio();
//
//        toMap.PerformTransition();
//    }

//    public void ReplayButtonClick()
//    {
//        AudioManager.instance.ButtonClickAudio();
//
//        Configure.instance.autoPopup = LevelLoader.instance.level;
//
//        toMap.PerformTransition();
//    }

//    public void SkipButtonClick()
//    {
//        AudioManager.instance.ButtonClickAudio();
//
//        var cost = Configure.instance.skipLevelCost;
//
//        // enough coin
//        if (cost <= GameData.instance.playerCoin)
//        {
//            AudioManager.instance.CoinPayAudio();
//
//            // reduce coin
//            GameData.instance.SavePlayerCoin(GameData.instance.playerCoin - cost);
//
//            var board = GameObject.Find("Board").GetComponent<Board>();
//
//            if (board)
//            {
//                // save info
//                board.SaveLevelInfo();
//            }
//
//            // go to map with auto popup of next level
//            Configure.instance.autoPopup = LevelLoader.instance.level + 1;
//
//            toMap.PerformTransition();
//        }
//        else
//        {
//            shopPopup.OpenPopup();
//        }
//    }

    public void KeepButtonClick()
    {
        AudioManager.instance.ButtonClickAudio();



        // enough coin
        if (cost <= PlayerData.instance.getCoinNum())
        {
            AudioManager.instance.CoinPayAudio();

            NetManager.instance.eliminateLevelFiveMore(cost, stepsbuyconfig.GetItemBagDicByTimes(times));

            if (board)
            {
                // add 5 more moves
                board.moveLeft = addsteps;

                // change the label
                board.UITop.Set5Moves(addsteps);

                // change the game state
                board.state = GAME_STATE.WAITING_USER_SWAP;

                board.FiveMoreTimes++;

                foreach (var tmp in stepsbuyconfig.GetEffectDicByTimes(times))
                {
                    for (int i = 0; i < tmp.Value; i++)
                    {
                        board.BoosterEffect(tmp.Key);
                    }
                }
                Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
                // reset and call hint
                board.checkHintCall = 0;
                board.Hint();
                
            }

            // close the popup`
            var popup = GameObject.Find("LosePopup(Clone)");

            if (popup)
            {
                popup.GetComponent<Popup>().Close();
            }
        }
        else
        {
			//btnText.text = LanguageManager.instance.GetValueByKey ("200023");
			shopPopup.OpenPopup();
        }
        // not enough coin
//        else
//        {
//            shopPopup.OpenPopup();
//        }
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void OnCloseClick()
    {
        NetManager.instance.eliminateLevelEnd(LevelLoader.instance.level, 0, board.allstep,0);

        var beginPopup = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/BeginPopup"), GameObject.Find("Canvas").transform) as GameObject;
        Destroy(gameObject);
    }
}
