using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Messenger
{
    private static Dictionary<int, Delegate> eventTable = new Dictionary<int, Delegate>();

    #region ///addlistener

    public static void AddListener(ELocalMsgID msgid, Callback listener)
    {
        OnListenerAdding((int)msgid, listener);
        eventTable[(int)msgid] = (Callback)eventTable[(int)msgid] + listener;
    }

    public static void AddListener<T>(ELocalMsgID id, Callback<T> listener)
    {
        OnListenerAdding((int)id, listener);
        eventTable[(int)id] = (Callback<T>)eventTable[(int)id] + listener;
    }


    public static void AddListener<T, U>(ELocalMsgID id, Callback<T, U> listener)
    {
        OnListenerAdding((int)id, listener);
        eventTable[(int)id] = (Callback<T, U>)eventTable[(int)id] + listener;
    }
    #endregion

   

    public static void RemoveListener(ELocalMsgID id, Callback listener)
    {
        eventTable[(int)id] = (Callback)eventTable[(int)id] - listener;
        OnListenerRemoveing((int)id);
    }


    public static void RemoveListener<T>(ELocalMsgID id, Callback<T> listener)
    {
        eventTable[(int)id] = (Callback<T>)eventTable[(int)id] - listener;
        OnListenerRemoveing((int)id);
    }

    

    public static void RemoveListener<T, U>(ELocalMsgID id, Callback<T, U> listener)
    {
        eventTable[(int)id] = (Callback<T, U>)eventTable[(int)id] - listener;
        OnListenerRemoveing((int)id);
    }

 
    public static void Broadcast(ELocalMsgID id)
    {
        Broadcast((int)id);
    }

   
    public static void Broadcast(int id)
    {
        OnListenerBroadcasting(id);
        Delegate d;
        if (eventTable.TryGetValue(id, out d))
        {
            Callback callback = d as Callback;
            if (callback != null)
            {
                callback();
            }
            else
            {
                throw new Exception(string.Format("broadcast error callback is null,please check eventtable id:{0}", id));
            }
        }
    }


    public static void Broadcast<T>(ELocalMsgID id, T arg1)
    {
        Broadcast((int)id, arg1);
    }

   
    public static void Broadcast<T>(int id, T arg1)
    {
        OnListenerBroadcasting(id);
        Delegate d;
        if (eventTable.TryGetValue(id, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(arg1);
            }
            else
            {
                throw new Exception(string.Format("broadcast error callback is null,please check eventtable id:{0}", id));
            }
        }
    }

    public static void Broadcast<T, U>(ELocalMsgID id, T arg1, U arg2)
    {
        Broadcast((int)id, arg1, arg2);
    }

   

    public static void Broadcast<T, U>(int id, T arg1, U arg2)
    {
        OnListenerBroadcasting((int)id);
        Delegate d;
        if (eventTable.TryGetValue((int)id, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("broadcast error callback is null,please check eventtable id:{0}", id));
            }
        }
    }


    private static void OnListenerAdding(int id, Delegate listener)
    {
        if (!eventTable.ContainsKey(id))
        {
            eventTable.Add(id, null);
        }
    }

    private static void OnListenerRemoveing(int id)
    {
        if (eventTable.ContainsKey(id))
        {
            if (eventTable[id] == null)
            {
                eventTable.Remove(id);
            }
        }
        else
        {
            throw new Exception(string.Format("OnListenerRemoveing null id = {0}", id));
        }
    }

    private static void OnListenerBroadcasting(int id)
    {
        if (!eventTable.ContainsKey(id))
        {
#if UNITY_EDITOR
           Debug.LogWarning(string.Format("broadcasting message is not found id:{0}", id));
#endif
        }
    }
}
