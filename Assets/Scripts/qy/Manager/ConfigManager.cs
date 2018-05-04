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
        public GuideConfig guideConfig
        {
            get
            {
                BaseConfig config = dic[typeof(GuideConfig)];
                return config as GuideConfig;
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

        public LevelConfig levelConfig
        {
            get
            {
                BaseConfig config = dic[typeof(LevelConfig)];
                return config as LevelConfig;
            }
        }

        public RoleConfig roleConfig
        {
            get
            {
                BaseConfig config = dic[typeof(RoleConfig)];
                return config as RoleConfig;
            }
        }

        private Dictionary<Type, BaseConfig> dic = new Dictionary<Type, BaseConfig>();
        private int allCount = 0;

        private Action onComplate;

        public ConfigManager()
        {
            InitTypeDict<LanguageConfig>();
            InitTypeDict<PropsConfig>();
            InitTypeDict<StorysConfig>();
            InitTypeDict<QuestConfig>();
            InitTypeDict<StoryheadConfig>();
            InitTypeDict<MatchLevelConfig>();
            InitTypeDict<ExchangeConfig>();
            InitTypeDict<GuideSetupConfig>();
            InitTypeDict<SettingConfig>();
            InitTypeDict<RoleConfig>();
            InitTypeDict<LevelConfig>();
        }

        private void InitTypeDict<T>() where T : BaseConfig, new()
        {
            dic.Add(typeof(T), new T());
        }

        public void LoadConfig(Action onComplate)
        {
            this.onComplate = onComplate;

            foreach (var pair in dic)
            {
                var config = pair.Value;

                Debug.Log("加载配置文件:" + config.Name());

                var asset = March.Core.ResourceManager.ResourceManager.instance.Load<TextAsset>(Configure.ConfigurePath, config.Name().Replace(".xml", ""));
                config.Read(asset.text);
            }

            onComplate();
        }

        private void Load<T>(Action onComplate) where T : BaseConfig, new()
        {
            allCount++;
            T config = new T();
            string path = FilePathTools.getXmlPath(config.Name());
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

