using System.Collections.Generic;
using Assets.Scripts.CustomAnimation;
using Assets.SimpleLocalization.Scripts;
using CandyCabinets.Components.Colour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
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

    [SerializeField]
    private Image _controlButtonImage;

    [SerializeField]
    private Sprite _controlDragSprite;

    [SerializeField]
    private Sprite _controlClickSprite;

    [SerializeField]
    private TextMeshProUGUI _controlLabel;

    [SerializeField]
    private Button _languageButton;

    [SerializeField]
    private Button _themeButton;

    [SerializeField]
    private Button _controlButton;

    [SerializeField]
    private Button _vibrationButton;

    [SerializeField]
    private Button _musicButton;

    [SerializeField]
    private Button _soundButton;

    [SerializeField]
    private Button _supportButton;
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private GradientBg _gradientBg;

    public void LoadData(GameManager gameManager)
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _gradientBg = FindObjectOfType<GradientBg>();
        _gameManager = gameManager;
        _gradientBg.UpdateTheme(
            Constants.GetSelectedPaletteColors(
                _gameManager.SavedGameData.SettingsData.SelectedThemeIndex
            )
        );

        UpdateSoundIcon();
        UpdateMusicIcon();
        UpdateVibrateIcon();
        UpdateControlIcon();
        UpdateControlTranslation();
        LocalizationManager.OnLocalizationChanged += () => UpdateControlTranslation();
    }

    public async void ChangeTheme()
    {
        await CustomAnimation.ButtonClicked(_themeButton.transform);
        int _selectedColorsIndex = _gameManager.SavedGameData.SettingsData.SelectedThemeIndex;
        int _newSelectedColorsIndex = GetNextTheme(_selectedColorsIndex);
        ColourManager.Instance.SelectPalette(_newSelectedColorsIndex);
        _gameManager.SavedGameData.SettingsData.SelectedThemeIndex = _newSelectedColorsIndex;
        _gameManager.SavedGameData.PersistData();

        _gradientBg.UpdateTheme(Constants.GetSelectedPaletteColors(_newSelectedColorsIndex));
        _gameManager.CheckResult(false);
        _gameManager.RemoveHints();

        FindObjectOfType<TopBarManager>().SelectSettingsButton();
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.UpdateTextColor();
        }
    }

    private int GetNextTheme(int selectedColorsIndex)
    {
        if (selectedColorsIndex == ColourManager.Instance.Palettes.Count - 2)
        {
            return 0;
        }
        if (selectedColorsIndex == ColourManager.Instance.Palettes.Count - 3)
        {
            if (_gameManager.SavedGameData.PurchaseData.SunsetTheme)
            {
                return selectedColorsIndex + 1;
            }
            return 0;
        }
        if (selectedColorsIndex == ColourManager.Instance.Palettes.Count - 4)
        {
            if (_gameManager.SavedGameData.PurchaseData.SunriseTheme)
            {
                return selectedColorsIndex + 1;
            }
            return GetNextTheme(selectedColorsIndex + 1);
        }
        return selectedColorsIndex + 1;
    }

    private void UpdateSoundIcon()
    {
        UpdateSetting(
            _gameManager.SavedGameData.SettingsData.SoundEnabled,
            _soundButtonImage,
            _soundOnSprite,
            _soundOffSprite
        );
    }

    public async void ChangeSound()
    {
        await CustomAnimation.ButtonClicked(_soundButton.transform);
        _audioManager.ToggleSFX();
        UpdateSoundIcon();
    }

    private void UpdateMusicIcon()
    {
        UpdateSetting(
            _gameManager.SavedGameData.SettingsData.MusicEnabled,
            _musicButtonImage,
            _musicOnSprite,
            _musicOffSprite
        );
    }

    public async void ChangeMusic()
    {
        await CustomAnimation.ButtonClicked(_musicButton.transform);
        _audioManager.ToggleMusic();
        UpdateMusicIcon();
    }

    private void UpdateVibrateIcon()
    {
        UpdateSetting(
            _gameManager.SavedGameData.SettingsData.VibrationEnabled,
            _vibrateButtonImage,
            _vibrateOnSprite,
            _vibrateOffSprite
        );
    }

    public async void ChangeVibration()
    {
        await CustomAnimation.ButtonClicked(_vibrationButton.transform);
        _audioManager.ToggleVibration();
        UpdateVibrateIcon();
    }

    private void UpdateControlIcon()
    {
        UpdateSetting(
            _gameManager.SavedGameData.SettingsData.ControlMethodDrag,
            _controlButtonImage,
            _controlDragSprite,
            _controlClickSprite
        );
        UpdateControlTranslation();
    }

    private void UpdateControlTranslation()
    {
        _controlLabel.text = _gameManager.SavedGameData.SettingsData.ControlMethodDrag
            ? LocalizationManager.Localize("settings-control-drag")
            : LocalizationManager.Localize("settings-control-click");
    }

    public async void ChangeControl()
    {
        await CustomAnimation.ButtonClicked(_controlButton.transform);
        _gameManager.SavedGameData.SettingsData.ControlMethodDrag = !_gameManager
            .SavedGameData
            .SettingsData
            .ControlMethodDrag;
        if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag)
        {
            _gameManager.ResetSelectedBlock();
        }

        UpdateControlIcon();
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

    public async void ChangeLanguage()
    {
        await CustomAnimation.ButtonClicked(_languageButton.transform);
        List<string> languagesAvailable = new List<string>
        {
            "English",
            "Tamil",
            "Thai",
            "Burmese",
            "Chinese",
            "Japanese",
            "Korean",
            "Spanish",
            "Portuguese"
        };
        int currentIndex = languagesAvailable.IndexOf(LocalizationManager.Language);
        int nextIndex = currentIndex == languagesAvailable.Count - 1 ? 0 : currentIndex + 1;
        LocalizationManager.Language = languagesAvailable[nextIndex];

        _gameManager.SavedGameData.SettingsData.LanguageSelected = LocalizationManager.Language;
        _gameManager.SavedGameData.PersistData();
        UpdateLanguageIcon();
        FindObjectOfType<UIManager>().UpdateHelpButtonText();
    }

    private void UpdateLanguageIcon() { }

    internal void LoadSettingsButtons()
    {
        CustomAnimation.ButtonLoad(_languageButton.transform);
        CustomAnimation.ButtonLoad(_themeButton.transform);
        CustomAnimation.ButtonLoad(_controlButton.transform);
        CustomAnimation.ButtonLoad(_vibrationButton.transform);
        CustomAnimation.ButtonLoad(_musicButton.transform);
        CustomAnimation.ButtonLoad(_soundButton.transform);
        CustomAnimation.ButtonLoad(_supportButton.transform);
    }
}
