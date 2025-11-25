using UnityEngine;
using UnityEngine.Purchasing;
using Assets.Scripts.AnalyticsEvent;

public class IAPScript : MonoBehaviour
{
    private const string HINTS_5 = "hints_5";
    private const string HINTS_UNLIMITED = "hints_unlimited";
    private const string REMOVE_ADS = "remove_ads";
    private const string SUNRISE_THEME = "sunrise_theme";
    private const string SUNSET_THEME = "sunset_theme";
    public CodelessIAPButton ButtonRemoveAds;
    private GameManager _gameManager;
    private AdBanner _adBanner;
    private UIManager _uiManager;
    private SettingsHandler _settingsHandler;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _adBanner = FindObjectOfType<AdBanner>();
        _uiManager = FindObjectOfType<UIManager>();
        _settingsHandler = FindObjectOfType<SettingsHandler>();
    }

    public void OnPurchaseComplete(Product product)
    {
        ProcessPurchase(product);
    }

    private void ProcessPurchase(Product product)
    {
        switch (product.definition.id)
        {
            case HINTS_5:
                _gameManager.SavedGameData.IncrementHintsAvailableClassic(5);
                _gameManager.SavedGameData.PersistData();
                GameObject.Find("HintPurchasePopup").GetComponent<Popup>().ClosePopupGameplay();
                _uiManager.ToggleHintButton(true);
                break;
            case HINTS_UNLIMITED:
                _gameManager.SavedGameData.GrantUnlimitedHints();
                _gameManager.SavedGameData.PersistData();
                GameObject.Find("HintPurchasePopup").GetComponent<Popup>().ClosePopupGameplay();
                _uiManager.ToggleHintButton(true);
                break;
            case REMOVE_ADS:
                _gameManager.SavedGameData.RemoveAds();
                _gameManager.SavedGameData.PersistData();
                _adBanner.HideBannerAd();
                break;
            case SUNRISE_THEME:
                _gameManager.SavedGameData.EnableSunriseTheme();
                _gameManager.SavedGameData.PersistData();
                _settingsHandler.ChangeTheme(2);
                break;
            case SUNSET_THEME:
                _gameManager.SavedGameData.EnableSunsetTheme();
                _gameManager.SavedGameData.PersistData();
                _settingsHandler.ChangeTheme(3);
                break;
            default:
                Debug.LogError("Unknown bought");
                break;
        }
        Purchase.SendAnalyticsEvent(product.definition.id, true);
    }

    public void OnPurchaseFailed(
        Product product,
        PurchaseFailureDescription purchaseFailureDescription
    )
    {
        if (purchaseFailureDescription.reason == PurchaseFailureReason.DuplicateTransaction)
        {
            ProcessPurchase(product);
        }
        else
        {
            Debug.LogError(
                $"Failed to purchase {product.definition.id}. Reason: {purchaseFailureDescription.reason}"
            );
            switch (product.definition.id)
            {
                case SUNRISE_THEME:
                case SUNSET_THEME:
                    _settingsHandler.RevertTheme();
                    break;
            }
            Purchase.SendAnalyticsEvent(product.definition.id, false);
        }
    }

    internal void RemoveAds()
    {
        ButtonRemoveAds.Invoke("PurchaseProduct", 0);
    }
}
