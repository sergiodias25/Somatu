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

    public async void ClosePopup()
    {
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
        _popupPanel.SetActive(false); //TODO: animation
    }

    public async void CloseOnboarding()
    {
        await CustomAnimation.ButtonClicked(_closePopupButton.transform);
        FindObjectOfType<GameManager>().EnableGameplayBlocks();
        CustomAnimation
            .PopupUnload(_popupWindow.transform)
            .OnComplete(() => _popupPanel.SetActive(false));
    }

    public async void ActionQuitApplication()
    {
        await CustomAnimation.ButtonClicked(_actionButton.transform);
        FindObjectOfType<UIManager>().QuitApplicationClick();
    }

    public async void ActionChangeDifficulty()
    {
        await CustomAnimation.ButtonClicked(_actionButton.transform);
        FindObjectOfType<UIManager>().ChangeModeClick();
        _popupPanel.SetActive(false); //TODO: animation
    }
}
