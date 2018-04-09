using System;
using System.Collections.Generic;
using UnityEngine;

namespace March.Scene
{
    public class SceneLayerInfo : MonoBehaviour
    {
        public bool Modifiable;

        public List<PrefabInfo> PrefabList;
    }

    [Serializable]
    public class PrefabInfo
    {
        public string Name;
        public GameObject Prefab;
        public Texture2D Texture;
    }
}