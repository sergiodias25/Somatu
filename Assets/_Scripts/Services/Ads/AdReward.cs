using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Assets.Scripts.AnalyticsEvent;

public class AdRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField]
    Button _showAdButton;

    string _androidAdUnitId = "Rewarded_Android";

#pragma warning disable 414
    string _iOSAdUnitId = "Rewarded_iOS";
#pragma warning restore 414
    string _adUnitId = null; // This will remain null for unsupported platforms

    private bool isAdsRunning = false;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;
        LoadAd();
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        if (Advertisement.isShowing)
        {
            return;
        }
        if (isAdsRunning)
        {
            return;
        }

        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Ads start callback
    public void OnUnityAdsShowStart(string placementId)
    {
        isAdsRunning = true;
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(
        string adUnitId,
        UnityAdsShowCompletionState showCompletionState
    )
    {
        if (
            adUnitId.Equals(_adUnitId)
            && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)
        )
        {
            isAdsRunning = false;
            Debug.Log("Unity Ads Rewarded Ad Completed");
            FindObjectOfType<GameManager>().SavedGameData.IncrementHintsAvailableClassic(1);
            GameObject.Find("HintPurchasePopup").GetComponent<Popup>().ClosePopupGameplay();
            FindObjectOfType<UIManager>().ToggleHintButton(true);
            Purchase.SendAnalyticsEvent("HINT_1", true);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Purchase.SendAnalyticsEvent("HINT_1_L", false);
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        Purchase.SendAnalyticsEvent("HINT_1_S", false);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Purchase.SendAnalyticsEvent("HINT_1_C", true);
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}
