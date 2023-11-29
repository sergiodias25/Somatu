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
        UpdateSetting(
            _audioManager._sfxEnabled,
            _soundButtonImage,
            _soundOnSprite,
            _soundOffSprite
        );
    }

    public void ChangeSound()
    {
        _audioManager.ToggleSFX();
        UpdateSoundIcon();
    }

    private void UpdateMusicIcon()
    {
        UpdateSetting(
            _audioManager._musicEnabled,
            _musicButtonImage,
            _musicOnSprite,
            _musicOffSprite
        );
    }

    public void ChangeMusic()
    {
        _audioManager.ToggleMusic();
        UpdateMusicIcon();
    }

    private void UpdateVibrateIcon()
    {
        UpdateSetting(
            _audioManager._vibrationEnabled,
            _vibrateButtonImage,
            _vibrateOnSprite,
            _vibrateOffSprite
        );
    }

    public void ChangeVibration()
    {
        _audioManager.ToggleVibration();
        UpdateVibrateIcon();
    }

    private void UpdateSetting(
        bool settingToChange,
        Image imageToUpdate,
        Sprite spriteOn,
        Sprite spriteOff
    )
    {
        if (settingToChange)
        {
            imageToUpdate.sprite = spriteOn;
        }
        else
        {
            imageToUpdate.sprite = spriteOff;
        }
    }
}
