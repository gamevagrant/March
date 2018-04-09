using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect aotu add SortingLayer
public class EffectAddSortingLayer : MonoBehaviour {

	void Awake(){
		foreach (Transform child in GetComponentsInChildren<Transform>())  
		{   
			child.GetComponent<Renderer> ().sortingLayerName = "Effect";
		}
	}
}
