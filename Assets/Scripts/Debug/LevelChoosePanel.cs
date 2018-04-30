using March.Core.WindowManager;
using System;
using qy;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoosePanel : MonoBehaviour
{
    private InputField input;

    void Awake()
    {
        input = transform.Find("InputField").GetComponent<InputField>();
        input.text = GameMainManager.Instance.playerData.eliminateLevel.ToString();
    }

    public void OnOkayButtonClick()
    {
        var num = Convert.ToInt32(input.text);
        GameMainManager.Instance.playerData.eliminateLevel = num;

        WindowManager.instance.Show<BeginPopupWindow>();
    }

    public void OnCloseButtonClick()
    {
        Destroy(gameObject);
    }
}
