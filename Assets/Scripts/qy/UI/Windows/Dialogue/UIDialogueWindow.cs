using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
using qy.config;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIDialogueWindow : UIWindowBase {

    public override UIWindowData windowData
    {
        get
        {
            UIWindowData windowData = new UIWindowData
            {
                id = qy.ui.UISettings.UIWindowID.UIDialogueWindow,
                type = qy.ui.UISettings.UIWindowType.Fixed,
            };

            return windowData;
        }
    }

    public RectTransform dialogueBox;
    public Image imgBG;
    public Text dialogText;

    public RectTransform topBlack;
    public RectTransform bottomBlack;

    public Image blood;

    private qy.config.StoryItem lastDialogue;//上一句对话
    private qy.config.StoryItem curDialogue;//当前对话
    private UIDialoguePerson personLeft;
    private UIDialoguePerson personRight;
    private UIDialoguePerson talkingPerson;
    private bool isMoveing = false;
    private Dictionary<string, List<GameObject>> personCache = new Dictionary<string, List<GameObject>>();
    private string beginID;

    private void Awake()
    {
        blood.gameObject.SetActive(false);
        
        Material material = new Material(Shader.Find("Custom/Gray"));
        imgBG.material = material;
        SetBGGray(0);
    }

    protected override void StartShowWindow(object[] data)
    {
        beginID = data[0].ToString();
        Debug.Log("播放剧情"+beginID.ToString());
        lastDialogue = null;
        curDialogue = null;
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0;
        topBlack.sizeDelta = new Vector2(topBlack.sizeDelta.x, 0);
        bottomBlack.sizeDelta = new Vector2(bottomBlack.sizeDelta.x, 0);
        dialogueBox.gameObject.SetActive(false);
        if (personLeft != null)
            personLeft.gameObject.SetActive(false);
        if (personRight != null)
            personRight.gameObject.SetActive(false);
        imgBG.color = new Color(1, 1, 1, 0.01f);
    }

    protected override void EnterAnimation(Action onComplete)
    {
        isMoveing = true;
        topBlack.DOSizeDelta(new Vector2(topBlack.sizeDelta.x, 150),0.5f);
        bottomBlack.DOSizeDelta(new Vector2(bottomBlack.sizeDelta.x, 150), 0.5f).OnComplete(()=> {
            onComplete();
            isMoveing = false;
        });
    }

    protected override void ExitAnimation(Action onComplete)
    {
        isMoveing = true;
        topBlack.DOSizeDelta(new Vector2(topBlack.sizeDelta.x, 0), 0.5f);
        bottomBlack.DOSizeDelta(new Vector2(bottomBlack.sizeDelta.x, 0), 0.5f).OnComplete(() => {
            onComplete();
            isMoveing = false;
        });
        imgBG.DOFade(0, 0.5f);
        if(personLeft!=null)
            personLeft.Hide();
        if(personRight!=null)
            personRight.Hide();
        HideDialogBox(curDialogue.personLocation);
    }

    protected override void EndShowWindow()
    {
        ShowStory(qy.GameMainManager.Instance.configManager.storysConfig.GetItem(beginID));
    }

    protected override void EndHideWindow()
    {

    }

    private void ShowStory(qy.config.StoryItem story)
    {
        lastDialogue = curDialogue;
        curDialogue = story;
        if(lastDialogue!=null && curDialogue!=null )
        {
            if(lastDialogue.weather != curDialogue.weather)
            {
                qy.GameMainManager.Instance.weatherManager.StopWeather((WeatherManager.WeatherEnum)lastDialogue.weather);
            }
            if(lastDialogue.effect==3 && lastDialogue.effect != curDialogue.effect)
            {
                SetBGGray(0);
            }
        }
        if(string.IsNullOrEmpty(curDialogue.personLocation))
        {
            curDialogue.personLocation = UnityEngine.Random.Range(0, 2).ToString();
        }
        
        
        if(string.IsNullOrEmpty(curDialogue.bgFile))
        {
            imgBG.gameObject.SetActive(false);
        }
        else if (lastDialogue == null || curDialogue.bgFile != lastDialogue.bgFile)
        {
            //AssetsManager.Instance.LoadAssetAsync<Sprite>(FilePathTools.GetStorySpritePath(curDialogue.bgFile), (sp) =>
            //{
            //    if (sp != null)
            //    {
            //        imgBG.sprite = sp;
            //        imgBG.DOFade(1, 0.5f);
            //        RectTransform imgRectTF = imgBG.transform as RectTransform;
            //        imgRectTF.sizeDelta = new Vector2(0, imgRectTF.rect.width / (sp.texture.width / (float)sp.texture.height));
            //        imgBG.gameObject.SetActive(true);
            //    }

            //});

            var sp = March.Core.ResourceManager.ResourceManager.instance.Load<Sprite>(Configure.StoryBackground,curDialogue.bgFile);
            if (sp != null)
            {
                imgBG.sprite = sp;
                imgBG.DOFade(1, 0.5f);
                RectTransform imgRectTF = imgBG.transform as RectTransform;
                imgRectTF.sizeDelta = new Vector2(0, imgRectTF.rect.width / (sp.texture.width / (float)sp.texture.height));
                imgBG.gameObject.SetActive(true);
            }
        }

        string dialog = curDialogue.dialogue;
        string personFile = lastDialogue==null || curDialogue.personFile != lastDialogue.personFile ? curDialogue.personFile : "";
        //如果这句对话和上句对话不是一个人说的则播放对话框动画
        ShowDialog(dialog, personFile, curDialogue.personLocation, lastDialogue==null || lastDialogue.personLocation != curDialogue.personLocation);

        WeatherManager.WeatherEnum weather = (WeatherManager.WeatherEnum)curDialogue.weather;
        qy.GameMainManager.Instance.weatherManager.StartWeather(weather);

        StartCoroutine(ShowEffect(curDialogue.effect));
    }

    private IEnumerator ShowEffect(int effect)
    {
        yield return new WaitForSeconds(0.5f);
        switch (effect)
        {
            case 1:
                (imgBG.transform as RectTransform).DOShakeAnchorPos(0.5f, 20, 80);
                break;
            case 2:
                blood.color = new Color(1, 1, 1, 0);
                blood.gameObject.SetActive(true);
                Sequence sq = DOTween.Sequence();
                sq.Append(blood.DOFade(1, 0.3f).SetEase(Ease.OutBounce));
                sq.Append(blood.DOFade(0, 0.3f).SetEase(Ease.InBack));
                sq.AppendCallback(() => {
                    blood.gameObject.SetActive(false);

                });
                break;
            case 3:
                SetBGGray(1);
                break;
        }
    }

    private void SetBGGray(float value)
    {
        DOTween.To(() => imgBG.material.GetFloat("_LuminosityAmount"), x => imgBG.material.SetFloat("_LuminosityAmount", x), value, 0.5f);
    }

    /// <summary>
    /// 展示对话
    /// </summary>
    /// <param name="dialog">对话内容</param>
    /// <param name="talkerPic">人物头像</param>
    /// <param name="talkerPosition">人物位置 0:left 1:right</param>
    /// <param name="isTurn">是否更换说话者</param>
    private void ShowDialog(string dialog,string talkerPic,string talkerPosition,bool isTurn)
    {
        //DOTween.KillAll();
        dialogText.text = "";
        if (isTurn)
        {
            isMoveing = true;
            Sequence sq = DOTween.Sequence();
            if (dialogueBox.gameObject.activeSelf)
            {
                HideTalker(talkerPosition == "0" ? "1" : "0");
                sq.AppendCallback(()=> {
                    HideDialogBox(talkerPosition=="0"?"1":"0");
                });
                sq.AppendInterval(0.3f);
            }
            ShowTalker(talkerPic,talkerPosition);
            sq.AppendCallback(() =>
            {
                ShowDialogBox( talkerPosition);
            });
            sq.AppendInterval(0.3f);
            sq.AppendCallback(()=> {
                isMoveing = false;
                talkingPerson.StartTalk();
            });
            sq.Append(dialogText.DOText(dialog, 1, true)).OnComplete(()=> {
                talkingPerson.StopTalk();
            });

        }
        else
        {
            talkingPerson.StartTalk();
            dialogText.DOText(dialog, 1, true).OnComplete(() => {
                talkingPerson.StopTalk();
            });
        }
    }

    private void ShowTalker(string talkerPic, string position)
    {
        //talkerPic = "person";
        talkingPerson = position == "0" ? personLeft : personRight;

        if(talkingPerson != null && string.IsNullOrEmpty(talkerPic))
        {
            talkingPerson.gameObject.SetActive(false);
            
        }
        else
        {
            talkingPerson = GetPerson(talkerPic, position);
            talkingPerson.Show();
            
        }

    }

    private void HideTalker(string position)
    {
        UIDialoguePerson person = position == "0" ? personLeft : personRight;
        person.Hide();
    }

    //todo 待优化 不要平凡创建销毁
    private UIDialoguePerson GetPerson(string name,string position)
    {
        List<GameObject> cacheList;
        personCache.TryGetValue(name,out cacheList);
        GameObject personGO = null;
        if (cacheList!=null)
        {
            foreach(GameObject cache in cacheList)
            {
                if(!cache.activeSelf)
                {
                    personGO = cache;
                    break;
                }
            }
        }
        if(personGO==null)
        {
            GameObject go = March.Core.ResourceManager.ResourceManager.instance.Load<GameObject>(Configure.StoryDialogPerson, name);
            personGO = GameUtils.CreateGameObject(transform, go);
            if(!personCache.ContainsKey(name))
            {
                personCache.Add(name, new List<GameObject>() { personGO });
            }else
            {
                personCache[name].Add(personGO);
            }
        }

        personGO.transform.SetSiblingIndex(1);
        RectTransform rt = personGO.transform as RectTransform;
        UIDialoguePerson person = personGO.GetComponent<UIDialoguePerson>();
        if (position == "0")
        {
            rt.pivot = new Vector2(0,0);
            rt.anchorMin = new Vector2(0,0);
            rt.anchorMax = new Vector2(0,0);
            rt.anchoredPosition = new Vector2(25, 0);
            if(personLeft!=null)
            {
                personLeft.gameObject.SetActive(false);
            }
            personLeft = person;
        }
        else
        {
            rt.pivot = new Vector2(1, 0);
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.anchoredPosition = new Vector2(-25, 0);
            if (personRight != null)
            {
                personRight.gameObject.SetActive(false);
            }
            personRight = person;
        }
        
        
        return person;
    }
    /// <summary>
    /// 显示
    /// </summary>
    private void ShowDialogBox(string position)
    {
       
        CanvasGroup dialogCanvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        //dialogCanvasGroup.alpha = 0.5f;
        dialogCanvasGroup.DOFade(1, 0.2f);
        dialogueBox.gameObject.SetActive(true);
        
        if (position == "0")
        {
            dialogueBox.anchorMin = new Vector2(0, 0);
            dialogueBox.anchorMax = new Vector2(0.5f, 0);

            dialogueBox.DOAnchorMax(new Vector2(1, 0), 0.2f);
            
        }
        else
        {
            dialogueBox.anchorMin = new Vector2(0.5f, 0);
            dialogueBox.anchorMax = new Vector2(1, 0);

            dialogueBox.DOAnchorMin(new Vector2(0, 0), 0.2f);
        }
        
    }

    private void HideDialogBox(string position)
    {
        
        CanvasGroup dialogCanvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        dialogCanvasGroup.DOFade(0, 0.2f);

        dialogText.text = "";
        if (position == "0")
        {

            dialogueBox.DOAnchorMax(new Vector2(0.5f, 0), 0.2f);

        }
        else
        {
            dialogueBox.DOAnchorMin(new Vector2(0.5f, 0), 0.2f);
        }
    }

    public void OnClickHandle()
    {
        if(isMoveing)
        {
            return;
        }
        if(DOTween.TotalPlayingTweens()>0)
        {
            DOTween.CompleteAll();
            DOTween.KillAll();
            return;
        }
        
        qy.config.StoryItem story = curDialogue.next;
        if(story!=null)
        {

            ShowStory(story);
        }else
        {
            qy.config.QuestItem nextQuest = qy.GameMainManager.Instance.playerData.GetQuest();
            if(nextQuest.type == qy.config.QuestItem.QuestType.Ending)
            {
                qy.GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UIEndingWindow,nextQuest);
            }else
            {
                qy.GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UITaskWindow,qy.GameMainManager.Instance.playerData);
            }
            qy.GameMainManager.Instance.weatherManager.StopAll();
            OnClickClose();
        }
        
    }



}
