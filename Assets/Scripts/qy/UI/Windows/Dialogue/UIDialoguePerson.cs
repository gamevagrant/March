using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIDialoguePerson : MonoBehaviour
{

    public SpriteAnimation mouth;

    // Use this for initialization
    private void Awake()
    {
        mouth.Loop = true;
        mouth.AutoPlay = false;
        GetComponent<Image>().material.shader = Shader.Find("Custom/Flutter");
    }
    public void StartTalk()
    {
        mouth.Play();
    }

    public void StopTalk()
    {
        mouth.Stop();
    }

    public void Show()
    {
        //img.DOColor(Color.white, 0.5f);
        transform.DOScale(1.1f, 0.5f);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        //img.DOFade(0.5f, 0.5f);
        transform.DOScale(0.9f, 0.5f);
        //img.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.5f);
    }
}
