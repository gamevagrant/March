using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace qy
{
    public enum PlayerModelErr : int
    {
        NULL = 0,
        /// <summary>
        /// 金币不足
        /// </summary>
        NOT_ENOUGH_COIN,
        /// <summary>
        /// 生命不足
        /// </summary>
        NOT_ENOUGH_HEART,
        /// <summary>
        /// 物品不足
        /// </summary>
        NOT_ENOUGH_PROP,
        /// <summary>
        /// 星星不足
        /// </summary>
        NOT_ENOUGH_STAR,
        /// <summary>
        /// 任务id错误
        /// </summary>
        QUEST_ID_ERROR,
        /// <summary>
        /// 物品id错误
        /// </summary>
        PROP_ID_ERROR,
        /// <summary>
        /// 倒计时未完成
        /// </summary>
        COUNT_DOWN_NOT_END,
        /// <summary>
        /// //生命满
        /// </summary>
        HEART_IS_FULL,
        /// <summary>
        /// //角色已死亡
        /// </summary>
        ROLE_IS_DIE,
    }
    public interface IPlayerModel
    {
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="questId">下一个任务</param>
        /// <returns></returns>
        PlayerModelErr QuestComplate(out string storyID,string selectedID="");
        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        PlayerModelErr UseProp(string itemID, int count);
        /// <summary>
        /// 购买道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        PlayerModelErr BuyProp(string itemId, int num);
        /// <summary>
        /// 开始任务
        /// </summary>
        /// <returns></returns>
        PlayerModelErr StartLevel();
        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="level">关卡数</param>
        /// <param name="result">是否胜利</param>
        /// <param name="step">使用步数</param>
        /// <param name="wingold">非固定奖励</param>
        /// <returns></returns>
        PlayerModelErr EndLevel(int level, bool result, int step, int wingold);
        /// <summary>
        /// 购买生命
        /// </summary>
        /// <returns></returns>
        PlayerModelErr BuyHeart();
        /// <summary>
        /// 购买 再来五步
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        PlayerModelErr BuyFiveMore(int step);
        /// <summary>
        /// 更改昵称
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        PlayerModelErr ModifyNickName(string nickName);
        /// <summary>
        /// 以id指定的角色重新开始游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PlayerModelErr StartGameWithRole(string id);
        /// <summary>
        /// 使用金币复活
        /// </summary>
        /// <returns></returns>
        PlayerModelErr CallBackRoleWithCoin();
        /// <summary>
        /// 使用复活卡复活
        /// </summary>
        /// <returns></returns>
        PlayerModelErr CallBackRoleWithCard();
        /// <summary>
        /// 倒计时结束增加生命
        /// </summary>
        /// <returns></returns>
        PlayerModelErr UpdateHeart();
        /// <summary>
        /// 获取错误描述
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        string GetErrorDes(PlayerModelErr err);

    }
}

