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
    private GameObject _mainMenuPanel;

    [SerializeField]
    private Button _continueGameButton;

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
    private PlayerStats _playerStats;

    [SerializeField]
    private GameObject _gameNodes;

    [SerializeField]
    private GameObject _shopPanel;

    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private GameObject _profilePanel;

    [SerializeField]
    private GameObject _backgroundsPanel;

    [SerializeField]
    private GameObject _topPanel;

    [SerializeField]
    private GameObject _gameTitle;

    [SerializeField]
    private GameObject _classicMenu;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        _playerStats = FindObjectOfType<PlayerStats>();
        HideClassicMenu();
        HideObject(_gameplayPanel);
        ToggleContinueButton();
    }

    private void HideClassicMenu()
    {
        HideObject(_classicMenu);
    }

    public void ShowMainMenu()
    {
        _gameManager = FindObjectOfType<GameManager>();
        ShowObject(_mainMenuPanel);
        HideObject(_shopPanel);
        HideObject(_settingsPanel);
        HideObject(_profilePanel);
        ShowObject(_backgroundsPanel);
        ShowObject(_topPanel);
        HideObject(_classicMenu);
    }

    private void ShowClassicMenu()
    {
        ShowObject(_classicMenu);
        ToggleContinueButton();
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
        HideObject(_mainMenuPanel);
        ShowClassicMenu();
    }

    public void ClickOnContinueGame()
    {
        HideObject(_mainMenuPanel);
        ShowGameplayButtons();
        _gameManager.Init(
            (Constants.Difficulty)_gameManager.SavedGameData.GameInProgressData.Difficulty,
            true
        );
    }

    public void ClickOnChallenge()
    {
        HideObject(_mainMenuPanel);
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Challenge);
    }

    public void ClickOnEasyMode()
    {
        HideClassicMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Easy);
    }

    public void ClickOnMediumMode()
    {
        HideClassicMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Medium);
    }

    public void ClickOnHardMode()
    {
        HideClassicMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Hard);
    }

    public void ClickOnExtremeMode()
    {
        HideClassicMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Extreme);
    }

    public void ShowGameplayButtons()
    {
        ShowObject(_gameNodes);
        ShowObject(_gameplayPanel);
        ShowObject(_gameTitle);
        HideObject(_classicMenu);

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
        HideClassicMenu();
        HideObject(_gameNodes);
        HideObject(_gameplayPanel);
        HideObject(_shopPanel);
        HideObject(_profilePanel);
        HideObject(_settingsPanel);
        HideObject(_classicMenu);
        ShowMainMenu();
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

    public void RestoreGameplayPanel()
    {
        ToggleGameplayElements(true);
        if (_gameManager.IsGameInProgress() && !_gameManager.HasGameEnded())
        {
            _timer.UnpauseTimer();
        }
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
            HideObject(_gameTitle);
        }
        _gameNodes.SetActive(statusToChangeTo);
    }

    public void ToggleSettingsPanel()
    {
        if (_settingsPanel.activeSelf)
        {
            HideObject(_settingsPanel);
            if (_gameManager.IsGameInProgress())
            {
                RestoreGameplayPanel();
            }
            else
            {
                ShowMainMenu();
            }
        }
        else
        {
            HideObject(_mainMenuPanel);
            HideClassicMenu();
            ToggleGameplayElements(false);
            if (_shopPanel.activeSelf)
            {
                HideObject(_shopPanel);
            }
            if (_profilePanel.activeSelf)
            {
                HideObject(_profilePanel);
            }
            if (_gameManager.IsGameInProgress())
            {
                _timer.PauseTimer();
            }
            ShowObject(_settingsPanel);
        }
    }

    public void ToggleProfilePanel()
    {
        if (_profilePanel.activeSelf)
        {
            HideObject(_profilePanel);
            if (_gameManager.IsGameInProgress())
            {
                RestoreGameplayPanel();
            }
            else
            {
                ShowMainMenu();
            }
        }
        else
        {
            HideObject(_mainMenuPanel);
            HideClassicMenu();
            ToggleGameplayElements(false);
            if (_shopPanel.activeSelf)
            {
                HideObject(_shopPanel);
            }
            if (_settingsPanel.activeSelf)
            {
                HideObject(_settingsPanel);
            }
            if (_gameManager.IsGameInProgress())
            {
                _timer.PauseTimer();
            }

            ShowObject(_profilePanel);
        }
    }

    public void ToggleShopPanel()
    {
        if (_shopPanel.activeSelf)
        {
            HideObject(_shopPanel);
            if (_gameManager.IsGameInProgress())
            {
                RestoreGameplayPanel();
            }
            else
            {
                ShowMainMenu();
            }
        }
        else
        {
            HideObject(_mainMenuPanel);
            HideClassicMenu();
            ToggleGameplayElements(false);
            if (_settingsPanel.activeSelf)
            {
                HideObject(_settingsPanel);
            }
            if (_profilePanel.activeSelf)
            {
                HideObject(_profilePanel);
            }
            if (_gameManager.IsGameInProgress())
            {
                _timer.PauseTimer();
            }
            ShowObject(_shopPanel);
        }
    }
}
