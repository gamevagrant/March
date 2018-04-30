using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy;

public class UILoseSBICell : MonoBehaviour
{

    public Image itemIcon;
    public Text itemNum;

    public void Init(string itemId, int num)
    {
        itemIcon.gameObject.SetActive(true);
        //itemNum.gameObject.SetActive(true);

        //itemNum.text = num.ToString();
        //string icon = DefaultConfig.getInstance().GetConfigByType<item>().GetItemByID(itemId).icon;
        string icon = GameMainManager.Instance.configManager.propsConfig.GetItem(itemId).icon;
        itemIcon.sprite = Resources.Load(string.Format("Sprites/UI/{0}", icon), typeof(Sprite)) as Sprite;
		itemIcon.SetNativeSize ();
		if (icon == "item005" || icon == "item002") {
			itemIcon.GetComponent<Transform> ().localScale = new Vector3 (0.35f, 0.35f, 0.35f);
		}
    }

	public void Init()
	{
		itemIcon.gameObject.SetActive(true);
		//itemNum.gameObject.SetActive(true);
		itemIcon.sprite = Resources.Load("Sprites/UI/loseAdd", typeof(Sprite)) as Sprite;

		itemIcon.transform.localPosition = new Vector3 (20, 0, 0);
		itemIcon.SetNativeSize ();
	}
}
