using System;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    //public float timer;
    //public string exitDateTime;
    public float oneLifeRecoveryTime;
    public bool runTimer;

    // UI
    public Text lifeText;
    public Text timerText;

    // Use this for initialization
    void Start()
    {
        // set life for the first time
        if (PlayerPrefs.HasKey(PlayerPrefEnums.Life) == false)
        {
            //print("Set life first time");

            Configure.instance.life = Configure.instance.maxLife;

            // set text
            lifeText.text = Configure.instance.life.ToString();
            timerText.text = Configure.instance.life + "/" + Configure.instance.life;

            PlayerPrefs.SetInt(PlayerPrefEnums.Life, Configure.instance.life);
            PlayerPrefs.Save();
        }
        else
        {
            //print("Load from PlayerPrefs or Configure");
        }

        // load from splash screen
        if (Configure.instance.timer == 0)
        {
            //print("timer = 0 (load from PlayerPrefs)");

            // get exit date time
            Configure.instance.exitDateTime = PlayerPrefs.GetString(PlayerPrefEnums.ExitDateTime, new DateTime().ToString());

            // get timer
            Configure.instance.timer = PlayerPrefs.GetFloat(PlayerPrefEnums.Timer, 0f);

            // life
            Configure.instance.life = PlayerPrefs.GetInt(PlayerPrefEnums.Life, Configure.instance.maxLife);
        }
        else
        {
            //print("Load from Configure. Timer: " + Configure.instance.timer);
        }

        //print(
        //    "exit date time: " + Configure.instance.exitDateTime.ToString() + 
        //    " / timer: " + Configure.instance.timer.ToString() + 
        //    " / life: " + Configure.instance.life.ToString()
        //);

        // calculate one life recovery time
        oneLifeRecoveryTime = Configure.instance.lifeRecoveryHour * 60f * 60f + Configure.instance.lifeRecoveryMinute * 60f + Configure.instance.lifeRecoverySecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer == false)
        {
            if (Configure.instance.life < Configure.instance.maxLife)
            {
                if (CheckRecoveryTime())
                {
                    runTimer = true;
                }
            }
        }

        if (runTimer == true)
        {
            CalculateTimer(Time.deltaTime);
        }

        // update timerText
        if (Configure.instance.life < Configure.instance.maxLife)
        {
            var hour = Mathf.FloorToInt(Configure.instance.timer / 3600);
            var minute = Mathf.FloorToInt((Configure.instance.timer - hour * 3600) / 60);
            var second = Mathf.FloorToInt((Configure.instance.timer - hour * 3600) - minute * 60);

            if (Configure.instance.lifeRecoveryHour > 0)
            {
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
            }
            else
            {
                timerText.text = string.Format("{0:00}:{1:00}", minute, second);
            }
        }
        else
        {
            lifeText.text = Configure.instance.life.ToString();
            timerText.text = Configure.instance.life + "/" + Configure.instance.life;

            runTimer = false;
            Configure.instance.timer = 0;
        }
    }

    bool CheckRecoveryTime()
    {
        // check exit date time
        if (Configure.instance.exitDateTime == new DateTime().ToString())
        {
            //print("Exit date time is default");

            Configure.instance.exitDateTime = DateTime.Now.ToString();
        }
        else
        {
            //print("Exit data is not default");
        }

        // convert string to date time
        DateTime _exitDateTime = DateTime.Parse(Configure.instance.exitDateTime);

        if (DateTime.Now.Subtract(_exitDateTime).TotalSeconds > oneLifeRecoveryTime * (Configure.instance.maxLife - Configure.instance.life))
        {
            // enough time to recovery all the life
            Configure.instance.life = Configure.instance.maxLife;

            // update text
            lifeText.text = Configure.instance.life.ToString();

            // set timer
            Configure.instance.timer = 0f;

            return false;
        }
        else
        {
            //print("Recovery duration: " + (float)DateTime.Now.Subtract(_exitDateTime).TotalSeconds);

            CalculateTimer((float)DateTime.Now.Subtract(_exitDateTime).TotalSeconds);

            return true;
        }
    }

    void CalculateTimer(float duration)
    {
        if (Configure.instance.timer <= 0 && duration < 1)
        {
            Configure.instance.timer = oneLifeRecoveryTime;
        }

        if (Configure.instance.timer <= duration)
        {
            //print("Duration: " + (int)duration + " / Recovery time: " + (int)oneLifeRecoveryTime);

            // add one or more life
            if (duration < 1)
            {
                AddLife(1);

                Configure.instance.timer = oneLifeRecoveryTime;
            }
            else
            {
                if (duration >= oneLifeRecoveryTime)
                {
                    AddLife((int)duration / (int)oneLifeRecoveryTime);

                    Configure.instance.timer -= (int)duration % (int)oneLifeRecoveryTime;
                }
                else
                {
                    AddLife(1);

                    Configure.instance.timer = oneLifeRecoveryTime - (duration - Configure.instance.timer);
                }
            }
        }
        else
        {
            Configure.instance.timer -= duration;

            // update text
            lifeText.text = Configure.instance.life.ToString();
        }
    }

    public void AddLife(int count)
    {
        //print("Add life: " + count);

        Configure.instance.life += count;

        if (Configure.instance.life > Configure.instance.maxLife)
        {
            Configure.instance.life = Configure.instance.maxLife;
        }

        // update text
        lifeText.text = Configure.instance.life.ToString();
    }

    public void ReduceLife(int count)
    {
        //print("Reduce life");

        var life = Configure.instance.life;

        Configure.instance.life = (life - 1 < 0) ? 0 : life - 1;

        // update text
        lifeText.text = Configure.instance.life.ToString();

        // update exit date time
        Configure.instance.exitDateTime = DateTime.Now.ToString();
    }
}
