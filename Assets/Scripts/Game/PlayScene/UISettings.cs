﻿using March.Core.WindowManager;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public Board board;

    public void SettingsClick()
    {
        if (board.state == GAME_STATE.WAITING_USER_SWAP && board.lockSwap == false && board.moveLeft > 0)
        {
            AudioManager.instance.ButtonClickAudio();

            WindowManager.instance.Show<SettingPopupWindow>();

            board.state = GAME_STATE.OPENING_POPUP;
        }
    }
}
