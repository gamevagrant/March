using System.Collections;
using UnityEngine;

public class ConnectionChecker : MonoBehaviour
{
    public string Url = "www.baidu.com";

    IEnumerator Start()
    {
        var www = new WWW(Url);
        yield return www;

        if (www.isDone && www.bytesDownloaded > 0)
        {
            Debug.LogWarning("Connection succeed.");
        }
        if (www.isDone && www.bytesDownloaded == 0)
        {
            Debug.LogWarning("Connection failed.");
        }
    }
}