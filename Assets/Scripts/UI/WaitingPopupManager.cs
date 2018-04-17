using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPopupManager : Singleton<WaitingPopupManager>
{
    private Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

    private GameObject m_waitingPopup = null;
    private GameObject m_waitingIcon = null;

    public void show(GameObject parent)
    {
        if (m_waitingPopup == null)
        {
            AddBackground(parent);
            m_waitingIcon = new GameObject("WaitingIcon");
            m_waitingIcon.AddComponent<Image>().sprite = Resources.Load("Sprites/UI/loading_01", typeof(Sprite)) as Sprite;
            m_waitingIcon.transform.SetParent(m_waitingPopup.transform,false);
            m_waitingIcon.transform.DOLocalRotate(new Vector3(0, 0, 360f), 3f,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Incremental);
        }
        else
        {
            m_waitingPopup.transform.parent = parent.transform;
        }
        m_waitingPopup.SetActive(true);
    }

    public void close()
    {
        if (m_waitingPopup != null)
        {
            GameObject.Destroy(m_waitingPopup.gameObject);
            m_waitingPopup = null;
        }
    }

    private void AddBackground(GameObject parent)
    {
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, backgroundColor);
        bgTex.Apply();

        m_waitingPopup = new GameObject("WaitingPopup");
        var image = m_waitingPopup.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);
        image.CrossFadeAlpha(1.0f, 0.4f, false);

        m_waitingPopup.transform.localScale = new Vector3(1, 1, 1);
        if (parent.GetComponent<RectTransform>() != null)
        {
            m_waitingPopup.GetComponent<RectTransform>().sizeDelta = parent.GetComponent<RectTransform>().sizeDelta;
        }
        m_waitingPopup.transform.SetParent(parent.transform, false);
        //m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

}
