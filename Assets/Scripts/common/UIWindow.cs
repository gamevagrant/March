using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmUIRooot
{
    Root2D,
    Root3D
}

public enum EmUIGroup
{
    /// <summary>
    /// 登录界面
    /// </summary>
    LoginUI,
    /// <summary>
    /// 主界面UI
    /// </summary>
    MainUi,
}
public class UIWindow : MonoBehaviour
{


    /*uiState.m_nTag				= pUI->getTag();
    uiState.m_strName			= pUI->GetName();
    uiState.m_nTableID			= pData->nID;
	uiState.m_nOrder			= pData->nOrder;
	uiState.m_nGroup			= pData->nGroup;
	uiState.m_bTouchLock		= (bool)pData->nTouchLock;
	uiState.m_bShow				= false;
	uiState.m_strScript			= pData->szScript;
	uiState.m_strBGM			= pData->szBGMFile;
	uiState.m_bOpenReleaseRes	= pData->nOpenReleaseRes;
	uiState.m_bCloseReleaseRes	= pData->nCloseReleaseRes;*/
    public EmUIRooot root = EmUIRooot.Root2D;
    /// <summary>
    /// 窗口标签 为空的 UI不是用的UIwindow初始化的对象
    /// </summary>
    public string wndTag;
    /// <summary>
    /// 同组互斥关系
    /// </summary>
    public EmUIGroup group;
    [HideInInspector]
    /// <summary>
    /// 同组的顺序
    /// </summary>
    public int order;
    [HideInInspector]
    /// <summary>
    /// 关闭触摸
    /// </summary>
    public int lockTouch;

    public bool IsShow
    {
        get { return this.gameObject.activeInHierarchy; }
    }

    public UIWindow ParentWindow;
    public List<UIWindow> ChildWindow; 
    protected virtual void Awake()
    {
        ChildWindow = new List<UIWindow>();
    }

    protected virtual void Start()
    {
        
    }
    //todo  处理子界面
    public virtual void AddChild(UIWindow window)
    {
        ChildWindow.Add(window);
    }

    public virtual void RemoveChild(UIWindow window)
    {
        ChildWindow.Remove(window);
    }
    /// <summary>
    /// 打开UI使用
    /// </summary>
    /// <param name="isAdd"></param>
    public virtual void OnOpen(bool isAdd)
    {
        this.gameObject.SetActive(true);
        if (isAdd)
        {
            this.OnAdd();
        }
        this.OnShow();
    }
    /// <summary>
    /// 关闭UI使用
    /// </summary>
    public virtual void OnClose()
    {
        this.gameObject.SetActive(false);
        OnHide();
    }
    /// <summary>
    /// 资源回收调用
    /// </summary>
    public virtual void OnRelease()
    {
    }
    /// <summary>
    /// 首次加载
    /// </summary>
    public virtual void OnAdd()
    {
        
    }
    /// <summary>
    /// 打开后 显示调用
    /// </summary>
    public virtual void OnShow()
    {
        
    }
    /// <summary>
    /// 关闭时候调用
    /// </summary>
    public virtual void OnHide()
    {
    }
    public virtual void Destroy()
    {
        
    }

    public virtual void OnDestroy()
    {
        ChildWindow =null;
    }
   /* public virtual void Update()
    {
        
    }*/
}
