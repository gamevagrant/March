using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;
using UnityEngine.UI;
public class UIPropCell : MonoBehaviour {

    public Image img;
    public Text text;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetData(PropItem prop,int haveCount)
    {
        string url = FilePathTools.GetPropItemIconPath(prop.icon);
        AssetsManager.Instance.LoadAssetAsync<Sprite>(url, (sp) =>
        {
            img.sprite = sp;
        });
        text.text =  haveCount.ToString() + "/" + prop.count.ToString();
    }
}
