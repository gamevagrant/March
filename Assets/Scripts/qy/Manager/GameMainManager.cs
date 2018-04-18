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

        public ui.IUIManager uiManager;
        public net.NetManager netManager;
        public IAudioManager audioManager;
        public config.ConfigManager configManager;
        public PlayerData playerData;
        public IPlayerModel playerModel;
        /// <summary>
        /// //全局脚本，可以使用monobehaviour方法
        /// </summary>
        public MonoBehaviour mono;
        

        public GameMainManager()
        {
            playerData = new PlayerData();
            playerModel = new PlayerModel(playerData);
            netManager = net.NetManager.Instance;
            configManager = config.ConfigManager.Instance;
            uiManager = ui.UIManager.Instance;
            audioManager = AudioManager.Instance;
            audioManager.SetSoundPathProxy(FilePathTools.getAudioPath);
            audioManager.SetMusicPathProxy(FilePathTools.getAudioPath);
        }

        public void Init()
        {

            


        }
    }
}


