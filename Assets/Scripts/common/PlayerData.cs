using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Assets.Scripts.Common;
using LitJson;

public class PlayerData : Singleton<PlayerData>
{
    public  string userId = "";
    private string nickName = ""; 
    private int starNum = 0; //星数
    private int heartNum = 0;//心数
    private int coinNum = 0; //金币
    private int eliminateLevel = 1; //消除关卡
    private long time;//时间戳
    private long recoveryLeftTime = 0; //下一次生命恢复剩余时间
    private string questId = "10001";//剧情ID
    private int heartRecoverTime = 30; //心数恢复间隔是30min
    private int maxLives = 5; //最大生命数
    private int livePrice = 900; //补满生命金币价格

	private bool isSaveDayInfo = false;
    private int indexDay = 0; //0-6表示某天
    private int awardState = 1; 

	private bool isPlayScene = false;

    private string gameUid = "";
    private string referrer = "";
    private string afUID= "";
	private string lang = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified) ? "cn" : "en";
    private string gaid = "";
    private int gmLogin = 0;
    private string terminal = "";
    private string SecurityCode = "";
    private string packageName = "";
    private string isHDLogin = "";
    
    private string recallId = "";

	private bool m_isShowedLoginAward = false;

    public Dictionary<string, string> goodsMap = new Dictionary<string, string>();

	public JsonData jsonObj = null;

    private setting m_setting;
    public setting Item { get { if (m_item == null) { m_item = DefaultConfig.getInstance().GetConfigByType<setting>(); } return m_item; } }
    private setting m_item;
    public setting Setting { get { if (m_setting == null) { m_setting = DefaultConfig.getInstance().GetConfigByType<setting>(); } return m_setting; } }
    private quest m_quest;
    public quest Quest { get { if (m_quest == null) { m_quest = DefaultConfig.getInstance().GetConfigByType<quest>(); } return m_quest; } }


    public void initGameGlobalData()
    {
        SettingItem m_setting = Setting.GetItemByID("life");
        if (m_setting != null)
        {
            heartRecoverTime = int.Parse(m_setting.livesRecoverTime);
            maxLives = int.Parse(m_setting.maxLives);
            livePrice = int.Parse(m_setting.livesPrice);
        }
    }

    public int getHeartRecoverTime()
    {
        return heartRecoverTime;
    }

    public void setShowLoginAward(bool value)
	{
		m_isShowedLoginAward = value;
	}

	public bool getShowLoginAward()
	{
		return m_isShowedLoginAward;
	}

    public string getNickName()
    {
        return nickName;
    }

	public string getLang()
	{
		return lang;
	}

    /// <summary>
    /// 根据item ID去获取响应item的数量
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public string getNumByItemId(string itemId)
    {
        if (goodsMap.ContainsKey(itemId))
            return goodsMap[itemId];
        else
            return null;
    }

    public void setIndexDay(int index)
    {
        indexDay = index;
    }

    public int getIndexDay()
    {
        return indexDay;
    }

    public void setAwardState(int state)
    {
        awardState = state;
    }

    public int getAwardState()
    {
        return awardState;
    }

	public void setSaveDayInfo(bool state)
	{
		isSaveDayInfo = state;
	}

	public bool getSaveDayInfo()
	{
		return isSaveDayInfo;
	}

	public void setPlayScene(bool state)
	{
		isPlayScene = state;
	}

	public bool getPlayScene()
	{
		return isPlayScene;
	}

    public void setStarNum(int _value)
    {
        starNum = _value;
    }

    public int getStarNum()
    {
        return starNum;
    }

    public void setHeartNum(int _value)
    {
        heartNum = _value;
    }

    public int getHeartNum()
    {
        return heartNum;
    }

    public void setCoinNum(int _value)
    {
        coinNum = _value;
    }

    public int getCoinNum()
    {
        return coinNum;
    }

    public void setEliminateLevel(int _value)
    {
        eliminateLevel = _value;
    }

    public int getEliminateLevel()
    {
        return eliminateLevel;
    }

    public void setTime(long _value)
    {
        time = _value;
    }
    public long getTime()
    {
        return time;
    }

    public void setQuestId(string _value)
    {
        questId = _value;
    }

    public string getQuestId()
    {
        if (questId.Equals(""))
            return "10001";
        return questId;
    }

    public int getLivePrice()
    {
        return livePrice;
    }

    public int getMaxLives()
    {
        return maxLives;
    }

    public void setRecoveryLeftTime(long _value)
    {
        recoveryLeftTime = _value;
    }

    public long getRecoveryLeftTime()
    {
        return recoveryLeftTime;
    }


    public void RefreshData(JsonData data)
    {
        if (null == data)
        {
            Debug.LogError("josn data is null");
            return;
        }
		if (data.Keys.Contains ("err")) 
		{
			Debug.LogError ("recv msg err");
			string errorcode = data ["err"].ToString ();
			string lang = getMsgByErrorCode(errorcode);
			MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey(lang));
			return;
		}
        //更新本地数据
        SaveDataManager.instance.SaveString(SaveDataDefine.serverdata, data.ToJson());
        coinNum = int.Parse(data["gold"].ToString());
        heartNum = int.Parse(data["heart"].ToString());
        starNum = int.Parse(data["star"].ToString());
        nickName = data["name"].ToString();
        userId = data["uid"].ToString();
        
        time = long.Parse(data["heartTime"].ToString());
        if (!data["heartTime"].ToString().Equals("0"))
        {
            long temp = time - TimeUtil.instance.ConvertDateTimeToUnix();
            recoveryLeftTime = temp/1000;  //ms转换成秒
            TimeMonoManager.instance.setTotalTime(recoveryLeftTime);
            Debug.Log("系统时间:" + TimeUtil.instance.ConvertDateTimeToUnix());
            Debug.Log("下一次心数恢复是:"+time);
            Debug.Log("下一次心数恢复还剩余时间是:" + recoveryLeftTime.ToString());
            
            SaveDataManager.instance.SaveString(SaveDataDefine.heartrecoveryLeftTime, recoveryLeftTime.ToString());
        }

        questId = data["storyid"].ToString();
        if (!questId.ToString().Equals(""))
        {
            questId = Quest.GetItemByID(questId).gotoId;
        }
        eliminateLevel = int.Parse(data["level"].ToString());
        Debug.Log("refresh data is:"+ data["items"]);
        goodsMap.Clear();
        for (int i = 0; i < data["items"].Count; i++)
        {
            if (!goodsMap.ContainsKey(data["items"][i]["itemId"].ToString()))
                goodsMap.Add(data["items"][i]["itemId"].ToString(),data["items"][i]["count"].ToString());
            else
            {
                goodsMap[data["items"][i]["itemId"].ToString()] = data["items"][i]["count"].ToString();
            }
        }
        Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
    }

	public string getMsgByErrorCode(string errorcode)
	{
		string temLang = "";
		if (errorcode.Equals ("E00000"))
			temLang = "200041";
		else if (errorcode.Equals("10001"))
			temLang = "200050";
		else if (errorcode.Equals("10003"))
			temLang = "200042";
		else if (errorcode.Equals("10004"))
			temLang = "200043";
		else if (errorcode.Equals ("10005"))
			temLang = "200044";
		else if (errorcode.Equals ("10006"))
			temLang = "200045";
		else if (errorcode.Equals ("105251"))
			temLang = "200046";
		else if (errorcode.Equals ("105252"))
			temLang = "200047";
		else if (errorcode.Equals ("113995"))
			temLang = "200048";
		else
			temLang = "200051";
		return temLang;
	}

    /// <summary>
    /// 根据itemID去查目前玩家拥有的道具数量
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public int getHasItemCountByItemId(string itemId)
    {
        if (goodsMap.ContainsKey(itemId))
        {
            int count = int.Parse(goodsMap[itemId]);
            return count;
        }
        return 0;
    }


    public override string ToString()
    {
        var result = string.Format("star count-{0}\nheart count-{1}\ncoin count-{2}\nmain level-{3}\nquest id-{4}", starNum, heartNum, coinNum, eliminateLevel, questId);
        return result;
    }
}
