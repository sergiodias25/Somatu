using System;
using Assets.Scripts.CustomAnimation;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class HoldButtonToClose : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image button;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    public VideoPlayer vid;

    [SerializeField]
    private float maxDuration = 2f;
    private float durationHeld = 0f;

    private bool isPressing = false;
    private bool videoFinished = false;

    private void Start()
    {
        if (vid != null)
        {
            UpdateButtonAvailability(false);
            vid.loopPointReached += VideoFinished;
        }
        else
        {
            UpdateButtonAvailability(true);
            videoFinished = true;
        }
        buttonText.text = LocalizationManager.Localize("btn-close");
    }

    private void VideoFinished(VideoPlayer source)
    {
        if (!videoFinished)
        {
            UpdateButtonAvailability(true);
        }
        videoFinished = true;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!isPressing & videoFinished)
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

    private void UpdateButtonAvailability(bool buttonEnabled)
    {
        Color originalColor = button.color;
        if (buttonEnabled && button.color.a != 1)
        {
            button.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        }
        else if (!buttonEnabled && button.color.a != 0.5f)
        {
            button.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }
    }
}
