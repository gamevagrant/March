using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugInfor : MonoBehaviour
{
    public List<string> ChapterId;

    private GameObject contentPanel;
    private InputField uidInput;
    private InputField deviceIdInput;
    private InputField urlInput;
    private InputField infoInput;
    private Dropdown chapterDropdown;
    private Button chapterButton;

    private string playerdata;
    private string playerid;

    private void Awake()
    {
        contentPanel = transform.Find("DebugCanvas/Panel").gameObject;

        uidInput = contentPanel.transform.Find("Container1/InputField").GetComponent<InputField>();
        deviceIdInput = contentPanel.transform.Find("Container2/InputField").GetComponent<InputField>();
        urlInput = contentPanel.transform.Find("Container3/InputField").GetComponent<InputField>();
        infoInput = contentPanel.transform.Find("Container4/InputField").GetComponent<InputField>();
        chapterDropdown = contentPanel.transform.Find("Container5/Dropdown").GetComponent<Dropdown>();
        chapterButton = contentPanel.transform.Find("Container5/Button").GetComponent<Button>();

        chapterButton.onClick.AddListener(OnChapterClicked);
    }

    private void OnChapterClicked()
    {
        Utils.instance.StoryHeadId = ChapterId[chapterDropdown.value];
        SceneManager.LoadScene("Film");
    }

    public void OnDebugClicked()
    {
        playerid = qy.GameMainManager.Instance.playerData.userId;
        uidInput.text = playerid;

        playerdata = qy.GameMainManager.Instance.playerData.ToString();
        infoInput.text = playerdata;

        urlInput.text = Configure.instance.ServerUrl;

        deviceIdInput.text = Utils.instance.getDeviceID();

        contentPanel.SetActive(!contentPanel.activeSelf);

    }
}
