using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager {

    public enum WeatherEnum
    {
        Null = 0,
        Rain =1,
        Snow,
    }

    private static WeatherManager _instance;
    public static WeatherManager Instance
    {
        get
        {
            if (Application.isPlaying && _instance == null)
            {
                _instance = new WeatherManager();
            }
            return _instance;
        }
    }

    private Dictionary<WeatherEnum, GameObject> dic = new Dictionary<WeatherEnum, GameObject>();
    private GameObject root;
    private Camera camera;
    private const string LAYER_NAME  = "Weather";

    public void StartWeather(WeatherEnum weather)
    {
        if(weather == WeatherEnum.Null)
        {
            StopAll();
            return;
        }
        GameObject go;
        dic.TryGetValue(weather, out go);
        if(go!=null)
        {
            go.SetActive(true);
        }else
        {
           string path = FilePathTools.GetParticle(weather.ToString());
            AssetsManager.Instance.LoadAssetAsync<GameObject>(path, (res) =>
            {
                ConfigWeather(weather,res);
            });
        }
    }

    public void StopWeather(WeatherEnum weather)
    {
        GameObject go;
        dic.TryGetValue(weather, out go);
        if(go!=null)
        {
            go.SetActive(false);
        }
    }

    public void StopAll()
    {
        foreach(GameObject go in dic.Values)
        {
            go.SetActive(false);
        }
    }

    private void ConfigWeather(WeatherEnum weather, GameObject go)
    {
        if(root == null)
        {
            root = new GameObject("WeatherRoot");
            root.transform.position = Vector3.zero;
            root.layer = LayerMask.NameToLayer(LAYER_NAME);
            
            camera = new GameObject("Camera").AddComponent<Camera>();
            camera.transform.parent = root.transform;
            camera.transform.localPosition = new Vector3(0,0,-10);
            camera.cullingMask = LayerMask.GetMask(new string[]{ LAYER_NAME });
            camera.depth = 2;
            camera.clearFlags = CameraClearFlags.Depth;
        }

        GameObject weatherGO = GameUtils.CreateGameObject(root.transform, go);
        weatherGO.transform.position += new Vector3(0,10,0);
        if(dic.ContainsKey(weather))
        {
            dic[weather] = weatherGO;
        }else
        {
            dic.Add(weather, weatherGO);
        }
        
        weatherGO.SetActive(true);
    }
}
