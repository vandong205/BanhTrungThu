using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;

    private  Dictionary<string,AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
    [Header("SFX sources")]
    public List<AudioClip> sounds= new List<AudioClip>();
    void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeClipDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeClipDict()
    {
        foreach (var clip in sounds)
        {
            if (clip != null && !audioClipDic.ContainsKey(clip.name))
            {
                audioClipDic.Add(clip.name, clip);
            }
        }
    }
    void Start()
    {
    
        if (backgroundMusic != null)
        {
            PlayBGM(backgroundMusic);
        }
    }

    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM() => bgmSource.Pause();
    public void ResumeBGM() => bgmSource.UnPause();

    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (audioClipDic.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName);
        }
    }
}
