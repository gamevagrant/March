using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private AsyncOperation m_async;
    private Image m_progressBar;
	private Image m_progressBar_right;
    // Use this for initialization

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        AddConfig<storyhead>();
        AddConfig<quest>();
        AddConfig<story>();
        AddConfig<item>();
        AddConfig<setting>();
        AddConfig<matchlevel>();
        AddConfig<exchange>();
        AddConfig<guidesetup>();

        LanguageManager.instance.initConfig();
    }

    void Start()
    {
        Debug.Log("LoadingScene Start ...");

        //PlayerPrefs.DeleteAll(); //这里测试测试新手
        var canvas = FindObjectOfType<Canvas>().gameObject;
        m_progressBar = canvas.transform.Find("ProgressBar").GetComponent<Image>();
		var go = canvas.transform.Find ("ProgressBar_bg");
		m_progressBar_right = go.transform.Find ("right").GetComponent<Image> ();

#if UNITY_ANDROID
         PltformManager.instance.setPlatform("android");
#endif

#if UNITY_IPHONE
        PltformManager.instance.setPlatform("ios");
#endif

#if UNITY_STANDALONE_WIN
        PltformManager.instance.setPlatform("win32");
#endif        
        LoadScene();
    }

    void Update()
    {
        if (m_async != null && m_progressBar != null)
        {
            m_progressBar.fillAmount = m_async.progress;
			Debug.Log ("------" + m_async.progress.ToString ());
			if (m_async.progress >= 1.0f) {
				m_progressBar_right.gameObject.SetActive (true);
			}
        }
    }

    private void AddConfig<T>() where T : DatabaseConfig, new()
    {
        T config = new T();
        StartCoroutine(XMLDataManager.instance.loadXML(config));
    }

    private void LoadScene()
    {
        //m_async = SceneManager.LoadSceneAsync("main");
        m_async = SceneManager.LoadSceneAsync("Film");
    }
}
