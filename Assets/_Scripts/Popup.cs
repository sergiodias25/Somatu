using Assets.Scripts.CustomAnimation;
using DG.Tweening;
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

    public void OnEnable()
    {
        FindObjectOfType<GameManager>().DisableGameplayBlocks();
    }

    public async void ClosePopup()
    {
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
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
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
    }

    public async void CloseOnboarding()
    {
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
        FindObjectOfType<GameManager>().EnableGameplayBlocks();
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
        CustomAnimation.PopupUnload(
            _popupPanel.transform,
            _popupPanel.transform.Find("Interactible")
        );
    }
}
