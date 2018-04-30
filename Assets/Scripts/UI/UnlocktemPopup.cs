using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using qy;
using qy.config;
public class UnlocktemPopup : MonoBehaviour
{
    public Text m_titleText;
    public Image m_itemImg;
    public Text m_itemNum;
    public Text m_itemDes;
    public Text m_btnText;

    void Start()
    {
        m_titleText.text = LanguageManager.instance.GetValueByKey("210001");
		m_btnText.text = LanguageManager.instance.GetValueByKey ("210136");
        if (GameMainManager.Instance.playerData.eliminateLevel == 9)
        {
            List<PropItem> rewards = GameMainManager.Instance.configManager.matchLevelConfig.GetItem("1000008").itemReward;
            foreach(PropItem item in rewards)
            {
                if (item.id == "200006")
                {
                    m_itemNum.text = "×" + item.count.ToString();
                    break;
                }
            }
           /*
            string itemReward = DefaultConfig.getInstance().GetConfigByType<matchlevel>().GetItemByID("1000008").itemReward;
            string[] arr = itemReward.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr)
            {
                string[] arr1 = s.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);
                if (arr1[0] == "200006")
                {
                    m_itemNum.text = "×" + arr1[1];
                    break;
                }
            }
            */
            m_itemDes.text = LanguageManager.instance.GetValueByKey("210002");
            m_itemImg.sprite = Resources.Load("Sprites/UI/item005", typeof(Sprite)) as Sprite;
            m_itemImg.SetNativeSize();
        }
        else if (GameMainManager.Instance.playerData.eliminateLevel == 15)
        {
            GameMainManager.Instance.playerData.showUnlockItemStatus = "1";

            List<PropItem> rewards = GameMainManager.Instance.configManager.matchLevelConfig.GetItem("1000014").itemReward;
            foreach (PropItem item in rewards)
            {
                if (item.id == "200004")
                {
                    m_itemNum.text = "×" + item.count.ToString();
                    break;
                }
            }

            /*
            string itemReward = DefaultConfig.getInstance().GetConfigByType<matchlevel>().GetItemByID("1000014").itemReward;
            string[] arr = itemReward.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr)
            {
                string[] arr1 = s.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);
                if (arr1[0] == "200004")
                {
                    m_itemNum.text = "×" + arr1[1];
                    break;
                }
            }
            */
            m_itemDes.text = LanguageManager.instance.GetValueByKey("210003");
            m_itemImg.sprite = Resources.Load("Sprites/UI/item003", typeof(Sprite)) as Sprite;
            m_itemImg.SetNativeSize();
        }
        else if (GameMainManager.Instance.playerData.eliminateLevel == 17)
        {
            GameMainManager.Instance.playerData.showUnlockItemStatus = "2";

            List<PropItem> rewards = GameMainManager.Instance.configManager.matchLevelConfig.GetItem("1000016").itemReward;
            foreach (PropItem item in rewards)
            {
                if (item.id == "200003")
                {
                    m_itemNum.text = "×" + item.count.ToString();
                    break;
                }
            }

            /*
            string itemReward = DefaultConfig.getInstance().GetConfigByType<matchlevel>().GetItemByID("1000016").itemReward;
            string[] arr = itemReward.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr)
            {
                string[] arr1 = s.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);
                if (arr1[0] == "200003")
                {
                    m_itemNum.text = "×" + arr1[1];
                    break;
                }
            }
            */
            m_itemDes.text = LanguageManager.instance.GetValueByKey("210004");
            m_itemImg.sprite = Resources.Load("Sprites/UI/item002", typeof(Sprite)) as Sprite;
            m_itemImg.SetNativeSize();
        }
        else if (GameMainManager.Instance.playerData.eliminateLevel == 21)
        {
            GameMainManager.Instance.playerData.showUnlockItemStatus = "3";

            List<PropItem> rewards = GameMainManager.Instance.configManager.matchLevelConfig.GetItem("1000020").itemReward;
            foreach (PropItem item in rewards)
            {
                if (item.id == "200005")
                {
                    m_itemNum.text = "×" + item.count.ToString();
                    break;
                }
            }

            /*
            string itemReward = DefaultConfig.getInstance().GetConfigByType<matchlevel>().GetItemByID("1000020").itemReward;
            string[] arr = itemReward.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr)
            {
                string[] arr1 = s.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);
                if (arr1[0] == "200005")
                {
                    m_itemNum.text = "×" + arr1[1];
                    break;
                }
            }
            */
            m_itemDes.text = LanguageManager.instance.GetValueByKey("210005");
            m_itemImg.sprite = Resources.Load("Sprites/UI/item004", typeof(Sprite)) as Sprite;
            m_itemImg.SetNativeSize();
        }
    }
}
