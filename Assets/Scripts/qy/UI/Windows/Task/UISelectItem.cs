using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;
using UnityEngine.UI;

public class UISelectItem : MonoBehaviour {

    public SelectItem data;
    public Text text;
    public GameObject selectedTag;
    public Toggle toggle;


    public void SetData(SelectItem data)
    {
        this.data = data;
        text.text = data.name;

        bool isSelected = qy.GameMainManager.Instance.playerData.ContainsSelected(qy.GameMainManager.Instance.playerData.questId,data.id);
        selectedTag.SetActive(isSelected);
    }
}
