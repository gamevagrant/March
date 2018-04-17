using UnityEngine;

public class Window : MonoBehaviour
{
    public virtual void Open()
    {
        Debug.Log(string.Format("Window-{0}is opened.", name));
    }

    public virtual void Close()
    {
        Debug.Log(string.Format("Window-{0}is closed.", name));
    }
}
