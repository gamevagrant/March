using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using LitJson;

public class TimeMonoManager : MonoSingleton<TimeMonoManager>
{

    public float lastTime;
    public float curTime;
    private float totalTime = 30; //30min
    private float intervalTime = 1;

    private float reconnectTime = 0;
  

    public float getTotalTime()
    {
        return totalTime;
    }

    public void setTotalTime(float _value)
    {
        totalTime = _value;
    }

    // Use this for initialization
	void Start () 
    {
        InvokeRepeating("perMinCallback", 0, 1);
      /*  Debug.Log("本地时间日期：" + System.DateTime.Now);
        Debug.Log("本地时间戳:" + TimeUtil.instance.DateTimeToUnixTimestamp());*/
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (totalTime > 0)
	    {
	        intervalTime -= Time.deltaTime;
	        if (intervalTime <= 0)
	        {
	            intervalTime += 1;
	            totalTime--;
	           // Debug.LogError(string.Format("{0:D2}: {1:D2}",(int)totalTime/60,(int)totalTime%60));
                if (0 == totalTime && 5 > qy.GameMainManager.Instance.playerData.heartNum)
	            {
                    Debug.Log("30min倒计时结束");
                    //30min之后heart恢复一个
                    qy.GameMainManager.Instance.playerData.heartNum += 1;
                    Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
                    if(qy.GameMainManager.Instance.configManager.settingConfig.livesRecoverTime == 0)
                        totalTime = totalTime * 60;
                    else
                        totalTime = qy.GameMainManager.Instance.configManager.settingConfig.livesRecoverTime * 60;
	            }
	        }
	    }
        /*
        if (NetManager.instance.isNetWorkStatusGood())
        {
            if (NetManager.instance.needSendDataToServer()) //网络连接失败
            {
                if (reconnectTime >= 0)
                {
                    reconnectTime -= Time.deltaTime;

                    if (reconnectTime <= 0)
                    {
                        reconnectTime = NetManager.instance.m_connectedDeltaTime;
                
                        if (!SaveDataManager.instance.HasData(SaveDataDefine.serverdata))
                        {
                            return;
                        }
                        
                        JsonData jsonData = JsonMapper.ToObject(SaveDataManager.instance.GetString(SaveDataDefine.serverdata));
                        PlayerData.instance.userId = jsonData["uid"].ToString();
                        PlayerData.instance.jsonObj = jsonData;
                        PlayerData.instance.jsonObj["heartTime"] = PlayerData.instance.jsonObj["heartTime"].ToString();  //后端没办法解析int，heartTime这个字段只能客户端转换成string了
                        Debug.Log("heartTime str:" + PlayerData.instance.jsonObj["heartTime"]);
                        Debug.Log("当前本地数据" + PlayerData.instance.jsonObj.ToJson());
                        NetManager.instance.offLineDataSave(PlayerData.instance.jsonObj.ToJson());  //离线数据
                        
                        qy.GameMainManager.Instance.netManager.UpLoadOffLineData(qy.GameMainManager.Instance.playerData.ToPlayerDataMessage(), (ret, res) => { });
                    }
                }
            }
        }*/
	}
    private void perMinCallback()
    {


    }
}
