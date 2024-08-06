using CandyCabinets.Components.Colour;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class IAPScript : MonoBehaviour
{
    private const string HINTS_5 = "HINTS_5";
    private const string HINTS_UNLIMITED = "HINTS_UNLIMITED";
    private const string UNLOCK_LEVELS = "UNLOCK_LEVELS";
    private const string REMOVE_ADS = "REMOVE_ADS";
    private const string SUNRISE_THEME = "SUNRISE_THEME";
    private const string SUNSET_THEME = "SUNSET_THEME";
    public Button Button5Hints;
    public Button ButtonUnlimitedHints;
    public Button ButtonUnlockLevels;
    public Button ButtonRemoveAds;
    public Button ButtonSunriseTheme;
    public Button ButtonSunsetTheme;
    private GameManager _gameManager;
    private AdBanner _adBanner;
    private UIManager _uiManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _adBanner = FindObjectOfType<AdBanner>();
        _uiManager = FindObjectOfType<UIManager>();
        LoadIAPStatus();
    }

    private void OnEnable()
    {
        LoadIAPStatus();
    }

    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case HINTS_5:
                _gameManager.SavedGameData.IncrementHintsAvailable(5);
                _gameManager.SavedGameData.PersistData();
                _uiManager.UpdateHintButtonText();
                break;
            case HINTS_UNLIMITED:
                _gameManager.SavedGameData.GrantUnlimitedHints();
                _gameManager.SavedGameData.PersistData();
                _uiManager.UpdateHintButtonText();
                break;
            case UNLOCK_LEVELS:
                _gameManager.SavedGameData.UnlockAllLevels();
                _gameManager.SavedGameData.PersistData();
                break;
            case REMOVE_ADS:
                _gameManager.SavedGameData.RemoveAds();
                _gameManager.SavedGameData.PersistData();
                _adBanner.HideBannerAd();
                break;
            case SUNRISE_THEME:
                _gameManager.SavedGameData.EnableSunriseTheme();
                _gameManager.SavedGameData.PersistData();
                break;
            case SUNSET_THEME:
                _gameManager.SavedGameData.EnableSunsetTheme();
                _gameManager.SavedGameData.PersistData();
                break;
            default:
                Debug.LogError("Unknown bought");
                break;
        }
        LoadIAPStatus();
    }

    public void LoadIAPStatus()
    {
        if (_gameManager.SavedGameData.PurchaseData.UnlimitedHints)
        {
            Button5Hints.interactable = false;
            FadeButton(Button5Hints);
        }
        if (_gameManager.SavedGameData.PurchaseData.UnlimitedHints)
        {
            ButtonUnlimitedHints.interactable = false;
            FadeButton(ButtonUnlimitedHints);
        }
        if (_gameManager.SavedGameData.UnlockedDifficulty == Constants.Difficulty.Challenge)
        {
            ButtonUnlockLevels.interactable = false;
            FadeButton(ButtonUnlockLevels);
        }
        if (_gameManager.SavedGameData.PurchaseData.RemovedAds)
        {
            ButtonRemoveAds.interactable = false;
            FadeButton(ButtonRemoveAds);
        }
        if (_gameManager.SavedGameData.PurchaseData.SunriseTheme)
        {
            ButtonSunriseTheme.interactable = false;
            FadeButton(ButtonSunriseTheme);
        }
        if (_gameManager.SavedGameData.PurchaseData.SunsetTheme)
        {
            ButtonSunsetTheme.interactable = false;
            FadeButton(ButtonSunsetTheme);
        }
    }

    private void FadeButton(Button button)
    {
        button.GetComponent<Image>().color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_GREEN
        ];
    }

    public void OnPurchaseFailed(
        Product product,
        PurchaseFailureDescription purchaseFailureDescription
    )
    {
        Debug.LogError(
            $"Failed to purchase {product.definition.id}. Reason: {purchaseFailureDescription.reason}"
        );
    }
}
