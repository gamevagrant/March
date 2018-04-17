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

        PlayerData.instance.setCoinNum(PlayerData.instance.getCoinNum() + board.winGold);
        goldNum.text = board.winGold.ToString();
    }

    void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    public void BackToUI()
    {
        PlayerData.instance.setPlayScene(true);
        SceneManager.LoadScene("main");
    }
}
