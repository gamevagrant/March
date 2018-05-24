using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimation : MonoBehaviour {

    private Image ImageSource;
    private RectTransform rectTransform;
    private int mCurFrame = 0;
    private float progress = 0;
    public float FPS = 5;
    public List<Sprite> SpriteFrames;
    public bool IsPlaying = false;
    public bool AutoPlay = false;
    public bool Loop = false;

    public int FrameCount
    {
        get
        {
            return SpriteFrames.Count;
        }
    }
    void Awake()
    {
        ImageSource = GetComponent<Image>();
        rectTransform = transform as RectTransform;
    }

    void OnEnable()
    {
        if (AutoPlay)
        {
            Play();
        }
        else
        {
            IsPlaying = false;
        }
    }

    void Update()
    {
        if (!IsPlaying || 0 == FrameCount || FPS == 0)
        {
            return;
        }

        float delta = 1f / FPS;//每帧间隔
        progress += Time.deltaTime;
        if(Loop)
        {
            progress = progress % (SpriteFrames.Count * delta);
        }
        int index = Mathf.FloorToInt(progress / delta);
        if(index < SpriteFrames.Count)
        {
            mCurFrame = index;
        }else
        {
            Stop();
            return;
        }
        SetSprite(mCurFrame);
        
    }
    public void Play()
    {
        if (!gameObject.activeInHierarchy)
            return;
        SetSprite(mCurFrame);
        IsPlaying = true;
    }
    public void Stop()
    {
        if (!gameObject.activeInHierarchy)
            return;
        mCurFrame = 0;
        progress = 0;
        SetSprite(mCurFrame);
        IsPlaying = false;
    }
    public void Pause()
    {
        if (!gameObject.activeInHierarchy)
            return;
        IsPlaying = false;
    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        if (rectTransform.anchorMin == rectTransform.anchorMax)
        {
            ImageSource.SetNativeSize();
        }

    }
}
