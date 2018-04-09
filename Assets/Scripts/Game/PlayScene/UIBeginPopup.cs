using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIBeginPopup : MonoBehaviour
{

    public Text HeadText;
    public Text NumText;
    public Text BeginBottomText;
    public Text BeginItemText;
    public GameObject BeginItemListLayout;
    public GameObject TargetListLayout;
	public BeginProp m_BeginProp;

	void Start ()
	{
        //todo:改为多语言
		HeadText.text = LanguageManager.instance.GetValueByKey("200014") + PlayerData.instance.getEliminateLevel().ToString();
		BeginBottomText.text = LanguageManager.instance.GetValueByKey ("200016");
        if (SceneManager.GetActiveScene().name == "Play")
	    {
			BeginBottomText.text = LanguageManager.instance.GetValueByKey ("210141");
	    }
		BeginItemText.text = LanguageManager.instance.GetValueByKey ("200017");

	    LevelLoader.instance.level = PlayerData.instance.getEliminateLevel();
	    LevelLoader.instance.LoadLevel();

	    if (TargetListLayout != null)
	    {
	        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
	        {
	            var tmp = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UITargetCell"), TargetListLayout.transform) as GameObject;
                tmp.GetComponent<UITargetCell>().Init(LevelLoader.instance.targetList[i].Type, LevelLoader.instance.targetList[i].Amount, LevelLoader.instance.targetList[i].color);
	        }
	    }
        LevelLoader.instance.beginItemList.Clear();

	    if (BeginItemListLayout != null)
	    {
			int[] itemids = {200004, 200003, 200005};
			foreach (int itemid in itemids) {
				var tmp = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/BeginProp"), BeginItemListLayout.transform) as GameObject;
				tmp.GetComponent<BeginProp>().Init(itemid);
			}
	    }
    }

    void Awake()
    {
    }

    public void onStartGameClick()
    {
        int eliminateHeartNum = 1;
#if UNITY_EDITOR

#else
        if (PlayerData.instance.getHeartNum() < eliminateHeartNum)
        {
            BeginBottomText.text = "心数不足！";
            return;
        }
#endif
        SceneManager.LoadScene("Play");
    }

    public void onCloseClick()
    {
        if (SceneManager.GetActiveScene().name != "main")
        {
            SceneManager.LoadScene("main");
        }
        Destroy(this.gameObject);
    }
}
