using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace March.Scene
{
    public class SceneSerializer : MonoBehaviour
    {
        public class SceneData
        {
            public Vector3 Position;
            public Vector3 Scale;
            public int SortingOrder;
        }

        public bool AutoLoad;
        public TextAsset Data;

        public SceneLevel SceneLevel;
        public SceneAssetLoader Loader;
        public SceneLoader SceneLoader;

        public List<GameObject> ListenerList;

        public event EventHandler<EventArgs> SceneIsReady;

        public string Message { get; set; }

        public readonly Dictionary<string, SceneLayerInfo> SceneLayerMap = new Dictionary<string, SceneLayerInfo>();
        public readonly Dictionary<string, List<GameObject>> GoMap = new Dictionary<string, List<GameObject>>();

        private IEnumerator Start()
        {
            Enum.GetNames(typeof(SceneLevel.StuffType)).ToList().ForEach(key =>
            {
                var value = transform.Find(key);
                if (value == null)
                    Debug.LogError("Cannot find transform from key: " + key);
                var layerInfo = value.GetComponent<SceneLayerInfo>();
                SceneLayerMap.Add(key, layerInfo);
            });

            //yield return SceneUtils.ScanAndLoad();
            if (SceneLoader != null)
                yield return SceneLoader.Load();

            qy.config.QuestItem quest = qy.GameMainManager.Instance.playerData.GetQuest();
            int chapter = quest!=null? quest.chapter:1;
            Data = Resources.Load<TextAsset>("Level_" + chapter);

            if (AutoLoad)
            {
                SceneLevel = JsonUtility.FromJson<SceneLevel>(Data.text);
                GenerateGoMap();
            }

            if (SceneIsReady != null)
                SceneIsReady(this, EventArgs.Empty);
        }

        public void Write(int level, string path)
        {
            SceneLevel.Level = level;

            foreach (var pair in SceneLevel.GetSceneMap())
            {
                var key = pair.Key;
                var list = pair.Value;
                list.Clear();
                if (GoMap.ContainsKey(key))
                {
                    list.AddRange(GoMap[key].Select(go =>
                    {
                        var controller = go.GetComponentInChildren<ItemActionController>();
                        return new GameInfo { ID = controller.Index, Position = new Position(go.transform.position) };
                    }));
                }
            }
            var json = JsonUtility.ToJson(SceneLevel);
            File.WriteAllText(path, json);

            Message = string.Format("Save level to file: {0}", new FileInfo(path).FullName);
        }

        public void Read(string path)
        {
            Message = string.Format("Load level from file: {0}", new FileInfo(path).FullName); ;
            try
            {
                var json = File.ReadAllText(path);
                SceneLevel = JsonUtility.FromJson<SceneLevel>(json);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            GenerateGoMap();
        }

        private void GenerateGoMap()
        {
            ClearGoMap();
            foreach (var pair in SceneLevel.GetSceneMap())
            {
                var key = pair.Key;
                var list = pair.Value;
                GoMap.Add(key, new List<GameObject>());
                foreach (var gameInfo in list)
                {
                    var position = new Vector3(gameInfo.Position.X, gameInfo.Position.Y, gameInfo.Position.Z);
                    var go = Loader.CreateIdentifyGameObject(SceneLayerMap[key], gameInfo.ID, position);
                    if (SceneLoader != null)
                    {
                        go = SceneLoader.CreateIdentifyGameObject(SceneLayerMap[key], gameInfo.ID, position);
                    }
                    GoMap[pair.Key].Add(go);
                }
            }
        }

        public void ClearGoMap()
        {
            // destroy game objects.
            foreach (var pair in GoMap)
            {
                pair.Value.ForEach(Destroy);
            }

            GoMap.Clear();
        }

        public void RemoveItem(string path, GameObject go)
        {
            var index = GoMap[path].IndexOf(go);
            if (index == -1)
            {
                Message = "Cannot find gameobject: " + go.name;
                return;
            }

            Message = "Remove gameobject: " + go.name;
            GoMap[path].RemoveAt(index);

            Destroy(go);
        }
    }
}