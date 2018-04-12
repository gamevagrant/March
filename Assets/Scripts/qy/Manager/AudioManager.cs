using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace qy
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {

        AudioSource audioSource;
        Dictionary<string, AudioClip> sounds;

        GetAudioPath musicProxy;
        GetAudioPath soundProxy;

        float _musicVolume = 1f;
        public float musicVolume
        {
            get
            {
                return _musicVolume;
            }
            set
            {
                _musicVolume = value;
                audioSource.volume = _musicVolume;
            }
        }
        float _soundVolume = 1f;
        public float soundVolume
        {
            get
            {
                return _soundVolume;
            }
            set
            {
                _soundVolume = value;
            }
        }

        private static IAudioManager _instance;
        public static IAudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    go.AddComponent<AudioManager>();
                }
                return _instance;
            }
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogError("AudioManager 只能有一个");
            }
            audioSource = gameObject.AddComponent<AudioSource>();
            sounds = new Dictionary<string, AudioClip>();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public void SetMusicPathProxy(GetAudioPath proxy)
        {
            musicProxy = proxy;
        }
        public void SetSoundPathProxy(GetAudioPath proxy)
        {
            soundProxy = proxy;
        }

        public void PlayMusic()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        public void PlayMusic(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        public void PlayMusic(string name)
        {
            string path = musicProxy(name);

            AssetsManager.Instance.LoadAssetAsync<AudioClip>(path, (clip) =>
            {
                sounds.Add(name, clip);
                PlayMusic(clip);
            });

        }
        public void StopMusic()
        {
            audioSource.Stop();
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip, _soundVolume);
        }
        public void PlaySound(string name)
        {
            if (sounds.ContainsKey(name))
            {
                PlaySound(sounds[name]);
            }
            else
            {
                string path = soundProxy(name);
                AssetsManager.Instance.LoadAssetAsync<AudioClip>(path, (clip) =>
                {
                    if (!sounds.ContainsKey(name))
                    {
                        sounds.Add(name, clip);
                    }

                    PlaySound(clip);
                });
            }
        }
    }

    public class AudioNameEnum
    {
        public static string wheel_roll_btn = "wheel_Roll_btn";//点击转盘按钮
        public static string wheel_rot_start = "wheel_rot_start";//转盘转动
        public static string wheel_rot_end = "wheel_rot_end";//转盘减速
        public static string wheel_gold_small = "wheel_gold_small";//少量金币
        public static string wheel_gold_med = "wheel_gold_small";//中量金币
        public static string wheel_gold_large = "wheel_gold_large";//大量金币
        public static string wheel_energy_start = "wheel_energy_start";//显示能量
        public static string wheel_energy_transform = "wheel_energy_transform";//能量飞行
        public static string wheel_energy_change = "wheel_energy_change";//获得能量
        public static string wheel_shield_full = "wheel_shield_full";//盾牌满
        public static string wheel_shield_got = "wheel_shield_got";//获得盾牌
        public static string wheel_shiled_start = "wheel_shiled_start";//显示盾牌
        public static string wheel_steal = "wheel_steal";//显示偷窃动画
        public static string wheel_steal_gone = "wheel_steal_gone";//偷窃动画结束
        public static string wheel_attack_prepare = "wheel_attack_prepare";//进入攻击界面
        public static string wheel_come = "wheel_come";//转盘过来时
        public static string wheel_Star = "wheel_star";//转到星星

        public static string wheel_view_switch_in = "view_switch_in";//转盘界面和建造界面切换
        public static string wheel_view_switch_out = "view_switch_out";



        public static string shoot_island_come = "shoot_island_come";//攻击界面中 岛屿飞过来
        public static string shoot_target_change = "shoot_target_change";//更换攻击目标 岛屿飞走
        public static string shoot_target_come = "shoot_target_come";//更换攻击目标岛屿飞过来
        public static string shoot_target_leave = "shoot_target_leave";//结束攻击界面 岛屿飞走

        public static string shoot_fire = "cannon_shoot";//开火
        public static string shoot_bomb_fly = "bomb_flying";//炮弹飞行
        public static string shoot_boob_explode = "bomb_explode";//炮弹爆炸
        public static string shoot_aim_target = "attack_aim_at_target";//瞄准
        public static string shoot_hit_sheild = "cannon_hit_sheild";//集中护盾

        public static string button_click = "btn_normal";
        public static string panel_in = "panel_open";
        public static string panel_out = "panel_close";

        public static string steal_got_king = "steal_got_king";
        public static string steal_miss_king = "steal_miss_king";
        public static string steal_result = "steal_result";

        public static string building_upgrade = "upgrade_build";
        public static string building_level_up = "level_success";//岛屿升级的时候
        public static string building_island_change = "passland_con_text";//升级更换岛屿外观的时候
        public static string building_box_down = "passland_box_down";//包厢掉落的时候

    }
}

