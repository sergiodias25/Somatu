using Assets.Scripts.CustomAnimation;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeBeforeAvailable : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.gameObject.SetActive(false);
        Invoke("EnableButton", 5);
    }

    private void EnableButton()
    {
        button.gameObject.SetActive(true);
        CustomAnimation.ButtonLoad(button.transform);
    }
}
