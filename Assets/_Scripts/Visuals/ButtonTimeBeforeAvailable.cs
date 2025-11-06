using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeBeforeAvailable : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        UpdateButtonAvailability(false);
        Invoke("EnableButton", 5);
    }

    private void EnableButton()
    {
        UpdateButtonAvailability(true);
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
