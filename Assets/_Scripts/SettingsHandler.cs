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
    private AudioManager _audioManager;
    private bool _soundEnabled = true;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (_soundButtonImage != null)
        {
            UpdateSoundIcon();
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
}
