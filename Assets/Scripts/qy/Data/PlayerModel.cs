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
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
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
            playerData.hertTimestamp = 0;
            SaveData();
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
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
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
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
            
            storyID = "";
            qy.config.Ability ability = new config.Ability();
            //检测完成任务条件
            config.QuestItem questItem = playerData.GetQuest();

            //完成过的任务无消耗 无经验
            if (!playerData.complatedQuests.ContainsKey(questItem.id))
            {
                if (playerData.starNum < questItem.requireStar)
                {
                    return PlayerModelErr.NOT_ENOUGH_STAR;
                }
                List<PropItem> needProps = questItem.requireItem;
                foreach (PropItem item in needProps)
                {
                    PropItem haveItem = playerData.GetPropItem(item.id);
                    int haveCount = haveItem == null ? 0 : haveItem.count;
                    if (haveCount < item.count)
                    {
                        return PlayerModelErr.NOT_ENOUGH_PROP;
                    }
                }

                //扣除完成任务物品
                playerData.starNum -= questItem.requireStar;
                foreach (PropItem item in needProps)
                {
                    playerData.RemovePropItem(item.id, item.count);
                }
                playerData.complatedQuests.Add(questItem.id,0);

                //获得奖励物品
                foreach (PropItem item in questItem.prize)
                {

                    bool isGet = Random.Range(0, 100)<item.rate;
                    if(isGet)
                    {
                        playerData.AddPropItem(item.id, item.count);
                    }
                    
                }

                //获得经验值
                playerData.totalExp += questItem.exp;
                playerData.currExp += questItem.exp;
                //判断升级
                config.LevelItem levelItem = GameMainManager.Instance.configManager.levelConfig.GetItem(playerData.level);
                while (levelItem != null && playerData.currExp >= levelItem.exp)
                {
                    playerData.level++;
                    playerData.currExp -= levelItem.exp;
                    levelItem = GameMainManager.Instance.configManager.levelConfig.GetItem(playerData.level);
                }
            }

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
                        ability = item.ability;
                       
                        string selectID = questItem.id + "_" + item.id;
                        if(!playerData.selectedItems.ContainsKey(selectID))
                        {
                            playerData.selectedItems.Add(selectID,0);
                        }
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
            if(string.IsNullOrEmpty(playerData.nextQuestId))
            {
                return PlayerModelErr.QUEST_ID_ERROR;
            }

            //增加属性值
            config.Ability addAbilty = config.Ability.GetAddAbilty(playerData.ability, ability);
            playerData.ability += addAbilty;

            GameMainManager.Instance.netManager.ComplateQuestId(playerData.questId, addAbilty, (ret, res) => {

            });
            playerData.questId = playerData.nextQuestId;
            
            //标记角色状态
            config.QuestItem nextQuest = playerData.GetQuest();
            if (nextQuest.type == config.QuestItem.QuestType.Ending)
            {
                GameMainManager.Instance.netManager.ComplateQuestId(nextQuest.id, new config.Ability(), (ret, res) =>{});
                GameMainManager.Instance.netManager.EndingRole(nextQuest.endingType, (ret, res) => { });
                switch (nextQuest.endingType)
                {
                    case 1://死亡
                        playerData.SetRoleState(playerData.role.id, PlayerData.RoleState.Dide);
                        break;
                    case 2://通关
                        playerData.SetRoleState(playerData.role.id,PlayerData.RoleState.Pass);
                        break;
                   
                }
                
            }

            playerData.dirty = true;
            SaveData();
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);

            if(questItem.chapter!=nextQuest.chapter)
            {
                Messenger.Broadcast<string>(ELocalMsgID.LoadScene, "Film");
            }

            return PlayerModelErr.NULL;
        }

        public PlayerModelErr QuestComplateCondition()
        {
            config.QuestItem questItem = playerData.GetQuest();
            if (!playerData.complatedQuests.ContainsKey(questItem.id))
            {
                if (playerData.starNum < questItem.requireStar)
                {
                    return PlayerModelErr.NOT_ENOUGH_STAR;
                }
                List<PropItem> needProps = questItem.requireItem;
                foreach (PropItem item in needProps)
                {
                    PropItem haveItem = playerData.GetPropItem(item.id);
                    int haveCount = haveItem == null ? 0 : haveItem.count;
                    if (haveCount < item.count)
                    {
                        return PlayerModelErr.NOT_ENOUGH_PROP;
                    }
                }
            }
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
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr StartGameWithRole(string id)
        {
            GameMainManager.Instance.netManager.SwitchRole(id,(ret,res)=> { });
            if(playerData.GetRoleState(id) == PlayerData.RoleState.Dide)
            {
                return PlayerModelErr.ROLE_IS_DIE;
            }

            SwitchRole(id);
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr CallBackRoleWithCoin(string id)
        {
            GameMainManager.Instance.netManager.RecoverRole(id,0, (ret, res) => { });
            int cost = GameMainManager.Instance.configManager.settingConfig.callBackPrice;
            if(playerData.coinNum<cost)
            {
                return PlayerModelErr.NOT_ENOUGH_COIN;
            }
            playerData.coinNum -= cost;
            playerData.SetRoleState(id, PlayerData.RoleState.Normal);
            SwitchRole(id);
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
            playerData.dirty = true;
            return PlayerModelErr.NULL;
        }

        private void SwitchRole(string id)
        {
            RoleItem role = GameMainManager.Instance.configManager.roleConfig.GetItem(id).Clone();
            playerData.role = role;
            playerData.nextQuestId = role.questID;
            playerData.SetRoleState(id, PlayerData.RoleState.Normal);
            SaveData();
        }

        public PlayerModelErr CallBackRoleWithCard(string id)
        {
            GameMainManager.Instance.netManager.RecoverRole(id, 1, (ret, res) => { });
            Messenger.Broadcast(ELocalMsgID.RefreshBaseData);
            playerData.dirty = true;
            return PlayerModelErr.NULL;
        }

        public PlayerModelErr UpdateHeart()
        {
            int maxHeart = GameMainManager.Instance.configManager.settingConfig.maxLives;
            if (playerData.heartNum>= maxHeart)
            {
                return PlayerModelErr.HEART_IS_FULL;
            }

            long now = GameUtils.DateTimeToTimestamp(System.DateTime.Now);
            long last = playerData.hertTimestamp;
            long space = GameMainManager.Instance.configManager.settingConfig.livesRecoverTime * 60;
            if (now<last)
            {
                return PlayerModelErr.COUNT_DOWN_NOT_END;
            }
            int addHeart = (int)((now - last) / space)+1;
            playerData.heartNum = Mathf.Min(maxHeart, playerData.heartNum + addHeart);
            playerData.hertTimestamp += addHeart * space;
            

            return PlayerModelErr.NULL;
        }

        public string GetErrorDes(PlayerModelErr err)
        {
            string str = "";
            switch(err)
            {
                case PlayerModelErr.NOT_ENOUGH_COIN:
                    str = LangrageManager.Instance.GetItemWithID("200044");
                    break;
                case PlayerModelErr.NOT_ENOUGH_HEART:
                    str = LangrageManager.Instance.GetItemWithID("200043");
                    break;
                case PlayerModelErr.NOT_ENOUGH_PROP:
                    str = LangrageManager.Instance.GetItemWithID("200042");
                    break;
                case PlayerModelErr.NOT_ENOUGH_STAR:
                    str = LangrageManager.Instance.GetItemWithID("200045");
                    break;
                case PlayerModelErr.PROP_ID_ERROR:
                    break;
                case PlayerModelErr.QUEST_ID_ERROR:
                    break;
                case PlayerModelErr.ROLE_IS_DIE:
                    break;
            }
            return str;
        }
       
        private void SaveData()
        {
            LocalDatasManager.playerData = playerData;
        }

    }
}

