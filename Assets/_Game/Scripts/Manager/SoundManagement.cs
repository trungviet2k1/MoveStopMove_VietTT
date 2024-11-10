using UnityEngine;

public class SoundManagement : Singleton<SoundManagement>
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip clickSound;
    public AudioClip attackAudio;
    public AudioClip weaponImpact;
    public AudioClip loseAudio;
    public AudioClip dieAudio;
    public AudioClip sizeUpAudio;
    public AudioClip winAudio;

    [HideInInspector] public bool openSound;

    void Awake()
    {
        musicSource.clip = background;
        SoundOn(true);
    }

    public void SoundOn(bool open)
    {
        openSound = open;
        musicSource.mute = !openSound;
        SFXSource.mute = !openSound;
        if (openSound && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else if (!openSound && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (openSound) SFXSource.PlayOneShot(clip);
    }

    public void ToggleMusic(bool play)
    {
        if (play && openSound)
        {
            if (!musicSource.isPlaying) musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }
}