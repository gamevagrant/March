using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UITarget1 : MonoBehaviour
{
	public List<UITargetCell1> TargetCellList;

	public GameObject TargetLayout = null;

	void Start () 
	{
		if (TargetCellList == null)
		{
			TargetCellList = new List<UITargetCell1>();
		}
	}

	void Awake()
	{
		for (int i = 0; i < LevelLoader.instance.targetList.Count; i++)
		{
			var tmp = Instantiate(Resources.Load("Prefabs/PlayScene/Popup/UITargetCell1"), TargetLayout.transform) as GameObject;
			tmp.GetComponent<UITargetCell1>().Init(LevelLoader.instance.targetList[i].Type, LevelLoader.instance.targetList[i].Amount, LevelLoader.instance.targetList[i].color);

			TargetCellList.Add(tmp.GetComponent<UITargetCell1>());
		}
	}

	public void UpdateTargetAmount(int index)
	{
		if (index < TargetCellList.Count)
		{
			TargetCellList[index].Amount.text = (GameObject.Find("Board").GetComponent<Board>().targetLeftList[index]).ToString();
			if (int.Parse(TargetCellList[index].Amount.text) <= 0)
			{
				TargetCellList[index].Amount.gameObject.SetActive(false);
				TargetCellList[index].TargetTick.gameObject.SetActive(true);
			}
		}
	}
}
