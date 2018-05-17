using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
public class AutoTestEditor :MonoBehaviour{

    private static AutoTestNetWork autoTestNetWork;

    [MenuItem("Tools/Test/Init Auto Test Script")]
    public static void LoadAutoTestScript()
    {
        if (autoTestNetWork == null)
        {
            Debug.Log("---初始化自动测试脚本---");
            autoTestNetWork = AutoTestNetWork.Instance;
            
        }
        //CallMethod("Login",new object[] { new qy.LoginInfo(), null });
    }

    [MenuItem("Tools/Test/Play Auto Test")]
    public static void PlayAutoTest()
    {
        LoadAutoTestScript();
        Debug.Log("---开始自动化测试---");
        autoTestNetWork.PlayAutoTest();
       
    }
    [MenuItem("Tools/Test/Start Record Data")]
    public static void StartRecordData()
    {
        LoadAutoTestScript();
        Debug.Log("---开始记录---");
        autoTestNetWork.StartRecordData();
    }
    [MenuItem("Tools/Test/Stop Record Data")]
    public static void StopRecordData()
    {
        LoadAutoTestScript();
        Debug.Log("---结束记录---");
        autoTestNetWork.StopRecordData();
    }
    [MenuItem("Tools/Test/Clear Auto Test Data")]
    public static void ClearAutoTestData()
    {
        if (EditorUtility.DisplayDialog("删除自动化测试数据","确定删除自动测试数据？\n此数据很重要并且无法恢复", "删除", "取消"))
        {
            Debug.Log("---清除已经记录的数据---");
            qy.LocalDatasManager.callMethodList = null;
        }
        
    }
    [MenuItem("Tools/Test/Clear Other Data")]
    public static void ClearOtherData()
    {
        Debug.Log("---清除其他数据---");
        List<CallMethodInfo> data = qy.LocalDatasManager.callMethodList;
        PlayerPrefs.DeleteAll();
        qy.LocalDatasManager.callMethodList = data;
    }
   

}
