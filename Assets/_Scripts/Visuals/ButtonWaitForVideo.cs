using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ButtonWaitForVideo : MonoBehaviour
{
    [SerializeField]
    public VideoPlayer vid;
    private bool videoFinished = false;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        UpdateButtonAvailability(false);
        vid.loopPointReached += VideoFinished;
    }

    private void VideoFinished(VideoPlayer source)
    {
        if (!videoFinished)
        {
            UpdateButtonAvailability(true);
        }
        videoFinished = true;
    }

    private void UpdateButtonAvailability(bool buttonEnabled)
    {
        Color originalColor = button.GetComponent<Image>().color;
        if (buttonEnabled && button.GetComponent<Image>().color.a != 1)
        {
            button.interactable = true;
            button.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                1
            );
        }
        else if (!buttonEnabled && button.GetComponent<Image>().color.a != 0.5f)
        {
            button.interactable = false;
            button.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.5f
            );
        }
    }
}
