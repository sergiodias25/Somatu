using Assets.Scripts.CustomAnimation;
using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    public async void ButtonClicked(Button button)
    {
        await CustomAnimation.ButtonClicked(button.transform);
    }
}
