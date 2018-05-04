using LitJson;
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
            yield return Loader.Load();

            int chapter = qy.GameMainManager.Instance.playerData.GetQuest().chapter;
            Data = Resources.Load<TextAsset>("Level_" + chapter);

            if (AutoLoad)
            {
                SceneLevel = JsonMapper.ToObject<SceneLevel>(Data.text);
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
            var json = JsonMapper.ToJson(SceneLevel);
            File.WriteAllText(path, json);

            Message = string.Format("Save level to file: {0}", new FileInfo(path).FullName);
        }

        public void Read(string path)
        {
            Message = string.Format("Load level from file: {0}", new FileInfo(path).FullName); ;
            try
            {
                var json = File.ReadAllText(path);
                SceneLevel = JsonMapper.ToObject<SceneLevel>(json);
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
                var isModifiable = SceneLayerMap[pair.Key].Modifiable;
                foreach (var gameInfo in list)
                {
                    //var go = SceneUtils.CreateIdentifyGameObject(key, gameInfo.ID, ParentMap[key],
                    //    new Vector3((float)gameInfo.Position.X, (float)gameInfo.Position.Y, (float)gameInfo.Position.Z),
                    //    DefaultSceneDataMap[key].Scale, DefaultSceneDataMap[key].SortingOrder, isModifiable);
                    var position = new Vector3((float)gameInfo.Position.X, (float)gameInfo.Position.Y, (float)gameInfo.Position.Z);
                    var go = Loader.CreateIdentifyGameObject(SceneLayerMap[key], gameInfo.ID, position);
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