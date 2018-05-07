using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace qy
{
    /// <summary>
    /// 
    /// </summary>
    public class GuideManager : MonoBehaviour
    {
        private static GuideManager _instance;
        public static GuideManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    Init();
                }

                return _instance;
            }
        }

        public RectTransform dialog;
        public RectTransform arrow;
        public RectTransform mask;
        public Text text;

        private string guideStepID;//每个阶段的id 用于判断是否完成此次引导步骤
        private config.GuideItem guideItem;
        private Dictionary<string, string> displayedGuides;//展示过的引导id
        private GameObject maskClickGO;

        private static void Init()
        {
            AssetsManager.Instance.LoadAssetAsync<GameObject>(FilePathTools.getUIPath("GuideManager"),(go)=> {

                GameObject guideGO = GameObject.Instantiate(go);

            });
        }

        private void Awake()
        {
            _instance = this;
            mask.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            HideDialog();
            GameObject.DontDestroyOnLoad(gameObject);
            Messenger.AddListener<ui.UISettings.UIWindowID>(ELocalMsgID.OpenUI, OnOpenUIHandle);
            Messenger.AddListener<ui.UISettings.UIWindowID>(ELocalMsgID.CloseUI, OnCloseUIHandle);
            displayedGuides = LocalDatasManager.displayedGuides;
        }

        private void OnDestroy()
        {
            _instance = null;
            Messenger.RemoveListener<ui.UISettings.UIWindowID>(ELocalMsgID.OpenUI, OnOpenUIHandle);
            Messenger.RemoveListener<ui.UISettings.UIWindowID>(ELocalMsgID.CloseUI, OnCloseUIHandle);
        }

        private void OnOpenUIHandle(ui.UISettings.UIWindowID uiID)
        {
            Debug.Log("打开面板"+uiID.ToString());
            config.GuideItem item = GetUnDisplayedGuideWithUIid(uiID);
            
            if (item!=null)
            {
                guideStepID = item.id;
                Show(item);
            }
        }

        private void OnCloseUIHandle(ui.UISettings.UIWindowID uiID)
        {
            Debug.Log("关闭面板" + uiID.ToString());
            //关闭ui时的处理

        }

        private void OnGuideItemEnd(config.GuideItem item)
        {
            //特殊处理 结束引导时的处理
            if(item.id == "10001")
            {
                GameMainManager.Instance.uiManager.OpenWindow(ui.UISettings.UIWindowID.UINickNameWindow);
            }
        }

        private void Show(config.GuideItem item)
        {
            guideItem = item;
            mask.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(guideItem.highlight))
            {
                ShowHightLight(guideItem.highlight);
            }

            if (item.type != config.GuideConfig.GuideType.Click)
            {
                if (!string.IsNullOrEmpty(guideItem.dialogue))
                {
                    ShowDialog(guideItem);
                }
            }
        }

        private void Next()
        {
            CancelHightLight();
            if (guideItem == null)
            {
                return;
            }
            
            config.GuideItem nextItem = guideItem.next as config.GuideItem;
            if (nextItem !=null)
            {
                Show(nextItem);
            }else
            {
                AddDisplayedGuide(guideStepID);
                OnGuideItemEnd(guideItem);
            }
        }

        private void ShowHightLight(string widgtName)
        {
            StartCoroutine(FindTarget(widgtName,(go)=> {

                
                maskClickGO = go;
                Canvas canvas = go.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 3;
                go.AddComponent<GraphicRaycaster>();
                if (guideItem.type == config.GuideConfig.GuideType.Click)
                {
                    EventTriggerListener.GetListener(go).onPointerClick += OnClickTargetHandle;
                    //arrow.position = go.transform.position;
                    //arrow.gameObject.SetActive(true);
                }
            }));

        }

        private IEnumerator FindTarget(string widgtName,System.Action<GameObject> onFinded)
        {
            Transform tf = GameMainManager.Instance.uiManager.root.Find(widgtName);
            while (tf == null || !tf.gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.2f);
                if (tf == null)
                    tf = GameMainManager.Instance.uiManager.root.Find(widgtName);
            }
            if (tf != null)
                onFinded(tf.gameObject);
        }

        private void CancelHightLight()
        {
            if(maskClickGO==null)
            {
                return;
            }
            GameObject target = maskClickGO;
            mask.gameObject.SetActive(false);
            Destroy(target.GetComponent<GraphicRaycaster>());
            Destroy(target.GetComponent<Canvas>());
            arrow.gameObject.SetActive(false);
            
            EventTriggerListener.GetListener(target).onPointerClick -= OnClickTargetHandle;
            maskClickGO = null;
        }



        private void AddDisplayedGuide(string id)
        {
            if (!displayedGuides.ContainsKey(id))
            {
                displayedGuides.Add(id, id);
                LocalDatasManager.displayedGuides = displayedGuides;
            }
        }

        private config.GuideItem GetUnDisplayedGuideWithUIid(ui.UISettings.UIWindowID id)
        {
            config.GuideItem guide = null;
            //除了特殊情况 从配置表里遍历寻找没有展示过的 当前面板的引导
            List<config.GuideItem> allItems = GameMainManager.Instance.configManager.guideConfig.GetItemWithWindowName(id.ToString());
            List<config.GuideItem> list = new List<config.GuideItem>();//步骤id 每个步骤起始引导的id
            if (allItems!=null)
            {
                //查找每个步骤的起始id
                for (int i = 0; i < allItems.Count; i++)
                {
                    config.GuideItem item = allItems[i];
                    if (i > 0)
                    {
                        config.GuideItem lastItem = allItems[i - 1];
                        if (lastItem.nextId == item.id)
                        {
                            continue;
                        }
                    }
                    list.Add(item);
                }
            }


            for (int i = 0; i < list.Count; i++)
            {
                if (!displayedGuides.ContainsKey(list[i].id))
                {
                    guide = list[i];
                    break;
                }

            }
            if (guide == null)
            {
                return null;
            }
            //特殊处理
            if(id == ui.UISettings.UIWindowID.UITaskWindow)
            {
                config.QuestItem quest = GameMainManager.Instance.playerData.GetQuest();
                if (quest.type == config.QuestItem.QuestType.Branch && guide.id != "10014")
                {
                    return null;
                }
            }
            

            return guide;
            string guideID = "";
            if (id == ui.UISettings.UIWindowID.UITaskWindow)
            {
                config.QuestItem quest = GameMainManager.Instance.playerData.GetQuest();
                string[] ids = new string[] { "10006", "10012", "10014", "10020" };
                if (quest.type == config.QuestItem.QuestType.Branch)
                {
                    guideID = "10014";

                }
                else
                {
                    guideID = "10006";
                    if (displayedGuides.ContainsKey(guideID))
                    {
                        guideID = "10012";
                    }
                }
                if (string.IsNullOrEmpty(guideID) || displayedGuides.ContainsKey(guideID))
                {
                    guideID = "10020";
                }
            }
            if (!string.IsNullOrEmpty(guideID))
            {
                if (!displayedGuides.ContainsKey(guideID))
                {
                    return GameMainManager.Instance.configManager.guideConfig.GetItem(guideID);
                }
                else
                {
                    return null;
                }

            }

            return null;
        }

        private void OnClickTargetHandle(GameObject go)
        {
            Next();
        }

        public void OnClickPanel()
        {
            if (guideItem!=null && guideItem.type== config.GuideConfig.GuideType.Dialog)
            {
                HideDialog();
                Next();
            }
            
        }
        private void ShowDialog(config.GuideItem item)
        {
            dialog.gameObject.SetActive(true);
            (dialog.transform as RectTransform).anchoredPosition = item.position;
            text.text = item.dialogue;
        }

        private void HideDialog()
        {
            dialog.gameObject.SetActive(false);
            mask.gameObject.SetActive(false);
        }
    }
}

