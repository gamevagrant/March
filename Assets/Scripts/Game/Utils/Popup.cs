using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public bool AutoClose;
    public bool NeedBackground = true;

    public float Duration = 1f;

    public Color backgroundColor = new Color(10.0f/255.0f, 10.0f/255.0f, 10.0f/255.0f, 0.6f);

    private GameObject background;
    private bool isOpening;

	public void Open()
	{
	    if (isOpening)
	        return;

	    isOpening = true;

	    if (NeedBackground)
	    {
	        AddBackground();
	    }

        if (AutoClose)
            Invoke("Close", Duration);
	}

	public void Close()
	{
	    if (!isOpening)
	        return;

	    isOpening = false;

        var animator = GetComponent<Animator>();
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
			animator.Play("Close");

	    if (NeedBackground)
	    {
	        RemoveBackground();
	    }
		StartCoroutine(RunPopupDestroy());
	}

	private IEnumerator RunPopupDestroy()
    {
		yield return new WaitForSeconds(0.5f);
        if (background != null)
        {
            Destroy(background);
        }
		Destroy(gameObject);
	}

	private void AddBackground()
	{
		var bgTex = new Texture2D(1, 1);
		bgTex.SetPixel(0, 0, backgroundColor);
		bgTex.Apply();

		background = new GameObject("PopupBackground");
		var image = background.AddComponent<Image>();
		var rect = new Rect(0, 0, bgTex.width, bgTex.height);
		var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
		image.material.mainTexture = bgTex;
		image.sprite = sprite;
		var newColor = image.color;
		image.color = newColor;
		image.canvasRenderer.SetAlpha(0.0f);
		image.CrossFadeAlpha(1.0f, 0.4f, false);

        //var canvas = GameObject.Find("Canvas");
        var canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        background.transform.localScale = new Vector3(1, 1, 1);
        background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
		background.transform.SetParent(canvas.transform, false);
		background.transform.SetSiblingIndex(transform.GetSiblingIndex());
	}

	private void RemoveBackground()
	{
		var image = background.GetComponent<Image>();
		if (image != null)
			image.CrossFadeAlpha(0.0f, 0.2f, false);
	}
}
