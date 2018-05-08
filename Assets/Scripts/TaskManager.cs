using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public string gotoQuestID;
    public string questID = "10001";
    //public StoryItem m_storyDescribe = null;
    //public StoryItem SetStoryDescribe { set { m_storyDescribe = value; } }
    private QuestItem _questItem;
    private quest m_quest;
    public quest Quest { get { if (m_quest == null) { m_quest = DefaultConfig.getInstance().GetConfigByType<quest>(); } return m_quest; } }
    private story m_story;
    public story Story { get { if (m_story == null) { m_story = DefaultConfig.getInstance().GetConfigByType<story>(); } return m_story; } }
    private item m_item;
    public item Item { get { if (m_item == null) { m_item = DefaultConfig.getInstance().GetConfigByType<item>(); } return m_item; } }
    private language_cn m_language_cn;
    [HideInInspector]
    public GameObject m_panelInfo;

    private TaskChoicePanel m_taskChoicePanel;

    void Awake()
    {
        Instance = this;
        m_panelInfo = transform.Find("TaksPanel").gameObject;

        m_taskChoicePanel = transform.Find("TaskListPanel").GetComponent<TaskChoicePanel>();
    }

    public void onClickCollect()
    {
        int star = 0;
        if (_questItem != null && _questItem.requireStar != "")
            star = int.Parse(_questItem.requireStar);
        //int coin = Item.GetItemByID(_questItem)
        int playerStar = qy.GameMainManager.Instance.playerData.starNum;
        playerStar -= star;
        //播放剧情时条件判断，星数/requireItem
        /* if (playerStar >= 0)
        {
          //  MessageBox.Instance.Show("剧情解锁成功");
            PlayerData.instance.setStarNum(playerStar);
            MainScene.Instance.RefreshPlayerData();
        }
        else*/
        if (!(playerStar >= 0))
        {
            MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200011"));
            return;
        }
        if (_questItem != null && _questItem.requireItem != "0")
        {
            string requireItemId = _questItem.requireItem.Split(':')[0];
            int requireItemCount = int.Parse(_questItem.requireItem.Split(':')[1]);
            Debug.Log("解锁剧情需要的item:" + requireItemId);
            Debug.Log("解锁剧情需要的item count:" + requireItemCount);
            int ownerItemCount = qy.GameMainManager.Instance.playerData.GetPropItem(requireItemId).count;
            int leftItemCount = ownerItemCount - requireItemCount;
            if (leftItemCount >= 0)
            {
                MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200012"));

                qy.GameMainManager.Instance.playerData.starNum = playerStar;
                qy.GameMainManager.Instance.playerData.AddPropItem(requireItemId, leftItemCount);
                //MainScene.Instance.RefreshPlayerData();
                // NetManager.instance.userToolsToServer(requireItemId,requireItemCount.ToString());  //消耗道具之后发送http
            }
            else
            {
                MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200010"));
                return;
            }
        }
        if (PlotManager.Instance.m_storyDescribe == null)
        {
            Debug.Log("not storyID");
            // MessageBox.Instance.Show("剧情播放story id  为空");
            if (!gotoQuestID.Equals("0"))
            {
                questID = gotoQuestID;
                PlayerPrefs.SetString("QuestID", gotoQuestID);
                qy.GameMainManager.Instance.playerData.questId = gotoQuestID;
                //NetManager.instance.sendQuestIdToServer(gotoQuestID);
                gameObject.SetActive(false);
                onClickTask();
            }
            return;
        }
        gameObject.SetActive(false);
        //NetManager.instance.sendQuestIdToServer(qy.GameMainManager.Instance.playerData.questId);
        //qy.GameMainManager.Instance.playerModel.QuestComplate(gotoQuestID);
        //PlotManager.Instance.ShowPlot();
        qy.ui.UIManager.Instance.OpenWindow(qy.ui.UISettings.UIWindowID.UIDialogueWindow, PlotManager.Instance.m_storyDescribe.id);
    }


    public void onClickTask()
    {
        qy.GameMainManager.Instance.uiManager.OpenWindow(qy.ui.UISettings.UIWindowID.UITaskWindow, qy.GameMainManager.Instance.playerData);
    }
}
