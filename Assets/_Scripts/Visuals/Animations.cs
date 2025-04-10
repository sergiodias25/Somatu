using Assets.Scripts.CustomAnimation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    public void ButtonClicked(Button button)
    {
        _ = CustomAnimation.ButtonClicked(button.transform);
    }

    public void TextClicked(TextMeshProUGUI text)
    {
        _ = CustomAnimation.TextClicked(text.transform);
    }
}
