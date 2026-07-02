using Assets.Scripts.CustomAnimation;
using DG.Tweening;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private GameObject _popupPanel;

    [SerializeField]
    private GameObject _popupWindow;

    [SerializeField]
    private Button _closePopupButton;

    [SerializeField]
    private Button _actionButton;

    [SerializeField]
    private Button _secondActionButton;

    private GameManager _gameManager;

    public void OnEnable()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        _gameManager.DisableGameplayBlocks();
    }

    public async void ClosePopup()
    {
        await CustomAnimation.ButtonClicked(
            _closePopupButton.transform,
            Constants.AudioClip.Undo,
            true
        );
        ClosePopupGameplay();
    }

    public void ClosePopupGameplay()
    {
        FindObjectOfType<GameManager>().EnableGameplayBlocks();
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
    }

    public async void ClosePopupMenu()
    {
        await CustomAnimation.ButtonClicked(
            _closePopupButton.transform,
            Constants.AudioClip.Undo,
            true
        );
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
    }

    public async void CloseOnboarding()
    {
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
        FindObjectOfType<GameManager>().EnableGameplayBlocks();
        FindObjectOfType<AudioManager>().UnpauseMusic();
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
    }

    public async void ActionQuitApplication()
    {
        await CustomAnimation.ButtonClicked(_actionButton.transform);
        FindObjectOfType<UIManager>().QuitApplicationClick();
    }

    public async void ActionChangeDifficulty()
    {
        await CustomAnimation.ButtonClicked(_actionButton.transform);
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
        FindObjectOfType<UIManager>().ChangeModeClick();
    }

    public async void ActionRemoveBanner()
    {
        await CustomAnimation.ButtonClicked(_actionButton.transform);
        FindObjectOfType<IAPScript>().RemoveAds();
    }

    public async void ActionRateUs()
    {
        await CustomAnimation.ButtonClicked(_secondActionButton.transform);
        Application.OpenURL("market://details?id=" + Application.identifier);
    }

    public async void ActionConsent(bool gaveConsent)
    {
        _gameManager.SavedGameData.ConsentAnswered = true;
        if (gaveConsent)
        {
            _gameManager.SavedGameData.ConsentGiven = true;
            await CustomAnimation.ButtonClicked(
                _actionButton.transform,
                Constants.AudioClip.MenuInteraction,
                true
            );
            CustomAnimation.PopupUnload(_popupPanel.transform, _popupWindow.transform);
            AnalyticsService.Instance.StartDataCollection();
        }
        else
        {
            _gameManager.SavedGameData.ConsentGiven = false;
            await CustomAnimation.ButtonClicked(
                _secondActionButton.transform,
                Constants.AudioClip.Undo,
                true
            );
            CustomAnimation.PopupUnload(_popupPanel.transform, _popupWindow.transform);
            AnalyticsService.Instance.StopDataCollection();
        }
    }
}
