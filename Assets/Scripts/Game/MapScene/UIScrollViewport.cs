using UnityEngine;
using System.Collections;

public class UIScrollViewport : MonoBehaviour 
{
	void Start () 
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(720f, 1280f, 0);	
	}	
}
