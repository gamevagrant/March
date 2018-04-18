using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.config;

namespace qy
{
    public class LangrageManager
    {
        private static LangrageManager _instance;
        public static LangrageManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new LangrageManager();
                }
                return _instance;
            }
        }

        public string GetItemWithID(string id)
        {
            return ConfigManager.Instance.languageConfig.GetItem(id);
        }
    }
}

