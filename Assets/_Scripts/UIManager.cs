using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Timer _timer;

    [SerializeField]
    private Button _startGameButton;

    [SerializeField]
    private Button _continueGameButton;

    [SerializeField]
    private Button _challengeButton;

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

    [SerializeField]
    private Button _undoButton;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        HideSubMenus();
        HideEndOfGameButtons();
        ToggleContinueButton();
    }

    private void Update()
    {
        _undoButton.enabled = _gameManager._undoMoveData.IsUndoEnabled();
        _mediumModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Médio
        );
        _hardModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Difícil
        );
        _extremeModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Extremo
        );
        _challengeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Desafio
        );
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
        ShowButton(_continueGameButton);
        ShowButton(_challengeButton);
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
        HideButton(_continueGameButton);
        HideButton(_challengeButton);
        ShowSubMenus();
    }

    public void ClickOnContinueGame()
    {
        HideButton(_startGameButton);
        HideButton(_continueGameButton);
        HideButton(_challengeButton);
        ShowGameplayButtons();
        _gameManager.Init(_gameManager.SelectedDifficulty, true);
    }

    public void ClickOnSurvival()
    {
        HideButton(_startGameButton);
        HideButton(_continueGameButton);
        HideButton(_challengeButton);
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Desafio);
    }

    public void ClickOnEasyMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Fácil);
    }

    public void ClickOnMediumMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Médio);
    }

    public void ClickOnHardMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Difícil);
    }

    public void ClickOnExtremeMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Extremo);
    }

    public void ShowGameplayButtons()
    {
        ShowButton(_playAgainButton);
        ShowButton(_exitButton);
        ShowButton(_undoButton);
        ShowButton(_helpButton);
    }

    public void PlayAgainClick()
    {
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false, false);
            _gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(_gameManager.SelectedDifficulty),
                    Constants.GetRepeatedNumbersCount(_gameManager.SelectedDifficulty)
                ),
                false
            );
            if (_gameManager.SelectedDifficulty == Constants.Difficulty.Desafio)
            {
                _timer.Init(_gameManager.SelectedDifficulty == Constants.Difficulty.Desafio);
                _gameManager.ResetTimesSolved();
            }
            else
            {
                _timer.UnpauseTimer();
            }
        }
    }

    public void ExitGameClick()
    {
        _gameManager.ResetBoard(true, false);
        HideSubMenus();
        HideEndOfGameButtons();
        ShowMainMenu();
    }

    public void HelpClick()
    {
        if (_helpButton.enabled)
        {
            _gameManager.ShowHints();
        }
    }

    public void UndoClick()
    {
        if (_undoButton.enabled)
        {
            _gameManager.UndoLastMove();
            FindObjectOfType<GameManager>().RemoveHints();
        }
    }

    public void HideEndOfGameButtons()
    {
        HideButton(_playAgainButton);
        HideButton(_exitButton);
        HideButton(_helpButton);
        HideButton(_undoButton);
        ToggleContinueButton();
    }

    private void ToggleContinueButton()
    {
        if (!_gameManager.SavedGameExists())
        {
            _continueGameButton.enabled = false;
        }
        else
        {
            _continueGameButton.enabled = true;
        }
    }

    internal void ToggleUndoButton(bool enabled)
    {
        _undoButton.enabled = enabled;
    }

    internal void ToggleHelpButton(bool enabled)
    {
        _helpButton.enabled = enabled;
    }
}
