using System;
using System.Collections.Generic;
using UnityEngine;

namespace qy.config
{
    public class ConfigManager
    {
        private static ConfigManager _instance;
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigManager();
                }

                return _instance;
            }
        }
        public LangrageConfig langrageConfig
        {
            get
            {
                BaseConfig config = dic[typeof(LangrageConfig)];
                return config as LangrageConfig;
            }
        }

        public PropsConfig propsConfig
        {
            get
            {
                BaseConfig config = dic[typeof(PropsConfig)];
                return config as PropsConfig;
            }
        }

        public StorysConfig storysConfig
        {
            get
            {
                BaseConfig config = dic[typeof(StorysConfig)];
                return config as StorysConfig;
            }
        }

        public StoryheadConfig StoryHeadConfig
        {
            get
            {
                BaseConfig config = dic[typeof(StoryheadConfig)];
                return config as StoryheadConfig;
            }
        }

        public QuestConfig questConfig
        {
            get
            {
                BaseConfig config = dic[typeof(QuestConfig)];
                return config as QuestConfig;
            }
        }

        public Dictionary<Type, BaseConfig> dic;
        private int allCount;
        private Action onComplate;

        public ConfigManager()
        {

        }

        public void LoadConfig(Action onComplate)
        {
            allCount = 0;
            dic = new Dictionary<Type, BaseConfig>();
            this.onComplate = onComplate;

            //以下加载顺序不可变更
            Load<LangrageConfig>(LoadHandle);
            Load<PropsConfig>(LoadHandle);
            Load<StorysConfig>(LoadHandle);
            Load<QuestConfig>(LoadHandle);
            Load<StoryheadConfig>(LoadHandle);
            Load<MatchLevelConfig>(LoadHandle);
            Load<ExchangeConfig>(LoadHandle);
            Load<GuideSetupConfig>(LoadHandle);
            Load<SettingConfig>(LoadHandle);
        }

        private void Load<T>(Action onComplate) where T : BaseConfig, new()
        {
            allCount++;
            T config = new T();
            string path = FilePathTools.getXmlPath(config.Name);
            Debug.Log("加载配置文件:" + path);
            AssetsManager.Instance.LoadAssetWithWWW(path, (www) =>
            {
                config.Read(www.text);
                dic.Add(typeof(T), config);
                onComplate();
            });

        }

        private void LoadHandle()
        {
            Debug.Log("配置文件加载进度:" + dic.Count + "/" + allCount);
            if (dic.Count >= allCount)
            {
                onComplate();
            }
        }
    }
}

