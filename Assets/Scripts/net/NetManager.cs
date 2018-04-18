using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using BestHTTP;
using LitJson;
using March.Core.Network;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.SceneManagement;

public class NetManager : Singleton<NetManager>
{

	private bool m_isConnected = false;
	private bool m_isLogin = false;

    private bool waitForOffLineSaveRev = false;
    public float m_connectedDeltaTime = 2;
//    private bool m_canConnect = true;
//
//
//    public bool getCanConnect()
//    {
//        return m_canConnect;
//    }
//    public void setCanConnect(bool value)
//    {
//        m_canConnect = value;
//    }
    public bool getIsConnected()
	{
		return m_isConnected;
	}
	public void setIsConnected(bool value)
	{
		m_isConnected = value;
	}

    public bool needSendDataToServer()
    {
        if (!getIsConnected() && !waitForOffLineSaveRev)
        {
            return true;
        }
        return false;
    }

	public bool getIsLogin()
	{
		return m_isLogin;
	}
	public void setIsLogin(bool value)
	{
		m_isLogin = value;
	}

    private class ItemObj
    {
        public string item;
        public int num;
    }

    /// <summary>
    /// 上下这两个结构其实是一样的，服务器要求的字段不一样，所有客户端就定义了2遍。囧
    /// </summary>
    private class ItemBuyObj
    {
        public string itemId;
        public int num;
    }

    class StoryData
    {
        public string storyid;
    }

    class leveData
    {
        public int star;
    }

    class  ModifyName
    {
        public string nickName;
    }

    class LevelResult
    {
        public int succ;
        public int step;
        public int gold;
    }

	class BindData
	{
		public int optType;
		public string facebook;
		public string facebookAccountName;
	}

	class UnBindData
	{
		public int type;
	}

