﻿using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using qy;

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

    void Start()
    {
        m_titleText.text = LanguageManager.instance.GetValueByKey("200019");
        m_des.text = LanguageManager.instance.GetValueByKey("210010");
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
            if (dic.Count > 0)
            {
                GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseStepBuyItemCell"), loseSBILayout.transform) as GameObject;

                cell.GetComponent<UILoseSBICell>().Init();
            }
            foreach (var tmp in dic)
            {
                if (loseSBILayout != null)
                {
                    GameObject cell = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/LoseStepBuyItemCell"), loseSBILayout.transform) as GameObject;

                    cell.GetComponent<UILoseSBICell>().Init(tmp.Key, tmp.Value);
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

    public void KeepButtonClick()
    {
        AudioManager.instance.ButtonClickAudio();

        // enough coin
        if (cost <= GameMainManager.Instance.playerData.coinNum)
        {
            AudioManager.instance.CoinPayAudio();

            //NetManager.instance.eliminateLevelFiveMore(cost, stepsbuyconfig.GetItemBagDicByTimes(times));
            GameMainManager.Instance.playerModel.BuyFiveMore(times);

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
            WindowManager.instance.Show<ShopPopupPlayWindow>();
        }
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void OnCloseClick()
    {
        //NetManager.instance.eliminateLevelEnd(LevelLoader.instance.level, 0, board.allstep, 0);
        GameMainManager.Instance.playerModel.EndLevel(LevelLoader.instance.level, false, board.allstep, 0);

        WindowManager.instance.Show<BeginPopupWindow>();
    }
}
