using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.CustomAnimation;
using GridSum.Assets._Scripts.Visuals;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private TopBarManager _topBarManager;
    private Timer _timer;

    [SerializeField]
    private GameObject _mainMenuPanel;

    [SerializeField]
    private Button _continueGameButton;

    [SerializeField]
    private Button _playAgainButton;

    [SerializeField]
    private Button _hintButton;

    [SerializeField]
    private TextMeshProUGUI _hintButtonText;

    [SerializeField]
    private Button _undoButton;

    [SerializeField]
    private GameObject _gameplayStatsPanel;

    [SerializeField]
    private GameObject _gameplayInGamePanel;

    [SerializeField]
    private GameObject _gameplayEndGamePanel;
    private PlayerStats _playerStats;

    [SerializeField]
    private GameObject _gameNodes;

    [SerializeField]
    private GameObject _shopPanel;

    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private GameObject _supportPanel;

    [SerializeField]
    private Button _deleteDataButton;

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

    [SerializeField]
    private GameObject _classicModeButton;

    [SerializeField]
    private GameObject _easyModeButton;

    [SerializeField]
    private GameObject _mediumModeButton;

    [SerializeField]
    private GameObject _hardModeButton;

    [SerializeField]
    private GameObject _extremeModeButton;

    [SerializeField]
    private GameObject _challengeModeButton;

    [SerializeField]
    private GameObject _difficultyPanel;

    [SerializeField]
    private TextMeshProUGUI _difficultyPanelText;

    [SerializeField]
    private GameObject _unlockPanel;

    [SerializeField]
    private TextMeshProUGUI _unlockPanelText;

    [SerializeField]
    private GameObject _quitPanel;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        _playerStats = FindObjectOfType<PlayerStats>();
        _topBarManager = FindObjectOfType<TopBarManager>();
        HideClassicMenu();
        HideObject(_gameplayStatsPanel);
        HideObject(_gameplayInGamePanel);
        HideObject(_gameplayEndGamePanel);
        ToggleContinueButton();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_settingsPanel.activeSelf)
            {
                ToggleSettingsPanel();
            }
            if (_supportPanel.activeSelf)
            {
                ToggleSupportPanel();
            }
            else if (_shopPanel.activeSelf)
            {
                ToggleShopPanel();
            }
            else if (_profilePanel.activeSelf)
            {
                ToggleProfilePanel();
            }
            else if (_gameManager.IsGameInProgress())
            {
                ClickOnHome();
            }
            else if (_classicMenu.activeSelf)
            {
                // TODO: add challenge menu to if clause when it is implemented
                ClickOnHome();
            }
            else
            {
                ShowObject(_quitPanel);
            }
        }
    }

    private void HideClassicMenu()
    {
        HideObject(_classicMenu);
    }

    public void ShowMainMenu()
    {
        _gameManager = FindObjectOfType<GameManager>();
        ShowObject(_mainMenuPanel);
        CustomAnimation.ButtonLoad(_classicModeButton.transform);
        CustomAnimation.ButtonLoad(_challengeModeButton.transform);
        HideObject(_shopPanel);
        HideObject(_settingsPanel);
        HideObject(_supportPanel);
        HideObject(_profilePanel);
        ShowObject(_topPanel);
        HideObject(_classicMenu);
        _topBarManager.SelectHomeButton();
    }

    public async void ShowClassicMenu()
    {
        await CustomAnimation.ButtonClicked(_classicModeButton.transform);
        HideObject(_mainMenuPanel);
        ShowObject(_classicMenu);
        CustomAnimation.ButtonLoad(_easyModeButton.transform);
        CustomAnimation.ButtonLoad(_mediumModeButton.transform);
        CustomAnimation.ButtonLoad(_hardModeButton.transform);
        CustomAnimation.ButtonLoad(_extremeModeButton.transform);
        ToggleContinueButton();
        _topBarManager.DeselectAll();
    }

    private void HideObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void ShowObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public async void ClickOnContinueGame()
    {
        await CustomAnimation.ButtonClicked(_continueGameButton.transform);
        HideObject(_mainMenuPanel);
        ShowGameplayButtons();
        _gameManager.Init(
            (Constants.Difficulty)_gameManager.SavedGameData.GameInProgressData.Difficulty,
            true
        );
    }

    public async void ClickOnChallenge()
    {
        await CustomAnimation.ButtonClicked(_challengeModeButton.transform);
        /*if (_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Challenge))
        {*/
        _topBarManager.DeselectHomeButton();
        HideObject(_mainMenuPanel);
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Challenge);
        /*}
        else
        {
            ShowDifficultyPopup(
                Constants.Difficulty.Challenge - 1,
                LocalizationManager.Localize("mode-challenge"),
                LocalizationManager.Localize("mode-extreme"),
                Constants.GetNumberOfSolvesToUnlockNextDifficulty(
                    Constants.Difficulty.Challenge - 1
                )
            );
        }*/
    }

    public async void ClickOnEasyMode()
    {
        await CustomAnimation.ButtonClicked(_easyModeButton.transform);
        HideClassicMenu();
        ShowGameplayButtons();
        _gameManager.Init(Constants.Difficulty.Easy);
    }

    public async void ClickOnMediumMode()
    {
        await CustomAnimation.ButtonClicked(_mediumModeButton.transform);

        if (_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Medium))
        {
            HideClassicMenu();
            ShowGameplayButtons();
            _gameManager.Init(Constants.Difficulty.Medium);
        }
        else
        {
            ShowDifficultyPopup(
                Constants.Difficulty.Medium - 1,
                LocalizationManager.Localize("mode-medium"),
                LocalizationManager.Localize("mode-easy"),
                Constants.GetNumberOfSolvesToUnlockNextDifficulty(Constants.Difficulty.Medium - 1)
            );
        }
    }

    private void ShowDifficultyPopup(
        Constants.Difficulty originalDiff,
        string targetDifficulty,
        string previousDifficulty,
        int numberOfSolvesNeeded
    )
    {
        int finalSolvesNeededText = numberOfSolvesNeeded;
        string popuTextKey = "popup-difficulty-plural";
        if (
            originalDiff == _gameManager.SavedGameData.UnlockedDifficulty.Value
            && _gameManager.SavedGameData.TimesBeatenCurrentDifficulty > 0
        )
        {
            finalSolvesNeededText =
                numberOfSolvesNeeded - _gameManager.SavedGameData.TimesBeatenCurrentDifficulty;
        }
        if (finalSolvesNeededText == 1)
        {
            popuTextKey = "popup-difficulty-singular";
        }
        _difficultyPanelText.text = LocalizationManager.Localize(
            popuTextKey,
            targetDifficulty,
            previousDifficulty,
            finalSolvesNeededText
        );
        ShowObject(_difficultyPanel);
    }

    public void ShowUnlockPopup(Constants.Difficulty newDifficulty)
    {
        string popuTextKey = "popup-unlock-difficulty";

        _unlockPanelText.text = LocalizationManager.Localize(
            popuTextKey,
            LocalizationManager.Localize("mode-" + newDifficulty.ToString().ToLower())
        );
        ShowObject(_unlockPanel);
    }

    public async void ClickOnHardMode()
    {
        await CustomAnimation.ButtonClicked(_hardModeButton.transform);
        if (_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Hard))
        {
            HideClassicMenu();
            ShowGameplayButtons();
            _gameManager.Init(Constants.Difficulty.Hard);
        }
        else
        {
            ShowDifficultyPopup(
                Constants.Difficulty.Hard - 1,
                LocalizationManager.Localize("mode-hard"),
                LocalizationManager.Localize("mode-medium"),
                Constants.GetNumberOfSolvesToUnlockNextDifficulty(Constants.Difficulty.Hard - 1)
            );
        }
    }

    public async void ClickOnExtremeMode()
    {
        await CustomAnimation.ButtonClicked(_extremeModeButton.transform);
        if (_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Extreme))
        {
            HideClassicMenu();
            ShowGameplayButtons();
            _gameManager.Init(Constants.Difficulty.Extreme);
        }
        else
        {
            ShowDifficultyPopup(
                Constants.Difficulty.Extreme - 1,
                LocalizationManager.Localize("mode-extreme"),
                LocalizationManager.Localize("mode-hard"),
                Constants.GetNumberOfSolvesToUnlockNextDifficulty(Constants.Difficulty.Extreme - 1)
            );
        }
    }

    public void ShowGameplayButtons()
    {
        _topBarManager.DeselectAll();
        ShowObject(_backgroundsPanel);
        ShowObject(_gameNodes);
        ShowObject(_gameplayStatsPanel);
        ShowObject(_gameplayInGamePanel);
        CustomAnimation.AnimateStartGameButtons(
            _gameplayInGamePanel.transform,
            _gameplayEndGamePanel.transform
        );
        HideObject(_gameplayEndGamePanel);
        ShowObject(_gameTitle);
        HideObject(_classicMenu);
        ToggleUndoButton(
            _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
        );
    }

    public async void PlayAgainClick()
    {
        await CustomAnimation.ButtonClicked(_playAgainButton.transform);
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false, true, true);
            _gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(_gameManager.ActualDifficulty),
                    Constants.GetRepeatedNumbersCount(
                        _gameManager.ActualDifficulty,
                        _gameManager.SavedGameData.IsHalfwayThroughCurrentDifficulty(
                            _gameManager.ActualDifficulty,
                            _gameManager.SelectedDifficulty,
                            0
                        )
                    )
                ),
                false
            );
            _playerStats.StartedGame(_gameManager.SelectedDifficulty);
            ShowGameplayButtons();

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

    public void ChangeModeClick()
    {
        _gameManager.ResetBoard(true, true, true);
        _gameManager.Init((Constants.Difficulty)_gameManager.SavedGameData.UnlockedDifficulty);

        _playerStats.StartedGame(_gameManager.SelectedDifficulty);
        ShowGameplayButtons();
    }

    public void ClickOnHome()
    {
        if (_gameManager.IsGameInProgress())
        {
            _gameManager.CheckResult(false);
            _gameManager.ResetBoard(true, false, true);
        }
        HideClassicMenu();
        HideObject(_gameNodes);
        HideObject(_gameplayStatsPanel);
        HideObject(_gameplayInGamePanel);
        HideObject(_gameplayEndGamePanel);
        HideObject(_shopPanel);
        HideObject(_profilePanel);
        HideObject(_settingsPanel);
        HideObject(_supportPanel);
        HideObject(_classicMenu);
        HideObject(_backgroundsPanel);
        ShowMainMenu();
        _topBarManager.SelectHomeButton();
    }

    public async void HintClick()
    {
        await CustomAnimation.ButtonClicked(_hintButton.transform);
        if (_hintButton.enabled)
        {
            if (_gameManager.UseHint())
            {
                _playerStats.UsedHint(_gameManager.SelectedDifficulty);
                UpdateHintButtonText();
            }
        }
        ToggleHintButton();
    }

    public async void UndoClick()
    {
        if (_undoButton.enabled)
        {
            _gameManager.UndoLastMove();
            FindObjectOfType<GameManager>().RemoveHints();
            await CustomAnimation.ButtonClicked(_undoButton.transform);
            ToggleUndoButton(
                _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
            );
        }
    }

    private void ToggleContinueButton()
    {
        bool savedGameExists = _gameManager.SavedGameExists();
        _continueGameButton.gameObject.SetActive(savedGameExists);
        if (savedGameExists)
        {
            CustomAnimation.ButtonLoad(_continueGameButton.transform);
        }
    }

    public void ToggleUndoButton()
    {
        ToggleUndoButton(
            _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
        );
    }

    internal void ToggleUndoButton(bool enabled)
    {
        Color originalColor = _undoButton.GetComponent<Image>().color;
        if (enabled && _undoButton.GetComponent<Image>().color.a != 1)
        {
            _undoButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                1
            );
        }
        else if (!enabled && _undoButton.GetComponent<Image>().color.a != 0.5f)
        {
            _undoButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.5f
            );
        }

        _undoButton.enabled = enabled;
    }

    public void ToggleHintButton()
    {
        ToggleHintButton(
            _gameManager.SavedGameData.HintsAvailable > 0
                || _gameManager.SavedGameData.PurchaseData.UnlimitedHints
        );
    }

    internal void ToggleHintButton(bool enabled)
    {
        _hintButton.enabled =
            enabled
            && !_gameManager.HasGameEnded()
            && (
                _gameManager.SavedGameData.HintsAvailable >= 0
                || _gameManager.SavedGameData.PurchaseData.UnlimitedHints
            );

        Color originalColor = _hintButton.GetComponent<Image>().color;
        if (
            (
                _gameManager.SavedGameData.HintsAvailable > 0
                || _gameManager.SavedGameData.PurchaseData.UnlimitedHints
            )
            && _hintButton.GetComponent<Image>().color.a != 1
        )
        {
            _hintButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                1
            );
        }
        else if (
            (
                _gameManager.SavedGameData.HintsAvailable <= 0
                && !_gameManager.SavedGameData.PurchaseData.UnlimitedHints
            )
            && _hintButton.GetComponent<Image>().color.a != 0.5f
        )
        {
            _hintButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.5f
            );
        }

        UpdateHintButtonText();
    }

    public void UpdateHintButtonText()
    {
        string translationText = LocalizationManager.Localize("btn-hint");
        if (!_gameManager.SavedGameData.PurchaseData.UnlimitedHints)
        {
            _hintButtonText.text =
                translationText + ": " + _gameManager.SavedGameData.HintsAvailable.ToString();
        }
        else
        {
            _hintButtonText.text = translationText;
        }
    }

    public async void DeleteSaves()
    {
        _gameManager.SavedGameData = new Assets.Scripts.SaveGame.SaveGame();
        /*ISaveClient _client = new CloudSaveClient();
        await SaveService.DeleteData(
            _client,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
        );*/
        ISaveClient _client2 = new PlayerPrefClient();
        await SaveService.DeleteData(
            _client2,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
        );
        FindObjectOfType<GameManager>().ResetSavedGameData();
        _gameManager = FindObjectOfType<GameManager>();
        await CustomAnimation.ButtonClicked(_deleteDataButton.transform);
        QuitApplicationClick();
    }

    public void RestoreGameplayPanel()
    {
        ToggleGameplayElements(true);
        _topBarManager.DeselectAll();
        if (_gameManager.IsGameInProgress() && !_gameManager.HasGameEnded())
        {
            _timer.UnpauseTimer();
            ToggleUndoButton(
                _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
            );
        }
    }

    public async void ShowEndOfGameButton()
    {
        ShowObject(_gameplayEndGamePanel);
        CustomAnimation.GameCompletedButtonSwitch(
            _gameplayInGamePanel.transform,
            _gameplayEndGamePanel.transform
        );
        await CustomAnimation.WaitForAnimation("EndGameButtonSwitch");
        HideObject(_gameplayInGamePanel);
    }

    public void ToggleGameplayElements(bool statusToChangeTo)
    {
        if (statusToChangeTo)
        {
            ShowObject(_gameplayStatsPanel);
            ShowObject(_backgroundsPanel);
            if (_gameManager.HasGameEnded())
            {
                HideObject(_gameplayInGamePanel);
                ShowObject(_gameplayEndGamePanel);
            }
            else
            {
                HideObject(_gameplayEndGamePanel);
                ShowObject(_gameplayInGamePanel);
            }
            ShowObject(_gameTitle);
        }
        else
        {
            HideObject(_backgroundsPanel);
            HideObject(_gameplayStatsPanel);
            HideObject(_gameplayInGamePanel);
            HideObject(_gameplayEndGamePanel);
            HideObject(_gameTitle);
        }
        _gameNodes.SetActive(statusToChangeTo);
    }

    public void ToggleSettingsPanel()
    {
        if (_settingsPanel.activeSelf)
        {
            _topBarManager.DeselectSettingsButton();
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
            _topBarManager.SelectSettingsButton();
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
            if (_supportPanel.activeSelf)
            {
                HideObject(_supportPanel);
            }
            if (_gameManager.IsGameInProgress())
            {
                _timer.PauseTimer();
            }
            ShowObject(_settingsPanel);
            FindObjectOfType<SettingsHandler>().LoadSettingsButtons();
        }
    }

    public void ToggleSupportPanel()
    {
        if (_supportPanel.activeSelf)
        {
            _topBarManager.DeselectSettingsButton();
            HideObject(_supportPanel);
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
        { /*
            _topBarManager.SelectSettingsButton();
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
            FindObjectOfType<SettingsHandler>().LoadSettingsButtons();*/
        }
    }

    public void ToggleProfilePanel()
    {
        if (_profilePanel.activeSelf)
        {
            _topBarManager.DeselectProfileButton();
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
            _topBarManager.SelectProfileButton();
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
            if (_supportPanel.activeSelf)
            {
                HideObject(_supportPanel);
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
            _topBarManager.DeselectShopButton();
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
            _topBarManager.SelectShopButton();
            HideObject(_mainMenuPanel);
            HideClassicMenu();
            ToggleGameplayElements(false);
            if (_settingsPanel.activeSelf)
            {
                HideObject(_settingsPanel);
            }
            if (_supportPanel.activeSelf)
            {
                HideObject(_supportPanel);
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

    public void QuitApplicationClick()
    {
        if (_gameManager.IsGameInProgress())
        {
            _gameManager.ResetBoard(true, false, true);
        }

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void InteractionPerformed(Constants.AudioClip audioClip)
    {
        FindObjectOfType<Clouds>().MoveClouds();
        if (audioClip != Constants.AudioClip.NoClip)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySFX(audioManager.GetAudioClip(audioClip));
        }
    }
}
