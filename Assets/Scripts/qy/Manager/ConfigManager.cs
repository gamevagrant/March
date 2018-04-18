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
        public LanguageConfig languageConfig
        {
            get
            {
                BaseConfig config = dic[typeof(LanguageConfig)];
                return config as LanguageConfig;
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


        public MatchLevelConfig matchLevelConfig
        {
            get
            {
                BaseConfig config = dic[typeof(MatchLevelConfig)];
                return config as MatchLevelConfig;
            }
        }

        public ExchangeConfig exchangeConfig
        {
            get
            {
                BaseConfig config = dic[typeof(ExchangeConfig)];
                return config as ExchangeConfig;
            }
        }

        public GuideSetupConfig guideSetupConfig
        {
            get
            {
                BaseConfig config = dic[typeof(GuideSetupConfig)];
                return config as GuideSetupConfig;
            }
        }

        public StoryheadConfig storyheadConfig
        {
            get
            {
                BaseConfig config = dic[typeof(StoryheadConfig)];
                return config as StoryheadConfig;
            }
        }

        public SettingConfig settingConfig
        {
            get
            {
                BaseConfig config = dic[typeof(SettingConfig)];
                return config as SettingConfig;
            }
        }

        private Dictionary<Type,BaseConfig> dic;
        private int allCount = 0;

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
            Load<LanguageConfig>(LoadHandle);
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

