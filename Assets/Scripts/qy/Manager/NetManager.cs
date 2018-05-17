using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using qy.config;
using March.Core.Network;
using BestHTTP;
namespace qy.net
{
    
    public class NetManager:MonoBehaviour
    {
        public static string LOGIN_CMD = "user.login";
        public static string BIND_CMD = "user.bind";
        public static string UNBIND_CMD = "bind.cancel";
        public static string LEVEL_UP_CMD = "level.up";
        public static string LEVEL_FAIL_CMD = "level.fail";
        public static string SAVE_STORY_CMD = "story.unlock";
        public static string ITEM_DEL_CMD = "item.del";
        public static string ITEM_BUY_CMD = "item.buy";
        public static string LEVEL_START_CMD = "level.start";
        public static string LEVEL_END = "level.end";
        public static string HEART_BUY = "heart.buy";
        public static string LEVEL_FIVEMORE = "level.fivemore";
        public static string CHANGE_NAME = "user.modify.nickName";
        public static string SAVE_OFF_LINE = "offline.save";
        public static string SEVEN_DAY_INFO = "sevenDay.info";
        public static string SEVEN_DAY_AWARD = "sevenDay.award";
        public static string MAKE_POINT_ELIMINATEGUIDE = "eliminateGuide.makepoint";
        public static string MAKE_POINT_CLICK = "buttonClick.makepoint";

        public static string ROLE_SWITCH = "role.choose";
        public static string ROLE_RECOVER = "role.recover";
        public static string ROLE_END = "role.end";

        private Queue<Dictionary<string, object>> bufferQueue = new Queue<Dictionary<string, object>>();
        private Queue<Action> queue = new Queue<Action>();
        private bool isSending = false;

        private static NetManager _instance;
        public static NetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("MainManager");

