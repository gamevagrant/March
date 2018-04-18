using UnityEngine;
using UnityEngine.UI;

public class PopupOpener : MonoBehaviour
{
    public GameObject popupPrefab;

    protected Canvas m_canvas;

	protected void Start()
	{
		m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
	}

	public virtual GameObject OpenPopup()
	{
		var popup = Instantiate(popupPrefab);
		popup.SetActive(true);
		popup.transform.localScale = Vector3.zero;

        // BEGIN_MECANIM_HACK
        // This works around a Mecanim bug present in Unity 5.2.1 where
        // the animation does not start until a frame after the prefab
        // has been instantiated. See:
        // http://forum.unity3d.com/threads/unity-5-2-mecanim-transitions-not-working-the-same-as-5-1.353815
#if UNITY_5_2_1
        var animator = popup.GetComponent<Animator>();
        animator.Update(0.01f);
#endif
        // END_MECANIM_HACK

        if (!m_canvas) m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

		popup.transform.SetParent(m_canvas.transform, false);
		popup.GetComponent<Popup>().Open();
		return popup;
	}
}
