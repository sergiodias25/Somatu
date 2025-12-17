using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.CustomAnimation;
using DG.Tweening;
using CandyCabinets.Components.Colour;

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
    private TextMeshProUGUI _hintRewardText;

    [SerializeField]
    private Button _undoButton;

    [SerializeField]
    private GameObject _gameplayStatsPanel;

    [SerializeField]
    private GameObject _gameplayInGamePanel;

    [SerializeField]
    private GameObject _gameplayEndGamePanel;

    [SerializeField]
    private PlayerStats _playerStats;

    [SerializeField]
    private GameObject _gameNodes;

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
    private GameObject _mainTitle;

    [SerializeField]
    private GameObject _classicTitle;

    [SerializeField]
    private GameObject _gameplayTitle;

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
    private GameObject _achievementsButton;

    [SerializeField]
    private GameObject _creditsButton;

    [SerializeField]
    private GameObject _achievementsButtonInStats;

    [SerializeField]
    private GameObject _rankingButtonInStats;

    [SerializeField]
    private GameObject _modeSelect;

    [SerializeField]
    private GameObject _difficultyLockedPopup;

    [SerializeField]
    private TextMeshProUGUI _difficultyPanelText;

    [SerializeField]
    private GameObject _unlockLevelPopup;

    [SerializeField]
    private TextMeshProUGUI _unlockPanelText;

    [SerializeField]
    private GameObject _quitGamePopup;

    [SerializeField]
    private GameObject _languagePopup;

    [SerializeField]
    private GameObject _challengeFinishedPopup;

    [SerializeField]
    private GameObject _onboardingWelcome;

    [SerializeField]
    private GameObject _onboardingClassicExplanation;

    [SerializeField]
    private GameObject _onboardingClassicHint;

    [SerializeField]
    private GameObject _onboardingClassicUndo;

    [SerializeField]
    private GameObject _onboardingChallenge;

    [SerializeField]
    private GameObject _hintPurchasePopup;

    [SerializeField]
    private GameObject _popupTheme;

    [SerializeField]
    private GameObject _creditsPopup;

    [SerializeField]
    private GameObject _removeBannerPopup;

    [SerializeField]
    private Button _removeBannerPopupCloseButton;
    AudioManager _audioManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
        _topBarManager = FindObjectOfType<TopBarManager>();
        _audioManager = FindObjectOfType<AudioManager>();
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
            if (_languagePopup.activeSelf)
            {
                if (_gameManager.SavedGameData.SettingsData.LanguageChangedOnce)
                {
                    CustomAnimation.PopupUnload(
                        _languagePopup.transform,
                        _languagePopup.transform.Find("Interactible")
                    );
                }
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_creditsPopup.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _creditsPopup.transform,
                    _creditsPopup.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_removeBannerPopup.activeSelf)
            {
                if (_removeBannerPopupCloseButton.isActiveAndEnabled)
                {
                    CustomAnimation.PopupUnload(
                        _removeBannerPopup.transform,
                        _removeBannerPopup.transform.Find("Interactible")
                    );
                    _ = CustomAnimation.ButtonClicked(_removeBannerPopupCloseButton.transform);
                    InteractionPerformed(Constants.AudioClip.Undo);
                }
            }
            else if (_onboardingWelcome.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _onboardingWelcome.transform,
                    _onboardingWelcome.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_onboardingClassicExplanation.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _onboardingClassicExplanation.transform,
                    _onboardingClassicExplanation.transform.Find("Interactible")
                );
                FindObjectOfType<GameManager>().EnableGameplayBlocks();
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_onboardingClassicHint.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _onboardingClassicHint.transform,
                    _onboardingClassicHint.transform.Find("Interactible")
                );
                FindObjectOfType<GameManager>().EnableGameplayBlocks();
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_onboardingClassicUndo.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _onboardingClassicUndo.transform,
                    _onboardingClassicUndo.transform.Find("Interactible")
                );
                FindObjectOfType<GameManager>().EnableGameplayBlocks();
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_onboardingChallenge.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _onboardingChallenge.transform,
                    _onboardingChallenge.transform.Find("Interactible")
                );
                FindObjectOfType<GameManager>().EnableGameplayBlocks();
                FindObjectOfType<GameManager>().EnableGameplayBlocks();
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_quitGamePopup.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _quitGamePopup.transform,
                    _quitGamePopup.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_unlockLevelPopup.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _unlockLevelPopup.transform,
                    _unlockLevelPopup.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_difficultyLockedPopup.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _difficultyLockedPopup.transform,
                    _difficultyLockedPopup.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_popupTheme.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _popupTheme.transform,
                    _popupTheme.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_settingsPanel.activeSelf)
            {
                ToggleSettingsPanel();
            }
            else if (_supportPanel.activeSelf)
            {
                ToggleSupportPanel();
            }
            else if (_profilePanel.activeSelf)
            {
                ToggleProfilePanel();
            }
            else if (_challengeFinishedPopup.activeSelf)
            {
                CustomAnimation.PopupUnload(
                    _challengeFinishedPopup.transform,
                    _challengeFinishedPopup.transform.Find("Interactible")
                );
                InteractionPerformed(Constants.AudioClip.Undo);
            }
            else if (_hintPurchasePopup.activeSelf)
            {
                GameObject.Find("HintPurchasePopup").GetComponent<Popup>().ClosePopupGameplay();
                InteractionPerformed(Constants.AudioClip.Undo);
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
                InteractionPerformed(Constants.AudioClip.Undo);
                ShowObjectWithAnimation(_quitGamePopup);
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
        CustomAnimation.ButtonLoad(_achievementsButton.transform);
        CustomAnimation.ButtonLoad(_creditsButton.transform);
        CustomAnimation.AnimateTitle(_mainTitle.transform);
        HideObject(_settingsPanel);
        HideObject(_supportPanel);
        HideObject(_profilePanel);
        ShowObject(_topPanel);
        HideObject(_classicMenu);
        _topBarManager.SelectHomeButton();
        ShowOnboardingWelcome();
    }

    public async void ShowClassicMenu()
    {
        await CustomAnimation.ButtonClicked(_classicModeButton.transform);
        CustomAnimation.AnimateTitle(_classicTitle.transform);
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

    private void ShowObjectWithAnimation(GameObject gameObject)
    {
        CustomAnimation.PopupLoad(gameObject.GetComponent<RectTransform>());
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
        if (_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Challenge))
        {
            _topBarManager.DeselectHomeButton();
            HideObject(_mainMenuPanel);
            ShowGameplayButtons();
            _gameManager.Init(Constants.Difficulty.Challenge);
        }
        else
        {
            ShowDifficultyPopup(Constants.Difficulty.Challenge, null, null, 0);
        }
    }

    public async void ClickOnLeaderboard()
    {
        await CustomAnimation.ButtonClicked(_rankingButtonInStats.transform);
        Social.ShowLeaderboardUI();
    }

    public async void ClickOnAchievements()
    {
        await CustomAnimation.ButtonClicked(_achievementsButton.transform);
        Social.ShowAchievementsUI();
    }

    public async void ClickOnAchievementsInStats()
    {
        await CustomAnimation.ButtonClicked(_achievementsButtonInStats.transform);
        Social.ShowAchievementsUI();
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
        string popupTextKey = "popup-difficulty-plural";
        if (originalDiff != Constants.Difficulty.Challenge)
        {
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
                popupTextKey = "popup-difficulty-singular";
            }
            _difficultyPanelText.text = LocalizationManager.Localize(
                popupTextKey,
                targetDifficulty,
                previousDifficulty,
                finalSolvesNeededText
            );
        }
        else
        {
            _difficultyPanelText.text = LocalizationManager.Localize("popup-difficulty-challenge");
        }
        ShowObjectWithAnimation(_difficultyLockedPopup);
    }

    public void ShowUnlockPopup(Constants.Difficulty newDifficulty)
    {
        string popuTextKey = "popup-unlock-difficulty";

        _unlockPanelText.text = LocalizationManager.Localize(
            popuTextKey,
            LocalizationManager.Localize("mode-" + newDifficulty.ToString().ToLower())
        );
        ShowObjectWithAnimation(_unlockLevelPopup);
    }

    public void ShowRemoveAdsPopup()
    {
        CustomAnimation.PopupLoad(_removeBannerPopup.transform);
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
        CustomAnimation.AnimateTitle(_gameplayTitle.transform);
        CustomAnimation.AnimateStartGameButtons(
            _gameplayInGamePanel.transform,
            _gameplayEndGamePanel.transform
        );
        HideObject(_gameplayEndGamePanel);
        ShowObject(_gameplayTitle);
        HideObject(_classicMenu);
        ToggleUndoButton(
            _gameManager.SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo()
        );
    }

    public async void PlayAgainClick()
    {
        DOTween.Kill("AnimatePlayAgainButtonCallToAction");
        _playAgainButton.transform.DOScale(1f, 0.01f);
        _playAgainButton
            .GetComponent<Image>()
            .DOColor(
                ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_NODE_NEUTRAL],
                0.01f
            );
        await CustomAnimation.ButtonClicked(_playAgainButton.transform);
        if (_playAgainButton.enabled)
        {
            _gameManager.ResetBoard(false, true, true);
            _gameManager.GenerateGrid(_gameManager.GenerateNumbersMain(0), false);
            _audioManager.UnpauseMusic();
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
            _gameManager.ShowOnboardings();
        }
    }

    public void ChangeModeClick()
    {
        _gameManager.ResetBoard(true, true, false);
        _gameManager.Init((Constants.Difficulty)_gameManager.SavedGameData.UnlockedDifficulty);
        ShowGameplayButtons();
    }

    public void ClickOnHome()
    {
        if (_gameManager.IsGameInProgress())
        {
            _gameManager.CheckResult(false);
            _gameManager.ResetBoard(true, false, true);
        }
        _audioManager.PlayMusic(AudioManager.MusicType.Menu);
        HideClassicMenu();
        HideObject(_gameNodes);
        HideObject(_gameplayStatsPanel);
        HideObject(_gameplayInGamePanel);
        HideObject(_gameplayEndGamePanel);
        HideObject(_profilePanel);
        HideObject(_settingsPanel);
        HideObject(_supportPanel);
        HideObject(_classicMenu);
        HideObject(_backgroundsPanel);
        ShowMainMenu();
    }

    public void HintClick()
    {
        if (_hintButton.enabled)
        {
            _ = CustomAnimation.ButtonClicked(_hintButton.transform);
            if (_gameManager.IsHintAvailable())
            {
                if (_gameManager.UseHint())
                {
                    _playerStats.UsedHint(_gameManager.SelectedDifficulty);
                    UpdateHintButtonText();
                }
            }
            else if (_gameManager.SelectedDifficulty != Constants.Difficulty.Challenge)
            {
                _audioManager.PauseMusic();
                ShowObjectWithAnimation(_hintPurchasePopup);
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
            await CustomAnimation.ButtonClicked(_undoButton.transform, Constants.AudioClip.Undo);
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

    public void SelectThemeClick()
    {
        ShowObjectWithAnimation(_popupTheme);
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
            _undoButton.GetComponent<Shadow>().enabled = true;
        }
        else if (!enabled && _undoButton.GetComponent<Image>().color.a != 0.5f)
        {
            _undoButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.5f
            );
            _undoButton.GetComponent<Shadow>().enabled = false;
        }

        _undoButton.enabled = enabled;
    }

    public void ToggleHintButton()
    {
        ToggleHintButton(_gameManager.IsGameInProgress() && !_gameManager.HasGameEnded());
    }

    internal void ToggleHintButton(bool enabled)
    {
        _hintButton.enabled = enabled && !_gameManager.HasGameEnded();
        if (_gameManager.SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            _hintButton.enabled = _hintButton.enabled && _gameManager.IsHintAvailable();
        }
        UpdateHintButtonText();
        UpdateHintButtonColor(_hintButton.enabled);
    }

    private void UpdateHintButtonColor(bool buttonEnabled)
    {
        Color originalColor = _hintButton.GetComponent<Image>().color;
        if (buttonEnabled && _hintButton.GetComponent<Image>().color.a != 1)
        {
            _hintButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                1
            );
        }
        else if (!buttonEnabled && _hintButton.GetComponent<Image>().color.a != 0.5f)
        {
            _hintButton.GetComponent<Image>().color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.5f
            );
        }
    }

    public void UpdateHintButtonText()
    {
        int HintsAvailable =
            _gameManager.SelectedDifficulty == Constants.Difficulty.Challenge
                ? _gameManager.SavedGameData.HintsAvailableChallenge
                : _gameManager.SavedGameData.HintsAvailableClassic;
        string translationText = LocalizationManager.Localize("btn-hint");
        if (
            _gameManager.SelectedDifficulty == Constants.Difficulty.Challenge
            || (!_gameManager.SavedGameData.PurchaseData.UnlimitedHints && HintsAvailable > 0)
        )
        {
            _hintButtonText.text = translationText + ": " + HintsAvailable.ToString();
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
        InteractionPerformed(Constants.AudioClip.MenuInteraction);
        if (_gameManager.IsGameInProgress() && !_gameManager.HasGameEnded())
        {
            _timer.UnpauseTimer();
            _gameManager.RemoveHints();
            _gameManager.CheckResult(false);
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
            FindObjectOfType<GameManager>().EnableGameplayBlocks();
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
            ShowObject(_gameplayTitle);
        }
        else
        {
            if (_gameManager.IsGameInProgress())
            {
                _audioManager.PauseMusic();
            }
            HideObject(_backgroundsPanel);
            HideObject(_gameplayStatsPanel);
            HideObject(_gameplayInGamePanel);
            HideObject(_gameplayEndGamePanel);
            HideObject(_gameplayTitle);
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
            CustomAnimation.ButtonLoad(_achievementsButtonInStats.transform);
            CustomAnimation.ButtonLoad(_rankingButtonInStats.transform);
            CustomAnimation.ButtonLoad(_modeSelect.transform);

            ShowObject(_profilePanel);
        }
    }

    public void ShowOnboardingClassicExplanation()
    {
        _audioManager.PauseMusic();
        CustomAnimation.PopupLoad(_onboardingClassicExplanation.transform);
    }

    public void ShowOnboardingWelcome()
    {
        if (
            _gameManager.SavedGameData.EasyStats.GamesCompleted == 0
            && !_gameManager.SavedGameData.Onboardings.Welcome
        )
        {
            _audioManager.PauseMusic();
            _gameManager.SavedGameData.Onboardings.Welcome = true;
            CustomAnimation.PopupLoad(_onboardingWelcome.transform);
        }
    }

    public void ShowOnboardingClassicHint()
    {
        _audioManager.PauseMusic();
        CustomAnimation.PopupLoad(_onboardingClassicHint.transform);
    }

    public void ShowOnboardingClassicUndo()
    {
        _audioManager.PauseMusic();
        CustomAnimation.PopupLoad(_onboardingClassicUndo.transform);
    }

    public void ShowOnboardingChallenge()
    {
        _audioManager.PauseMusic();
        CustomAnimation.PopupLoad(_onboardingChallenge.transform);
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
        if (audioClip != Constants.AudioClip.NoClip)
        {
            _audioManager.PlaySFX(audioClip);
        }
    }

    internal void AnimateHintReward()
    {
        CustomAnimation.AnimateHintReward(_hintRewardText.transform, _hintButton);
    }
}