                    if (go == null)
                    {
                        go = new GameObject("MainManager");
                    }
                    GameObject.DontDestroyOnLoad(go);
                    go.AddComponent<NetManager>();
                }
                return _instance;
            }
        }

        private string APIDomain
        {
            get
            {
                if (GameSetting.isRelease)
                {
                    return GameSetting.serverPath;
                }
                else
                {
                    return GameSetting.serverPathDevelop;
                }

            }
        }

        private string uid
        {
            get
            {
                return GameMainManager.Instance.playerData.userId;
            }
        }
        /// <summary>
        /// 毫秒时间戳
        /// </summary>
        private long time
        {
            get
            {
                return GameUtils.DateTimeToMilliTimeStamp(DateTime.Now);
            }
        }

        private string MakeUrl(string domain, string api)
        {
            string str = string.Format("{0}/{1}", domain, api);
            return str;
        }



        private void AlertError(NetMessage res, string title)
        {
            if (!res.isOK)
            {

                //string describe = GameMainManager.instance.configManager.errorDescribeConfig.GetDescribe(res.errcode.ToString(), res.errmsg);
                //Debug.Log(title + ":" + describe);
                //Alert.Show(string.Format("{0}\n{1}:{2}", describe, title, res.errcode));
            }

        }

        public bool isNetWorkStatusGood
        {
            get
            {
                //return false;
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        private bool isNeedUpLoadOffLine
        {
            get
            {

                return GameMainManager.Instance.playerData.isNeedUpLoadOffLine;
            }
            set
            {
                GameMainManager.Instance.playerData.isNeedUpLoadOffLine = value;
            }
        }
        private void Awake()
        {
            _instance = this;
            SendOfflineData();
        }
        private void Start()
        {
            
        }

        private float timer;
        private const float space = 1; 
        private void Update()
        {
            if(timer>0)
            {
                timer -= Time.unscaledDeltaTime;
            }else
            {
                timer += space;
                UpdateSecond();
            }
            
        }

        private void UpdateSecond()
        {
            if (queue.Count > 0 && !isSending)
            {
                isSending = true;
                Action act = queue.Peek();
                act();
            }
        }

        private void OnApplicationQuit()
        {
            if(bufferQueue.Count>0)
            {
                Queue<Dictionary<string, object>> localQueue = new Queue<Dictionary<string, object>>(LocalDatasManager.netBufferQueue);
                while (bufferQueue.Count > 0)
                {
                    Dictionary<string, object> data = bufferQueue.Dequeue();
                    if(data["cmd"].ToString()!=SAVE_OFF_LINE)
                    {
                        localQueue.Enqueue(data);
                    }
                }
                LocalDatasManager.netBufferQueue = new List<Dictionary<string, object>>(localQueue);
                Debug.Log("保存离线数据："+localQueue.Count);
            }
        }

        private bool SendData(string cmd,object jd, Action<bool, PlayerDataMessage> callBack)
        {
            Debug.Log("加入" + cmd);

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("cmd", cmd);
            data.Add("uid", uid);
            data.Add("device", Utils.instance.getDeviceID());
            data.Add("timestamp", time.ToString());
            data.Add("data", jd == null ? "" :JsonMapper.ToJson(jd));

            bufferQueue.Enqueue(data);
            queue.Enqueue(() => {
                
                Dictionary<string, object> bufferData = bufferQueue.Peek();
                Send(bufferData, callBack);
                Debug.Log("--执行" + bufferData["cmd"]);
            });

            return true;
        }
        private bool Send(Dictionary<string, object> data, Action<bool, PlayerDataMessage> callBack)
        {
            string url = APIDomain;
            return HttpProxy.SendPostRequest<PlayerDataMessage>(url, data, (ret, res) =>
            {
                if(ret)
                {
                    if (res.isOK)
                    {
                        GameMainManager.Instance.playerData.RefreshData(res as PlayerDataMessage);
                    }
                    else
                    {
                        Debug.Log("-----服务器返回错误：" + res.errMsg + "-----");
                        Debug.LogWarning("-----服务器返回错误：" + res.errMsg + "-----");
                        ui.Alert.Show(GetMsgByErrorCode(res.err));
                    }
                    callBack(ret, res);
                    queue.Dequeue();
                    bufferQueue.Dequeue();
                }
                isSending = false;
            });
        }
        public void SendRequest<T>(T handler) where T : INetHandler
        {
            HTTPRequest request = new HTTPRequest(new Uri(Configure.instance.ServerUrl), HTTPMethods.Post, handler.OnRecieve);
            request.AddField("uid", uid);
            request.AddField("device", Utils.instance.getDeviceID());
            request.AddField("cmd", handler.GetCommand());
            request.AddField("data", handler.GetData());
            request.Send();

        }

        /// <summary>
        /// 尝试进行同步
        /// </summary>
        private void SendOfflineData()
        {
            Queue<Dictionary<string, object>> localQueue = new Queue<Dictionary<string, object>>(LocalDatasManager.netBufferQueue);
            Debug.Log("提取离线数据：" + localQueue.Count);
            if (localQueue.Count > 0)
            {
                OfflineData offlineData = new OfflineData(GameMainManager.Instance.playerData);
                offlineData.offline = LocalDatasManager.netBufferQueue;
                UpLoadOffLineData(offlineData, (ret, res) => {
                    //LocalDatasManager.netBufferQueue = null;
                });
            }

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">打点数据</param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool Login(LoginInfo loginInfo, Action<bool, PlayerDataMessage> callBack)
        {
            return SendData(LOGIN_CMD, loginInfo, callBack);
        }

        

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool UseTools(string itemID,int count, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["item"] = itemID;
            jd["num"] = count;
            jd["timestamp"] = time.ToString();
            return SendData(ITEM_DEL_CMD, jd, callBack);
        }
        /// <summary>
        /// 完成当前任务
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool ComplateQuestId(string questId,qy.config.Ability ability, Action<bool, PlayerDataMessage> callBack)
        {

            JsonData jd = new JsonData();
            jd["storyid"] = questId;
            jd["choice"] = string.Format("{0}|{1}|{2}",ability.discipline,ability.loyalty,ability.wisdom);
            jd["timestamp"] = time.ToString();
            return SendData(SAVE_STORY_CMD, jd, callBack);
        }

        /// <summary>
        /// 购买物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="num"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool BuyItem(string itemId,int num, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["itemId"] = itemId;
            jd["num"] = num;
            jd["timestamp"] = time.ToString();
            return SendData(ITEM_BUY_CMD, jd, callBack);
        }

        /// <summary>
        /// 关卡开始
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="num"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool LevelStart(Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["timestamp"] = time.ToString();
            return SendData(LEVEL_START_CMD, jd, callBack);
        }
        /// <summary>
        /// 关卡结束
        /// </summary>
        /// <param name="level">关卡数</param>
        /// <param name="result">结果 </param>
        /// <param name="step">总步数</param>
        /// <param name="wingold">除固定值外获得的金币</param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool LevelEnd(int level, int result, int step, int wingold, Action<bool, PlayerDataMessage> callBack)
        {
           
            JsonData jd = new JsonData();
            jd["succ"] = result;
            jd["step"] = step;
            jd["gold"] = wingold;
            jd["timestamp"] = time.ToString();
            return SendData(LEVEL_END, jd, callBack);
        }
        /// <summary>
        /// 引导打点
        /// </summary>
        /// <param name="level"></param>
        /// <param name="step"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool MakePointInGuide(int level, int step, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["level"] = level;
            jd["step"] = step;
            jd["timestamp"] = time.ToString();
            return SendData(MAKE_POINT_ELIMINATEGUIDE, jd, callBack);
        }

        public bool MakePointInEliminateStart(Action<bool, PlayerDataMessage> callBack)
        {
            return MakePointInClickButton("EliminateStart", 1, callBack);
        }
        public bool MakePointInEliminateClick(Action<bool, PlayerDataMessage> callBack)
        {
            return MakePointInClickButton("Eliminate", 1, callBack);
        }
        public bool MakePointInClickButton(string name, int num, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["name"] = name;
            jd["num"] = num;
            jd["timestamp"] = time.ToString();
            Debug.Log("MakePointInClickButton Result:" + jd.ToJson());
            return SendData(ServerGlobal.MAKE_POINT_CLICK, jd, callBack);
        }
        /// <summary>
        /// 购买生命
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool BuyHeart(Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["timestamp"] = time.ToString();
            return SendData(HEART_BUY, jd, callBack);
        }

        /// <summary>
        /// 关卡结束时 购买再来5步
        /// </summary>
        /// <param name="step"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool EliminateLevelFiveMore(int step,Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["timestamp"] = time.ToString();
            return SendData(LEVEL_FIVEMORE, jd, callBack);
        }
        /// <summary>
        /// 更改姓名
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool ModifyNickName(string nickName, Action<bool, PlayerDataMessage> callBack)
        {

            JsonData jd = new JsonData();
            jd["nickName"] = nickName;
            jd["timestamp"] = time.ToString();
            return SendData(CHANGE_NAME, jd, (ret,res)=> {
                callBack(ret, res);
                Messenger.Broadcast(ELocalMsgID.CloseModifyPanel);
            });
        }

        /// <summary>
        /// 7天奖励
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool SevenDayAward( Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["timestamp"] = time.ToString();
            return SendData(SEVEN_DAY_AWARD, jd, (ret,res)=> {
                callBack(ret, res);
                Messenger.Broadcast(ELocalMsgID.ShowDailyLandingActivites);
            });
        }

        public bool SevenDayInfo(Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["timestamp"] = time.ToString();
            return SendData(SEVEN_DAY_INFO, jd, callBack);
        }

        public bool UserBind(string id, string name, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["optType"] = 1;
            jd["facebook"] = id;
            jd["facebookAccountName"] = name;
            jd["timestamp"] = time.ToString();
            return SendData(BIND_CMD, jd, callBack);
        }

        public bool UserUnBind(Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["type"] = 1;
            jd["timestamp"] = time.ToString();
            return SendData(BIND_CMD, jd, callBack);
        }
        /// <summary>
        /// 选择角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool SwitchRole(string id, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["roleId"] = id;
            jd["timestamp"] = time.ToString();
            return SendData(ROLE_SWITCH, jd, callBack);
        }
        /// <summary>
        /// 复活角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="type"> 0金币 ，1道具</param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool RecoverRole(string id,int type, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["roleId"] = id;
            jd["type"] = type;
            jd["timestamp"] = time.ToString();
            return SendData(ROLE_RECOVER, jd, callBack);
        }
        /// <summary>
        /// 角色结局
        /// </summary>
        /// <param name="type"> 1死亡 ，2通关 </param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool EndingRole(int type, Action<bool, PlayerDataMessage> callBack)
        {
            JsonData jd = new JsonData();
            jd["type"] = type;
            jd["timestamp"] = time.ToString();
            return SendData(ROLE_END, jd, callBack);
        }

        /// <summary>
        /// 上传离线数据
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        private bool UpLoadOffLineData(OfflineData playerData, Action<bool, PlayerDataMessage> callBack)
        {

            return SendData(SAVE_OFF_LINE, playerData, (ret, res) => {
                if (res.isOK)
                {
                    Debug.Log("===============【上传离线数据成功】==============");
                }
                else
                {
                    Debug.Log("============【上传离线数据失败】==============" + GetMsgByErrorCode(res.err));
                }
                callBack(ret, res);
            });
        }
        

        private string GetMsgByErrorCode(string errorcode)
        {
            string temLang = "";
            if (errorcode.Equals("E00000"))
                temLang = "200041";
            else if (errorcode.Equals("10001"))
                temLang = "200050";
            else if (errorcode.Equals("10003"))
                temLang = "200042";
            else if (errorcode.Equals("10004"))
                temLang = "200043";
            else if (errorcode.Equals("10005"))
                temLang = "200044";
            else if (errorcode.Equals("10006"))
                temLang = "200045";
            else if (errorcode.Equals("105251"))
                temLang = "200046";
            else if (errorcode.Equals("105252"))
                temLang = "200047";
            else if (errorcode.Equals("113995"))
                temLang = "200048";
            else
                temLang = "200051";
            return LangrageManager.Instance.GetItemWithID(temLang);
        }
    }
    
}
