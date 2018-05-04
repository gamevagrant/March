using UnityEngine;
using UnityEngine.UI;
namespace qy.ui
{
    public interface IUIManager
    {

        UIWindowBase curWindow { get; }
        RectTransform root { get; }
        bool isWaiting { get; set; }

        void OpenWindow(UISettings.UIWindowID id, bool needTransform, System.Action onComplate, params object[] data);
        void OpenWindow(UISettings.UIWindowID id, bool needTransform, params object[] data);
        void OpenWindow(UISettings.UIWindowID id, params object[] data);

        void CloseWindow(UISettings.UIWindowID id, bool needTransform = true, System.Action onComplate = null);
        void CloseWindow(UISettings.UIWindowID id);

        void EnableOperation();
        void DisableOperation();
    }
}


