using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISettingsPopup : MonoBehaviour
{
    private SoundButton musicButton;
    private SoundButton soundButton;

    void Awake()
    {
        musicButton = transform.Find("ButtonPanel/MusicButton").GetComponent<SoundButton>();
        soundButton = transform.Find("ButtonPanel/SoundButton").GetComponent<SoundButton>();
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

        if (GameObject.Find("Board"))
        {
            GameObject.Find("Board").GetComponent<Board>().state = GAME_STATE.WAITING_USER_SWAP;
        }

        gameObject.SetActive(false);
    }

    public void OnQuitBtnClick()
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
