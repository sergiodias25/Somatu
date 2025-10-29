using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _neutralAudioSource;

    [SerializeField]
    private AudioSource _lowDiffPitchAudioSource;

    [SerializeField]
    private AudioSource _highDiffPitchAudioSource;

    [SerializeField]
    private List<AudioClip> _menuMusics;

    [SerializeField]
    private List<AudioClip> _classicMusics;

    [SerializeField]
    private List<AudioClip> _challengeMusics;

    public AudioClip GameplayInteraction;
    public AudioClip Undo;
    public AudioClip ClassicFinish;
    public AudioClip ChallengeFinish;
    public AudioClip NodeLoaded;
    public AudioClip TimerTicking;
    public AudioClip Firework;
    public AudioClip MenuInteraction;
    public AudioClip NoHintAvailable;
    private GameManager _gameManager;
    List<int> PentatonicSemitones = new List<int>();

    public enum MusicType
    {
        Nothing,
        Menu,
        Classic,
        Challenge
    };

    private bool _musicPlaying = false;
    private MusicType _musicTypePlaying = MusicType.Nothing;

    /*
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>(
        "getSystemService",
        "vibrator"
    );
#else
    public static AndroidJavaObject vibrator;
#endif*/
    public static AndroidJavaObject vibrator;

    private void Start()
    {
        PentatonicSemitones.Add(0);
        PentatonicSemitones.Add(1);
        PentatonicSemitones.Add(3);
        PentatonicSemitones.Add(4);
        PentatonicSemitones.Add(5);
        PentatonicSemitones.Add(8);
        _gameManager = FindObjectOfType<GameManager>();
        _musicSource.loop = true;
    }

    public void PlaySFX(Constants.AudioClip audioClip)
    {
        if (_gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            ShouldChangePitch(audioClip);
        }
        if (_gameManager.SavedGameData.SettingsData.VibrationEnabled)
        {
            Vibrate();
        }
    }

    public void StopSFX()
    {
        if (_gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            _neutralAudioSource.Stop();
            _lowDiffPitchAudioSource.Stop();
            _highDiffPitchAudioSource.Stop();
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        if (_gameManager.SavedGameData.SettingsData.MusicEnabled)
        {
            if (_musicPlaying)
            {
                if (_musicTypePlaying != musicType)
                {
                    _musicSource.clip = GetMusicFromList(musicType);
                    _musicSource.Play();
                }
                else
                {
                    _musicSource.UnPause();
                }
            }
            else
            {
                _musicSource.clip = GetMusicFromList(musicType);
                _musicSource.Play();
            }
            _musicPlaying = true;
            _musicTypePlaying = musicType;
        }
    }

    public void PauseMusic()
    {
        _musicPlaying = false;
        _musicSource.Pause();
    }

    public void UnpauseMusic()
    {
        if (!_gameManager.HasGameEnded())
        {
            _musicPlaying = true;
            _musicSource.UnPause();
        }
    }

    private AudioClip GetMusicFromList(MusicType musicType)
    {
        switch (musicType)
        {
            case MusicType.Menu:
                return _menuMusics.ToArray()[Random.Range(0, _menuMusics.Count)];
            case MusicType.Classic:
                return _classicMusics.ToArray()[Random.Range(0, _classicMusics.Count)];
            case MusicType.Challenge:
                return _challengeMusics.ToArray()[Random.Range(0, _challengeMusics.Count)];

            default:
                return null;
        }
    }

    public void ToggleSFX()
    {
        _gameManager.SavedGameData.SettingsData.SoundEnabled = !_gameManager
            .SavedGameData
            .SettingsData
            .SoundEnabled;
        _gameManager.SavedGameData.PersistData();
        _neutralAudioSource.mute = !_gameManager.SavedGameData.SettingsData.SoundEnabled;
        _lowDiffPitchAudioSource.mute = !_gameManager.SavedGameData.SettingsData.SoundEnabled;
        _highDiffPitchAudioSource.mute = !_gameManager.SavedGameData.SettingsData.SoundEnabled;
    }

    public void ToggleMusic()
    {
        _gameManager.SavedGameData.SettingsData.MusicEnabled = !_gameManager
            .SavedGameData
            .SettingsData
            .MusicEnabled;
        _gameManager.SavedGameData.PersistData();

        _musicSource.mute = !_gameManager.SavedGameData.SettingsData.MusicEnabled;
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
        _gameManager.SavedGameData.SettingsData.VibrationEnabled = !_gameManager
            .SavedGameData
            .SettingsData
            .VibrationEnabled;
        _gameManager.SavedGameData.PersistData();
    }

    public void Vibrate()
    {
        if (_gameManager.SavedGameData.SettingsData.VibrationEnabled)
        {
            if (isAndroid())
            {
                vibrator.Call("vibrate", 100);
            }
            else
            {
                Handheld.Vibrate();
            }
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

    private void ShouldChangePitch(Constants.AudioClip clipToPlay)
    {
        if (
            clipToPlay == Constants.AudioClip.ClassicFinish
            || clipToPlay == Constants.AudioClip.ChallengeFinish
        )
        {
            PlaySFXWithSource(_neutralAudioSource, clipToPlay, 1);
        }
        else if (
            clipToPlay == Constants.AudioClip.NodeLoaded
            || clipToPlay == Constants.AudioClip.TimerTicking
            || clipToPlay == Constants.AudioClip.Firework
            || clipToPlay == Constants.AudioClip.NoHintAvailable
        )
        {
            PlaySFXWithSource(_lowDiffPitchAudioSource, clipToPlay, 3);
        }
        else if (
            clipToPlay == Constants.AudioClip.GameplayInteraction
            || clipToPlay == Constants.AudioClip.Undo
            || clipToPlay == Constants.AudioClip.MenuInteraction
        )
        {
            PlaySFXWithSource(_highDiffPitchAudioSource, clipToPlay, 5);
        }
    }

    private void PlayControlledVolume(AudioSource source, Constants.AudioClip clipToPlay)
    {
        if (
            clipToPlay == Constants.AudioClip.NodeLoaded
            || clipToPlay == Constants.AudioClip.ClassicFinish
        )
        {
            source.PlayOneShot(GetAudioClip(clipToPlay), .2f);
        }
        else
        {
            source.PlayOneShot(GetAudioClip(clipToPlay), 1f);
        }
    }

    private void PlaySFXWithSource(
        AudioSource audioSource,
        Constants.AudioClip clipToPlay,
        int varianceNotesCount
    )
    {
        audioSource.pitch = 1;
        int x = PentatonicSemitones[Random.Range(0, varianceNotesCount)];

        for (int i = 0; i < x; i++)
        {
            audioSource.pitch *= 1.059463f;
        }
        PlayControlledVolume(audioSource, clipToPlay);
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
