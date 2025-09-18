using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;

    void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}
