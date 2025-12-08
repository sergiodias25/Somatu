using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginController : MonoBehaviour
{
    [SerializeField]
    private Canvas _loadingCanvas;

    [SerializeField]
    public Slider _slider;
    public string Token;
    public string Error;

    async void Awake()
    {
        _slider.value = 0.12f;
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
            await SignInAnonymouslyAsync();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("SignIn is successful.");
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to authenticate. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            Debug.Log("SignIn has failed.");
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
