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

    public void PlaySFX(AudioClip audioClip)
    {
        _sfxSource.PlayOneShot(audioClip);
    }

    public void ToggleSFX(bool sfxEnabled)
    {
        _sfxSource.mute = !sfxEnabled;
    }

    public void ToggleMusic(bool musicEnabled)
    {
        _musicSource.mute = !musicEnabled;
    }
}
