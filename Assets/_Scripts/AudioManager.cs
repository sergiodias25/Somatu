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
    public bool _sfxEnabled { get; private set; }
    public bool _musicEnabled { get; private set; }
    public bool _vibrationEnabled { get; private set; }

    private void Start()
    {
        _sfxEnabled = true;
        _musicEnabled = false;
        _vibrationEnabled = true;
    }

    public void PlaySFX(AudioClip audioClip)
    {
        if (_sfxEnabled)
        {
            _sfxSource.PlayOneShot(audioClip);
        }
        if (_vibrationEnabled)
        {
            Vibrate();
        }
    }

    public void PlayMusic()
    {
        if (_musicEnabled)
        {
            _musicSource.loop = true;
            _musicSource.clip = MainMusicTheme;
            _musicSource.Play();
        }
    }

    public void ToggleSFX()
    {
        _sfxEnabled = !_sfxEnabled;
        _sfxSource.mute = !_sfxEnabled;
    }

    public void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
        _musicSource.mute = !_musicEnabled;
        if (!_musicEnabled)
        {
            _musicSource.Pause();
        }
        else
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.UnPause();
            }
            else
            {
                PlayMusic();
            }
        }
    }

    public void ToggleVibration()
    {
        _vibrationEnabled = !_vibrationEnabled;
    }

    public void Vibrate()
    {
#if !UNITY_EDITOR
        if (_vibrationEnabled)
        {
            Handheld.Vibrate();
        }
#endif
    }
}
