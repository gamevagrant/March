using System.Collections;
using AssetBundles;
using March.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using qy;
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

        GameMainManager.Instance.configManager.LoadConfig(() =>
        {
            StartCoroutine(LoadScene());
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

    private IEnumerator LoadScene()
    {
        //m_async = SceneManager.LoadSceneAsync("main");
        m_async = SceneManager.LoadSceneAsync("Film");
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
}
