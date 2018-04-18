using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace March.Core.WindowManager
{
    public class WindowManager : MonoSingleton<WindowManager>
    {
        public const string MapPath = "Config/WindowMap";

        private WindowManagerConfig windowManagerConfig;
        private readonly Dictionary<Type, GameObject> windowMap = new Dictionary<Type, GameObject>();

        protected override void Init()
        {
            base.Init();

            var textasset = Resources.Load<TextAsset>(MapPath);
            windowManagerConfig = JsonUtility.FromJson<WindowManagerConfig>(textasset.text);
        }

        /// <summary>
        /// Show window by type.
        /// </summary>
        /// <typeparam name="T">Type of window</typeparam>
        /// <returns>The window handle.</returns>
		public T Show<T>() where T : Window
        {
            var window = GetWindow<T>();

            if (window == null)
            {
                Debug.LogError(string.Format("Cannot find window according to type {0}", typeof(T)));
                return null;
            }

            var canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            var clone = Instantiate(windowMap[typeof(T)]).GetComponent<T>();
            clone.gameObject.transform.localScale = Vector3.zero;
            clone.gameObject.transform.SetParent(canvas.transform, false);
            clone.gameObject.SetActive(true);
            clone.gameObject.GetComponent<Popup>().Open();
            return clone;
        }

        /// <summary>
        /// Get window by type.
        /// </summary>
        /// <typeparam name="T">Window type.</typeparam>
        /// <returns>The window</returns>
		public T GetWindow<T>() where T : Window
        {
            var type = typeof(T);
            if (!windowMap.ContainsKey(type))
            {
                var windowConfig = windowManagerConfig.WindowMap[type.Name];
                if (windowConfig == null)
                {
                    Debug.LogError(string.Format("No window name-{0} registered in window map {1}", type.Name, MapPath));
					return null;
                }

                var prefab = Resources.Load<GameObject>(windowConfig.Path);
                windowMap.Add(type, prefab);
            }

			return windowMap[type].GetComponent<T>();
        }
    }
}