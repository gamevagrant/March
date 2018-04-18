using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using qy.config;

namespace qy.net
{
    
    public class NetManager
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
        public static string SAVE_DAY_INFO = "sevenDay.info";
        public static string SAVE_DAY_AWARD = "sevenDay.award";
        public static string MAKE_POINT_ELIMINATEGUIDE = "eliminateGuide.makepoint";

        private static NetManager _instance;
        public static NetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NetManager();
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

        private long time
        {
            get
            {
                return GameUtils.DateTimeToTimestamp(DateTime.Now);
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

        public bool isNetWorkStatusGood()
        {
            //todo:加入ping机制 ping不通服务器的话取用本地数据
            if (!(Application.internetReachability == NetworkReachability.NotReachable))
                return true;
            else
            {

                return false;
            }
        }

        private bool SendData(string cmd,object jd, Action<bool, PlayerDataMessage> callBack)
        {
            string url = APIDomain;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("cmd", cmd);
            data.Add("uid", uid);
            data.Add("data",JsonMapper.ToJson(jd ?? "") );

            return HttpProxy.SendPostRequest<PlayerDataMessage>(url, data, (ret, res) =>
            {
                if (res.isOK)
                {
                    GameMainManager.Instance.playerData.RefreshData(res as PlayerDataMessage);

                }
                else
                {
                    Debug.Log(GetMsgByErrorCode(res.err));
                }
                callBack(ret, res);
            });
        }

        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="obj"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool SendQuest<T>(string cmd,object obj, Action<bool, T> callBack)where T:NetMessage
        {
            string url = APIDomain;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("cmd", cmd);
            data.Add("uid", uid);
            data.Add("data", JsonMapper.ToJson(obj ?? ""));
            return HttpProxy.SendPostRequest<T>(url, data, (ret, res) =>
            {
                if (res.isOK)
                {
                    

                }
                else
                {
                    Debug.Log(GetMsgByErrorCode(res.err));
                }
                callBack(ret, res);
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">打点数据</param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool Login(LoginInfo loginInfo, Action<bool, PlayerDataMessage> callBack)
        {
            if(!isNetWorkStatusGood())
            {
                PlayerData pd = LocalDatasManager.playerData;
                if(pd!=null)
                {
                    GameMainManager.Instance.playerData = pd;
                }else
                {
                    Debug.Log("数据错误，请联网同步数据");
                }
               
                return true;
            }
            string url = APIDomain;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("cmd", LOGIN_CMD);
            data.Add("device", Utils.instance.getDeviceID());
            data.Add("data", JsonMapper.ToJson(loginInfo));

            return HttpProxy.SendPostRequest<PlayerDataMessage>(url, data, (ret, res) =>
            {
                if (res.isOK)
                {
                    GameMainManager.Instance.playerData.RefreshData(res);

                }
                else
                {
                    Debug.Log(GetMsgByErrorCode(res.err));
                }
                callBack(ret, res);
            });

        }

        /// <summary>
        /// 上传离线数据
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool UpLoadOffLineData(PlayerDataMessage playerData, Action<bool, PlayerDataMessage> callBack)
        {

            return SendData(SAVE_OFF_LINE, playerData, callBack);
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
            PropItem item = GameMainManager.Instance.playerData.GetPropItem(itemID);
            if (item == null || item.count<count)
            {
                Debug.LogError("物品不存在或数量不足！");
            }else
            {
                GameMainManager.Instance.playerData.RemovePropItem(itemID,count);
                LocalDatasManager.playerData = GameMainManager.Instance.playerData;

            }

            JsonData jd = new JsonData();
            jd["item"] = itemID;
            jd["num"] = count;

            return SendData(ITEM_DEL_CMD, jd, callBack);
        }
        /// <summary>
        /// 更新当前任务id
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool UpdateQuestId(string questId, Action<bool, PlayerDataMessage> callBack)
        {
            LocalDatasManager.playerData = GameMainManager.Instance.playerData;

            JsonData jd = new JsonData();
            jd["storyid"] = questId;

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
            PropItem item = GameMainManager.Instance.configManager.propsConfig.GetItem(itemId);
            if(item!=null )
            {
                int cost = item.price * num;
                if (GameMainManager.Instance.playerData.coinNum< cost)
                {
                    Debug.LogError("金币不足！");
                }else
                {
                    GameMainManager.Instance.playerData.coinNum -= cost;
                    GameMainManager.Instance.playerData.AddPropItem(itemId,num);
                    LocalDatasManager.playerData = GameMainManager.Instance.playerData;

                }
            }

            JsonData jd = new JsonData();
            jd["itemId"] = itemId;
            jd["num"] = num;

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
            
            if (GameMainManager.Instance.playerData.heartNum<=0)
            {
                Debug.Log("心数不足！");
            }else
            {
                GameMainManager.Instance.playerData.heartNum-= 1;
                LocalDatasManager.playerData = GameMainManager.Instance.playerData;
            }

            return SendData(LEVEL_START_CMD, null, callBack);
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
            MatchLevelItem levelItem = GameMainManager.Instance.configManager.matchLevelConfig.GetItem((1000000 + level).ToString());
            if(result == 1)
            {
                GameMainManager.Instance.playerData.coinNum += wingold + levelItem.coin;
                GameMainManager.Instance.playerData.starNum += levelItem.star;
                GameMainManager.Instance.playerData.heartNum += 1;
                GameMainManager.Instance.playerData.eliminateLevel += 1;

                foreach(PropItem propItem in levelItem.itemReward)
                {
                    int rate = UnityEngine.Random.Range(1, 101);
                    if(rate<=propItem.rate)
                    {
                        GameMainManager.Instance.playerData.AddPropItem(propItem.id, propItem.count);

                    }
                }

                LocalDatasManager.playerData = GameMainManager.Instance.playerData;

            }
            JsonData jd = new JsonData();
            jd["succ"] = result;
            jd["step"] = step;
            jd["gold"] = wingold;

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
            return SendData(MAKE_POINT_ELIMINATEGUIDE, jd, callBack);
        }
        /// <summary>
        /// 购买生命
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool BuyHeart(Action<bool, PlayerDataMessage> callBack)
        {
            int cost = GameMainManager.Instance.configManager.settingConfig.livesPrice;
            if(GameMainManager.Instance.playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
            }else
            {
                GameMainManager.Instance.playerData.coinNum -= cost;
                GameMainManager.Instance.playerData.heartNum = GameMainManager.Instance.configManager.settingConfig.maxLives;
                GameMainManager.Instance.playerData.recoveryLeftTime = 0;
                LocalDatasManager.playerData = GameMainManager.Instance.playerData;
            }

            return SendData(HEART_BUY, null, callBack);
        }

        /// <summary>
        /// 关卡结束时 购买再来5步
        /// </summary>
        /// <param name="step"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool EliminateLevelFiveMore(int step,Action<bool, PlayerDataMessage> callBack)
        {
            int cost = GameMainManager.Instance.configManager.settingConfig.GetPriceWithStep(step);
            List<PropItem> items = GameMainManager.Instance.configManager.settingConfig.GetBonusItemBagWithStep(step);
            if (GameMainManager.Instance.playerData.coinNum < cost)
            {
                Debug.LogError("金币不足！");
            }
            else
            {
                GameMainManager.Instance.playerData.coinNum -= cost;
                foreach(PropItem item in items)
                {
                    GameMainManager.Instance.playerData.AddPropItem(item.id,item.count);
                }
                
                LocalDatasManager.playerData = GameMainManager.Instance.playerData;
            }

            return SendData(LEVEL_FIVEMORE, null, callBack);
        }
        /// <summary>
        /// 更改姓名
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool ModifyNickName(string nickName, Action<bool, PlayerDataMessage> callBack)
        {
            GameMainManager.Instance.playerData.nickName = nickName;
            GameMainManager.Instance.playerData.SaveData();

            JsonData jd = new JsonData();
            jd["nickName"] = nickName;

            return SendData(CHANGE_NAME, jd, callBack);
        }

        /// <summary>
        /// 7天奖励
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool SaveDayAward( Action<bool, PlayerDataMessage> callBack)
        {

            JsonData jd = new JsonData();


            return SendData(SAVE_DAY_AWARD, jd, callBack);
        }

        public bool UserBind(string id, string name, Action<bool, PlayerDataMessage> callBack)
        {

            JsonData jd = new JsonData();
            jd["optType"] = 1;
            jd["facebook"] = id;
            jd["facebookAccountName"] = name;

            return SendData(BIND_CMD, jd, callBack);
        }

        public bool UserUnBind(Action<bool, PlayerDataMessage> callBack)
        {

            JsonData jd = new JsonData();
            jd["type"] = 1;

            return SendData(BIND_CMD, jd, callBack);
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
