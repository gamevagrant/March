using March.Core.WindowManager;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    private Board board;

    private GameObject settingPopup;

    private void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    public void SettingsClick()
    {
        if (board.state == GAME_STATE.WAITING_USER_SWAP && board.lockSwap == false && board.moveLeft > 0)
        {
            AudioManager.instance.ButtonClickAudio();

            if (settingPopup == null)
            {
                settingPopup = WindowManager.instance.Show<SettingPopupWindow>().gameObject;
            }
            else
            {
                settingPopup.SetActive(true);
            }

            board.state = GAME_STATE.OPENING_POPUP;
        }
    }
}
