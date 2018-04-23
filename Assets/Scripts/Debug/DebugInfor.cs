using UnityEngine;
using UnityEngine.UI;

public class DebugInfor : MonoBehaviour
{
    private GameObject contentPanel;
    private InputField uidInput;
    private InputField deviceIdInput;
    private InputField urlInput;
    private InputField infoInput;

    private string playerdata;
    private string playerid;

    private void Awake()
    {
		contentPanel = transform.Find("DebugCanvas/Panel").gameObject;

        uidInput = contentPanel.transform.Find("Container1/InputField").GetComponent<InputField>();
        deviceIdInput = contentPanel.transform.Find("Container2/InputField").GetComponent<InputField>();
        urlInput = contentPanel.transform.Find("Container3/InputField").GetComponent<InputField>();
        infoInput = contentPanel.transform.Find("Container4/InputField").GetComponent<InputField>();
    }

    public void OnDebugClicked()
    {
        playerid = PlayerData.instance.userId;
        uidInput.text = playerid;

        playerdata = PlayerData.instance.ToString();
        infoInput.text = playerdata;

        urlInput.text = Configure.instance.ServerUrl;

        deviceIdInput.text = Utils.instance.getDeviceID();

        contentPanel.SetActive(!contentPanel.activeSelf);
    }
}
