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
            GameMainManager.Instance.netManager.EliminateLevelFiveMore(step, (ret, res) => {
                
            });
            playerData.dirty = true;
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
            GameMainManager.Instance.netManager.BuyHeart((ret, res) => {
                
            });
            playerData.dirty = true;
            int cost = GameMainManager.Instance.configManager.settingConfig.livesPrice;
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return (int)ErrType.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            playerData.heartNum = GameMainManager.Instance.configManager.settingConfig.maxLives;
            playerData.recoveryLeftTime = 0;
            SaveData();

            return (int)ErrType.NULL;
        }

        public int BuyProp(string itemId, int num)
        {
            GameMainManager.Instance.netManager.BuyItem(itemId,num,(ret, res) => {

            });
            playerData.dirty = true;
            PropItem item = GameMainManager.Instance.configManager.propsConfig.GetItem(itemId);
            if(item==null)
            {
                Debug.Log("物品不存在");
                return (int)ErrType.PROP_ID_ERROR;
            }
            int cost = item.price * num;
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return (int)ErrType.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            playerData.AddPropItem(itemId,num);
            SaveData();

            return (int)ErrType.NULL;
        }

        public int EndLevel(int level, bool result, int step, int wingold)
        {
            GameMainManager.Instance.netManager.LevelEnd(level,result?1:0,step, wingold, (ret, res) => {
                
            });
            playerData.dirty = true;
            if (result)
            {
                MatchLevelItem matchItem = GameMainManager.Instance.configManager.matchLevelConfig.GetItem((1000000+level).ToString());
                playerData.coinNum += wingold + matchItem.coin;
                playerData.starNum += matchItem.star;
                playerData.heartNum += 1;
                playerData.eliminateLevel += 1;

                foreach(PropItem prop in matchItem.itemReward)
                {
                    playerData.AddPropItem(prop.id,prop.count);
                }
                SaveData();
            }

            return (int)ErrType.NULL;
        }

        public int ModifyNickName(string nickName)
        {
            GameMainManager.Instance.netManager.ModifyNickName(nickName, (ret, res) => {
                Debug.Log("===============修改姓名成功");
            });
            playerData.dirty = true;
            playerData.nickName = nickName;
            SaveData();
            return (int)ErrType.NULL;
        }

        public int QuestComplate(string questId)
        {
            GameMainManager.Instance.netManager.UpdateQuestId(questId,(ret,res)=> {
                
            });
            playerData.dirty = true;
            config.QuestItem quest = GameMainManager.Instance.configManager.questConfig.GetItem(questId);
            if(quest==null)
            {
                return (int)ErrType.QUEST_ID_ERROR;
            }
            playerData.questId = questId;
            SaveData();
            return (int)ErrType.NULL;
        }

        public int StartLevel()
        {
            GameMainManager.Instance.netManager.LevelStart((ret, res) => {
                
            });
            playerData.dirty = true;
            if (playerData.heartNum<=0)
            {
                return (int)ErrType.NOT_ENOUGH_HEART;
            }
            playerData.heartNum -= 1;
            SaveData();

            return (int)ErrType.NULL;
        }

        public int UseProp(string itemID, int count)
        {
            GameMainManager.Instance.netManager.UseTools(itemID, count, (ret, res) =>
            {
                
            });
            playerData.dirty = true;
            PropItem prop = playerData.GetPropItem(itemID);
            if(prop==null || prop.count<count)
            {
                return (int)ErrType.NOT_ENOUGH_PROP;
            }

            playerData.RemovePropItem(itemID, count);

            return (int)ErrType.NULL;
        }
       

        private void SaveData()
        {
            LocalDatasManager.playerData = playerData;
        }

    }
}

