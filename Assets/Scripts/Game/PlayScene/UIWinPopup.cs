using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWinPopup : MonoBehaviour
{
    public Text topTxet;
    public Text goldNum;
    public Text bottomText;
    public Board board;

    void Start()
    {
        topTxet.text = LanguageManager.instance.GetValueByKey("210135");
		bottomText.text = LanguageManager.instance.GetValueByKey ("200020");
        //上传结算数据
        NetManager.instance.eliminateLevelEnd(LevelLoader.instance.level, 1, board.allstep, board.winGold - board.minWinGold);

        qy.GameMainManager.Instance.playerData.coinNum += board.winGold;
        goldNum.text = board.winGold.ToString();
    }

    void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    public void BackToUI()
    {
        qy.GameMainManager.Instance.playerData.isPlayScene = true;
        SceneManager.LoadScene("main");
    }
}
