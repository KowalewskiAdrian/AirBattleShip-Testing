using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController SharedInstance;

    [Header("Audio Mixer Groups")]
    public AudioMixerGroup MusicGroup;
    public AudioMixerGroup EffectsGroup;

    [Header("Audio Clips")]
    public AudioClip BackgroundMusic;
    public AudioClip GameOverSoundWithLowScore;
    public AudioClip GameOverSoundWithHighScore;
    public AudioClip DamageSound;
    public AudioClip DestroyEnemySound;
    public AudioClip HealingPickupSound;

    private AudioSource _musicSource;
    private AudioSource _effectsSource;


    void Awake() 
    {
        if (SharedInstance == null) {
            SharedInstance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = MusicGroup;
        _musicSource.loop = true;

        _effectsSource = gameObject.AddComponent<AudioSource>();
        _effectsSource.outputAudioMixerGroup = EffectsGroup;
    }

    public void PlayBackgroundMusic() 
    {
        if (BackgroundMusic != null) 
        {
            _musicSource.clip = BackgroundMusic;
            _musicSource.Play();
        }
    }

    public void PlayEffect(AudioClip clip)
    {
        if (clip != null) 
        {
            _effectsSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float volume) 
    {
        MusicGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); // Convert to dB
    }

    public void SetEffectsVolume(float volume) 
    {
        EffectsGroup.audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }
}
