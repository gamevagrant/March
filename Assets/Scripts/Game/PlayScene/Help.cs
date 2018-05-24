using qy;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public static Help instance;

    [Header("Variables")]
    public int step;
    public GameObject current;

    [Header("Check")]
    public bool help;
    public bool onMap;

    private const string GuidePrefab = "GuideGenerater";
    private GuideController guideController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Image image = GetComponent<Image>();
            Destroy(image);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // show help
        if (LevelLoader.instance.level == 1  // match 3
            || LevelLoader.instance.level == 2  // match 4
            || LevelLoader.instance.level == 3  // match bomb
            || LevelLoader.instance.level == 4  // match 5
            || LevelLoader.instance.level == 5 // match 2 special treats
            || LevelLoader.instance.level == 6 //grass
            || LevelLoader.instance.level == 7 //grass
            || LevelLoader.instance.level == 8 //cage
            || LevelLoader.instance.level == 9 //铲子
            || LevelLoader.instance.level == 12 //草莓
            || LevelLoader.instance.level == 18 //面包
            )
        {
            help = true;
        }
        else
        {
            help = false;
            gameObject.SetActive(false);
        }
    }

    private string GetGuidePath(int level, int step)
    {
        return string.Format("PlayGuide/Guide_{0}_{1}", level, step);
    }

    public void Show()
    {
        GameObject prefab = null;

        Debug.Log("step:" + step);

        if (LevelLoader.instance.level == 1)
        {
            if (step == 2)
            {
                step = 0;
                SelfDisactive();
            }
            else
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 2)
        {
            if (step < 5)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 3)
        {
            if (step < 5)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 4)
        {
            if (step < 5)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 5)
        {
            if (step == 2)
            {
                step = 0;
                SelfDisactive();
            }
            else
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }

        }
        else if (LevelLoader.instance.level == 6)
        {
            if (step < 2)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 7)
        {
            if (step < 4)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
            else if (step == 4)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 8)
        {
            if (step < 1)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
            else if (step == 1)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 9)
        {
            if (step < 2)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
        }
        else if (LevelLoader.instance.level == 12)
        {
            if (step < 1)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);
                prefab = guideController.gameObject;
            }
            else if (step == 1)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 18)
        {
            if (step < 1)
            {
                step++;
                var guide = Instantiate(Resources.Load<GameObject>(GuidePrefab), transform.parent);
                guideController = guide.GetComponent<GuideController>();
                guideController.GuideText =
                    Instantiate(Resources.Load(GetGuidePath(LevelLoader.instance.level, step))) as TextAsset;
                guideController.Generate();
                guideController.name = string.Format("Level {0} Step {1}", LevelLoader.instance.level, step);

                prefab = guideController.gameObject;
            }
        }

        if (prefab != null)
        {
            if (step != 0)
            {
                //NetManager.instance.MakePointInGuide(LevelLoader.instance.level, step);//引导打点
                GameMainManager.Instance.netManager.MakePointInGuide(LevelLoader.instance.level, step, (ret, res) => { });
            }


            prefab.gameObject.transform.SetParent(gameObject.transform);
            prefab.GetComponent<RectTransform>().localScale = Vector3.one;

            current = prefab;
        }
    }

    public void Hide()
    {
        if (LevelLoader.instance.level == 1 ||
            LevelLoader.instance.level == 2 ||
            LevelLoader.instance.level == 3 ||
            LevelLoader.instance.level == 4 ||
            LevelLoader.instance.level == 5 ||
            LevelLoader.instance.level == 6 ||
            LevelLoader.instance.level == 7 ||
            LevelLoader.instance.level == 8 ||
            LevelLoader.instance.level == 9
            )
        {
            if (current != null)
            {
                current.SetActive(false);
            }
        }
    }

    public void HideOnSwapBack()
    {
        if (LevelLoader.instance.level == 1 && step == 2
            || LevelLoader.instance.level == 2
            || LevelLoader.instance.level == 3)
        {
            step = 0;
            SelfDisactive();
        }
    }

    public void SelfDisactive()
    {
        if (GameObject.Find("Board"))
        {
            GameObject.Find("Board").GetComponent<Board>().Hint();
        }

        help = false;

        gameObject.SetActive(false);
    }
}
