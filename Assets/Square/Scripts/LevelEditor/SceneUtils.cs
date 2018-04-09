using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace March.Scene
{
    public class SceneUtils
    {
        public static Dictionary<string, List<PrefabInfo>> DictMap = new Dictionary<string, List<PrefabInfo>>();
        public const string SceneEditorPath = "SceneEditor";

        public static IEnumerator ScanAndLoad()
        {
            DictMap.Clear();

            var basePath = Path.Combine(Application.streamingAssetsPath, SceneEditorPath);
            foreach (var directory in Directory.GetDirectories(basePath))
            {
                var key = new FileInfo(directory).Name;
                DictMap.Add(key, new List<PrefabInfo>());
                foreach (var file in Directory.GetFiles(directory).Where(item => item.EndsWith(".png")))
                {
                    var uri = new Uri(file);
                    var www = new WWW(uri.AbsoluteUri);
                    yield return www;
                    DictMap[key].Add(new PrefabInfo { Name = new FileInfo(file).Name, Texture = www.texture });
                }
            }
        }

        public static GameObject CreateIdentifyGameObject(string path, int index, Transform parent, Vector3 position, Vector3 scale, int sortingOrder, bool isModifiable)
        {
            var go = new GameObject(DictMap[path][index].Name);
            go.transform.parent = parent;
            go.transform.position = position;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = scale;
            go.layer = parent.gameObject.layer;
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = CreateSprite(path, index);
            spriteRenderer.sortingOrder = sortingOrder;

            var itemController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ModifiableItem")).GetComponent<ItemActionController>();
            itemController.transform.parent = go.transform;
            itemController.transform.localPosition = Vector3.zero;
            itemController.Path = path;
            itemController.Index = index;
            itemController.IsModifiable = isModifiable;
            itemController.SceneEditor = parent.GetComponentInParent<SceneEditor>();

            return go;
        }

        public static Sprite CreateSprite(string path, int index)
        {
            var info = DictMap[path][index];
            return CreateIconSprite(path, index, info.Texture.width, info.Texture.height);
        }

        public static Sprite CreateIconSprite(string path, int index, float width, float height)
        {
            var info = DictMap[path][index];
            var sprite = Sprite.Create(info.Texture, new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = info.Texture.name;
            return sprite;
        }

        public static Vector3 GetScreenCenterPosition()
        {
            var vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            return new Vector3(vec.x, vec.y, 0);
        }
    }
}