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
        gameManager = FindObjectOfType<GameManager>();
        _musicSource.loop = true;
        _musicSource.clip = MainMusicTheme;
        _musicSource.PlayOneShot(MainMusicTheme, _musicSource.volume);
        _musicSource.Pause();
    }

    public void PlaySFX(Constants.AudioClip audioClip)
    {
        if (gameManager.SavedGameData.SettingsData.SoundEnabled)
        {
            ShouldChangePitch(audioClip);
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
            _neutralAudioSource.Stop();
            _lowDiffPitchAudioSource.Stop();
            _highDiffPitchAudioSource.Stop();
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
        _neutralAudioSource.mute = !gameManager.SavedGameData.SettingsData.SoundEnabled;
        _lowDiffPitchAudioSource.mute = !gameManager.SavedGameData.SettingsData.SoundEnabled;
        _highDiffPitchAudioSource.mute = !gameManager.SavedGameData.SettingsData.SoundEnabled;
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
            _neutralAudioSource.PlayOneShot(GetAudioClip(clipToPlay), 1f);
        }
        else if (
            clipToPlay == Constants.AudioClip.NodeLoaded
            || clipToPlay == Constants.AudioClip.TimerTicking
            || clipToPlay == Constants.AudioClip.Firework
            || clipToPlay == Constants.AudioClip.NoHintAvailable
        )
        {
            PlaySFXWithSource(_lowDiffPitchAudioSource, clipToPlay, 2);
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
        audioSource.PlayOneShot(GetAudioClip(clipToPlay), 1f);
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
