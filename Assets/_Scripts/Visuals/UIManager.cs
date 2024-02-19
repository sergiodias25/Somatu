using System;
using Assets.SimpleLocalization.Scripts;
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

    [SerializeField]
    private GameObject _gameplayPanel;
    private AnimationsHandler _animationsHandler;
    private PlayerStats _playerStats;

    [SerializeField]
    private GameObject _gameNodes;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        _animationsHandler = FindObjectOfType<AnimationsHandler>();
        _playerStats = FindObjectOfType<PlayerStats>();
        //ShowMainMenu();
        HideSubMenus();
        HideObject(_gameplayPanel);
        ToggleContinueButton();
    }

    private void HideSubMenus()
    {
        HideObject(_easyModeButton.gameObject);
        HideObject(_mediumModeButton.gameObject);
        HideObject(_hardModeButton.gameObject);
        HideObject(_extremeModeButton.gameObject);
    }

    public void ShowMainMenu()
    {
        _gameManager = FindObjectOfType<GameManager>();
        ShowObject(_startGameButton.gameObject);
        ToggleContinueButton();
        ShowObject(_challengeButton.gameObject);

        _challengeButton.enabled = _gameManager.SavedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Challenge
        );
    }

    private void ShowSubMenus()
    {
        ShowObject(_easyModeButton.gameObject);
        ShowObject(_mediumModeButton.gameObject);
        ShowObject(_hardModeButton.gameObject);
        ShowObject(_extremeModeButton.gameObject);

        _mediumModeButton.enabled = _gameManager.SavedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Medium
        );
        _hardModeButton.enabled = _gameManager.SavedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Hard
        );
        _extremeModeButton.enabled = _gameManager.SavedGameData.IsDifficultyUnlocked(
            Constants.Difficulty.Extreme
        );
    }

    private void HideObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void ShowObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void ClickOnStartGame()
    {
        HideMainMenu();
        ShowSubMenus();
    }

    public void ClickOnContinueGame()
    {
        HideMainMenu();
        ShowGameplayButtons();
        _gameManager.Init(
            (Constants.Difficulty)_gameManager.SavedGameData.GameInProgressData.Difficulty,
            true
        );
    }

    public void ClickOnChallenge()
    {
        HideMainMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Challenge);
    }

    public void HideMainMenu()
    {
        HideObject(_startGameButton.gameObject);
        HideObject(_continueGameButton.gameObject);
        HideObject(_challengeButton.gameObject);
    }

    public void ClickOnEasyMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Easy);
    }

    public void ClickOnMediumMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Medium);
    }

    public void ClickOnHardMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Hard);
    }

    public void ClickOnExtremeMode()
    {
        HideSubMenus();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Extreme);
    }

    public void ShowGameplayButtons()
    {
        ShowObject(_gameplayPanel);

        ToggleUndoButton(
            _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
        );
    }

    public void PlayAgainClick()
    {
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false, true, true);
            _gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(_gameManager.ActualDifficulty),
                    Constants.GetRepeatedNumbersCount(
                        _gameManager.ActualDifficulty,
                        _gameManager.SavedGameData.IsHalfwayThroughCurrentDifficulty(
                            _gameManager.ActualDifficulty
                        )
                    )
                ),
                false
            );
            _playerStats.StartedGame(_gameManager.SelectedDifficulty);

            if (_gameManager.SelectedDifficulty == Constants.Difficulty.Challenge)
            {
                _timer.Init(_gameManager.SelectedDifficulty == Constants.Difficulty.Challenge);
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
        HideObject(_gameplayPanel);
        ToggleContinueButton();
        ShowMainMenu();

        _animationsHandler.HideStats();
        _animationsHandler.HideSettings();
    }

    public void HelpClick()
    {
        if (_helpButton.enabled)
        {
            _gameManager.ShowHints();
            _playerStats.UsedHelp(_gameManager.SelectedDifficulty);
            ToggleHelpButton(false);
        }
    }

    public void UndoClick()
    {
        if (_undoButton.enabled)
        {
            _gameManager.UndoLastMove();
            FindObjectOfType<GameManager>().RemoveHints();
        }
        ToggleUndoButton(
            _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
        );
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
        _helpButton.enabled =
            enabled
            && !_gameManager.HasGameEnded()
            && (
                _gameManager.SavedGameData.HelpsAvailable != 0
                || _gameManager.SavedGameData.PurchaseData.UnlimitedHelps
            );
        UpdateHelpButtonText();
    }

    public void UpdateHelpButtonText()
    {
        string translationText = LocalizationManager.Localize("btn-hint");
        if (!_gameManager.SavedGameData.PurchaseData.UnlimitedHelps)
        {
            _helpButtonText.text =
                translationText + ": " + _gameManager.SavedGameData.HelpsAvailable.ToString();
        }
        else
        {
            _helpButtonText.text = translationText;
        }
    }

    public async void DeleteSaves()
    {
        _gameManager.SavedGameData = new Assets.Scripts.SaveGame.SaveGame();
        ISaveClient _client = new CloudSaveClient();
        await SaveService.DeleteData(
            _client,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
        );
        ISaveClient _client2 = new PlayerPrefClient();
        await SaveService.DeleteData(
            _client2,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
        );
    }

    public bool OpenShop()
    {
        ToggleShopElements(true);

        if (_gameManager.IsGameInProgress())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RestoreGameplayPanel()
    {
        ToggleGameplayElements(true);
        _timer.ToggleTimer();
    }

    public void ToggleGameplayElements(bool statusToChangeTo)
    {
        if (statusToChangeTo)
        {
            ShowObject(_gameplayPanel);
        }
        else
        {
            HideObject(_gameplayPanel);
        }
        _gameNodes.SetActive(statusToChangeTo);
    }

    internal void ToggleShopElements(bool openedShop)
    {
        if (openedShop)
        {
            HideMainMenu();
            HideSubMenus();
            // hide settings
            // hide player stats
            if (_gameManager.IsGameInProgress())
            {
                ToggleGameplayElements(false);
                _timer.ToggleTimer();
            }
        }
    }
}
