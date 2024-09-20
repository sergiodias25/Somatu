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
    public AudioClip TimerTicking;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        PlaySFX(audioClip, 1f);
    }

    public void PlaySFX(AudioClip audioClip, float customVolumeModifierValue)
    {
        if (gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            /*if (_sfxSource.isPlaying)
            {
                _sfxSource.Stop();
            }*/
            _sfxSource.PlayOneShot(audioClip, customVolumeModifierValue);
        }
        if (gameManager.SavedGameData.SettingsData.VibrationEnabled)
        {
            Vibrate();
        }
    }

    public void StopSFX()
    {
        if (gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            _sfxSource.Stop();
        }
    }

    public void PlayMusic()
    {
        if (gameManager.SavedGameData.SettingsData.MusicEnabled)
        {
            _musicSource.loop = true;
            _musicSource.clip = MainMusicTheme;
            _musicSource.Play();
        }
    }

    public void ToggleSFX()
    {
        gameManager.SavedGameData.SettingsData.SoundEnabled = !gameManager
            .SavedGameData
            .SettingsData
            .SoundEnabled;
        gameManager.SavedGameData.PersistData();
        _sfxSource.mute = !gameManager.SavedGameData.SettingsData.SoundEnabled;
    }

    public void ToggleMusic()
    {
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
        gameManager.SavedGameData.SettingsData.VibrationEnabled = !gameManager
            .SavedGameData
            .SettingsData
            .VibrationEnabled;
        gameManager.SavedGameData.PersistData();
    }

    public void Vibrate()
    {
        if (gameManager.SavedGameData.SettingsData.VibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }

    public AudioClip GetAudioClip(Constants.AudioClip clip)
    {
        if (clip == Constants.AudioClip.DropBlock)
        {
            return DropBlock;
        }
        else if (clip == Constants.AudioClip.DropBlockUndo)
        {
            return DropBlockUndo;
        }
        else if (clip == Constants.AudioClip.NodeLoaded)
        {
            return NodeLoaded;
        }
        else if (clip == Constants.AudioClip.PuzzleSolved)
        {
            return PuzzleSolved;
        }
        else if (clip == Constants.AudioClip.TimerTicking)
        {
            return TimerTicking;
        }
        else
            return null;
    }
}
