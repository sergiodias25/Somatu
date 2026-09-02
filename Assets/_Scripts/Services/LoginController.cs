using System;
using System.Threading.Tasks;
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
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        _slider.value = 0.12f;
        try
        {
            await UnityServices.InitializeAsync();
            GameManager _gameManager = FindObjectOfType<GameManager>();
            _slider.value = 0.99f;
            _gameManager.StartGame(_loadingCanvas);
            await SignInAnonymouslyAsync();
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
}
