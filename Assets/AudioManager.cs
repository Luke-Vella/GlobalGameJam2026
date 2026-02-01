using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource backgroundSourceA; // Global ambience
    [SerializeField] private AudioSource backgroundSourceB; // Trigger-based ambience
    [SerializeField] private AudioSource sfxSource;

    [Header("Default Audio")]
    [SerializeField] private AudioClip defaultBackgroundMusic;
    [SerializeField] private AudioClip defaultBackgroundA;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (defaultBackgroundMusic != null)
            PlayMusic(defaultBackgroundMusic);

        if (defaultBackgroundA != null)
            PlayBackgroundA(defaultBackgroundA);
    }

    /* ================= MUSIC ================= */

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    /* ========== BACKGROUND A (GLOBAL) ========== */

    public void PlayBackgroundA(AudioClip clip)
    {
        if (clip == null) return;

        if (backgroundSourceA.clip == clip && backgroundSourceA.isPlaying)
            return;

        backgroundSourceA.clip = clip;
        backgroundSourceA.loop = true;
        backgroundSourceA.Play();
    }

    public void StopBackgroundA()
    {
        backgroundSourceA.Stop();
    }

    public void ToggleBackgroundA(bool enabled)
    {
        if (enabled)
        {
            if (!backgroundSourceA.isPlaying && backgroundSourceA.clip != null)
                backgroundSourceA.Play();
        }
        else
        {
            backgroundSourceA.Stop();
        }
    }

    /* ========== BACKGROUND B (TRIGGER-BASED) ========== */

    public void PlayBackgroundB(AudioClip clip)
    {
        if (clip == null) return;

        if (backgroundSourceB.clip == clip && backgroundSourceB.isPlaying)
            return;

        backgroundSourceB.clip = clip;
        backgroundSourceB.loop = true;
        backgroundSourceB.Play();
    }

    public void StopBackgroundB()
    {
        backgroundSourceB.Stop();
    }

    public void ToggleBackgroundB(bool enabled)
    {
        if (enabled)
        {
            if (!backgroundSourceB.isPlaying && backgroundSourceB.clip != null)
                backgroundSourceB.Play();
        }
        else
        {
            backgroundSourceB.Stop();
        }
    }

    /* ================= SFX ================= */

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }

    /* ========== GLOBAL CONTROLS (OPTIONAL) ========== */

    public void StopAllAudio()
    {
        StopMusic();
        StopBackgroundA();
        StopBackgroundB();
        sfxSource.Stop();
    }
}
