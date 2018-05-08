using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISettingsPopup : MonoBehaviour
{
    private SoundButton musicButton;
    private SoundButton soundButton;

    private Board board;

    void Awake()
    {
        musicButton = transform.Find("ButtonPanel/MusicButton").GetComponent<SoundButton>();
        soundButton = transform.Find("ButtonPanel/SoundButton").GetComponent<SoundButton>();

        board = GameObject.Find("Board").GetComponent<Board>();
    }

    void Start()
    {
        musicButton.IsOn = Configure.instance.MusicOn;
        musicButton.UpdateUI();
        soundButton.IsOn = Configure.instance.SoundOn;
        soundButton.UpdateUI();
    }

    public void OnMusicButtonClicked()
    {
        Configure.instance.MusicOn = !Configure.instance.MusicOn;
        musicButton.IsOn = Configure.instance.MusicOn;
        musicButton.UpdateUI();
    }

    public void OnSoundButtonClicked()
    {
        Configure.instance.SoundOn = !Configure.instance.SoundOn;
        soundButton.IsOn = Configure.instance.SoundOn;
        soundButton.UpdateUI();
    }

    public void OnCloseBtnClick()
    {
        AudioManager.instance.ButtonClickAudio();

        if (board != null)
        {
            board.state = GAME_STATE.WAITING_USER_SWAP;
        }

        gameObject.SetActive(false);
    }

    public void OnQuitBtnClick()
    {
        if (board != null && board.isFirstMove)
        {
            SceneManager.LoadScene("main");
        }
        else
        {
            WindowManager.instance.Show<SureQuitPopupWindow>();
        }
    }
}
