using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public bool IsOn;

	public Image m_on_image;
	public Image m_off_image;

    public void UpdateUI()
    {
        m_on_image.gameObject.SetActive(IsOn);
        m_off_image.gameObject.SetActive(!IsOn);
    }
}
