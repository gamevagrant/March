using Assets.Scripts.Common;
using UnityEngine;

public class TimeMonoManager : MonoSingleton<TimeMonoManager>
{
    public float lastTime;
    public float curTime;
    private float totalTime = 30; //30min
    private float intervalTime = 1;

    public float getTotalTime()
    {
        return totalTime;
    }

    public void setTotalTime(float _value)
    {
        totalTime = _value;
    }

	void Start () 
    {
        InvokeRepeating("perMinCallback", 0, 1);
	}
	
	void Update () 
    {
	    if (totalTime > 0)
	    {
	        intervalTime -= Time.deltaTime;
	        if (intervalTime <= 0)
	        {
	            intervalTime += 1;
	            totalTime--;

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
	}
}
