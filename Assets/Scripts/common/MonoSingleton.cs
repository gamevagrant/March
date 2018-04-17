using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static bool _destroyed;
        public static T instance
        {
            get
            {
                return GetInstance();
            }
        }

        public static T GetInstance()
        {
            if (_instance == null && !_destroyed)
            {
                Type typeFromHandle = typeof(T);
                _instance = (T)FindObjectOfType(typeFromHandle);
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                    GameObject gameObject2 = GameObject.Find("RootObj");
                    if (gameObject2 != null)
                    {
                        gameObject.transform.SetParent(gameObject2.transform);
                    }
                }
            }
            return _instance;
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }
            _destroyed = true;
            _instance = null;
        }

        public static void ClearDestroy()
        {
            DestroyInstance();
            _destroyed = false;
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }
            else
            {
                if (_instance == null)
                {
                    _instance = GetComponent<T>();
                }
            }
            DontDestroyOnLoad(gameObject);
            Init();

            SceneManager.sceneLoaded += sceneLoadedHandler;
        }

        protected virtual void OnDestroy()
        {
            if (_instance != null && _instance.gameObject == gameObject)
            {
                _instance = null;
            }

            SceneManager.sceneLoaded -= sceneLoadedHandler;
        }

        public static bool HasInstance()
        {
            return _instance != null;
        }

        public static bool HasActiveInstance()
        {
            return _instance != null && _instance.gameObject.activeInHierarchy;
        }

        protected virtual void Init()
        {
        }

        protected virtual void sceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
        }
    }
}
