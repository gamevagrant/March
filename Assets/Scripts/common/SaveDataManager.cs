using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using UnityEngine;
using LitJson;

    public enum SaveDataDefine
    {
        serverdata,
        storyid,
        heartTime,
        star,
        level,
        heart,
        gold,
        uid,
        name,
        items,
		isLogin,
        heartrecoveryLeftTime,
    }

    public class SaveDataManager:Singleton<SaveDataManager>
    {
        public override void Init()
        {
            base.Init();

        }

        public void SaveInt(SaveDataDefine key, int value)
        {
            PlayerPrefs.SetInt(key.ToString(), (int)value);
        }

        public void SaveString(SaveDataDefine key, string value)
        {
            PlayerPrefs.SetString(key.ToString(),value);
        }

        public void SaveFloat(SaveDataDefine key, string value)
        {
            PlayerPrefs.SetString(key.ToString(),value);
        }

        public int GetInt(SaveDataDefine key)
        {
            return PlayerPrefs.GetInt(key.ToString(),-1);
        }

        public float GetFloat(SaveDataDefine key)
        {
            return PlayerPrefs.GetFloat(key.ToString(),-1f);
        }
        public string GetString(SaveDataDefine key)
        {
            return PlayerPrefs.GetString(key.ToString());
        }

        public bool HasData(SaveDataDefine key)
        {
            return PlayerPrefs.HasKey(key.ToString());
        }

	public void saveFieldByKey(string key,string value)
	{
		if (PlayerData.instance.jsonObj == null)
			Debug.LogError ("本地数据实例化失败，无法存储数据＊＊＊＊＊＊＊＊");
		else
		{
            PlayerData.instance.jsonObj[key] = value;
            SaveDataManager.instance.SaveString(SaveDataDefine.serverdata, PlayerData.instance.jsonObj.ToJson());
			Debug.Log ("本地保存数据更新！！！"+SaveDataManager.instance.GetString (SaveDataDefine.serverdata));
		}
    }

    public void saveFieldByKey(string key, int value)
    {
        if (PlayerData.instance.jsonObj == null)
            Debug.LogError("本地数据实例化失败，无法存储数据＊＊＊＊＊＊＊＊");
        else
        {
            PlayerData.instance.jsonObj[key] = value;
            SaveDataManager.instance.SaveString(SaveDataDefine.serverdata, PlayerData.instance.jsonObj.ToJson());
            Debug.Log("本地保存数据更新！！！" + SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
        }
    }


    public void saveGoodsMap()
        {
            if (PlayerData.instance.jsonObj == null)
                Debug.LogError("本地数据实例化失败，无法存储数据＊＊＊＊＊＊＊＊");
            else
            {
                //PlayerData.instance.jsonObj[key] = value;
                Debug.Log(PlayerData.instance.jsonObj["items"]);

                JsonData array = new JsonData();
                array.SetJsonType(JsonType.Array);
                foreach (var tmp in PlayerData.instance.goodsMap)
                {
                    JsonData param = new JsonData();
                    param["itemId"] = tmp.Key;
                    param["count"] = int.Parse(tmp.Value);
                    param["uuid"] = "0";
                    param["vanishTime"] = 0;
                    array.Add(param);
                }
                PlayerData.instance.jsonObj["items"] = array;
            ;
                SaveDataManager.instance.SaveString(SaveDataDefine.serverdata, PlayerData.instance.jsonObj.ToJson());
                Debug.Log("本地保存数据更新！！！" + SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
            }
        }

}
