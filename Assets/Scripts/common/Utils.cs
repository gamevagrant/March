using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using System.Text.RegularExpressions;

public class Utils : MonoSingleton<Utils>
{

    public string A = "^[0-9]+$";//纯数字检测
    public string B = "^[A-Za-z0-9]+$"; //.数字或英文
    public string C = "^[\u4e00-\u9fa5]+$"; //纯汉字
     
    /// <summary>
    /// 获取当前设备id
    /// </summary>
    /// <returns></returns>
    public string getDeviceID()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        deviceId = "20202020202";
        return deviceId;
    }

    public bool isValid(string _content)
    {
        Regex reg =  new Regex(C);
        return reg.IsMatch(_content);
    }

    public bool isMatchNumberOrCharacter(string _content)
    {
        Regex reg = new Regex(B);
        return reg.IsMatch(_content);
    }

    public bool isMatchChinese(string _content)
    {
        Regex reg = new Regex(C);
        return reg.IsMatch(_content);
    }

    public bool isStrLengthValid(string _content)
    {
        int length = _content.Length;
        if (3 <= length && length<= 6)
            return true;
        else
            return false;
    }

}
