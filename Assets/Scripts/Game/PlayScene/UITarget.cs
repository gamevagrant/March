using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UITarget : MonoBehaviour
{
    public List<UITargetCell> TargetCellList;

    public GameObject TargetLayout = null;

	void Start () 
    {
        if (TargetCellList == null)
        {
            TargetCellList = new List<UITargetCell>();
        }
	}

    void Awake()
    {
        for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
        {
            var go = Instantiate(Resources.Load(Configure.UITargetCellPrefab), TargetLayout.transform) as GameObject;
            go.GetComponent<UITargetCell>().Init(LevelLoader.instance.targetList[i].Type, LevelLoader.instance.targetList[i].Amount, LevelLoader.instance.targetList[i].color);

            TargetCellList.Add(go.GetComponent<UITargetCell>());
        }
    }

    public void UpdateTargetAmount(int index)
    {
        if (index < TargetCellList.Count)
        {
            TargetCellList[index].Amount.text = GameObject.Find("Board").GetComponent<Board>().targetLeftList[index].ToString();
            if (int.Parse(TargetCellList[index].Amount.text) <= 0)
            {
                TargetCellList[index].Amount.gameObject.SetActive(false);
                TargetCellList[index].TargetTick.gameObject.SetActive(true);
            }
        }
    }
}
