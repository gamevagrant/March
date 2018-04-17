using Assets.Scripts.Common;
using MiniJSON;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : MonoSingleton<GameData>
{
    [Header("Data")]
    public int playerCoin;
    public int openedLevel;

    [Header("")]
    public int singleBreaker;
    public int rowBreaker;
    public int columnBreaker;
    public int rainbowBreaker;
    public int ovenBreaker;

    [Header("")]
    public int beginFiveMoves;
    public int beginRainbow;
    public int beginBombBreaker;

    public List<Dictionary<string, object>> levelStatistics = new List<Dictionary<string, object>>();

    protected override void Init()
    {
        base.Init();

        if (LoadGameData() == null)
        {
            SaveGameData(PrepareGameData());
        }
    }

    #region Load

    string LoadGameData()
    {
        //Debug.Log(Application.persistentDataPath + "/" + Configure.game_data);

        if (File.Exists(Application.persistentDataPath + "/" + Configure.game_data))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + Configure.game_data, FileMode.Open);

            string jsonString = (string)bf.Deserialize(file);

            file.Close();

            Dictionary<string, object> dict = Json.Deserialize(jsonString) as Dictionary<string, object>;

            playerCoin = int.Parse(dict[Configure.player_coin].ToString());
            openedLevel = int.Parse(dict[Configure.opened_level].ToString());
            openedLevel = (openedLevel > 0) ? openedLevel : 1;
            singleBreaker = int.Parse(dict[Configure.single_breaker].ToString());
            rowBreaker = int.Parse(dict[Configure.row_breaker].ToString());
            columnBreaker = int.Parse(dict[Configure.column_breaker].ToString());
            rainbowBreaker = int.Parse(dict[Configure.rainbow_breaker].ToString());
            ovenBreaker = int.Parse(dict[Configure.oven_breaker].ToString());
            beginFiveMoves = int.Parse(dict[Configure.begin_five_moves].ToString());
            beginRainbow = int.Parse(dict[Configure.begin_rainbow].ToString());
            beginBombBreaker = int.Parse(dict[Configure.begin_bomb_breaker].ToString());

            List<object> list = (List<object>)dict[Configure.level_statistics];
            foreach (object t in list)
            {
                Dictionary<string, object> d = (Dictionary<string, object>)t;

                levelStatistics.Add(d);
            }

            //Debug.Log("Serialized: " + jsonString);

            return jsonString;
        }

        return null;
    }

    #endregion

    #region Save

    void SaveGameData(string jsonString)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/" + Configure.game_data);

        bf.Serialize(file, jsonString);

        file.Close();
    }

    string PrepareGameData()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (openedLevel == 0) openedLevel = 1;

        // Test open all levels
        //if (openedLevel == 0) openedLevel = 84;

        // Test max coins
        //if (playerCoin == 0) playerCoin = 99999;

        dict.Add(Configure.player_coin, playerCoin);
        dict.Add(Configure.opened_level, openedLevel);
        dict.Add(Configure.single_breaker, singleBreaker);
        dict.Add(Configure.row_breaker, rowBreaker);
        dict.Add(Configure.column_breaker, columnBreaker);
        dict.Add(Configure.rainbow_breaker, rainbowBreaker);
        dict.Add(Configure.oven_breaker, ovenBreaker);
        dict.Add(Configure.begin_five_moves, beginFiveMoves);
        dict.Add(Configure.begin_rainbow, beginRainbow);
        dict.Add(Configure.begin_bomb_breaker, beginBombBreaker);

        dict.Add(Configure.level_statistics, levelStatistics);

        return Json.Serialize(dict);
    }

    #endregion

    #region Level

    public int GetOpendedLevel()
    {
        return openedLevel;
    }

    public void SaveOpendedLevel(int level)
    {
        openedLevel = level;

        SaveGameData(PrepareGameData());
    }

    public int GetLevelScore(int level)
    {
        foreach (Dictionary<string, object> statistics in levelStatistics)
        {
            if (int.Parse(statistics[Configure.level_number].ToString()) == level)
            {
                return int.Parse(statistics[Configure.level_score].ToString());
            }
        }

        return 0;
    }

    public int GetLevelStar(int level)
    {
        foreach (Dictionary<string, object> statistics in levelStatistics)
        {
            if (int.Parse(statistics[Configure.level_number].ToString()) == level)
            {
                return int.Parse(statistics[Configure.level_star].ToString());
            }
        }

        return 0;
    }

    public void SaveLevelStatistics(int level, int score, int star)
    {
        foreach (Dictionary<string, object> statistics in levelStatistics)
        {
            if (int.Parse(statistics[Configure.level_number].ToString()) == level)
            {
                // only update if new score/star is greater then the old one
                if (int.Parse(statistics[Configure.level_score].ToString()) < score)
                {
                    statistics[Configure.level_score] = score;
                }

                if (int.Parse(statistics[Configure.level_star].ToString()) < star)
                {
                    statistics[Configure.level_star] = star;
                }

                SaveGameData(PrepareGameData());

                return;
            }
        }

        // if don't find a old record then create a new one
        Dictionary<string, object> stats = new Dictionary<string, object>();

        stats.Add(Configure.level_number, level);
        stats.Add(Configure.level_score, score);
        stats.Add(Configure.level_star, star);

        levelStatistics.Add(stats);

        SaveGameData(PrepareGameData());
    }

    #endregion

    #region Data

    public int GetPlayerCoin()
    {
        return playerCoin;
    }

    public void SavePlayerCoin(int coin)
    {
        playerCoin = coin;

        SaveGameData(PrepareGameData());
    }

    public int GetBeginFiveMoves()
    {
        return beginFiveMoves;
    }

    public void SaveBeginFiveMoves(int number)
    {
        beginFiveMoves = number;

        SaveGameData(PrepareGameData());
    }

    public int GetBeginRainbow()
    {
        return beginRainbow;
    }

    public void SaveBeginRainbow(int number)
    {
        beginRainbow = number;

        SaveGameData(PrepareGameData());
    }

    public int GetBeginBombBreaker()
    {
        return beginBombBreaker;
    }

    public void SaveBeginBombBreaker(int number)
    {
        beginBombBreaker = number;

        SaveGameData(PrepareGameData());
    }

    public int GetSingleBreaker()
    {
        return singleBreaker;
    }

    public void SaveSingleBreaker(int number)
    {
        singleBreaker = number;

        SaveGameData(PrepareGameData());
    }

    public int GetRowBreaker()
    {
        return rowBreaker;
    }

    public void SaveRowBreaker(int number)
    {
        rowBreaker = number;

        SaveGameData(PrepareGameData());
    }

    public int GetColumnBreaker()
    {
        return columnBreaker;
    }

    public void SaveColumnBreaker(int number)
    {
        columnBreaker = number;

        SaveGameData(PrepareGameData());
    }

    public int GetRainbowBreaker()
    {
        return rainbowBreaker;
    }

    public void SaveRainbowBreaker(int number)
    {
        rainbowBreaker = number;

        SaveGameData(PrepareGameData());
    }

    public int GetOvenBreaker()
    {
        return ovenBreaker;
    }

    public void SaveOvenBreaker(int number)
    {
        ovenBreaker = number;

        SaveGameData(PrepareGameData());
    }

    #endregion
}
