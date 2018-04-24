using March.Core.WindowManager;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoosePanel : MonoBehaviour
{
    private InputField input;

    void Awake()
    {
        input = transform.Find("InputField").GetComponent<InputField>();
        input.text = PlayerData.instance.getEliminateLevel().ToString();
    }

    public void OnOkayButtonClick()
    {
        var num = Convert.ToInt32(input.text);
        PlayerData.instance.setEliminateLevel(num);

        WindowManager.instance.Show<BeginPopupWindow>();
    }

    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }
}
