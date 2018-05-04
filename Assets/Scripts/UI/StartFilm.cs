using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using qy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartFilm : MonoBehaviour
{
    private GameObject story;
    private Text storyText;
    private GameObject storyHead;
    private Text storyHeadText;
    private Image background;

    private Dictionary<string, Sprite> spriteMap;

    private qy.config.StoryHeadItem storyItem;
    private Tweener tweener;

    void Start()
    {
        story = transform.Find("Story").gameObject;
        storyText = transform.Find("Story/Text").GetComponent<Text>();

        storyHead = transform.Find("StoryHead").gameObject;
        storyHeadText = transform.Find("StoryHead/Text").GetComponent<Text>();

        background = GetComponent<Image>();

        var clickButton = transform.Find("ClickButton").GetComponent<Button>();
        clickButton.onClick.AddListener(PlayStory);

        var skipButton = transform.Find("SkipButton").GetComponent<Button>();
        skipButton.onClick.AddListener(SkipStory);

        spriteMap = March.Core.ResourceManager.ResourceManager.instance.LoadAll<Sprite>(Configure.FilmBackgroundPath)
            .ToDictionary(v => v.name, v => v);

        int chapter = GameMainManager.Instance.playerData.GetQuest().chapter;
        storyItem = GameMainManager.Instance.configManager.StoryHeadConfig.GetFirstWithChapter(chapter);

        PlayStoryHead();
    }

    private void PlayStoryHead()
    {
        story.SetActive(false);
        storyHead.SetActive(true);

        storyHeadText.text = string.Empty;
        tweener = storyHeadText.DOText(storyItem.dialogue, 1.5f);

        storyItem = storyItem.nextStory;
    }

    private void SkipStory()
    {
        SceneManager.LoadScene("main");
    }

    private void PlayStory()
    {
        story.SetActive(true);
        storyHead.SetActive(false);

        if (tweener != null)
            tweener.Kill(true);

        // end of all story head xml.
        if (storyItem == null)
        {
            LoadMainScene();
            return;
        }

        storyText.text = string.Empty;

        tweener = storyText.DOText(storyItem.dialogue, 1);

        background.sprite = spriteMap[storyItem.bgFile];

        storyItem = storyItem.nextStory;
    }
    private void LoadMainScene()
    {
        SceneManager.LoadScene("main");
    }
}
