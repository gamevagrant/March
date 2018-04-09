using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{


    private int _screenWith;
    private int _screenHeight;
    private int _screenHalfW;
    private int _screenHalfH;

    public int ScreenWith
    {
        get { return _screenWith; }
    }
    public int ScreenHeight
    {
        get { return _screenHeight; }
    }
    public int ScreenHalfWith
    {
        get { return _screenHalfW; }
    }
    public int ScreenHalfHeight
    {
        get { return _screenHalfH; }
    }

    protected override void Awake()
    {
        base.Awake();
        _screenWith = Screen.width;
        _screenHeight = Screen.height;
        _screenHalfW = _screenWith >> 1;
        _screenHalfH = _screenHeight >> 1;
        Application.runInBackground = true;
    }


    protected override void Init()
    {
        base.Init();
        UIWindowManager.CreateInstance();
      
    }

    // Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.Escape))
	    {
	        Application.Quit();
	    }
        UIWindowManager.instance.Update();
	}

   /* public void ShowMainView()
    {
        if (MainView != null)
        {
            UIPopupManager.instance.HidePopAll();
            MainView.gameObject.SetActive(true);
            UIPopupManager.instance.Push(MainView);
        }
    }*/
   /* public void ShowPiPei()
    {
        UIPopupManager.instance.HidePop();
        if (PiPeiView != null)
        {
            PiPeiView.gameObject.SetActive(true);
            UIPopupManager.instance.Push(PiPeiView);
        }
    }
*/
    /*public void ShowZhunBeiBattle()
    {
        if (ZhunBeiBattle != null)
        {
            ZhunBeiBattle.gameObject.SetActive(true);

        }
    }*/

    /*public void ShowMainBattle()
    {
        UIPopupManager.instance.HidePop();
        if (MainBattleView != null)
        {
            ViewPiPei.Hide();
            //补丁代码 
            if (MainView.gameObject.activeInHierarchy)
            {
                MainView.gameObject.SetActive(false);
            }
            MainBattleView.gameObject.SetActive(true);
            MainBattleView.ResetView();
            UIPopupManager.instance.Push(MainBattleView);
        }
    }*/

   /* public void ShowZhanDouJieSuan()
    {
        //UIPopupManager.instance.HidePop();
        if (ZhanDouJieSuanView != null)
        {
            ZhanDouJieSuanView.gameObject.SetActive(true);
            ZhanDouJieSuanView.init();
        }
    }*/
    /// <summary>
    /// 不用关闭主界面
    /// </summary>
    /*public void ShowHeroChose()
    {
        if (ChoseHeroView != null)
        {
            ChoseHeroView.gameObject.SetActive(true);
        }
    }*/
    /// <summary>
    /// 英雄图鉴
    /// </summary>
    /*public void ShowHeroMap()
    {
        if (HeroMapView != null)
        {
            HeroMapView.gameObject.SetActive(true);
            HeroMapView.Init();

        }
    }*/

   /* public void ShowAchievementView()
    {
        if (null != AchievementView)
        {
            AchievementView.gameObject.SetActive(true);
            AchievementView.init();
        }

    }*/

    public void ShowNotOpen()
    {
        Debug.LogError("暂未开放！");
    }

    private Dictionary<Vector3, short> dicHudPosition; 

    public T ShowWindow<T>() where T : UIWindow
    {
        var view = UIWindowManager.instance.ShowUi<T>();
        return view;
    }

    public void HideWindow<T>() where T : UIWindow
    {
        UIWindowManager.instance.HideUi<T>();
    }

    public void HideWindow(string tag)
    {
        UIWindowManager.instance.HideUi(tag);
    }
    public T GetWindow<T>() where T : UIWindow
    {
        return (T)UIWindowManager.instance.GetUiWindow<T>();
    }
    public bool IsWindowOpen<T>() where T:UIWindow
    {
        return UIWindowManager.instance.IsWindowOpen<T>();
    }

    public void RemoveUI<T>() where T : Component
    {
        //todo
    }
}
