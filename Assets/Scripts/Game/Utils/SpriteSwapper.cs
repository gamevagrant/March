using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
	public Sprite enabledSprite;
	public Sprite disabledSprite;

	private bool m_swapped = true;

	private Image m_image;

	public void Awake()
	{
		m_image = GetComponent<Image>();
	}

	public void SwapSprite()
	{
		if (m_swapped)
		{
			m_swapped = false;
			m_image.sprite = disabledSprite;
		}
		else
		{
			m_swapped = true;
			m_image.sprite = enabledSprite;
		}
	}

    public bool IsEnabled()
    {
        if (m_swapped == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
