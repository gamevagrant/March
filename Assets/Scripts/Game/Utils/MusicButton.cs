using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private bool m_on;
	public Image m_on_image;
	public Image m_off_image;

    private void Start()
    {
        m_on = PlayerPrefs.GetInt("music_on") == 1;
		if (!m_on)
			m_off_image.gameObject.SetActive (true);
		else
			m_on_image.gameObject.SetActive (true);
    }

    public void Toggle()
    {
        m_on = !m_on;
        var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        backgroundAudioSource.volume =  m_on ? 1 : 0;
        PlayerPrefs.SetInt("music_on", m_on ? 1 : 0);
		if (!m_on) {
			m_off_image.gameObject.SetActive (true);
			m_on_image.gameObject.SetActive (false);
		} else {
			m_off_image.gameObject.SetActive (false);
			m_on_image.gameObject.SetActive (true);
		}
    }
}
