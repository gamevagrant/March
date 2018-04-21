using UnityEngine;

public class UISettings : MonoBehaviour 
{
    public Board board;

    private UISettingsPopup settingPopup;

    public void SettingsClick()
    {
        if (board.state == GAME_STATE.WAITING_USER_SWAP && board.lockSwap == false && board.moveLeft > 0)
        {
            AudioManager.instance.ButtonClickAudio();

            // cache for better performance.
            if (settingPopup == null)
            {
                settingPopup = (Instantiate(Resources.Load("Prefabs/PlayScene/Popup/settingpopup"), transform.parent) as GameObject).GetComponent<UISettingsPopup>();
            }

            settingPopup.gameObject.SetActive(true);

            board.state = GAME_STATE.OPENING_POPUP;

            // ads
        }
    }

    public void OnTouchLayerClicked()
    {
        if (settingPopup != null && settingPopup.gameObject.activeSelf)
        {
            settingPopup.OnCloseBtnClick();
        }
    }
}
