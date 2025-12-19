using UnityEngine;
using UnityEngine.Advertisements;

public class AdInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    string _androidAdUnitId = "Interstitial_Android";
    string _iOSAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    void Awake()
    {
        _adUnitId =
            (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSAdUnitId
                : _androidAdUnitId;
    }

    public void ShowAd()
    {
        if (Advertisement.isShowing)
        {
            return;
        }
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Advertisement.Load(adUnitId, this);
    }

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
            FindObjectOfType<GameManager>()._gamesPlayedWithoutAds = 0;
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
}
