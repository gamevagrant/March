using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using qy;

public class Help : MonoBehaviour 
{
    public static Help instance = null;

    [Header("Variables")]
    public int step;
    public GameObject current;

    [Header("Check")]
    public bool help;
    public bool onMap;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
			Image image = GetComponent<Image> ();
			Destroy (image);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

	void Start () 
    {
        // Map scene
        if (onMap == true)
        {
            
        }
        // Play scene
        else
        {
            // show help
            if (LevelLoader.instance.level == 1  // match 3
                ||LevelLoader.instance.level == 2  // match 4
                ||LevelLoader.instance.level == 3  // match bomb
                ||LevelLoader.instance.level == 4  // match 5
                ||LevelLoader.instance.level == 5 // match 2 special treats
                ||LevelLoader.instance.level == 6 //grass
				||LevelLoader.instance.level == 7 //grass
                ||LevelLoader.instance.level == 8 //cage
                ||LevelLoader.instance.level == 9 //铲子
                ||LevelLoader.instance.level == 12 //草莓
                ||LevelLoader.instance.level == 18 //面包
                )
            {
                help = true;
				if (LevelLoader.instance.level == 9) {
					if (qy.GameMainManager.Instance.playerData.needShow9Help) {
                        qy.GameMainManager.Instance.playerData.needShow9Help = false;
					} else {
						help = false;
					}
				}
            }
            else
            {
                //SelfDisactive();

                help = false;

                gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        // don't show help when level is passed
//        if (GameData.instance.GetOpendedLevel() > LevelLoader.instance.level)
//        {
//            SelfDisactive();
//            return;
//        }

        GameObject prefab = null;

        Debug.Log("step:" + step);

		if (LevelLoader.instance.level == 1) {
			if (step == 0) {
				step = 1;
				prefab = Instantiate (Resources.Load (Configure.Level1Step1 ())) as GameObject;
				prefab.name = "Level 1 Step 1";
			} else if (step == 1) {
				step = 2;
				prefab = Instantiate (Resources.Load (Configure.Level1Step2 ())) as GameObject;
				prefab.name = "Level 1 Step 2";

			} else if (step == 2) {
				step = 0;
				SelfDisactive ();
			}
		} else if (LevelLoader.instance.level == 2) {
			if (step == 0) {
				step = 1;
				prefab = Instantiate (Resources.Load (Configure.Level2Step1 ())) as GameObject;
				prefab.name = "Level 2 Step 1";
			} else if (step == 1) {
				step = 2;
				prefab = Instantiate (Resources.Load (Configure.Level2Step2 ())) as GameObject;
				prefab.name = "Level 2 Step 2";
			} else if (step == 2) {
				step = 3;
				prefab = Instantiate (Resources.Load (Configure.Level2Step3 ())) as GameObject;
				prefab.name = "Level 2 Step 3";
			} else if (step == 3) {
				step = 4;
				prefab = Instantiate (Resources.Load (Configure.Level2Step4 ())) as GameObject;
				prefab.name = "Level 2 Step 4";
			} else if (step == 4) {
				step = 5;
				prefab = Instantiate (Resources.Load (Configure.Level2Step5 ())) as GameObject;
				prefab.name = "Level 2 Step 5";
			}
		} else if (LevelLoader.instance.level == 3) {
			if (step == 0) {
				step = 1;
				prefab = Instantiate (Resources.Load (Configure.Level3Step1 ())) as GameObject;
				prefab.name = "Level 3 Step 1";
			} else if (step == 1) {
				step = 2;
				prefab = Instantiate (Resources.Load (Configure.Level3Step2 ())) as GameObject;
				prefab.name = "Level 3 Step 2";
			} else if (step == 2) {
				step = 3;
				prefab = Instantiate (Resources.Load (Configure.Level3Step3 ())) as GameObject;
				prefab.name = "Level 3 Step 3";
			} else if (step == 3) {
				step = 4;
				prefab = Instantiate (Resources.Load (Configure.Level3Step4 ())) as GameObject;
				prefab.name = "Level 3 Step 4";
			} else if (step == 4) {
				step = 5;
				prefab = Instantiate (Resources.Load (Configure.Level3Step5 ())) as GameObject;
				prefab.name = "Level 3 Step 5";
			}
		} else if (LevelLoader.instance.level == 4) {
			if (step == 0) {
				step = 1;
				prefab = Instantiate (Resources.Load (Configure.Level4Step1 ())) as GameObject;
				prefab.name = "Level 4 Step 1";
			} else if (step == 1) {
				step = 2;
				prefab = Instantiate (Resources.Load (Configure.Level4Step2 ())) as GameObject;
				prefab.name = "Level 4 Step 2";
			} else if (step == 2) {
				step = 3;
				prefab = Instantiate (Resources.Load (Configure.Level4Step3 ())) as GameObject;
				prefab.name = "Level 4 Step 3";
			} else if (step == 3) {
				step = 4;
				prefab = Instantiate (Resources.Load (Configure.Level4Step4 ())) as GameObject;
				prefab.name = "Level 4 Step 4";
			} else if (step == 4) {
				step = 5;
				prefab = Instantiate (Resources.Load (Configure.Level4Step5 ())) as GameObject;
				prefab.name = "Level 4 Step 5";
			}
		} else if (LevelLoader.instance.level == 5) {
			if (step == 0) {
				prefab = Instantiate (Resources.Load (Configure.Level5Step1 ())) as GameObject;
				prefab.name = "Level 5 Step 1";

				step = 1;
			} else if (step == 1) {
				prefab = Instantiate (Resources.Load (Configure.Level5Step2 ())) as GameObject;
				prefab.name = "Level 5 Step 2";

				step = 2;
			} else if (step == 2) {
				step = 0;
				SelfDisactive ();
			}
		} else if (LevelLoader.instance.level == 6) {
			if (step == 0)
			{
				prefab = Instantiate(Resources.Load(Configure.Level7Step1())) as GameObject;
				prefab.name = "Level 7 Step 1";

				step = 1;
			}
			else if (step == 1)
			{
				step = 2;
				prefab = Instantiate (Resources.Load (Configure.Level7Step2 ())) as GameObject;
				prefab.name = "Level 7 Step 2";
			}
		}
        else if (LevelLoader.instance.level == 7)
        {
            if (step == 0)
            {
                prefab = Instantiate(Resources.Load(Configure.Level6Step1())) as GameObject;
                prefab.name = "Level 6 Step 1";

                step = 1;
            }
            else if (step == 1)
            {
                prefab = Instantiate(Resources.Load(Configure.Level6Step2())) as GameObject;
                prefab.name = "Level 6 Step 2";

                step = 2;
            }
            else if (step == 2)
            {
                prefab = Instantiate(Resources.Load(Configure.Level6Step3())) as GameObject;
                prefab.name = "Level 6 Step 3";

                step = 3;
            }
            else if (step == 3)
            {
                prefab = Instantiate(Resources.Load(Configure.Level6Step4())) as GameObject;
                prefab.name = "Level 6 Step 4";

                step = 4;
            }
            else if (step == 4)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 8)
        {
            if (step == 0)
            {
                prefab = Instantiate(Resources.Load(Configure.Level8Step1())) as GameObject;
                prefab.name = "Level 8 Step 1";

                step = 1;
            }
            else if (step == 1)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 9)
        {
			if (step == 0) {
				prefab = Instantiate (Resources.Load (Configure.Level9Step1 ())) as GameObject;
				prefab.name = "Level 9 Step 1";

				step = 1;
			} else if (step == 1) {
				prefab = Instantiate (Resources.Load (Configure.Level9Step2 ())) as GameObject;
				prefab.name = "Level 9 Step 2";

				step = 2;
			}
        }
        else if (LevelLoader.instance.level == 12)
        {
            if (step == 0)
            {
                prefab = Instantiate(Resources.Load(Configure.Level12Step1())) as GameObject;
                prefab.name = "Level 12 Step 1";

                step = 1;
            }
            else if (step == 1)
            {
                step = 0;
                SelfDisactive();
            }
        }
        else if (LevelLoader.instance.level == 18)
        {
            if (step == 0)
            {
                prefab = Instantiate(Resources.Load(Configure.Level18Step1())) as GameObject;
                prefab.name = "Level 18 Step 1";

                step = 1;
            }
        }

        if (prefab != null)
        {
            if (step != 0)
            {
                //NetManager.instance.MakePointInGuide(LevelLoader.instance.level, step);//引导打点
                GameMainManager.Instance.netManager.MakePointInGuide(LevelLoader.instance.level, step,(ret,res)=> { });
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

//        if (LevelLoader.instance.level == 6)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 7 && step == 2)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 9)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 12 && step == 2)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 15 && step == 2)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 16)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 18 && step == 2)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 25 && step == 2)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 31)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 61)
//        {
//            step = 0;
//            SelfDisactive();
//        }
//        else if (LevelLoader.instance.level == 76)
//        {
//            step = 0;
//            SelfDisactive();
//        }
    }

    public void HideOnSwapBack()
    {
        if (LevelLoader.instance.level == 1 && step == 2 
            ||LevelLoader.instance.level == 2 
            ||LevelLoader.instance.level == 3)
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
