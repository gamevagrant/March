using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISettingsPopup : MonoBehaviour
{
    public void onCloseBtnClick()
    {
        AudioManager.instance.ButtonClickAudio();

        if (GameObject.Find("Board"))
        {
            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
        }

        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    public void onQuitBtnClick()
    {
        var board = GameObject.Find("Board").GetComponent<Board>();
        if (board.isFirstMove)
        {
            SceneManager.LoadScene("main");
        }
        else
        {
            WindowManager.instance.Show<SureQuitPopupWindow>();
        }
    }
}
