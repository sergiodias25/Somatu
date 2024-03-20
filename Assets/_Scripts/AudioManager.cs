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
    public AudioClip NodeLoaded;

    public void PlaySFX(AudioClip audioClip)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            if (_sfxSource.isPlaying)
            {
                _sfxSource.Stop();
            }
            _sfxSource.PlayOneShot(audioClip);
        }
        if (gameManager.SavedGameData.SettingsData.VibrationEnabled)
        {
            Vibrate();
        }
    }

    public void PlayMusic()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.SavedGameData.SettingsData.MusicEnabled)
        {
            _musicSource.loop = true;
            _musicSource.clip = MainMusicTheme;
            _musicSource.Play();
        }
    }

    public void ToggleSFX()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SavedGameData.SettingsData.SoundEnabled = !gameManager
            .SavedGameData
            .SettingsData
            .SoundEnabled;
        gameManager.SavedGameData.PersistData();
        _sfxSource.mute = !gameManager.SavedGameData.SettingsData.SoundEnabled;
    }

    public void ToggleMusic()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SavedGameData.SettingsData.MusicEnabled = !gameManager
            .SavedGameData
            .SettingsData
            .MusicEnabled;
        gameManager.SavedGameData.PersistData();

        _musicSource.mute = !gameManager.SavedGameData.SettingsData.MusicEnabled;
        if (!gameManager.SavedGameData.SettingsData.MusicEnabled)
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
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SavedGameData.SettingsData.VibrationEnabled = !gameManager
            .SavedGameData
            .SettingsData
            .VibrationEnabled;
        gameManager.SavedGameData.PersistData();
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
