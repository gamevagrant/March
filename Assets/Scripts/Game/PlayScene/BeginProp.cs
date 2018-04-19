using March.Core.WindowManager;
using UnityEngine;
using UnityEngine.UI;

public delegate void BuyBtnCallBcak(int num);

public class BeginProp : MonoBehaviour
{
    public BOOSTER_TYPE BoosterType;

    public Image Icon;
    public Image m_lockImage;
    public Text Num;
    public Image m_numImage;
    public string ItemId;

    private bool isPress;
    private bool isLock;

    private void setGray()
    {
        m_lockImage.gameObject.SetActive(true);
        isLock = true;
    }

    public void Init(int itemid)
    {
        isPress = false;
        ItemId = string.Format("{0}", itemid);

        int num = qy.GameMainManager.Instance.playerData.GetPropItem(ItemId).count;
        if (num == 0)
        {
            Num.gameObject.SetActive(false);
            m_numImage.gameObject.SetActive(true);
            m_numImage.GetComponent<Image>().sprite = Resources.Load("Sprites/UI/add", typeof(Sprite)) as Sprite;
            Num.text = "+";
        }
        else
        {
            Num.gameObject.SetActive(true);
            m_numImage.gameObject.SetActive(false);
            Num.text = num.ToString();
        }

        if (LevelLoader.instance.level < 15)
        { //全部没有
            setGray();
        }
        else if (LevelLoader.instance.level < 17)
        { //炸弹
            if (itemid == 200003 || itemid == 200005)
            {
                setGray();
            }
        }
        else if (LevelLoader.instance.level < 21)
        { //飞机
            if (itemid == 200005)
            {
                setGray();
            }
        }

        GoodsItem goodsItem = DefaultConfig.getInstance().GetConfigByType<item>().GetItemByID(ItemId);
        Sprite sp = Resources.Load(string.Format("Sprites/UI/{0}", goodsItem.icon), typeof(Sprite)) as Sprite;
        Icon.sprite = sp;
        Icon.SetNativeSize();

        if (itemid == 200004 && qy.GameMainManager.Instance.playerData.showUnlockItemStatus == "1")
        {
            WindowManager.instance.Show<UIFlashPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210003"));
            onUseBtnClick();
            qy.GameMainManager.Instance.playerData.showUnlockItemStatus = "0";
        }
        else if (itemid == 200003 && qy.GameMainManager.Instance.playerData.showUnlockItemStatus == "2")
        {
            WindowManager.instance.Show<UIFlashPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210004"));
            onUseBtnClick();
            qy.GameMainManager.Instance.playerData.showUnlockItemStatus = "0";
        }
        else if (itemid == 200005 && qy.GameMainManager.Instance.playerData.showUnlockItemStatus == "3")
        {
            WindowManager.instance.Show<UIFlashPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210005"));
            onUseBtnClick();
            qy.GameMainManager.Instance.playerData.showUnlockItemStatus = "0";
        }
    }

    public void onUseBtnClick()
    {
        if (isLock)
        {
            WindowManager.instance.Show<UIAlertPopupWindow>().Init(LanguageManager.instance.GetValueByKey("210134"));

            return;
        }

        if (isPress)
        {
            isPress = false;
            Num.gameObject.SetActive(true);
            m_numImage.gameObject.SetActive(false);

            LevelLoader.instance.beginItemList.Remove(ItemId);
        }
        else
        {
            if (Num.text == "+")
            {
                var boosterWindow = WindowManager.instance.Show<SingleBoosterPopupWindow>().GetComponent<UIBoosterPopup>();

                switch (ItemId)
                {
                    case "200003":
                        boosterWindow.booster = BOOSTER_TYPE.BEGIN_RAINBOW_BREAKER;
                        break;
                    case "200004":
                        boosterWindow.booster = BOOSTER_TYPE.BEGIN_BOMB_BREAKER;
                        break;
                    case "200005":
                        boosterWindow.booster = BOOSTER_TYPE.BEGIN_PLANE_BREAKER;
                        break;
                }

                boosterWindow.Init(bCallBack);
            }
            else
            {
                isPress = true;
                Num.gameObject.SetActive(false);
                m_numImage.gameObject.SetActive(true);
                if (Num.text == "+")
                {
                    m_numImage.GetComponent<Image>().sprite = Resources.Load("Sprites/UI/btn_add_01", typeof(Sprite)) as Sprite;
                }
                else
                {
                    m_numImage.GetComponent<Image>().sprite = Resources.Load("Sprites/UI/sele", typeof(Sprite)) as Sprite;
                }

                LevelLoader.instance.beginItemList.Add(ItemId);
            }
        }
    }

    public void bCallBack(int num)
    {
        Num.text = num.ToString();
        Num.gameObject.SetActive(true);
        m_numImage.gameObject.SetActive(false);
    }
}
