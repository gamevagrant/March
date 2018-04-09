using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace March.Scene
{
    public class ScenePanelController : MonoBehaviour
    {
        public int LevelCount;

        public float MenuDuration = 0.5f;

        public Dictionary<string, List<PrefabInfo>> DataMap { get; set; }

        private bool menuAnimating;
        private int slip = -1;
        private RectTransform rectTrans;

        public string Info
        {
            set { InfoText.text = value; }
        }

        //public InputField LevelText;
        public Dropdown LevelDropDown;
        public List<InstanceController> InstanceControllerList;
        public Text InfoText;
        public Transform MenuTrans;

        public int Level
        {
            get { return int.Parse(LevelDropDown.captionText.text); }
            set { LevelDropDown.itemText.text = "" + value; }
        }

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
        }

        private void Start()
        {
            LevelDropDown.ClearOptions();
            for (var i = 0; i < LevelCount; ++i)
                LevelDropDown.options.Add(new Dropdown.OptionData { text = "" + (i + 1) });
            LevelDropDown.captionText.text = LevelDropDown.options[0].text;
        }

        public void FlushUI()
        {
            for (var i = 0; i < InstanceControllerList.Count; ++i)
            {
                var key = DataMap.ToList()[i].Key;
                var value = DataMap.ToList()[i].Value;
                InstanceControllerList[i].ItemText = key;
                InstanceControllerList[i].DropdownList.Clear();
                InstanceControllerList[i].DropdownList.AddRange(value.Select(item => item.Name));
                InstanceControllerList[i].FlushUI();
            }
        }

        public void OnMenuClicked()
        {
            if (menuAnimating)
                return;

            menuAnimating = true;
            rectTrans.DOLocalMoveX(rectTrans.localPosition.x + slip * rectTrans.rect.width, MenuDuration).OnComplete(() => menuAnimating = false);
            MenuTrans.DOScaleX(-MenuTrans.localScale.x, MenuDuration);
            slip = -slip;
        }
    }
}