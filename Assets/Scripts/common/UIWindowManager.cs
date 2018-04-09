using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Manager;
using UnityEngine;

public class UIWindowManager : Singleton<UIWindowManager> {
    public override void Init()
    {
        base.Init();
        DicUiWindow = new Dictionary<string, UIWindow>();
    }

    public Dictionary<string, UIWindow> DicUiWindow; 
    public T ShowUi<T>() where T : UIWindow
    {
        string tag = typeof (T).ToString();
        UIWindow window = GetUiWindow(tag);

        if (window != null)
        {
            if (window.IsShow)
            {
                return (T)window;//防止重复调用 onopen
            }
            window.OnOpen(false);
            HideDiffGroup(window.group);
            return (T)window;
        }
        //加载UI
        var obj = ResourceLoadManager.instance.SpawnUi<T>();
        obj.wndTag = tag;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.OnOpen(true);
        //处理隐藏关系
        HideDiffGroup(obj.group);
        DicUiWindow.Add(tag,obj);

        return obj;
    }

    public void HideUi<T>()
    {
        string tag = typeof(T).ToString();
        HideUi(tag);
    }
    public void HideUi(string tag)
    {
        UIWindow window = GetUiWindow(tag);
        if (window != null)
        {
            window.OnClose();
        }
    }
    public void HideDiffGroup(EmUIGroup group)
    {
        foreach (var window in DicUiWindow)
        {
            if (window.Value.IsShow && window.Value.group != group)
            {
                window.Value.OnClose();
            }
        }
    }
    public bool IsWindowOpen<T>() where T: UIWindow
    {
        var window = GetUiWindow<T>();
        if (window == null)
        {
            return false;
        }
        return window.IsShow;
    }
    public UIWindow GetUiWindow<T>() where T : UIWindow
    {
        string tag = typeof(T).ToString();
        if (DicUiWindow.ContainsKey(tag))
        {
            return DicUiWindow[tag];
        }
        return null;
    }
    public UIWindow GetUiWindow(string tag)
    {
        if (DicUiWindow.ContainsKey(tag))
        {
            return DicUiWindow[tag];
        }
        return null;
    }
   
    public void Update()
    {
        
    }

}
