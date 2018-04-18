using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;

namespace qy
{
    public class PlayerModel : IPlayerModel
    {
        private PlayerData playerData;
        public PlayerModel(PlayerData playerData)
        {
            this.playerData = playerData;
        }
        public enum ErrType:int
        {
            NULL=0,
            NOT_ENOUGH_COIN,
            NOT_ENOUGH_HEART,
            NOT_ENOUGH_PROP,
            QUEST_ID_ERROR,
            PROP_ID_ERROR,
        }

        public int BuyFiveMore(int step)
        {
            GameMainManager.Instance.netManager.EliminateLevelFiveMore(step, (ret, res) => { });

            int cost = GameMainManager.Instance.configManager.settingConfig.GetPriceWithStep(step);
            List<PropItem> props = GameMainManager.Instance.configManager.settingConfig.GetBonusItemBagWithStep(step); 
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return (int)ErrType.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            foreach(PropItem prop in props)
            {
                playerData.AddPropItem(prop.id,prop.count);
            }
            SaveData();

            return (int)ErrType.NULL;
        }

        public int BuyHeart()
        {
            throw new System.NotImplementedException();
        }

        public int BuyProp(string itemId, int num)
        {
            throw new System.NotImplementedException();
        }

        public int EndLevel(int level, bool result, int step, int wingold)
        {
            throw new System.NotImplementedException();
        }

        public int ModifyNickName(string nickName)
        {
            throw new System.NotImplementedException();
        }

        public int QuestComplate(string questId)
        {
            throw new System.NotImplementedException();
        }

        public int StartLevel()
        {
            throw new System.NotImplementedException();
        }

        public int UseProp(string itemID, int count)
        {
            throw new System.NotImplementedException();
        }

        private void SaveData()
        {
            LocalDatasManager.playerData = playerData;
        }
    }
}

