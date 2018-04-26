using System.Collections;
using AssetBundles;
using March.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private LoadingSceneLoader loader;

    private AsyncOperation m_async;
    private Image m_progressBar;
    private Image m_progressBar_right;

    private void Awake()
    {
        loader = GetComponent<LoadingSceneLoader>();
    }

    IEnumerator Start()
    {
        Debug.Log("LoadingScene Start ...");

        var canvas = FindObjectOfType<Canvas>().gameObject;
        m_progressBar = canvas.transform.Find("ProgressBar").GetComponent<Image>();
        var go = canvas.transform.Find("ProgressBar_bg");
        m_progressBar_right = go.transform.Find("right").GetComponent<Image>();

#if GAME_DEBUG
        Instantiate(Resources.Load<GameObject>(Configure.ReporterPath));
#endif

        yield return loader.Load();

        AddConfig<storyhead>();
        AddConfig<quest>();
        AddConfig<story>();
        AddConfig<item>();
        AddConfig<setting>();
        AddConfig<matchlevel>();
        AddConfig<exchange>();
        AddConfig<guidesetup>();
        //AddConfig<language>();

        LanguageManager.instance.initConfig();

        qy.GameMainManager.Instance.configManager.LoadConfig(() =>
        {
            LoadScene();
        });     
    }

    void Update()
    {
        if (m_async != null && m_progressBar != null)
        {
            m_progressBar.fillAmount = m_async.progress;
            Debug.Log("------" + m_async.progress.ToString());
            if (m_async.progress >= 1.0f)
            {
                m_progressBar_right.gameObject.SetActive(true);
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
