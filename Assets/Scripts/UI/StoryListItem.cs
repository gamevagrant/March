using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryListItem : MonoBehaviour
{

    public Text m_stroyItemText;
    public Button m_storyItemBtn;

    private void Awake()
    {

    }


    public void SetItemContent(StoryItem storyItem,MainScene scene)
    {
        m_stroyItemText.text = storyItem.dialogue;
        m_storyItemBtn.onClick.AddListener(() => {
            //scene.ShowStory(storyItem);
        });
    }
}
