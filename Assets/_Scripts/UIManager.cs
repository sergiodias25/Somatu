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
    private Button _playAgainButton;

    [SerializeField]
    private Button _helpButton;

    [SerializeField]
    private TextMeshProUGUI _helpButtonText;

    [SerializeField]
    private Button _undoButton;
    private AnimationsHandler _animationsHandler;
    private PlayerStats _playerStats;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        _animationsHandler = FindObjectOfType<AnimationsHandler>();
        _playerStats = FindObjectOfType<PlayerStats>();
        ShowMainMenu();
        HideSubMenus();
        HideEndOfGameButtons();
        ToggleContinueButton();
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
        ToggleContinueButton();
        ShowButton(_challengeButton);

        _challengeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Desafio
        );
    }

    private void ShowSubMenus()
    {
        ShowButton(_easyModeButton);
        ShowButton(_mediumModeButton);
        ShowButton(_hardModeButton);
        ShowButton(_extremeModeButton);

        _mediumModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Médio
        );
        _hardModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Difícil
        );
        _extremeModeButton.enabled = _gameManager._savedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Extremo
        );
    }

    private void HideButton(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void ShowButton(Button button)
    {
        button.gameObject.SetActive(true);
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
        _gameManager.Init(
            (Constants.Difficulty)_gameManager._savedGameData._savedGameDifficulty,
            true
        );
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
        ShowButton(_undoButton);
        ShowButton(_helpButton);
    }

    public void PlayAgainClick()
    {
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false, false, true);
            _gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(_gameManager.ActualDifficulty),
                    Constants.GetRepeatedNumbersCount(_gameManager.ActualDifficulty)
                ),
                false
            );
            _playerStats.StartedGame(_gameManager.SelectedDifficulty);

            if (_gameManager.SelectedDifficulty == Constants.Difficulty.Desafio)
            {
                _timer.Init(_gameManager.SelectedDifficulty == Constants.Difficulty.Desafio);
                _gameManager.ResetTimesSolved();
            }
            else
            {
                _timer.RestartTimer();
            }
        }
    }

    public void HomeClick()
    {
        if (_gameManager.IsGameInProgress())
        {
            _gameManager.CheckResult(false);
            _gameManager.ResetBoard(true, false, true);
        }
        HideSubMenus();
        HideEndOfGameButtons();
        ShowMainMenu();

        _animationsHandler.HideGameplayBar();
        _animationsHandler.HideStats();
        _animationsHandler.HideSettings();
    }

    public void HelpClick()
    {
        if (_helpButton.enabled)
        {
            _gameManager.ShowHints();
            _playerStats.UsedHelp(_gameManager.SelectedDifficulty);
            _gameManager._savedGameData.HelpsAvailable--;
            ToggleHelpButton(true);
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
        HideButton(_helpButton);
        HideButton(_undoButton);
        ToggleContinueButton();
    }

    private void ToggleContinueButton()
    {
        _continueGameButton.gameObject.SetActive(_gameManager.SavedGameExists());
    }

    internal void ToggleUndoButton(bool enabled)
    {
        _undoButton.enabled = enabled;
    }

    internal void ToggleHelpButton(bool enabled)
    {
        _helpButtonText.text = "Ajuda: " + _gameManager._savedGameData.HelpsAvailable.ToString();
        _helpButton.enabled =
            enabled
            && !_gameManager.HasGameEnded()
            && _gameManager._savedGameData.HelpsAvailable > 0;
        ;
    }
}
