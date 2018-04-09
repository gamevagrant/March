using UnityEngine;

namespace March.Scene
{
    public interface ObjectListener
    {
        void OnNotify(GameObject go);
    }
}