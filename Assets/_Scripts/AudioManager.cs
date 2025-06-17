using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _sfxSource;
    public AudioClip GameplayInteraction;
    public AudioClip Undo;
    public AudioClip ClassicFinish;
    public AudioClip ChallengeFinish;
    public AudioClip MainMusicTheme;
    public AudioClip NodeLoaded;
    public AudioClip TimerTicking;
    public AudioClip Firework;
    public AudioClip MenuInteraction;
    public AudioClip NoHintAvailable;
    private GameManager gameManager;
    List<int> PentatonicSemitones = new List<int>();

    private void Start()
    {
        PentatonicSemitones.Add(0);
        PentatonicSemitones.Add(1);
        PentatonicSemitones.Add(3);
        PentatonicSemitones.Add(4);
        PentatonicSemitones.Add(5);
        PentatonicSemitones.Add(8);
        gameManager = FindObjectOfType<GameManager>();
        _musicSource.loop = true;
        _musicSource.clip = MainMusicTheme;
        _musicSource.PlayOneShot(MainMusicTheme, _musicSource.volume);
        _musicSource.Pause();
    }

    public void PlaySFX(Constants.AudioClip audioClip)
    {
        PlaySFX(audioClip, 1f);
    }

    public void PlaySFX(Constants.AudioClip audioClip, float customVolumeModifierValue)
    {
        if (gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            _sfxSource.pitch = 1;
            int x = PentatonicSemitones[Random.Range(0, PentatonicSemitones.Count)];
            if (ShouldChangePitch(audioClip) && !_sfxSource.isPlaying)
            {
                for (int i = 0; i < x; i++)
                {
                    _sfxSource.pitch *= 1.059463f;
                }
            }
            _sfxSource.PlayOneShot(GetAudioClip(audioClip), customVolumeModifierValue);
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
            _musicSource.PlayOneShot(MainMusicTheme, _musicSource.volume);
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
        if (_musicSource.mute)
        {
            _musicSource.Pause();
        }
        else
        {
            _musicSource.UnPause();
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
        if (clip == Constants.AudioClip.GameplayInteraction)
        {
            return GameplayInteraction;
        }
        else if (clip == Constants.AudioClip.Undo)
        {
            return Undo;
        }
        else if (clip == Constants.AudioClip.NodeLoaded)
        {
            return NodeLoaded;
        }
        else if (clip == Constants.AudioClip.ClassicFinish)
        {
            return ClassicFinish;
        }
        else if (clip == Constants.AudioClip.ChallengeFinish)
        {
            return ChallengeFinish;
        }
        else if (clip == Constants.AudioClip.TimerTicking)
        {
            return TimerTicking;
        }
        else if (clip == Constants.AudioClip.Firework)
        {
            return Firework;
        }
        else if (clip == Constants.AudioClip.MenuInteraction)
        {
            return MenuInteraction;
        }
        else if (clip == Constants.AudioClip.NoHintAvailable)
        {
            return NoHintAvailable;
        }
        else
            return null;
    }

    private bool ShouldChangePitch(Constants.AudioClip clipToPlay)
    {
        if (clipToPlay == Constants.AudioClip.GameplayInteraction)
        {
            return true;
        }
        else if (clipToPlay == Constants.AudioClip.Undo)
        {
            return true;
        }
        else if (clipToPlay == Constants.AudioClip.NodeLoaded)
        {
            return false;
        }
        else if (clipToPlay == Constants.AudioClip.ClassicFinish)
        {
            return false;
        }
        else if (clipToPlay == Constants.AudioClip.ChallengeFinish)
        {
            return false;
        }
        else if (clipToPlay == Constants.AudioClip.TimerTicking)
        {
            return false;
        }
        else if (clipToPlay == Constants.AudioClip.Firework)
        {
            return false;
        }
        else if (clipToPlay == Constants.AudioClip.MenuInteraction)
        {
            return true;
        }
        else if (clipToPlay == Constants.AudioClip.NoHintAvailable)
        {
            return true;
        }
        else
            return false;
    }
}
