using March.Scene;
using qy;
using System.Collections;
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

        Messenger.AddListener<string>(ELocalMsgID.LoadScene, OnLoadSceneHandle);

    }
    private void OnDestroy()
    {
        Messenger.RemoveListener<string>(ELocalMsgID.LoadScene, OnLoadSceneHandle);
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

        GameMainManager.Instance.configManager.LoadConfig(() =>
        {
            StartCoroutine(LoadScene("Film"));
        });
    }

    void Update()
    {
        if (m_async != null && m_progressBar != null)
        {
            m_progressBar.fillAmount = m_async.progress;
            Debug.Log("------" + m_async.progress);
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

    private IEnumerator LoadScene(string scene)
    {
        //m_async = SceneManager.LoadSceneAsync("main");
        m_async = SceneManager.LoadSceneAsync(scene);
        yield return m_async;
        Login();
    }

    public void Login()
    {
        GameMainManager.Instance.playerModel.UpdateHeart();
        bool isNetGood = GameMainManager.Instance.netManager.isNetWorkStatusGood;
        bool isInitPlayer = !string.IsNullOrEmpty(GameMainManager.Instance.playerData.userId);
        if (isNetGood)
        {
            if (!isInitPlayer)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    WaitingPopupManager.instance.Show(canvas.gameObject);
                }
            }
            GameMainManager.Instance.netManager.Login(new LoginInfo(), (ret, res) =>
            {

                WaitingPopupManager.instance.Close();
            });
        }
        else if (!isInitPlayer)
        {
            qy.ui.Alert.Show(LanguageManager.instance.GetValueByKey("210157"), qy.ui.Alert.OK, (btn) =>
            {
                Debug.Log("----------");
                Application.Quit();
            });
        }
    }

    private void OnLoadSceneHandle(string scene)
    {
        StartCoroutine(LoadScene(scene));
    }
}
