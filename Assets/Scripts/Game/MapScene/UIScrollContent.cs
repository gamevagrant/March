using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIScrollContent : MonoBehaviour 
{
    public List<RawImage> images = new List<RawImage>();

    /*
     * This function auto set the Viewport content size base on the number of the background
     */
	void Start () 
    {
        float height = 0;

	    foreach (var image in images)
        {
            RectTransform rt = image.rectTransform;

            height += rt.rect.height;
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(720f, height + 400f, 0);
	}
}
