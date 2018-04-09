// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_5_3

using UnityEngine.SceneManagement;

#endif

// This class is responsible for managing the transitions between scenes that are performed
// in the demo via a classic fade.
public class Transition : MonoBehaviour
{
    private static GameObject m_canvas;

    private GameObject m_overlay;

    private void Awake()
    {
        // Create a new, ad-hoc canvas that is not destroyed after loading the new scene
        // to more easily handle the fading code.
        m_canvas = new GameObject("TransitionCanvas");
        var canvas = m_canvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // add canvas scaler
        var scaler = m_canvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.referenceResolution = new Vector2(720f, 1280f);
        scaler.matchWidthOrHeight = 0.5f;

        DontDestroyOnLoad(m_canvas);
    }

    public static void LoadLevel(string level, float duration, Color color)
    {
        var fade = new GameObject("Transition");
        fade.AddComponent<Transition>();
        fade.GetComponent<Transition>().StartFade(level, duration, color);
        fade.transform.SetParent(m_canvas.transform, false);
        fade.transform.SetAsLastSibling();
    }

    private void StartFade(string level, float duration, Color fadeColor)
    {
        StartCoroutine(RunFade(level, duration, fadeColor));
    }

    // This coroutine performs the core work of fading out of the current scene
    // and into the new scene.
    private IEnumerator RunFade(string level, float duration, Color fadeColor)
    {
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, fadeColor);
        bgTex.Apply();

        m_overlay = new GameObject("Background Layer");
        var image = m_overlay.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);

        m_overlay.transform.localScale = new Vector3(1, 1, 1);
        //m_overlay.GetComponent<RectTransform>().sizeDelta = m_canvas.GetComponent<RectTransform>().sizeDelta;
        m_overlay.GetComponent<RectTransform>().sizeDelta = new Vector2(900f, 1400f);
        m_overlay.transform.SetParent(m_canvas.transform, false);
        m_overlay.transform.SetAsFirstSibling();

        // add loading image
        var loadingImage = Instantiate(Resources.Load(Configure.LoadingImage())) as GameObject;
        loadingImage.transform.SetParent(m_canvas.transform, false);
        // end loading image

        var time = 0.0f;
        var halfDuration = duration / 2.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            loadingImage.GetComponent<Image>().canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }

        image.canvasRenderer.SetAlpha(1.0f);
        yield return new WaitForEndOfFrame();

        //yield return new WaitForSeconds(100f);

#if UNITY_5_3
        SceneManager.LoadScene(level);
#else
        Application.LoadLevel(level);
#endif

        time = 0.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
            loadingImage.GetComponent<Image>().canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }

        image.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForEndOfFrame();

        Destroy(m_canvas);
    }
}
