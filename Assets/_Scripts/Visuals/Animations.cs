using Assets.Scripts.CustomAnimation;
using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    public void ButtonClicked(Button button)
    {
        _ = CustomAnimation.ButtonClicked(button.transform);
    }
}
