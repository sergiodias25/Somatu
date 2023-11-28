using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _sfxSource;

    public AudioClip DropBlock;

    public AudioClip DropBlockUndo;
    public AudioClip PuzzleSolved;
    public AudioClip MainMusicTheme;

    public void PlaySFX(AudioClip audioClip)
    {
        _sfxSource.PlayOneShot(audioClip);
    }

    public void PlayMusic()
    {
        _musicSource.loop = true;
        _musicSource.clip = MainMusicTheme;
        _musicSource.Play();
    }

    public void ToggleSFX(bool sfxEnabled)
    {
        _sfxSource.mute = !sfxEnabled;
    }

    public void ToggleMusic(bool musicEnabled)
    {
        _musicSource.mute = !musicEnabled;
        if (_musicSource.mute)
        {
            _musicSource.Pause();
        }
        else
        {
            _musicSource.UnPause();
        }
    }
}
