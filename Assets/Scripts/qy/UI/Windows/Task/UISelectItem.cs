using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;
using UnityEngine.UI;

public class UISelectItem : MonoBehaviour {

    public SelectItem data;
    public Text text;
    public Toggle toggle;


    public void SetData(SelectItem data)
    {
        this.data = data;
        text.text = data.name;
    }
}
