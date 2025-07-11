using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdBanner : MonoBehaviour
{
    BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    string _androidAdUnitId = "Banner_Android";

#pragma warning disable 414
    string _iOSAdUnitId = "Banner_iOS";
#pragma warning restore 414
    string _adUnitId = null; // This will remain null for unsupported platforms.

    void Start()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);
        LoadBanner();
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        if (!FindObjectOfType<GameManager>().SavedGameData.PurchaseData.RemovedAds)
        {
            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        //Debug.Log("Banner loaded");
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }

    void OnBannerShown() { }

    void OnBannerHidden() { }
}
