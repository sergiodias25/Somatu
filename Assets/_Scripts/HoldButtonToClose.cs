using Assets.Scripts.CustomAnimation;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButtonToClose : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image button;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private float maxDuration = 2f;
    private float durationHeld = 0f;

    private bool isPressing = false;

    private void Start()
    {
        buttonText.text = LocalizationManager.Localize("btn-close");
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!isPressing)
        {
            buttonText.text = LocalizationManager.Localize("btn-closing");
            isPressing = true;
            _ = CustomAnimation.ButtonPressed(
                button.transform,
                Constants.AudioClip.MenuInteraction,
                true
            );
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (isPressing)
        {
            isPressing = false;
            if (slider.value < 1)
            {
                buttonText.text = LocalizationManager.Localize("btn-close");
                durationHeld = 0;
                slider.value = 0;
                _ = CustomAnimation.ButtonReleased(
                    button.transform,
                    Constants.AudioClip.Undo,
                    true
                );
            }
        }
    }

    void Update()
    {
        if (isPressing)
        {
            durationHeld += Time.deltaTime;
            slider.value = durationHeld / maxDuration;
            if (slider.value == 1)
            {
                isPressing = false;
                ClosePopup();
            }
        }
    }

    public async void ClosePopup()
    {
        buttonText.text = LocalizationManager.Localize("btn-close");
        await CustomAnimation.ButtonReleased(
            button.transform,
            Constants.AudioClip.MenuInteraction,
            true
        );
        GetComponentInParent<Transform>()
            .GetComponentInParent<Transform>()
            .GetComponentInParent<Popup>()
            .ClosePopupFromHolding();
    }
}
