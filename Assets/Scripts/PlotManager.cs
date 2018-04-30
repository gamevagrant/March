using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour
{

    public static PlotManager Instance;

    private Image background;//这是背景图片
    private GameObject m_plotBar;
    private GameObject m_dialog;
    private Image m_npcLeft;
    private Image m_npcRight;
    private Text m_contentText;
    private Tweener m_t;

    public StoryItem m_storyDescribe = null;
    public StoryItem SetStoryDescribe { set { m_storyDescribe = value; } }

    void Awake()
    {
        Instance = this;
        m_plotBar = transform.Find("bar").gameObject;
        m_dialog = transform.Find("dialog").gameObject;
        m_contentText = transform.Find("dialog/textcontent").GetComponent<Text>();
        //这里是添加的背景
        background = transform.root.Find("Background").GetComponent<Image>();
        m_npcLeft = transform.Find("npcLeft").GetComponent<Image>();
        m_npcRight = transform.Find("npcRight").GetComponent<Image>();
    }
    public void SetContent()
    {

    }

    public void ClearContent()
    {
        m_contentText.text = "";
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown((0)) && m_dialog.activeSelf)
        {
            if (m_storyDescribe != null)
            {
                m_contentText.text = "";
                if (m_t != null) m_t.Kill();
               // string dialog = MainScene.Instance.Language_CN.GetItemByID(m_storyDescribe.dialogue).value;
				string dialog = LanguageManager.instance.GetValueByKey(m_storyDescribe.dialogue);
                if (dialog.Contains("{0}"))
                {
                    dialog = string.Format(dialog, qy.GameMainManager.Instance.playerData.nickName);
                }
                m_t = m_contentText.DOText(dialog, 1.5f);
                print(m_storyDescribe.next);
                //判断后面还有没有对话
                CheckEndIsHaveDialog();
            }
        }
    }

    private void CheckEndIsHaveDialog()
    {
        if (!m_storyDescribe.next.Equals("0"))
        {
            m_storyDescribe = TaskManager.Instance.Story.GetItemByID(m_storyDescribe.next);
            //设置背景图片
            if (background.gameObject.activeSelf == false) background.gameObject.SetActive(true);
            background.sprite = Resources.Load<Sprite>("Story/" + m_storyDescribe.bgFile);
            print("Current BgFile :" + m_storyDescribe.bgFile);
        }
        else
        {
            SetDialogActiveFalse();
            Invoke("SetBarActiveFalse", 1.0f);
            m_storyDescribe = null;
            background.gameObject.SetActive(false);
        }
    }

    public void ShowPlot()
    {
        if (background.gameObject.activeSelf == false) background.gameObject.SetActive(true);
        background.transform.SetAsLastSibling();
        PlotManager.Instance.transform.SetAsLastSibling();
        MessageBox.Instance.transform.SetAsLastSibling();
        SetBarActiveTrue();
        Invoke("SetDialogActiveTrue", 1.0f);
    }

    public void SetDialogActiveFalse()
    {
        if (m_dialog)
            m_dialog.SetActive(false);
    }

    private void SetDialogActiveTrue()
    {
        if (m_dialog)
        {
            m_dialog.SetActive(true);
            if (m_storyDescribe != null)
            {
               // string dialog = MainScene.Instance.Language_CN.GetItemByID(m_storyDescribe.dialogue).value;
				string dialog = LanguageManager.instance.GetValueByKey(m_storyDescribe.dialogue);
                if (dialog.Contains("{0}"))
                {
                    print(dialog);
                    print("Nick Name :" + qy.GameMainManager.Instance.playerData.nickName);
                    dialog = string.Format(dialog, qy.GameMainManager.Instance.playerData.nickName);
                }
                m_contentText.text = dialog;
                m_npcLeft.gameObject.SetActive(false);
                m_npcRight.gameObject.SetActive(false);
                Image npcPic = m_storyDescribe.personLocation == "0" ? m_npcLeft : m_npcRight;
                npcPic.gameObject.SetActive(true);
                npcPic.sprite = Resources.Load<Sprite>("Story/" + m_storyDescribe.personFile);
                // m_t = m_contentText.DOText(dialog, 1.5f);
                CheckEndIsHaveDialog();
                // m_contentText.text = MainScene.Instance.Language_CN.GetItemByID(m_storyDescribe.dialogue).value;
            }
        }
    }

    public void SetBarActiveTrue()
    {
        if (m_plotBar != null)
            m_plotBar.SetActive(true);
    }

    private void SetBarActiveFalse()
    {
        if (m_plotBar != null)
        {
            if (!TaskManager.Instance.gotoQuestID.Equals("0"))
            {
                TaskManager.Instance.questID = TaskManager.Instance.gotoQuestID;
                PlayerPrefs.SetString("QuestID", TaskManager.Instance.gotoQuestID);
                qy.GameMainManager.Instance.playerData.questId = TaskManager.Instance.questID;
                //                NetManager.instance.sendQuestIdToServer(TaskManager.Instance.gotoQuestID);
                TaskManager.Instance.onClickTask();
            }
            m_plotBar.SetActive(false);
            m_npcLeft.gameObject.SetActive(false);
            m_npcRight.gameObject.SetActive(false);
        }

    }
}
