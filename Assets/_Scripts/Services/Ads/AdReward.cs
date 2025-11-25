using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdRewarded : MonoBehaviour, IUnityAdsShowListener
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
        FindObjectOfType<AdsInitializer>().LoadAd();
        if (
            adUnitId.Equals(_adUnitId)
            && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)
        )
        {
            isAdsRunning = false;
            Debug.Log("Unity Ads Rewarded Ad Completed");
            if (GameObject.Find("HintPurchasePopup") != null)
            {
                FindObjectOfType<GameManager>().SavedGameData.IncrementHintsAvailableClassic(1);
                GameObject.Find("HintPurchasePopup").GetComponent<Popup>().ClosePopupGameplay();
                FindObjectOfType<UIManager>().ToggleHintButton(true);
            }
            else if (GameObject.Find("RemoveBannerPopup") != null)
            {
                GameObject.Find("RemoveBannerPopup").GetComponent<Popup>().ClosePopupGameplay();
            }
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"Ad Unit with id {adUnitId} was clicked");
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}
