using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using qy;

public class UseGuide : MonoBehaviour
{
    private GameObject[] guides;
    private bool isGuided = false;
    private Transform guide;
    private string currentGuideStep;
    private bool isGuideRight;
    private int guideIndex = 0;
    //guidesetup _guidesetup;
    private string[] content = new string[]
    {
        "我们的领地被占领了",
        "让我们夺回属于我们的领地",
        "开始行动吧",
    };

    //private List<GuideItem> guideItems;
    private void Awake()
    {
        guide = transform.Find("Guide");
        guides = new GameObject[guide.childCount];
        //guidesetup guidesetup = DefaultConfig.getInstance().GetConfigByType<guidesetup>();
        //guideItems = guidesetup._guidesetup.guidesetups;

        for (int i = 0; i < guide.childCount; i++)
        {
            guides[i] = guide.GetChild(i).gameObject;
        }
        if (PlayerPrefs.HasKey("Guide"))
        {
            if (PlayerPrefs.GetString("Guide") == "Y")
            {
                isGuided = true;
                return;
            }
        }
        if (PlayerPrefs.GetString("GuideOnSecond") == "Y")
        {
            ShowGuide("1");
        }
        else
        {
            ShowGuideNext();
            //guide.transform.Find("StartGuidePage1").gameObject.SetActive(true);
        }
    }
    private void ShowGuideNext()
    {
        if (guideIndex >= 2)
        {
            ShowGuide("1");
            return;
        }
        bool test = guideIndex % 2 == 0;

        string prefabPath = "Guide/Guide" + (test ? "Right" : "Left");

        GameObject guide = Instantiate(Resources.Load<GameObject>(prefabPath),transform);
        //guide.transform.Find("Text").GetComponent<Text>().text = content[guideIndex];
        //guide.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey(guideItems[guideIndex].dialogue);
        guide.transform.Find("Text").GetComponent<Text>().text = GameMainManager.Instance.configManager.guideSetupConfig.GetItem(guideIndex.ToString()).dialogue;
        guide.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            guide.SetActive(false);
            ShowGuideNext();
        });

        guideIndex++;
    }
    public void ShowGuide(string guideName)
    {
        if (isGuided) return;
        currentGuideStep = guideName;
        if (guideName == "5")
        {
#if UNITY_ANDROID
            guideName = "6";
#endif

#if UNITY_EDITOR
            guideName = "5";
#endif
        }
        for (int i = 0; i < guides.Length; i++)
        {
            if (guides[i].name.Equals(guideName))
            {
                if (guideName == ("6") || guideName == ("5")) PlayerPrefs.SetString("GuideOnSecond", "Y");

                if (guideName == ("3") && PlayerPrefs.GetString("GuideOnSecond") == "Y")
                {
                    PlayerPrefs.SetString("Guide", "Y");
                    isGuided = true;
                    return;
                }
                guides[i].SetActive(true);


                guide.SetAsLastSibling();
            }
            else
            {
                guides[i].SetActive(false);
            }
        }
    }

}
