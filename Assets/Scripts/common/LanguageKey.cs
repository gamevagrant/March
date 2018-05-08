using UnityEngine;
using UnityEngine.UI;

public class LanguageKey : MonoBehaviour
{
    void Start()
    {
        var text = GetComponent<Text>();

        if (text == null)
            return;

        if (!string.IsNullOrEmpty(text.text))
        {
            text.text = LanguageManager.instance.GetValueByKey(text.text);
        }
    }
}
