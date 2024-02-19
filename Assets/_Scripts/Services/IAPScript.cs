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
    private const string METALLIC_THEME = "METALLIC_THEME";
    private const string GOLD_THEME = "GOLD_THEME";
    public Button Button5Hints;
    public Button ButtonUnlimitedHints;
    public Button ButtonUnlockLevels;
    public Button ButtonRemoveAds;
    public Button ButtonMetallicTheme;
    public Button ButtonGoldTheme;
    private GameManager _gameManager;
    private AdBanner _adBanner;
    private UIManager _uiManager;
    private bool _wasGamePausedOnLaunch = false;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _adBanner = FindObjectOfType<AdBanner>();
        _uiManager = FindObjectOfType<UIManager>();
        LoadIAPStatus();
    }

    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case HINTS_5:
                _gameManager.SavedGameData.IncrementHelpsAvailable(5);
                _gameManager.SavedGameData.PersistData();
                _uiManager.UpdateHelpButtonText();
                break;
            case HINTS_UNLIMITED:
                _gameManager.SavedGameData.GrantUnlimitedHelps();
                _gameManager.SavedGameData.PersistData();
                _uiManager.UpdateHelpButtonText();
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
            case METALLIC_THEME:
                _gameManager.SavedGameData.EnableMetallicTheme();
                _gameManager.SavedGameData.PersistData();
                break;
            case GOLD_THEME:
                _gameManager.SavedGameData.EnableGoldTheme();
                _gameManager.SavedGameData.PersistData();
                break;
            default:
                Debug.LogError("Unknown bought");
                break;
        }
    }

    public void LoadIAPStatus()
    {
        if (_gameManager.SavedGameData.PurchaseData.UnlimitedHelps)
        {
            Button5Hints.interactable = false;
        }
        if (_gameManager.SavedGameData.PurchaseData.UnlimitedHelps)
        {
            ButtonUnlimitedHints.interactable = false;
        }
        if (_gameManager.SavedGameData.UnlockedDifficulty == Constants.Difficulty.Challenge)
        {
            ButtonUnlockLevels.interactable = false;
        }
        if (_gameManager.SavedGameData.PurchaseData.RemovedAds)
        {
            ButtonRemoveAds.interactable = false;
        }
        if (_gameManager.SavedGameData.PurchaseData.MetallicTheme)
        {
            ButtonMetallicTheme.interactable = false;
        }
        if (_gameManager.SavedGameData.PurchaseData.GoldTheme)
        {
            ButtonGoldTheme.interactable = false;
        }
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

    private void OnEnable()
    {
        _wasGamePausedOnLaunch = _uiManager.OpenShop();
    }

    private void OnDisable()
    {
        if (_wasGamePausedOnLaunch)
        {
            _uiManager.RestoreGameplayPanel();
        }
        else
        {
            _uiManager.ShowMainMenu();
        }
    }
}
