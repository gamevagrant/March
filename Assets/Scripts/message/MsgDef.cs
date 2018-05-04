using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// local msg id define
/// </summary>
public enum ELocalMsgID
{
    None = 0,
    LoadingSceneProcess = 1,
    RefreshBaseData = 2,
    RecoveryPanelClosed = 3,
    CloseModifyPanel  = 4,
    EliminateBoosterRefresh = 5,
    ShowDailyLandingActivites = 6,
    OpenLevelBeginPanel,
    OpenUI,
    CloseUI,
    LoadScene,
}

/// <summary>
/// net error code
/// 
/// </summary>
public class ENetMsgErroeCode
{
    /// <summary>
    /// 操作无效
    /// </summary>
    public static string INVALID_OPTION = "E00000";
    /** uid 不存在*/
    public static string UID_NOT_EXIST = "10001";
    /** 道具不足*/
    public static string ITEM_NOT_ENOUGH = "10003";
    /** 生命值不足*/
    public static string LIFE_NOT_ENOUGH = "10004";
    /**金币不足*/
    public static string USER_GOLD_IS_NOT_ENOUGH = "10005";
    /**星星不足*/
    public static string STAR_NOT_ENOUGH = "10006";
    /**此名称已存在*/
    public static string ACCOUNT_ALREADY_USERNAME = "105251";
    /**此名称含有非法字符*/
    public static string ACCOUNT_ALREADY_USERNAME_BADWORDS = "105252";
    //名字非法  包含'(' 或emoji 或不匹配的{} 或长度不满足3~16
    public static string NAME_INVALID = "113995"; 

}
                                  