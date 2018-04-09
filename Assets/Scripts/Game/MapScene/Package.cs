using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour 
{
    public Text singleBreaker;
    public Text rowBreaker;
    public Text colBreaker;
    public Text rainbowBreaker;
    public Text ovenBreaker;

    public Text beginFiveMoves;
    public Text beginRainbow;
    public Text beginBombBreaker;

    void Start()
    {
        singleBreaker.text = "x" + GameData.instance.GetSingleBreaker().ToString();
        rowBreaker.text = "x" + GameData.instance.GetRowBreaker().ToString();
        colBreaker.text = "x" + GameData.instance.GetColumnBreaker().ToString();
        rainbowBreaker.text = "x" + GameData.instance.GetRainbowBreaker().ToString();
        ovenBreaker.text = "x" + GameData.instance.GetOvenBreaker().ToString();

        beginFiveMoves.text = "x" + GameData.instance.GetBeginFiveMoves().ToString();
        beginRainbow.text = "x" + GameData.instance.GetBeginRainbow().ToString();
        beginBombBreaker.text = "x" + GameData.instance.GetBeginBombBreaker().ToString();
    }

    public void ButtonClickAudio()
    {
        AudioManager.instance.ButtonClickAudio();
    }
}
