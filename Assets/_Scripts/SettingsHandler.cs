using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public int SelectedColorsIndex = 0;

    [SerializeField]
    public Animator SettingsBarAnimator;

    [SerializeField]
    public Animator GameBarAnimator;

    [SerializeField]
    private Image _soundButtonImage;

    [SerializeField]
    private Sprite _soundOnSprite;

    [SerializeField]
    private Sprite _soundOffSprite;

    [SerializeField]
    private Image _musicButtonImage;

    [SerializeField]
    private Sprite _musicOnSprite;

    [SerializeField]
    private Sprite _musicOffSprite;

    [SerializeField]
    private Image _vibrateButtonImage;

    [SerializeField]
    private Sprite _vibrateOnSprite;

    [SerializeField]
    private Sprite _vibrateOffSprite;
    private AudioManager _audioManager;
    private bool _soundEnabled = true;
    private bool _musicEnabled = true;
    public bool _vibrationEnabled = true;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (_soundButtonImage != null)
        {
            UpdateSoundIcon();
        }
        if (_musicButtonImage != null)
        {
            UpdateMusicIcon();
        }
        if (_vibrateButtonImage != null)
        {
            UpdateVibrateIcon();
        }
    }

    public void ChangeTheme()
    {
        GradientBg gradientBg = FindObjectOfType<GradientBg>();
        SelectedColorsIndex = GetNextIndex(Constants.ColorPalettes.Length);
        gradientBg.UpdateTheme(Constants.ColorPalettes[SelectedColorsIndex]);
    }

    private int GetNextIndex(int size)
    {
        if (SelectedColorsIndex == size - 1)
        {
            return 0;
        }
        return SelectedColorsIndex + 1;
    }

    public void HandleTopBarsSlide()
    {
        if (
            SettingsBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !SettingsBarAnimator.IsInTransition(0)
            && GameBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !GameBarAnimator.IsInTransition(0)
        )
        {
            SettingsBarAnimator.SetTrigger("Toggle");
            GameBarAnimator.SetTrigger("Toggle");
        }
    }

    private void UpdateSoundIcon()
    {
        if (_soundEnabled)
        {
            _soundButtonImage.sprite = _soundOnSprite;
        }
        else
        {
            _soundButtonImage.sprite = _soundOffSprite;
        }
    }

    public void ChangeSound()
    {
        _soundEnabled = !_soundEnabled;
        _audioManager.ToggleSFX(_soundEnabled);
        UpdateSoundIcon();
    }

    private void UpdateMusicIcon()
    {
        if (_musicEnabled)
        {
            _musicButtonImage.sprite = _musicOnSprite;
        }
        else
        {
            _musicButtonImage.sprite = _musicOffSprite;
        }
    }

    public void ChangeMusic()
    {
        _musicEnabled = !_musicEnabled;
        _audioManager.ToggleMusic(_musicEnabled);
        UpdateMusicIcon();
    }

    private void UpdateVibrateIcon()
    {
        if (_vibrationEnabled)
        {
            _vibrateButtonImage.sprite = _vibrateOnSprite;
        }
        else
        {
            _vibrateButtonImage.sprite = _vibrateOffSprite;
        }
    }

    public void ChangeVibration()
    {
        _vibrationEnabled = !_vibrationEnabled;
        _audioManager.ToggleVibration(_vibrationEnabled);
        UpdateVibrateIcon();
    }
}
