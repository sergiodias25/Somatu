using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Button _restartButton;

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
        _restartButton.gameObject.GetComponent<Image>().enabled = false;
    }

    private void ShowButton()
    {
        ColorBlock cb = _restartButton.colors;
        cb.normalColor = Constants.SuccessBackgroundColor;
        _restartButton.colors = cb;
        _text.alpha = 1f;
        _text.enabled = true;
        _restartButton.gameObject.GetComponent<Image>().enabled = true;
    }
}
