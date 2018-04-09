using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsBuyConfig
{
    class _stepbuyconfig
    {
        public Dictionary<string, int> bonusItems = new Dictionary<string, int>();

        public Dictionary<string, int> bonusItemsBag = new Dictionary<string, int>();

        public int price = 0;
    }

    private List<_stepbuyconfig> stepsConfigList;

    private int addSteps;

    public StepsBuyConfig()
    {
        init();
    }

    private void init()
    {
        addSteps = 0;
        stepsConfigList = new List<_stepbuyconfig>();

        var _stepsBuyData = DefaultConfig.getInstance().GetConfigByType<setting>().GetDictionaryByID("stepsBuy");

        addSteps = Int32.Parse(_stepsBuyData["addSteps"]);

        string[] _priceData = _stepsBuyData["price"].Split('|');

        string[] _itemEffectData= _stepsBuyData["bonusItems"].Split('|');

        string[] _itemBagData = _stepsBuyData["bonusItemsBag"].Split('|');

#if UNITY_EDITOR
        if (_priceData.Length != _itemEffectData.Length || _priceData.Length != _itemBagData.Length)
        {
            Debug.LogError("setting 配表错误 每项间存在数量不匹配");
            return;
        }
#endif

        for (int i = 0; i < _priceData.Length; i++)
        {
            _stepbuyconfig _config = new _stepbuyconfig();
            _config.price = Int32.Parse(_priceData[i]);

            if (_itemEffectData[i] != "0")
            {
                var _temp1 = _itemEffectData[i].Split(',');
                foreach (var tmp in _temp1)
                {
                    var _itemData = tmp.Split(':');
#if UNITY_EDITOR
                    if (_itemData.Length != 2)
                    {
                        Debug.LogError("setting 表 bonusItems字段 配置错误 存在格式不正确的字段！");
                        return;
                    }
                    if (Int32.Parse(_itemData[1]) < 0)
                    {
                        Debug.LogError("setting 表 bonusItems字段 配置错误 存在负数量！");
                        return;
                    }
#endif
                    _config.bonusItems.Add(_itemData[0], Int32.Parse(_itemData[1]));
                }
            }

            if (_itemBagData[i] != "0")
            {
                var _temp2 = _itemBagData[i].Split(',');
                foreach (var tmp in _temp2)
                {
                    var _itemData = tmp.Split(':');
#if UNITY_EDITOR
                    if (_itemData.Length != 2)
                    {
                        Debug.LogError("setting 表 bonusItemsBag字段 配置错误 存在格式不正确的字段！");
                        return;
                    }
                    if (Int32.Parse(_itemData[1]) < 0)
                    {
                        Debug.LogError("setting 表 bonusItemsBag字段 配置错误 存在负数量！");
                        return;
                    }
#endif
                    _config.bonusItemsBag.Add(_itemData[0], Int32.Parse(_itemData[1]));
                }
            }


            stepsConfigList.Add(_config);

        }
    }

    /// <summary>
    /// 获取当前次数的价格 times为本关已经使用的次数
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public int GetPriceByTimes(int times)
    {
        if (times >= stepsConfigList.Count)
        {
            times = stepsConfigList.Count - 1;
        }

        return stepsConfigList[times].price;
    }

    /// <summary>
    /// 获取当前次数所有立即使用的物品效果 times为本关已经使用的次数
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public Dictionary<string, int> GetEffectDicByTimes(int times)
    {
        if (times >= stepsConfigList.Count)
        {
            times = stepsConfigList.Count - 1;
        }
    
        return stepsConfigList[times].bonusItems;
    }

    /// <summary>
    /// 获取当前次数所有加入背包的物品 times为本关已经使用的次数
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public Dictionary<string, int> GetItemBagDicByTimes(int times)
    {
        if (times >= stepsConfigList.Count)
        {
            times = stepsConfigList.Count - 1;
        }

        return stepsConfigList[times].bonusItemsBag;
    }

    public int GetAddSteps()
    {
        return addSteps;
    }

}
