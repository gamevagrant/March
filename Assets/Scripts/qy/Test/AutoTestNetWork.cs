#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

public class AutoTestNetWork:MonoBehaviour {

    private static AutoTestNetWork _instance;
    public static AutoTestNetWork Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = GameObject.Find("MainManager");

                if (go == null)
                {
                    go = new GameObject("MainManager");
                }
                GameObject.DontDestroyOnLoad(go);
                go.AddComponent<AutoTestNetWork>();
            }
            return _instance;
        }
    }
    private List<CallMethodInfo> callList = new List<CallMethodInfo>();
    public Queue<CallMethodInfo> queue;
    private void Awake()
    {
        _instance = this;
        callList = qy.LocalDatasManager.callMethodList;
        Debug.Log("[AutoTest][数据量]" + callList.Count);
    }
    private void Update()
    {
        if (queue != null && queue.Count > 0)
        {
            CallMethodInfo info = queue.Dequeue();
            string parametersStr = "";
            foreach (object obj in info.parameters)
            {
                parametersStr += " " + obj.ToString();
            }
            Debug.Log("[AutoTest][自动化执行"+queue.Count+"]" + info.name + " | " + parametersStr);

            CallMethod(info.name, info.parameters);
            if(queue.Count==0)
            {
                Debug.Log("[AutoTest][执行完毕]");
            }
        }
    }
    private void OnDestroy()
    {

    }
    public void PlayAutoTest()
    {
        //callList = qy.LocalDatasManager.callMethodList;
        _instance.queue = new Queue<CallMethodInfo>(callList);
    }

    public void StartRecordData()
    {
        Messenger.AddListener<CallMethodInfo>(ELocalMsgID.CallPlayerModel, OnCallPlayerModelHandle);
        //callList = qy.LocalDatasManager.callMethodList;
    }
    public void StopRecordData()
    {
        Messenger.RemoveListener<CallMethodInfo>(ELocalMsgID.CallPlayerModel, OnCallPlayerModelHandle);
        qy.LocalDatasManager.callMethodList = callList;
    }


    private void OnCallPlayerModelHandle(CallMethodInfo info)
    {
        callList.Add(info);
        string parametersStr = "";
        foreach (object obj in info.parameters)
        {
            parametersStr += " " + obj.ToString();
        }
        Debug.Log("[AutoTest][收录数据]" + info.name + " | " + parametersStr);
    }

    private static void CallMethod(string name, object[] parameters)
    {
        Type[] parameterTypes = new Type[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameterTypes[i] = parameters[i] != null ? parameters[i].GetType() : null;
        }
        Type type = qy.net.NetManager.Instance.GetType();

        //MethodInfo mi = GetGenericMethod(typeof(qy.PlayerModel), name, BindingFlags.Public | BindingFlags.Instance, parameterTypes);
        MethodInfo mi = typeof(qy.PlayerModel).GetMethod(name, BindingFlags.Public | BindingFlags.Instance);
        object obj = mi.Invoke(qy.GameMainManager.Instance.playerModel, parameters);
        Debug.Log(obj);

    }

    public static MethodInfo GetGenericMethod(Type targetType, string name, BindingFlags flags, params Type[] parameterTypes)
    {

        var methods = targetType.GetMethods(flags).Where(m => m.Name == name && m.IsPublic);
        foreach (MethodInfo method in methods)
        {
            bool founded = true;
            var parameters = method.GetParameters();
            if (parameters.Length != parameterTypes.Length)
                continue;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameterTypes[i] != null && parameters[i].ParameterType != parameterTypes[i])
                {
                    founded = false;
                    break;
                }

            }
            if (founded)
            {
                return method;
            }

        }
        return null;
    }




}
#endif

public class CallMethodInfo
{
    public string name;
    public object[] parameters;
}

