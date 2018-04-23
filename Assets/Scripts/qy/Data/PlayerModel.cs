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
        

        public PlayerModelErr BuyFiveMore(int step)
        {
            GameMainManager.Instance.netManager.EliminateLevelFiveMore(step, (ret, res) => {
                
            });
            playerData.dirty = true;
            int cost = GameMainManager.Instance.configManager.settingConfig.GetPriceWithStep(step);
            List<PropItem> props = GameMainManager.Instance.configManager.settingConfig.GetBonusItemBagWithStep(step); 
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return PlayerModelErr.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            foreach(PropItem prop in props)
            {
                playerData.AddPropItem(prop.id,prop.count);
            }
            SaveData();

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr BuyHeart()
        {
            GameMainManager.Instance.netManager.BuyHeart((ret, res) => {
                
            });
            playerData.dirty = true;
            int cost = GameMainManager.Instance.configManager.settingConfig.livesPrice;
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return PlayerModelErr.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            playerData.heartNum = GameMainManager.Instance.configManager.settingConfig.maxLives;
            playerData.recoveryLeftTime = 0;
            SaveData();

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr BuyProp(string itemId, int num)
        {
            GameMainManager.Instance.netManager.BuyItem(itemId,num,(ret, res) => {

            });
            playerData.dirty = true;
            PropItem item = GameMainManager.Instance.configManager.propsConfig.GetItem(itemId);
            if(item==null)
            {
                Debug.Log("物品不存在");
                return PlayerModelErr.PROP_ID_ERROR;
            }
            int cost = item.price * num;
            if(playerData.coinNum<cost)
            {
                Debug.LogError("金币不足！");
                return PlayerModelErr.NOT_ENOUGH_COIN;
            }

            playerData.coinNum -= cost;
            playerData.AddPropItem(itemId,num);
            SaveData();

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr EndLevel(int level, bool result, int step, int wingold)
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

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr ModifyNickName(string nickName)
        {
            GameMainManager.Instance.netManager.ModifyNickName(nickName, (ret, res) => {
                Debug.Log("===============修改姓名成功");
            });
            playerData.dirty = true;
            playerData.nickName = nickName;
            SaveData();
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr QuestComplate(out string storyID, string selectedID = "")
        {
            GameMainManager.Instance.netManager.ComplateQuestId(playerData.questId, (ret, res) => {

            });
            storyID = "";
            //检测完成任务条件
            config.QuestItem questItem = playerData.GetQuest();
            if (playerData.starNum < questItem.requireStar)
            {
                MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200011"));
                return PlayerModelErr.NOT_ENOUGH_STAR;
            }
            List<PropItem> needProps = questItem.requireItem;
            foreach(PropItem item in needProps)
            {
                PropItem haveItem = playerData.GetPropItem(item.id);
                int haveCount = haveItem==null?0: haveItem.count;
                if(haveCount < item.count)
                {
                    return PlayerModelErr.NOT_ENOUGH_PROP;
                }
            }
            //扣除完成任务物品
            playerData.starNum -= questItem.requireStar;
            foreach(PropItem item in needProps)
            {
                playerData.RemovePropItem(item.id,item.count);
            }
            MainScene.Instance.RefreshPlayerData();

            //更新下个任务
            if (questItem.type == config.QuestItem.QuestType.Main)
            {
                playerData.nextQuestId = questItem.gotoId;
                storyID = questItem.storyID;
            }
            else if (questItem.type == config.QuestItem.QuestType.Branch)
            {
                foreach(SelectItem item in questItem.selectList)
                {
                    if(item.id == selectedID)
                    {
                        playerData.nextQuestId = item.toQuestId;
                        storyID = item.storyID;
                        playerData.ability += item.ability;
                        break;
                    }
                }
            }
            else if (questItem.type == config.QuestItem.QuestType.Important)
            {
                if(playerData.survival<questItem.endingPoint.survival)
                {
                    //进入分支任务
                    playerData.nextQuestId = questItem.endingPoint.questID;
                    storyID = questItem.endingPoint.storyID;
                }
                else
                {
                    //进入普通任务
                    playerData.nextQuestId = questItem.gotoId;
                    storyID = questItem.storyID;
                }
            }

            playerData.questId = playerData.nextQuestId;
            playerData.dirty = true;
            
            SaveData();
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr StartLevel()
        {
            GameMainManager.Instance.netManager.LevelStart((ret, res) => {
                
            });
            playerData.dirty = true;
            if (playerData.heartNum<=0)
            {
                return PlayerModelErr.NOT_ENOUGH_HEART;
            }
            playerData.heartNum -= 1;
            SaveData();

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr UseProp(string itemID, int count)
        {
            GameMainManager.Instance.netManager.UseTools(itemID, count, (ret, res) =>
            {
                
            });
            playerData.dirty = true;
            PropItem prop = playerData.GetPropItem(itemID);
            if(prop==null || prop.count<count)
            {
                return PlayerModelErr.NOT_ENOUGH_PROP;
            }

            playerData.RemovePropItem(itemID, count);
            SaveData();
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr StartGameWithRole(string id)
        {
            playerData.dirty = true;
            RoleItem role = GameMainManager.Instance.configManager.roleConfig.GetItem(id).Clone();
            playerData.role = role;
            playerData.nextQuestId = role.questID;
            SaveData();
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr CallBackRoleWithCoin()
        {
            int cost = GameMainManager.Instance.configManager.settingConfig.callBackPrice;
            if(playerData.coinNum<cost)
            {
                return PlayerModelErr.NOT_ENOUGH_COIN;
            }
            playerData.coinNum -= cost;
            return StartGameWithRole(playerData.role.id);
        }

        public PlayerModelErr CallBackRoleWithCard()
        {
            return PlayerModelErr.NULL;
        }
       
        private void SaveData()
        {
            LocalDatasManager.playerData = playerData;
        }

    }
}

