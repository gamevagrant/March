using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace March.Scene
{
    public class InstanceController : MonoBehaviour
    {
        public string ItemText;
        public List<string> DropdownList;

        public int CurrentItemIndex
        {
            get { return dropdown.value; }
        }

        private Text itemText;
        private Dropdown dropdown;

        void Awake()
        {
            itemText = transform.Find("ItemText").GetComponent<Text>();
            dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
            transform.Find("Button").GetComponent<Button>();
        }

        void Start()
        {
            FlushUI();
        }

        public void FlushUI()
        {
            itemText.text = ItemText;
            dropdown.options.Clear();
            dropdown.options.AddRange(DropdownList.Select(item =>
                new Dropdown.OptionData(item)));
            if (DropdownList.Count > 0)
                dropdown.captionText.text = DropdownList[0];
        }
    }
}