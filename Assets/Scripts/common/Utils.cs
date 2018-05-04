using Assets.Scripts.Common;
using System.Text.RegularExpressions;
using UnityEngine;

public class Utils : MonoSingleton<Utils>
{
    public string DeviceId;
    public string StoryHeadId;

    public const string A = "^[0-9]+$";//纯数字检测
    public const string B = "^[A-Za-z0-9]+$"; //.数字或英文
    public const string C = "^[\u4e00-\u9fa5]+$"; //纯汉字

    protected override void Init()
    {
        base.Init();

        DeviceId = SystemInfo.deviceUniqueIdentifier;
    }

    /// <summary>
    /// 获取当前设备id
    /// </summary>
    /// <returns></returns>
    public string getDeviceID()
    {
        return DeviceId;
    }

    public bool isValid(string _content)
    {
        Regex reg = new Regex(C);
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
        var length = _content.Length;
        if (3 <= length && length <= 16)
            return true;
        return false;
    }

}