    public bool isNetWorkStatusGood()
    {
        if (!(Application.internetReachability == NetworkReachability.NotReachable))
            return true;
        else
        {
            setIsConnected(false);
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"> 消息号</param>
    /// <param name="data">消息体</param>
    /// <param name="callback">消息回调</param>
    public void httpSend(string cmd,string data,OnRequestFinishedDelegate callback)
    {
        //Debug.LogError("网络连接错误");
        string localCacheData = SaveDataManager.instance.GetString(SaveDataDefine.serverdata);
        Debug.Log("本地数据：" + localCacheData);
        if (localCacheData.Equals(""))
        {
            MessageBox.Instance.Show("数据错误，请联网同步数据");
            return;
        }
        else
        {
            JsonData jsonData = JsonMapper.ToObject(SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
            PlayerData.instance.RefreshData(jsonData);
        }

        if (isNetWorkStatusGood() && getIsConnected())
        {
            HTTPRequest request = new HTTPRequest(new Uri(ServerGlobal.loginUrl), HTTPMethods.Post, callback);
            request.AddField("uid", PlayerData.instance.userId);
            request.AddField("cmd", cmd);
            request.AddField("data", data);
            request.Send();
        }
      
    }

    #region USE TOOLS
    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="itemId">道具ID</param>
    /// <param name="num">道具数量</param>
    public void userToolsToServer(string itemId, string num)
    {
        if (!PlayerData.instance.goodsMap.ContainsKey(itemId))
        {
            Debug.LogError("物品不存在！");
        }
        if (int.Parse(PlayerData.instance.goodsMap[itemId]) < int.Parse(num))
        {
           Debug.LogError("物品数量不足！");
        }
        PlayerData.instance.goodsMap[itemId] = (int.Parse(PlayerData.instance.goodsMap[itemId]) - int.Parse(num)).ToString();
        SaveDataManager.instance.saveGoodsMap();


        ItemObj _obj = new ItemObj();
        _obj.item = itemId;
        _obj.num = int.Parse(num);
        string itemDataJson = JsonUtility.ToJson(_obj);
        Debug.Log("user tools obj data:"+itemDataJson);
        httpSend(ServerGlobal.ITEM_DEL_CMD,itemDataJson,userToolsRev);
    }

    private void userToolsRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }

        Debug.Log("userToolsRev response" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }
    #endregion

    #region SEND_QUEST_ID
    public void sendQuestIdToServer(string questId)
    {
        StoryData _story = new StoryData();
        _story.storyid = questId;
        string storyDataJson = JsonUtility.ToJson(_story);
        Debug.Log("story Data Json:" + storyDataJson);
        httpSend(ServerGlobal.SAVE_STORY_CMD, storyDataJson, sendQuestIdRev);
    }

    private void sendQuestIdRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("sendQuest Rev :" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }
    #endregion

    #region ITEM_BUY
    public void buyItemToServer(string itemId,string num)
    {
        var itemConfig = DefaultConfig.getInstance().GetConfigByType<item>().GetItemByID(itemId);
        if (itemConfig != null)
        {
            int cost = int.Parse(itemConfig.price) * int.Parse(num);

            if (PlayerData.instance.getCoinNum() < cost)
            {
                Debug.LogError("金币不足！");
            }
            else
            {
                PlayerData.instance.setCoinNum(PlayerData.instance.getCoinNum() - cost);
                SaveDataManager.instance.saveFieldByKey(SaveDataDefine.gold.ToString(), PlayerData.instance.getCoinNum());

                if (PlayerData.instance.goodsMap.ContainsKey(itemId))
                {
                    PlayerData.instance.goodsMap[itemId] = (int.Parse(PlayerData.instance.goodsMap[itemId]) + int.Parse(num)).ToString();
                }
                else
                {
                    PlayerData.instance.goodsMap.Add(itemId, num);
                }
                SaveDataManager.instance.saveGoodsMap();
            }
        }


        ItemBuyObj itemBuy = new ItemBuyObj();
        itemBuy.itemId = itemId;
        itemBuy.num = int.Parse(num);
        string itemDataJson = JsonUtility.ToJson(itemBuy);
        Debug.Log("item Buy Data Json:" + itemDataJson);
        httpSend(ServerGlobal.ITEM_BUY_CMD, itemDataJson, buyItemRev);
    }

    private void buyItemRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("item Buy Rev :" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }
    #endregion

    #region ELIMINATE_START

    public void eliminateLevelStart()
    {
        if (PlayerData.instance.getHeartNum() <= 0)
        {
            Debug.LogError("心数不足！");
        }
        else
        {
            PlayerData.instance.setHeartNum(PlayerData.instance.getHeartNum() - 1);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.heart.ToString(), PlayerData.instance.getHeartNum());
//            if ()
//            {
//                
//            }
//            PlayerData.instance.setRecoveryLeftTime();
        }

        httpSend(ServerGlobal.LEVEL_START_CMD, "", eliminateLevelStartRev);
    }

    private void eliminateLevelStartRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("eliminateLevelStartRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }

    #endregion

    #region ELIMINATE_END

    /// <summary>
    /// 消除关卡结果上传 level:关卡数 result:结果 step:总步数 wingold:除固定值外获得的金币
    /// </summary>
    /// <param name="result"></param>
    public void eliminateLevelEnd(int level, int result, int step, int wingold)
    {
        var levelconfig = DefaultConfig.getInstance().GetConfigByType<matchlevel>().GetItemByID((1000000 + level).ToString());
        if (result == 1)
        {
            PlayerData.instance.setCoinNum(PlayerData.instance.getCoinNum() + wingold + Int32.Parse(levelconfig.coin));
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.gold.ToString(), PlayerData.instance.getCoinNum());

            PlayerData.instance.setStarNum(PlayerData.instance.getStarNum() + Int32.Parse(levelconfig.star));
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.star.ToString(), PlayerData.instance.getStarNum());

            PlayerData.instance.setHeartNum(PlayerData.instance.getHeartNum() + 1);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.heart.ToString(), PlayerData.instance.getHeartNum());

            PlayerData.instance.setEliminateLevel(PlayerData.instance.getEliminateLevel() + 1);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.level.ToString(), PlayerData.instance.getEliminateLevel());

