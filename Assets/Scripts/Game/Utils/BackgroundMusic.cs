using Assets.Scripts.Common;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoSingleton<BackgroundMusic>
{
    public List<AudioClip> ClipLevelList;
    public float FadeDuration = 1f;

    private AudioSource fadeOutAudioSource;
    private AudioSource fadeInAudioSource;

    private int lastLevel = -1;

    protected override void Init()
    {
        base.Init();

        fadeOutAudioSource = CreateAudioSource();
        fadeInAudioSource = CreateAudioSource();
    }

    public void Start()
    {
        OnSoundChange();

        Configure.instance.OnSoundChange += OnSoundChange;
    }

    private AudioSource CreateAudioSource()
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.ignoreListenerVolume = true;
        source.loop = true;
        return source;
    }

    public void OnSoundChange()
    {
        fadeOutAudioSource.enabled = Configure.instance.SoundOn;
        fadeInAudioSource.enabled = Configure.instance.SoundOn;
    }

#if UNITY_5_4_OR_NEWER
    protected override void sceneLoadedHandler(Scene scene, LoadSceneMode mode)
    {
        base.sceneLoadedHandler(scene, mode);

        OnLoad(scene.buildIndex);
    }
#else
    void OnLevelWasLoaded(int level)
	{
        OnLoad(level);
	}
#endif

    private void OnLoad(int level)
    {
        if (lastLevel == level)
        {
            return;
        }

        if (lastLevel != -1)
        {
            if (ClipLevelList[lastLevel] == ClipLevelList[level])
            {
                return;
            }

            fadeOutAudioSource.clip = ClipLevelList[lastLevel];
            fadeOutAudioSource.volume = 1f;
            fadeOutAudioSource.Play();
            fadeOutAudioSource.DOFade(0f, FadeDuration).OnComplete(() =>
            {
            });
        }

        fadeInAudioSource.clip = ClipLevelList[level];
        fadeInAudioSource.volume = 0f;
        fadeInAudioSource.Play();
        fadeInAudioSource.DOFade(1f, FadeDuration).OnComplete(() =>
        {
        });

        lastLevel = level;
    }
}
