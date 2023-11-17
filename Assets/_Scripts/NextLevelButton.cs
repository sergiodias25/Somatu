using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Button _button;

    private void Start()
    {
        HideRestartButton();
    }

    public void ActivateRestartButton()
    {
        ShowButton();
    }

    public void HandleClick()
    {
        if (enabled)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.ResetBoard();
            gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(),
                    Constants.GetRepeatedNumbersCount()
                )
            );
            HideRestartButton();
        }
    }

    public void HideRestartButton()
    {
        _text.enabled = false;
        _text.alpha = 0f;
        _button.gameObject.GetComponent<Image>().enabled = false;
    }

    private void ShowButton()
    {
        ColorBlock cb = _button.colors;
        cb.normalColor = Constants.SuccessBackgroundColor;
        _button.colors = cb;
        _text.alpha = 1f;
        _text.enabled = true;
        _button.gameObject.GetComponent<Image>().enabled = true;
    }
}
