using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;
using UnityEngine.UI;
public class UIPropCell : MonoBehaviour {

    public Image img;
    public Text text;
    public GameObject complateImg;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetData(PropItem prop,int haveCount,bool isComplate = false)
    {
        string url = FilePathTools.GetPropItemIconPath(prop.icon);
        AssetsManager.Instance.LoadAssetAsync<Sprite>(url, (sp) =>
        {
            img.sprite = sp;
        });
        if(isComplate)
        {
            complateImg.SetActive(true);
            text.text = "";
        }else
        {
            complateImg.SetActive(false);
            string str = "";
            if(haveCount<prop.count)
            {
                str = string.Format("<color=red>{0}</color>/{1}", haveCount, prop.count);
            }else
            {
                str = haveCount.ToString() + "/" + prop.count.ToString();
            }
            text.text = str;
        }
       
    }
}
