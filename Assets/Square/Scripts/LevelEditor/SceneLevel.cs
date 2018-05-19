using System;
using System.Collections.Generic;
using UnityEngine;

namespace March.Scene
{
    [Serializable]
    public class Position
    {
        public float X;
        public float Y;
        public float Z;

        public Position()
        {
            X = Y = Z = 0;
        }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(Vector3 vec)
        {
            X = vec.x;
            Y = vec.y;
            Z = vec.z;
        }
    }

    [Serializable]
    public class GameInfo
    {
        //public GameObject Object;
        public Position Position;
        public int ID;
    }

    [Serializable]
    public class SceneLevel
    {
        public enum StuffType
        {
            Background,
            Player,
            NPC,
            Building
        }

        public static int Level;

        public List<GameInfo> Background;
        public List<GameInfo> Player;

        public List<GameInfo> BuildingList;
        public List<GameInfo> NPCList;

        public Dictionary<string, List<GameInfo>> GetSceneMap()
        {
            var map = new Dictionary<string, List<GameInfo>>
            {
                {StuffType.Background.ToString(), Background},
                {StuffType.Player.ToString(), Player},
                {StuffType.NPC.ToString(), NPCList},
                {StuffType.Building.ToString(), BuildingList}
            };
            return map;
        }
    }
}