using Assets.Scripts.SaveGame;
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
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private GradientBg _gradientBg;

    void Awake() { }

    public void LoadData()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _gradientBg = FindObjectOfType<GradientBg>();
        _gameManager = FindObjectOfType<GameManager>();
        _gradientBg.UpdateTheme(
            Constants.ColorPalettes[_gameManager.SavedGameData.SettingsData.SelectedThemeIndex]
        );

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
        _gameManager = FindObjectOfType<GameManager>();
        int _selectedColorsIndex = _gameManager.SavedGameData.SettingsData.SelectedThemeIndex;
        _gradientBg.UpdateTheme(GetNextTheme(_selectedColorsIndex));
    }

    private Color[] GetNextTheme(int selectedColorsIndex)
    {
        if (selectedColorsIndex == Constants.ColorPalettes.Length - 1)
        {
            selectedColorsIndex = 0;
            _gameManager.SavedGameData.SettingsData.SelectedThemeIndex = selectedColorsIndex;
            _gameManager.SavedGameData.PersistData();
            return Constants.ColorPalettes[selectedColorsIndex];
        }
        if (selectedColorsIndex == Constants.ColorPalettes.Length - 2)
        {
            if (_gameManager.SavedGameData.PurchaseData.GoldTheme)
            {
                selectedColorsIndex += 1;
                _gameManager.SavedGameData.SettingsData.SelectedThemeIndex = selectedColorsIndex;
                _gameManager.SavedGameData.PersistData();
                return Constants.ColorPalettes[selectedColorsIndex];
            }
            selectedColorsIndex = -1;
            return GetNextTheme(selectedColorsIndex);
        }
        if (selectedColorsIndex == Constants.ColorPalettes.Length - 3)
        {
            if (_gameManager.SavedGameData.PurchaseData.MetallicTheme)
            {
                selectedColorsIndex += 1;
                _gameManager.SavedGameData.SettingsData.SelectedThemeIndex = selectedColorsIndex;
                _gameManager.SavedGameData.PersistData();
                return Constants.ColorPalettes[selectedColorsIndex];
            }
            selectedColorsIndex += 1;
            return GetNextTheme(selectedColorsIndex);
        }
        selectedColorsIndex += 1;
        _gameManager.SavedGameData.SettingsData.SelectedThemeIndex = selectedColorsIndex;
        _gameManager.SavedGameData.PersistData();
        return Constants.ColorPalettes[selectedColorsIndex];
    }

    private void UpdateSoundIcon()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        UpdateSetting(
            gameManager.SavedGameData.SettingsData.SoundEnabled,
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
        GameManager gameManager = FindObjectOfType<GameManager>();
        UpdateSetting(
            gameManager.SavedGameData.SettingsData.MusicEnabled,
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
        GameManager gameManager = FindObjectOfType<GameManager>();
        UpdateSetting(
            gameManager.SavedGameData.SettingsData.VibrationEnabled,
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
