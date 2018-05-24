using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace qy
{
    public class LocalDatasManager
    {

        /// <summary>
        /// 使用帐号登录过的用户数据
        /// </summary>
        public static PlayerData playerData
        {
            get
            {
                return GetData<PlayerData>("PLAYER_DATA")??new PlayerData();
            }
            set
            {
                SaveData("PLAYER_DATA", value);
            }
        }
        /// <summary>
        /// 展示过的新手引导
        /// </summary>
        public static Dictionary<string, string> displayedGuides
        {
            get
            {
                return GetData<Dictionary<string, string>>("SHOWED_GUIDE")??new Dictionary<string, string>();
            }
            set
            {
                SaveData("SHOWED_GUIDE", value);
            }
        }

        /// <summary>
        /// 邀请过的好友
        /// </summary>
        public static Dictionary<string, string> invitedFriends
        {
            get
            {
                return GetData<Dictionary<string, string>>("INVITED_FRIENDS");
            }
            set
            {
                SaveData("INVITED_FRIENDS", value);
            }
        }
        /// <summary>
        /// 召回过的好友
        /// </summary>
        public static Dictionary<string, string> callbackedFriends
        {
            get
            {
                return GetData<Dictionary<string, string>>("CALLBACKED_FRIENDS");
            }
            set
            {
                SaveData("CALLBACKED_FRIENDS", value);
            }
        }
        /// <summary>
        /// 网络请求缓冲队列
        /// </summary>
        public static List<Dictionary<string, object>> netBufferQueue
        {
            get
            {
                return GetData<List<Dictionary<string, object>>>("NET_BUFFER_QUEUE")??new List<Dictionary<string, object>>();
            }set
            {
                SaveData("NET_BUFFER_QUEUE", value);
            }
        }

        public static List<CallMethodInfo> callMethodList
        {
            get
            {
                return GetData<List<CallMethodInfo>>("CALL_METHOD_LIST") ?? new List<CallMethodInfo>();
            }
            set
            {
                SaveData("CALL_METHOD_LIST", value);
            }
        }


        private static T GetData<T>(string name)
        {
            string json = PlayerPrefs.GetString(name);
            T obj = LitJson.JsonMapper.ToObject<T>(json);
            return obj;
        }

        private static void SaveData(string name, System.Object obj)
        {
            if (obj != null)
            {
                string json = LitJson.JsonMapper.ToJson(obj);
                PlayerPrefs.SetString(name, json);
            }
            else
            {
                PlayerPrefs.DeleteKey(name);
            }

        }
    }

}