            var itemsRewardData = levelconfig.itemReward.Split(',');
            foreach (var item in itemsRewardData)
            {
                if (item != "0")
                {
                    var tmp = item.Split(':');
                    if (tmp.Length != 3)
                    {
                        Debug.LogError("matchlevel 表 reward字段 存在配表错误");
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(1, 101f) <= Int32.Parse(tmp[2]))
                        {
                            if (PlayerData.instance.goodsMap.ContainsKey(tmp[0]))
                            {
                                PlayerData.instance.goodsMap[tmp[0]] = (int.Parse(PlayerData.instance.goodsMap[tmp[0]]) + int.Parse(tmp[1])).ToString();
                            }
                            else
                            {
                                PlayerData.instance.goodsMap.Add(tmp[0], tmp[1]);
                            }
                        }
                        SaveDataManager.instance.saveGoodsMap();
                    }
                }
            }
        }
        LevelResult _levelResult = new LevelResult();
        _levelResult.succ = result;
        _levelResult.step = step;
        _levelResult.gold = wingold;
        string leveDataJson = JsonUtility.ToJson(_levelResult);
        Debug.Log("_level Result:" + leveDataJson);

        httpSend(ServerGlobal.LEVEL_END, leveDataJson, eliminateLevelEndRev);
    }

    private void eliminateLevelEndRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("eliminateLevelEndRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }

    #endregion

    #region MAKEPOINT

    public void MakePointInGuide( int level, int step)
    {
        JsonData _data = new JsonData();
        _data["level"] = level;
        _data["step"] = step;
        Debug.Log("MakePointInGuide Result:" + _data.ToJson());
        httpSend(ServerGlobal.MAKE_POINT_ELIMINATEGUIDE, _data.ToJson(), MakePointInGuideRev);
    }
    private void MakePointInGuideRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("MakePointInGuideRev response:" + response.DataAsText);
    }

    public void MakePointInEliminateStart()
    {
        MakePointInClickButton("EliminateStart", 1);
    }
    public void MakePointInEliminateClick()
    {
        MakePointInClickButton("Eliminate",1);
    }

    public void MakePointInClickButton(string name, int num)
    {
        JsonData _point = new JsonData();
        _point["name"] = name;
        _point["num"] = num;
        Debug.Log("MakePointInClickButton Result:" + _point.ToJson());
        httpSend(ServerGlobal.MAKE_POINT_CLICK, _point.ToJson(), MakePointInClickButtonRev);
    }
    private void MakePointInClickButtonRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("MakePointInClickButtonRev response:" + response.DataAsText);
    }


    #endregion

    #region HEART_BUY

    public void buyHeart()
    {
        int cost = int.Parse(DefaultConfig.getInstance().GetConfigByType<setting>().GetValueByIDAndKey("life", "livesPrice"));
        if (PlayerData.instance.getCoinNum() < cost)
        {
            Debug.LogError("金币不足！");
        }
        else
        {
            PlayerData.instance.setCoinNum(PlayerData.instance.getCoinNum() - cost);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.gold.ToString(), PlayerData.instance.getCoinNum());

            PlayerData.instance.setHeartNum(int.Parse(DefaultConfig.getInstance().GetConfigByType<setting>().GetValueByIDAndKey("life", "maxLives")));
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.heart.ToString(), PlayerData.instance.getHeartNum());

            PlayerData.instance.setRecoveryLeftTime(0);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.heartTime.ToString(), PlayerData.instance.getHeartNum());
        }

        httpSend(ServerGlobal.HEART_BUY, "", buyHeartRev);
    }

    private void buyHeartRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("buyHeartRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }

    #endregion

    #region FiveMore

    public void eliminateLevelFiveMore(int cost, Dictionary<string, int> items)
    {
        if (PlayerData.instance.getCoinNum() < cost)
        {
            Debug.LogError("金币不足！");
        }
        else
        {
            PlayerData.instance.setCoinNum(PlayerData.instance.getCoinNum() - cost);
            SaveDataManager.instance.saveFieldByKey(SaveDataDefine.gold.ToString(), PlayerData.instance.getCoinNum());
            foreach (var tmp in items)
            {
                if (PlayerData.instance.goodsMap.ContainsKey(tmp.Key))
                {
                    PlayerData.instance.goodsMap[tmp.Key] = (int.Parse(PlayerData.instance.goodsMap[tmp.Key]) + tmp.Value).ToString();
                }
                else
                {
                    PlayerData.instance.goodsMap.Add(tmp.Key, tmp.Value.ToString());
                }
            }
            SaveDataManager.instance.saveGoodsMap();
        }

        httpSend(ServerGlobal.LEVEL_FIVEMORE, "", eliminateLevelFiveMoreRev);
    }

    public void eliminateLevelFiveMoreRev(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("eliminateLevelFiveMoreRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
    }

    #endregion

    #region modify Nick Name
    public void modifyNickName(string _nickName)
    {
        ModifyName _mn = new ModifyName();
        _mn.nickName = _nickName;
        string _mnJson = JsonUtility.ToJson(_mn);
        SaveDataManager.instance.saveFieldByKey("name", _mn.nickName);

        Debug.Log("modifyNickName send data:"+_mnJson);
        httpSend(ServerGlobal.CHANGE_NAME, _mnJson, modifyNickNameRev);
    }

    private void modifyNickNameRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("modifyNickNameRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
        Messenger.Broadcast(ELocalMsgID.CloseModifyPanel);
    }
    #endregion

	/// <summary>
	/// off line data save 
	/// </summary>
	/// <param name="data">local saved data</param>
	#region offline data save 
	public void offLineDataSave(string data)
	{
	    waitForOffLineSaveRev = true;
	    HTTPRequest request = new HTTPRequest(new Uri(ServerGlobal.loginUrl), HTTPMethods.Post, offLineDataSaveRev);
	    request.AddField("uid", PlayerData.instance.userId);
	    request.AddField("cmd", ServerGlobal.SAVE_OFF_LINE);
	    request.AddField("data", data);
	    request.Send();
    }

	private void offLineDataSaveRev(HTTPRequest request,HTTPResponse response)
	{
	    Debug.Log("offLineDataSaveRev!");
        waitForOffLineSaveRev = false;
        if (response == null)
	    {
            setIsConnected(false);
	        m_connectedDeltaTime *= 2;
            return;
	    }
        Debug.Log ("offLineDataSaveRev response:" + response.DataAsText);
		if (response.DataAsText != "") {
		    m_connectedDeltaTime = 2;
		    if (!getIsConnected()) //保证离线模式转为在线模式 等待命令返回期间的数据 同步到服务器上
		    {
		        Debug.Log("网络连接成功！");
		        JsonData Jsontmp = JsonMapper.ToObject(SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
		        Jsontmp["heartTime"] = Jsontmp["heartTime"].ToString();
                Debug.Log(Jsontmp);
                offLineDataSave(Jsontmp.ToJson());
            }
		    setIsConnected(true);
            //            JsonData data = JsonMapper.ToObject(response.DataAsText);
            //			PlayerData.instance.RefreshData(data);
        }
	}
    #endregion
//    #region save day info 
//    public void saveDayInfo(string data)
//    {
//        httpSend(ServerGlobal.SAVE_DAY_INFO, data, saveDayInfoRev);
//    }
//
//    private void saveDayInfoRev(HTTPRequest request, HTTPResponse response)
//    {
//        Debug.Log("saveDayInfoRev response:" + response.DataAsText);
//        JsonData data = JsonMapper.ToObject(response.DataAsText);
//        PlayerData.instance.RefreshData(data);
//    }
//    #endregion

    #region save day award
    public void saveDayAward(string data)
    {
        httpSend(ServerGlobal.SAVE_DAY_AWARD, data, saveDayAwardRev);
    }

    private void saveDayAwardRev(HTTPRequest request, HTTPResponse response)
    {
        if (response == null)
        {
            setIsConnected(false);
            return;
        }
        Debug.Log("saveDayInfoRev response:" + response.DataAsText);
        JsonData data = JsonMapper.ToObject(response.DataAsText);
        PlayerData.instance.RefreshData(data);
        //PlayerData.instance.setIndexDay(int.Parse(data["sevenDay"]["sevenDayInfo"]["index"].ToString()));
        //PlayerData.instance.setAwardState(int.Parse(data["sevenDay"]["sevenDayInfo"]["state"].ToString()));
        Debug.Log("seven day info" + data["sevenDay"]["sevenDayInfo"]["index"]);
        Messenger.Broadcast(ELocalMsgID.ShowDailyLandingActivites);
    }
    #endregion
	
	#region user bind 
	public void userBind(string id, string name, OnRequestFinishedDelegate callback)
	{
		BindData _bindData = new BindData();
		_bindData.optType = 1;
		_bindData.facebook = id;
		_bindData.facebookAccountName = name;
		string bindDataJson = JsonUtility.ToJson(_bindData);
		Debug.Log("_bindData:" + bindDataJson);
		httpSend (ServerGlobal.BIND_CMD, bindDataJson, callback);
	}

	public void userUnBind(OnRequestFinishedDelegate callback)
	{
		UnBindData _unBindData = new UnBindData();
		_unBindData.type = 1;
		string unBindDataJson = JsonUtility.ToJson(_unBindData);
		Debug.Log("_unBindData:" + unBindDataJson);
		httpSend (ServerGlobal.BIND_CMD, unBindDataJson, callback);
	}
	#endregion

    public void SendRequest<T>(T handler) where T : INetHandler
    {
        httpSend(handler.GetCommand(), handler.GetData(), handler.OnRecieve);

        if (handler.OnSendComplete != null)
        {
            handler.OnSendComplete();
        }
    }
}
