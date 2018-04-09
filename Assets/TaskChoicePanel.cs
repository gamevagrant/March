using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskChoicePanel : MonoBehaviour
{
    private Text m_title;
    private Text m_describe;
    private GameObject m_storyListContent;

    private GameObject m_choicePrefab;
    private RectTransform m_storyListContentRectTransform;

    void Start()
    {
        TaskManager.Instance.m_taskChoicePanel = this;
        m_title = transform.Find("storyTitle").GetComponent<Text>();
        m_describe = transform.Find("storyDes").GetComponent<Text>();
        m_storyListContent = transform.Find("storyList/Viewport/Content").gameObject;
        m_storyListContentRectTransform = m_storyListContent.GetComponent<RectTransform>();
        m_choicePrefab = Resources.Load<GameObject>("TaskChoice");
        gameObject.SetActive(false);
    }
    //显示任务列表
    public void ShowChoiceList(string title,string describe,Dictionary<string, string> choices)
    {
        m_title.text = title;
        m_describe.text = describe;
        //清空之前按钮列表
		var viewport  = transform.Find("storyList/Viewport").gameObject;
		//m_storyListContentRectTransform.sizeDelta = new Vector2(viewport.transform.position.x, viewport.transform.position.y);
        int childCount = m_storyListContent.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Destroy(m_storyListContentRectTransform.GetChild(i).gameObject);
        }
        //开始设置任务选项列表
        foreach (string desID in choices.Keys)
        {
            //生成按钮
            GameObject button = Instantiate(m_choicePrefab, m_storyListContent.transform);
            //button.transform.parent = m_storyListContent.transform;
            //自适应UI按钮列表
            //m_storyListContentRectTransform.sizeDelta = new Vector2(m_storyListContentRectTransform.sizeDelta.x, m_storyListContentRectTransform.sizeDelta.y + 300);

            //设置按钮的title
            button.transform.Find("Text").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey(desID);
            //设置按钮事件
            button.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlotManager.Instance.SetStoryDescribe = TaskManager.Instance.Story.GetItemByID(choices[desID]);
                TaskManager.Instance.onClickCollect();
            });
        }
    }
}
