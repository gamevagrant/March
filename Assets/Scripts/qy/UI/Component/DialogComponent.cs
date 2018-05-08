using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using qy.config;
public class DialogComponent : MonoBehaviour {

    public RectTransform dialogueBox;
    public Text dialogText;
    public Image personLeft;
    public Image personRight;

    public System.Action onComplate;

    private qy.config.StoryItem lastDialogue;//上一句对话
    private qy.config.StoryItem curDialogue;//当前对话
    private bool isMoveing = false;


    public void ShowStory(qy.config.StoryItem story)
    {
        gameObject.SetActive(true);
        lastDialogue = null;
        curDialogue = null;
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0;
        dialogueBox.gameObject.SetActive(false);
        personLeft.gameObject.SetActive(false);
        personRight.gameObject.SetActive(false);

        lastDialogue = curDialogue;
        curDialogue = story;
        if (string.IsNullOrEmpty(curDialogue.personLocation))
        {
            curDialogue.personLocation = UnityEngine.Random.Range(0, 2).ToString();
        }

        string dialog = curDialogue.dialogue;
        string personFile = lastDialogue == null || curDialogue.personFile != lastDialogue.personFile ? curDialogue.personFile : "";
        //如果这句对话和上句对话不是一个人说的则播放对话框动画
        ShowDialog(dialog, personFile, curDialogue.personLocation, lastDialogue == null || lastDialogue.personLocation != curDialogue.personLocation);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 展示对话
    /// </summary>
    /// <param name="dialog">对话内容</param>
    /// <param name="talkerPic">人物头像</param>
    /// <param name="talkerPosition">人物位置 0:left 1:right</param>
    /// <param name="isTurn">是否更换说话者</param>
    private void ShowDialog(string dialog, string talkerPic, string talkerPosition, bool isTurn)
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
                sq.AppendCallback(() => {
                    HideDialogBox(talkerPosition == "0" ? "1" : "0");
                });
                sq.AppendInterval(0.3f);
            }
            ShowTalker(talkerPic, talkerPosition);
            sq.AppendCallback(() =>
            {
                ShowDialogBox(talkerPosition);
            });
            sq.AppendInterval(0.3f);
            sq.AppendCallback(() => {
                isMoveing = false;
            });
            sq.Append(dialogText.DOText(dialog, 1, true));

        }
        else
        {

            dialogText.DOText(dialog, 1, true);
        }
    }

    private void ShowTalker(string talkerPic, string position)
    {
        //talkerPic = "person";
        Image img = position == "0" ? personLeft : personRight;

        if (string.IsNullOrEmpty(talkerPic))
        {
            img.gameObject.SetActive(false);
        }
        else
        {
            var sp =
                March.Core.ResourceManager.ResourceManager.instance.Load<Sprite>(Configure.StoryPerson,
                    talkerPic);
            if (sp != null)
            {
                img.sprite = sp;
                img.SetNativeSize();
                img.gameObject.SetActive(true);
            }

            //AssetsManager.Instance.LoadAssetAsync<Sprite>(FilePathTools.GetPersonHeadPath(talkerPic), (sp) => {

            //    if (sp != null)
            //    {
            //        img.sprite = sp;
            //        img.SetNativeSize();
            //        img.gameObject.SetActive(true);
            //    }

            //});

            img.DOColor(Color.white, 0.5f);
            img.transform.DOScale(1.1f, 0.5f);
            img.gameObject.SetActive(true);
        }

    }

    private void HideTalker(string position)
    {
        Image img = position == "0" ? personLeft : personRight;
        //img.DOFade(0.5f, 0.5f);
        img.transform.DOScale(0.9f, 0.5f);
        img.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.5f);
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
        if (isMoveing)
        {
            return;
        }
        if (DOTween.TotalPlayingTweens() > 0)
        {
            List<Tween> tweens = DOTween.PlayingTweens();
            foreach (Tween t in tweens)
            {
                Debug.Log(t.target);
            }
            
            DOTween.CompleteAll();
            DOTween.KillAll();
            return;
        }
        if(onComplate!=null)
        {
            
            Close();
            onComplate();
        }
        else
        {
            qy.config.StoryItem story = curDialogue.next;
            if (story != null)
            {

                ShowStory(story);
            }
            else
            {
                Close();
            }
        }
    }
}
