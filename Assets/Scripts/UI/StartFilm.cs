using qy.config;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using qy;

public class StartFilm : MonoBehaviour
{

    //private storyhead m_stroy_head;
    //public storyhead Story_Head { get { if (m_stroy_head == null) { m_stroy_head = DefaultConfig.getInstance().GetConfigByType<storyhead>(); } return m_stroy_head; } }
    
    //private language_cn m_language_cn;
    //public language_cn Language_CN { get { if (m_language_cn == null) { m_language_cn = DefaultConfig.getInstance().GetConfigByType<language_cn>(); } return m_language_cn; } }
   
    private Text m_text;
    private Image m_filmBackground;

    public Button m_skipBtn;

    //需要显示剧情的图片只需要把图片改成剧情对应的index就行，然后把图片放在Resources/StartFilm
    private Sprite[] m_filmImages;

    //private StoryHeadItem m_storyItem;
    private qy.config.StoryHeadItem storyItem;

    void Start()
    {
        m_text = transform.Find("Story/Text").GetComponent<Text>();
        m_filmBackground = GetComponent<Image>();

        transform.Find("Button").GetComponent<Button>().onClick.AddListener(PlayStory);
        transform.Find("skip_btn").GetComponent<Button>().onClick.AddListener(SkipStory);

        LoadStartFilmImages();

        //开始索引
        //m_storyItem = Story_Head.GetItemByID("1000101");
        storyItem = GameMainManager.Instance.configManager.StoryHeadConfig.GetItem("1000101");

        PlayStory();
    }
    //加载播放时需要的图片
    private void LoadStartFilmImages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("StartFilm");
        m_filmImages = sprites;
    }
    //显示剧情对应的背景图片
    private void LoadFilmImage(ref string bgName)
    {
        for (int i = 0; i < m_filmImages.Length; i++)
        {
            if (m_filmImages[i].name == bgName)
            {
                m_filmBackground.sprite = m_filmImages[i];
                return;
            }
        }
    }

    private void SkipStory()
    {
        SceneManager.LoadScene("main");
    }

    private void PlayStory()
    {
        //如果当前剧情播放完就加载main场景
        if (storyItem == null)
        {
            LoadMainScene();
            return;
        }

		string story = storyItem.dialogue;
        m_text.text = story;

        LoadFilmImage(ref storyItem.bgFile);

        storyItem = storyItem.nextStory;
    }
    private void LoadMainScene()
    {
        SceneManager.LoadScene("main");
    }

}
