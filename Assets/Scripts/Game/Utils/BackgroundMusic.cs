using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    private AudioSource m_audioSource;

	private void Awake()
    {
        //if (Instance != null)
        //{
        //    DestroyImmediate(gameObject);
        //}
        //else
        //{
        //    DontDestroyOnLoad(gameObject);
        //    Instance = this;
        //    m_audioSource = GetComponent<AudioSource>();
        //    m_audioSource.ignoreListenerVolume = true;
        //    m_audioSource.volume = PlayerPrefs.GetInt("music_on");
        //    AudioListener.volume = PlayerPrefs.GetInt("sound_on");
        //}

        Instance = this;
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.ignoreListenerVolume = true;
        m_audioSource.volume = PlayerPrefs.GetInt("music_on");
	}

    public void FadeIn()
    {
        if (PlayerPrefs.GetInt("music_on") == 1)
        {
            StartCoroutine(FadeAudio(1.0f, Fade.In));
        }
    }

    public void FadeOut()
    {
        if (PlayerPrefs.GetInt("music_on") == 1)
        {
            StartCoroutine(FadeAudio(1.0f, Fade.Out));
        }
    }

    enum Fade
    {
        In,
        Out
    }

    private IEnumerator FadeAudio(float time, Fade fadeType)
    {
        var start = fadeType == Fade.In ? 0.0f : 1.0f;
        var end = fadeType == Fade.In ? 1.0f : 0.0f;
        var i = 0.0f;
        var step = 1.0f/time;

        while (i <= 1.0f)
        {
            i += step * Time.deltaTime;
            m_audioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }
}
