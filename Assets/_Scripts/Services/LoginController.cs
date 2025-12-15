using System;
using System.Threading.Tasks;
using GooglePlayGames;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [SerializeField]
    private Canvas _loadingCanvas;

    [SerializeField]
    public Slider _slider;

    async void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        _slider.value = 0.12f;
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
            await SignInAnonymouslyAsync();
            GoogleServices.Authenticate();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task SignInAnonymouslyAsync()
    {
        _slider.value = 0.60f;
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        _slider.value = 0.25f;
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            //Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            //Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            GameManager _gameManager = FindObjectOfType<GameManager>();
            _gameManager.StartGame(_loadingCanvas);
            _slider.value = 0.99f;
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
            GameManager _gameManager = FindObjectOfType<GameManager>();
            _gameManager.StartGame(_loadingCanvas);
            _slider.value = 0.99f;
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}
