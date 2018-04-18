using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace qy
{
    public interface IPlayerModel
    {
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="questId">下一个任务</param>
        /// <returns></returns>
        int QuestComplate(string questId);
        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        int UseProp(string itemID, int count);
        /// <summary>
        /// 购买道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        int BuyProp(string itemId, int num);
        /// <summary>
        /// 开始任务
        /// </summary>
        /// <returns></returns>
        int StartLevel();
        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="level">关卡数</param>
        /// <param name="result">是否胜利</param>
        /// <param name="step">使用步数</param>
        /// <param name="wingold">非固定奖励</param>
        /// <returns></returns>
        int EndLevel(int level, bool result, int step, int wingold);
        /// <summary>
        /// 购买生命
        /// </summary>
        /// <returns></returns>
        int BuyHeart();
        /// <summary>
        /// 购买 再来五步
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        int BuyFiveMore(int step);
        /// <summary>
        /// 更改昵称
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        int ModifyNickName(string nickName);


    }
}

