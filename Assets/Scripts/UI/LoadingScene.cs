using March.Scene;
using qy;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private LoadingSceneLoader loader;
    private LoadingProgressController progressController;

    private AsyncOperation asyncOperation;

    private void Awake()
    {
        loader = GetComponent<LoadingSceneLoader>();
        progressController = GameObject.Find("Canvas/ProgressBar").GetComponent<LoadingProgressController>();

        Messenger.AddListener<string>(ELocalMsgID.LoadScene, OnLoadSceneHandle);

    }
    private void OnDestroy()
    {
        Messenger.RemoveListener<string>(ELocalMsgID.LoadScene, OnLoadSceneHandle);
    }

    IEnumerator Start()
    {
        Debug.Log("LoadingScene Start ...");       

#if GAME_DEBUG
        Instantiate(Resources.Load<GameObject>(Configure.ReporterPath));
#endif

        yield return loader.Load();

        GameMainManager.Instance.configManager.LoadConfig(() =>
        {
            progressController.StopTween();

            StartCoroutine(LoadScene("Film"));
        });
    }


    private IEnumerator LoadScene(string scene)
    {
        //m_async = SceneManager.LoadSceneAsync("main");
        asyncOperation = SceneManager.LoadSceneAsync(scene);
        yield return asyncOperation;
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

    private void OnApplicationQuit()
    {
        Messenger.Broadcast(ELocalMsgID.OnApplicationQuit);
    }
}
