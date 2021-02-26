using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource BGS;
    public AudioSource SE;
    GameConfig gameConfig;
    private AudioManager _instance;

    //public GameObject AudioObj;
    //static AudioManager selfObject;

    //public AudioManager Obj
    //{
    //    get
    //    {
    //        if (selfObject == null) 
    //        {
    //            selfObject = FindObjectOfType<AudioManager>();
    //            if (selfObject == null)
    //            {
    //                selfObject = Instantiate(AudioObj).GetComponent<AudioManager>();
    //            }
    //        }
    //        return selfObject;
    //    }
    //}


    //AudioManager instance {
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
    //            _instance.BGM = new AudioSource();
    //            _instance.BGS = new AudioSource();
    //            _instance.SE = new AudioSource();
    //            gameConfig = GameConfig.Obj;
    //            DontDestroyOnLoad(_instance); // 轉場時不釋放
    //        }

    //        return _instance;
    //    }

    //}
    
    void Awake()
    {
        gameConfig = GameConfig.Obj;
        DontDestroyOnLoad(this.gameObject); // 轉場時不釋放
    }

    // 設定BGM音量
    public void SetBGMVolume(float volume)
    {
        BGM.volume = volume * gameConfig.BGM_volume;
        BGS.volume = volume * gameConfig.BGM_volume;
    }
    // 播放BGM
    public void PlayBGM(AudioClip bgm)
    {
        PlayBGM(bgm, 1);
    }
    public void PlayBGM(AudioClip bgm, float volume)
    {
        var lastbgm = BGM.clip;
        SetBGMVolume(volume);
        if (!BGM.isPlaying || lastbgm.name != bgm.name)
        {
            BGM.clip = bgm;
            BGM.Play();
        }
        else
        {
            BGM.UnPause();
        }
    }
    // 播放BGS
    public void PlayBGS(AudioClip bgs)
    {
        PlayBGS(bgs, 1);
    }
    public void PlayBGS(AudioClip bgs, float volume)
    {
        var lastbgs = BGS.clip;
        SetBGMVolume(volume);
        if (!BGS.isPlaying || lastbgs.name != bgs.name)
        {
            BGS.clip = bgs;
            BGS.Play();
        }
        else
        {
            BGS.UnPause();
        }
    }
    // 設定SE音量
    public void SetSEVolume(float volume)
    {
        SE.volume = volume * gameConfig.SE_volume;
    }
    // 播放SE
    public void PlaySE(AudioClip se)
    {
        PlaySE(se, 1);
    }
    public void PlaySE(AudioClip se, float volume)
    {
        SetSEVolume(volume);
        SE.PlayOneShot(se);
    }

    // 淡入音樂
    public IEnumerator FadeInBGM(AudioClip bgm, float FadeTime)
    {
        PlayBGM(bgm, 0);
        float targetVolume = 1 * gameConfig.BGM_volume;
        while (BGM.volume < targetVolume)
        {
            BGM.volume += targetVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        BGM.volume = targetVolume;
    }
    public IEnumerator FadeInBGM(AudioClip bgm, float FadeTime, float targetVolume)
    {
        PlayBGM(bgm, 0);
        targetVolume *= gameConfig.BGM_volume;
        while (BGM.volume < targetVolume)
        {
            BGM.volume += targetVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        BGM.volume = targetVolume;
    }

    // 淡出音樂
    public IEnumerator FadeOutBGM(float FadeTime)
    {
        float startVolume = BGM.volume;
        while (BGM.volume > 0)
        {
            BGM.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        BGM.Stop();
        BGM.volume = startVolume;
    }



}
