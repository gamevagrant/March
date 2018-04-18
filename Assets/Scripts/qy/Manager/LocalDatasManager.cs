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
                return GetData<PlayerData>("PLAYER_DATA");
            }
            set
            {
                SaveData("PLAYER_DATA", value);
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
