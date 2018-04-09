using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
	public static TaskManager Instance;
	public string gotoQuestID;
	public string questID="10001";
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

	[HideInInspector]
	public TaskChoicePanel m_taskChoicePanel;

	void Awake()
	{
		Instance = this;
		m_panelInfo = transform.Find("TaksPanel").gameObject;
		//PlayerPrefs.DeleteAll();
	}

	public void onClickCollect()
	{
		int star = 0;
		if (_questItem != null && _questItem.requireStar != "")
			star = int.Parse(_questItem.requireStar);
		//int coin = Item.GetItemByID(_questItem)
		int playerStar = PlayerData.instance.getStarNum();
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
			int ownerItemCount = PlayerData.instance.getHasItemCountByItemId(requireItemId);
			int leftItemCount = ownerItemCount - requireItemCount;
			if (leftItemCount >= 0)
			{
				MessageBox.Instance.Show(LanguageManager.instance.GetValueByKey("200012"));

				PlayerData.instance.setStarNum(playerStar);
				PlayerData.instance.goodsMap[requireItemId] = leftItemCount.ToString();
				MainScene.Instance.RefreshPlayerData();
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
				PlayerData.instance.setQuestId(gotoQuestID);
				//NetManager.instance.sendQuestIdToServer(gotoQuestID);
				gameObject.SetActive(false);
				onClickTask();
			}
			return;
		}
		gameObject.SetActive(false);
		NetManager.instance.sendQuestIdToServer(PlayerData.instance.getQuestId());
		PlotManager.Instance.ShowPlot();
	}


	//显示任务界面
	public void onClickTask()
	{
		gameObject.SetActive(true);
		this._questItem = null;
		//if (PlayerPrefs.HasKey("QuestID"))
		if(PlayerData.instance.getQuestId()!="")
		{
			// _questItem = Quest.GetItemByID(PlayerPrefs.GetString("QuestID"));
			Debug.Log("Player Data quest id:"+PlayerData.instance.getQuestId());
			_questItem =Quest.GetItemByID(PlayerData.instance.getQuestId());
			//PlayerData.instance.setQuestId(_questItem.gotoId);
			if (_questItem == null)
			{
				// MessageBox.Instance.Show("当前questID不存在 Prefabs:" + PlayerPrefs.GetString("QuestID"));
				string temStr = LanguageManager.instance.GetValueByKey("200013");
				MessageBox.Instance.Show(temStr + PlayerData.instance.getQuestId());
				return;
			}
		}
		else
		{
			_questItem = Quest.GetItemByID(questID);
			PlayerData.instance.setQuestId(_questItem.gotoId);
			if (_questItem == null)
			{
				string temStr = LanguageManager.instance.GetValueByKey("200013");
				MessageBox.Instance.Show(temStr + questID);
				return;
			}
		}
		//判断当前条件是否满足


		gotoQuestID = _questItem.gotoId;
		//显示type=1
		if (_questItem.type.Equals("1"))
		{
			m_panelInfo.SetActive(true);
			m_panelInfo.transform.Find("Des").GetComponent<Text>().text =LanguageManager.instance.GetValueByKey(_questItem.sectionDes);
			m_panelInfo.transform.Find("title").GetComponent<Text>().text = LanguageManager.instance.GetValueByKey(_questItem.sectionName);
			//显示背景图片
			m_panelInfo.transform.Find("quest_bg").GetComponent<Image>().sprite = Resources.Load<Sprite>("Story/" + _questItem.bg);

			string coinType = "";
			string coinCount = "0";
			if (_questItem != null && _questItem.requireItem != "0") {
				coinType = _questItem.requireItem.Split(':')[0];
				coinCount = _questItem.requireItem.Split(':')[1];
			}
			int itemCount = PlayerData.instance.getHasItemCountByItemId(_questItem.requireItem.Split(':')[0]);
			Debug.Log("剧情播放需要的道具：" + _questItem.requireItem.Split(':')[0]);
			Debug.Log("背包里面道具数量："+itemCount);
			// m_panelInfo.transform.Find("cion").GetComponent<Text>().text = coinCount;


			var condition = m_panelInfo.transform.Find ("condition");
			foreach (Transform child in condition)
			{
				if (child.tag == "TaskRequirementsCell") {
					Destroy(child.gameObject);
				}
			}

			var star = Instantiate(Resources.Load("Prefabs/TaskPanel/TaskRequirementsCell")) as GameObject;
			star.transform.parent = condition.gameObject.transform;
			star.transform.localScale = Vector3.one;
			star.transform.Find ("Image1").gameObject.SetActive (true);
			star.transform.Find ("Text").gameObject.SetActive (true);
			star.transform.Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}",PlayerData.instance.getStarNum(), _questItem.requireStar);

			if (coinCount != "0") {
				var cion = Instantiate(Resources.Load("Prefabs/TaskPanel/TaskRequirementsCell")) as GameObject;
				cion.transform.parent = condition.gameObject.transform;
				cion.transform.localScale = Vector3.one;
				cion.transform.Find ("Image").gameObject.SetActive (true);
				GoodsItem goodsItem = Item.GetItemByID (coinType);
				Sprite sp = Resources.Load(string.Format("Sprites/UI/{0}", goodsItem.icon), typeof(Sprite)) as Sprite;
				cion.transform.Find ("Image").GetComponent<Image> ().sprite = sp;
				cion.transform.Find ("Text").gameObject.SetActive (true);
				cion.transform.Find ("Text").GetComponent<Text> ().text = string.Format ("{0}/{1}", itemCount, coinCount);
				if (int.Parse (coinCount) > itemCount) {
					cion.transform.Find ("Text").GetComponent<Text> ().color = Color.red;
				}
			}
			m_taskChoicePanel.gameObject.SetActive(false);
			PlotManager.Instance.m_storyDescribe = Story.GetItemByID(_questItem.storyId);
		}
		//显示type=2
		else if (_questItem.type.Equals("2"))
		{
			m_panelInfo.SetActive(false);
			m_taskChoicePanel.gameObject.SetActive(true);
			Dictionary<string, string> value = Quest.GetQuestSelectListByID(_questItem.id);
			string title =LanguageManager.instance.GetValueByKey(_questItem.sectionName);
			string describe = LanguageManager.instance.GetValueByKey(_questItem.sectionDes);
			m_taskChoicePanel.ShowChoiceList(title, describe, value);
		}
	}
}
