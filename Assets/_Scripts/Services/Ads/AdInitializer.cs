using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener
{
    [SerializeField]
    string _androidGameId;

    [SerializeField]
    string _iOSGameId;

    [SerializeField]
    bool _testMode = false;
    private string _gameId;
    string _androidAdUnitId = "Rewarded_Android";

#pragma warning disable 414
    string _iOSAdUnitId = "Rewarded_iOS";
#pragma warning restore 414
    string _adUnitId = null; // This will remain null for unsupported platforms

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
        _gameId = _androidGameId; //Only for testing the functionality in the Editor
        _adUnitId = _androidAdUnitId;
#endif
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        FindObjectOfType<AdBanner>().LoadBanner();
        LoadAd();
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        // Use the error details to determine whether to try to load another ad.
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
