using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private Button _startGameButton;

    [SerializeField]
    private Button _easyModeButton;

    [SerializeField]
    private Button _mediumModeButton;

    [SerializeField]
    private Button _hardModeButton;

    [SerializeField]
    private Button _extremeModeButton;

    [SerializeField]
    private Button _exitButton;

    [SerializeField]
    private Button _playAgainButton;

    [SerializeField]
    private Button _helpButton;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        HideSubMenus();
        HideEndOfGameButtons();
    }

    private void HideSubMenus()
    {
        HideButton(_easyModeButton);
        HideButton(_mediumModeButton);
        HideButton(_hardModeButton);
        HideButton(_extremeModeButton);
    }

    public void ShowMainMenu()
    {
        ShowButton(_startGameButton);
    }

    private void ShowSubMenus()
    {
        ShowButton(_easyModeButton);
        ShowButton(_mediumModeButton);
        ShowButton(_hardModeButton);
        ShowButton(_extremeModeButton);
    }

    private void HideButton(Button button)
    {
        button.image.enabled = false;
        button.GetComponentInChildren<TMP_Text>().enabled = false;
    }

    private void ShowButton(Button button)
    {
        button.image.enabled = true;
        button.GetComponentInChildren<TMP_Text>().enabled = true;
    }

    public void ClickOnStartGame()
    {
        HideButton(_startGameButton);
        ShowSubMenus();
    }

    public void ClickOnEasyMode()
    {
        HideSubMenus();
        ShowEndGameButtons();
        _gameManager.Init(Constants.Difficulty.Fácil);
    }

    public void ClickOnMediumMode()
    {
        HideSubMenus();
        ShowEndGameButtons();
        _gameManager.Init(Constants.Difficulty.Médio);
    }

    public void ClickOnHardMode()
    {
        HideSubMenus();
        ShowEndGameButtons();
        _gameManager.Init(Constants.Difficulty.Difícil);
    }

    public void ClickOnExtremeMode()
    {
        HideSubMenus();
        ShowEndGameButtons();
        _gameManager.Init(Constants.Difficulty.Extremo);
    }

    public void ShowEndGameButtons()
    {
        ShowButton(_playAgainButton);
        ShowButton(_exitButton);
        ShowButton(_helpButton);
    }

    public void PlayAgainClick()
    {
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false);
            _gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(_gameManager.SelectedDifficulty),
                    Constants.GetRepeatedNumbersCount(_gameManager.SelectedDifficulty)
                )
            );
        }
    }

    public void ExitGameClick()
    {
        _gameManager.ResetBoard(true);
        HideSubMenus();
        HideEndOfGameButtons();
        ShowMainMenu();
    }

    public void HideEndOfGameButtons()
    {
        HideButton(_playAgainButton);
        HideButton(_exitButton);
        HideButton(_helpButton);
    }
}
