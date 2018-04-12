using UnityEngine;

public delegate string GetAudioPath(string name);
public interface IAudioManager {

    float musicVolume { get; set; }
    float soundVolume { get; set; }

    void SetMusicPathProxy(GetAudioPath proxy);
    void SetSoundPathProxy(GetAudioPath proxy);

    void PlayMusic();
    void PlayMusic(AudioClip clip);
    void PlayMusic(string name);
    void StopMusic();

    void PlaySound(AudioClip clip);
    void PlaySound(string name);

}
