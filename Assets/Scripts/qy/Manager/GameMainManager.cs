using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qy.ui;
namespace qy
{
    public class GameMainManager
    {
        private static GameMainManager _instance;
        public static GameMainManager Instance
        {
            get
            {
                if (Application.isPlaying && _instance == null)
                {
                    _instance = new GameMainManager();
                }
                return _instance;
            }
        }

        public ui.IUIManager uiManager
        {
            get
            {
                return ui.UIManager.Instance;
            }
        }
        public net.NetManager netManager
        {
            get
            {
                return net.NetManager.Instance;
            }
        }
        public IAudioManager audioManager
        {
            get
            {
                return AudioManager.Instance; 
            }
        }

        public config.ConfigManager configManager
        {
            get
            {
                return config.ConfigManager.Instance; 
            }
        }

        public WeatherManager weatherManager
        {
            get
            {
                return WeatherManager.Instance;
            }
        }

        public PlayerData playerData;
        public IPlayerModel playerModel;
        /// <summary>
        /// //全局脚本，可以使用monobehaviour方法
        /// </summary>
        public MonoBehaviour mono;
        

        public GameMainManager()
        {
            playerData = LocalDatasManager.playerData;
            playerModel = new PlayerModel(playerData);
            GuideManager guide = GuideManager.Instance;
            //netManager = net.NetManager.Instance;
            //configManager = config.ConfigManager.Instance;
            //uiManager = ui.UIManager.Instance;
            //audioManager = AudioManager.Instance;
            audioManager.SetSoundPathProxy(FilePathTools.getAudioPath);
            audioManager.SetMusicPathProxy(FilePathTools.getAudioPath);
            
        }

        public void Init()
        {

            


        }


    }
}


