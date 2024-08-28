using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using Assets.Scripts.SaveGame;
using System.Threading.Tasks;
using CandyCabinets.Components.Colour;
using Assets.Scripts.CustomAnimation;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private int _width = 3;

    private int _height = 3;

    [SerializeField]
    private Node _nodePrefab;

    [SerializeField]
    private Block _blockPrefab;

    [SerializeField]
    private SpriteRenderer _gameBackground;

    [SerializeField]
    private TextMeshProUGUI _timesSolvedText;

    [SerializeField]
    private TextMeshProUGUI _modeSelected;

    [SerializeField]
    private GameObject _generatedNodesObject;

    [SerializeField]
    private PlayerStats _playerStats;

    [SerializeField]
    private AdBanner _adBanner;

    [SerializeField]
    private Camera _backgroundCamera;

    private Timer _timer;
    private UIManager _uiManager;
    private List<int> _indexesUsedForStartingPosition = new();
    private List<int> _indexesUsedForSolution = new();
    private List<int> _solutionNumbers = new();
    private List<Node> _allNodes = new List<Node>();
    private Block _firstRowResultBlock;
    private Block _secondRowResultBlock;
    private Block _thirdRowResultBlock;
    private Block _firstColumnResultBlock;
    private Block _secondColumnResultBlock;
    private Block _thirdColumnResultBlock;
    private AudioManager _audioManager;
    public Constants.Difficulty SelectedDifficulty;
    public Constants.Difficulty ActualDifficulty;
    public SaveGame SavedGameData;
    private SettingsHandler _settingsHandler;

    void Start()
    {
        //FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);
        // This way you can subscribe to LocalizationChanged event.
        //LocalizationManager.OnLocalizationChanged += () => FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);

        var center = new Vector2((float)(_width + 1) / 2 - 0.5f, (float)(_height + 3.5) / 2 - 0.5f);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);

        SavedGameData = new SaveGame();
        _audioManager = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
    }

    public void Init(Constants.Difficulty selectedDifficulty)
    {
        Init(selectedDifficulty, false);
        _playerStats.StartedGame(SelectedDifficulty);
    }

    public void Init(Constants.Difficulty selectedDifficulty, bool loadGame)
    {
        SelectedDifficulty = selectedDifficulty;
        ActualDifficulty = GetActualDifficulty();
        _timesSolvedText.text = "0";
        UpdateModeTranslation();
        LocalizationManager.OnLocalizationChanged += () => UpdateModeTranslation();

        if (!loadGame)
        {
            SavedGameData.GameInProgressData.UndoData.ClearUndoData();
            _uiManager.ToggleUndoButton(false);
        }

        GenerateGrid(
            GenerateNumbersForLevel(
                Constants.GetNumbers(ActualDifficulty),
                Constants.GetRepeatedNumbersCount(
                    ActualDifficulty,
                    SavedGameData.IsHalfwayThroughCurrentDifficulty(
                        ActualDifficulty,
                        SelectedDifficulty,
                        int.Parse(_timesSolvedText.text)
                    )
                )
            ),
            loadGame
        );
        _timer.Init(
            SelectedDifficulty == Constants.Difficulty.Challenge,
            SavedGameData != null ? SavedGameData.GameInProgressData.TimerValue : 0d
        );
    }

    private void UpdateModeTranslation()
    {
        _modeSelected.text = LocalizationManager.Localize(
            "mode-" + SelectedDifficulty.ToString().ToLower()
        );
    }

    public Constants.Difficulty GetActualDifficulty()
    {
        if (SelectedDifficulty != Constants.Difficulty.Challenge)
        {
            return SelectedDifficulty;
        }
        int _timesBeaten = int.Parse(_timesSolvedText.text);
        if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Easy)
        )
        {
            return Constants.Difficulty.Easy;
        }
        else if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Medium)
        )
        {
            return Constants.Difficulty.Medium;
        }
        else if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Hard)
        )
        {
            return Constants.Difficulty.Hard;
        }
        return Constants.Difficulty.Extreme;
    }

    public static List<int> GenerateNumbersForLevel(List<int> possibleValues, int repeatedCount)
    {
        List<int> currentPossibleValues = new List<int>(possibleValues);
        List<int> result = new();
        int randomized = Random.Range(0, currentPossibleValues.Count);
        if (repeatedCount > 0)
        {
            for (int i = 0; i < repeatedCount; i++)
            {
                result.Add(currentPossibleValues[randomized]);
            }
            currentPossibleValues.RemoveAt(randomized);
        }

        for (int i = repeatedCount; i < 9; i++)
        {
            int nextNumber = Random.Range(0, currentPossibleValues.Count);
            result.Add(currentPossibleValues[nextNumber]);
            currentPossibleValues.RemoveAt(nextNumber);
        }
        return result;
    }

    private int GenerateNumber(List<int> numbers)
    {
        bool needsRandom = true;
        int randomized = -1;

        while (needsRandom == true)
        {
            randomized = UnityEngine.Random.Range(0, 9);
            if (!_indexesUsedForStartingPosition.Contains(randomized))
            {
                needsRandom = false;
                _indexesUsedForStartingPosition.Add(randomized);
            }
        }
        needsRandom = true;
        return numbers[randomized];
    }

    private void GenerateSolutionNumber(List<int> numbers)
    {
        bool needsRandom = true;
        while (needsRandom == true)
        {
            int randomized = UnityEngine.Random.Range(0, 9);
            if (!_indexesUsedForSolution.Contains(randomized))
            {
                needsRandom = false;
                _solutionNumbers.Add(numbers[randomized]);
                _indexesUsedForSolution.Add(randomized);
            }
        }
    }

    public void GenerateGrid(List<int> numbers, bool loadGame)
    {
        int _countTracker = 0;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 1; j < _height + 1; j++)
            {
                Node node = Node.Init(_nodePrefab, i, j, "GeneratedNodes");
                int generatedNumber;
                if (loadGame && SavedGameData.GameInProgressData.GameNumbers.Count > 0)
                {
                    generatedNumber = SavedGameData.GameInProgressData.GameNumbers[_countTracker];
                    _solutionNumbers.Add(
                        SavedGameData.GameInProgressData.SolutionNumbers[_countTracker]
                    );
                }
                else
                {
                    generatedNumber = GenerateNumber(numbers);
                    GenerateSolutionNumber(numbers);
                }
                Block generatedBLock = SpawnBlock(node, generatedNumber, true);
                node.SetBlockInNode(generatedBLock);
                _allNodes.Add(node);
                _countTracker += 1;
            }
        }
        if (loadGame && SavedGameData.GameInProgressData.GameNumbers.Count > 0)
        {
            SelectedDifficulty = (Constants.Difficulty)SavedGameData.GameInProgressData.Difficulty;
            UpdateModeTranslation();
            _timer.SetTimerValue(SavedGameData.GameInProgressData.TimerValue);
        }

        _firstRowResultBlock = GenerateResultBlock(3, 3, GetSolutionGroupSum(2, 5, 8));
        _secondRowResultBlock = GenerateResultBlock(3, 2, GetSolutionGroupSum(1, 4, 7));
        _thirdRowResultBlock = GenerateResultBlock(3, 1, GetSolutionGroupSum(0, 3, 6));
        _firstColumnResultBlock = GenerateResultBlock(0, 0, GetSolutionGroupSum(0, 1, 2));
        _secondColumnResultBlock = GenerateResultBlock(1, 0, GetSolutionGroupSum(3, 4, 5));
        _thirdColumnResultBlock = GenerateResultBlock(2, 0, GetSolutionGroupSum(6, 7, 8));
        RemoveHints();
        if (CheckResult(false))
        {
            ResetBoard(false, true, false);
            GenerateGrid(numbers, false);
        }
        _uiManager.ToggleHintButton(true);
        LogSolution();
    }

    private Block GenerateResultBlock(int x, int y, int numberValue)
    {
        Node node = Node.Init(_nodePrefab, x, y, "SolutionNodes");
        Block generatedBLock = SpawnBlock(node, numberValue, false);
        node.SetBlockInNode(generatedBLock);
        return generatedBLock;
    }

    Block SpawnBlock(Node node, int value, bool interactible)
    {
        var block = Instantiate(_blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        return block.Init(value, interactible, node);
    }

    private int GetNodesSum(int index1, int index2, int index3)
    {
        return _allNodes[index1].GetBlockInNode().Value
            + _allNodes[index2].GetBlockInNode().Value
            + _allNodes[index3].GetBlockInNode().Value;
    }

    private int GetSolutionGroupSum(int index1, int index2, int index3)
    {
        return _solutionNumbers[index1] + _solutionNumbers[index2] + _solutionNumbers[index3];
    }

    internal bool CheckResult(bool isActionable)
    {
        if (!IsGameInProgress())
        {
            return false;
        }
        bool firstRowCompleted = CheckLineOrColumnResult(
            GetNodesSum(2, 5, 8),
            GetSolutionGroupSum(2, 5, 8),
            _firstRowResultBlock
        );
        bool secondRowCompleted = CheckLineOrColumnResult(
            GetNodesSum(1, 4, 7),
            GetSolutionGroupSum(1, 4, 7),
            _secondRowResultBlock
        );
        bool thirdRowCompleted = CheckLineOrColumnResult(
            GetNodesSum(0, 3, 6),
            GetSolutionGroupSum(0, 3, 6),
            _thirdRowResultBlock
        );
        bool firstColumnCompleted = CheckLineOrColumnResult(
            GetNodesSum(0, 1, 2),
            GetSolutionGroupSum(0, 1, 2),
            _firstColumnResultBlock
        );
        bool secondColumnCompleted = CheckLineOrColumnResult(
            GetNodesSum(3, 4, 5),
            GetSolutionGroupSum(3, 4, 5),
            _secondColumnResultBlock
        );
        bool thirdColumnCompleted = CheckLineOrColumnResult(
            GetNodesSum(6, 7, 8),
            GetSolutionGroupSum(6, 7, 8),
            _thirdColumnResultBlock
        );

        if (
            firstRowCompleted
            && secondRowCompleted
            && thirdRowCompleted
            && firstColumnCompleted
            && secondColumnCompleted
            && thirdColumnCompleted
        )
        {
            if (isActionable)
            {
                Block[] blocksToAnimate = new[]
                {
                    _firstRowResultBlock,
                    _secondRowResultBlock,
                    _thirdRowResultBlock,
                    _firstColumnResultBlock,
                    _secondColumnResultBlock,
                    _thirdColumnResultBlock,
                    _allNodes[0].GetBlockInNode(),
                    _allNodes[1].GetBlockInNode(),
                    _allNodes[2].GetBlockInNode(),
                    _allNodes[3].GetBlockInNode(),
                    _allNodes[4].GetBlockInNode(),
                    _allNodes[5].GetBlockInNode(),
                    _allNodes[6].GetBlockInNode(),
                    _allNodes[7].GetBlockInNode(),
                    _allNodes[8].GetBlockInNode(),
                };
                CustomAnimation.AnimatePuzzleSolved(blocksToAnimate);
                DoEndGameActions();
            }
            return true;
        }
        else
        {
            AnimateSolutionBlocks(
                firstRowCompleted,
                secondRowCompleted,
                thirdRowCompleted,
                firstColumnCompleted,
                secondColumnCompleted,
                thirdColumnCompleted
            );
        }
        if (SelectedDifficulty != Constants.Difficulty.Challenge)
        {
            SavedGameData.UpdateInProgressSavedGame(
                _generatedNodesObject,
                _solutionNumbers,
                SelectedDifficulty,
                _timer.GetTimerValue()
            );
        }

        return false;
    }

    private async void AnimateSolutionBlocks(
        bool firstRowCompleted,
        bool secondRowCompleted,
        bool thirdRowCompleted,
        bool firstColumnCompleted,
        bool secondColumnCompleted,
        bool thirdColumnCompleted
    )
    {
        if (
            SelectedDifficulty < Constants.Difficulty.Extreme
            || (
                SelectedDifficulty == Constants.Difficulty.Challenge
                && ActualDifficulty != Constants.Difficulty.Extreme
            )
        )
        {
            await CustomAnimation.WaitForAnimation("MoveNumberBack");
            if (firstRowCompleted)
            {
                _firstRowResultBlock.AnimatePartialSumCorrect();
            }
            if (secondRowCompleted)
            {
                _secondRowResultBlock.AnimatePartialSumCorrect();
            }
            if (thirdRowCompleted)
            {
                _thirdRowResultBlock.AnimatePartialSumCorrect();
            }
            if (firstColumnCompleted)
            {
                _firstColumnResultBlock.AnimatePartialSumCorrect();
            }
            if (secondColumnCompleted)
            {
                _secondColumnResultBlock.AnimatePartialSumCorrect();
            }
            if (thirdColumnCompleted)
            {
                _thirdColumnResultBlock.AnimatePartialSumCorrect();
            }
        }
    }

    private void DoEndGameActions()
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().ChangeInteraction(false);
            node.UpdateColor(
                ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]
            );
            node.GetBlockInNode().UpdateTextColor();
        }

        _firstRowResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _firstRowResultBlock.UpdateTextColor();
        _secondRowResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _secondRowResultBlock.UpdateTextColor();
        _thirdRowResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _thirdRowResultBlock.UpdateTextColor();
        _firstColumnResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _firstColumnResultBlock.UpdateTextColor();
        _secondColumnResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _secondColumnResultBlock.UpdateTextColor();
        _thirdColumnResultBlock
            .GetNode()
            .UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]);
        _thirdColumnResultBlock.UpdateTextColor();

        SavedGameData.IncrementTimesBeaten(SelectedDifficulty);
        SavedGameData.IncrementHintsAvailable(1);
        _timesSolvedText.text = (int.Parse(_timesSolvedText.text) + 1).ToString();
        _uiManager.ToggleUndoButton(false);
        _uiManager.ToggleHintButton(false);
        SavedGameData.GameInProgressData.UndoData.ClearUndoData();

        if (SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            _timer.AddPuzzleSolvedBonus();
        }
        else
        {
            _timer.PauseTimer();
            SavedGameData.ClearInProgressSavedGame();
            _playerStats.CompletedGame(SelectedDifficulty, _timer.GetTimerValue(), -1);
            SavedGameData.PersistData();
            _uiManager.ShowEndOfGameButton();
        }
        _uiManager.InteractionPerformed(Constants.AudioClip.PuzzleSolved, false);

        if (SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            ResetBoard(false, false, false);
            GenerateGrid(
                GenerateNumbersForLevel(
                    Constants.GetNumbers(ActualDifficulty),
                    Constants.GetRepeatedNumbersCount(
                        ActualDifficulty,
                        SavedGameData.IsHalfwayThroughCurrentDifficulty(
                            ActualDifficulty,
                            SelectedDifficulty,
                            int.Parse(_timesSolvedText.text)
                        )
                    )
                ),
                false
            );
        }
    }

    public void PuzzleFailed(double _elapsedTime)
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().ChangeInteraction(false);
            node.UpdateColor(ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED]);
            node.GetBlockInNode().UpdateTextColor();
        }
        _uiManager.InteractionPerformed(Constants.AudioClip.PuzzleSolved, false);
        _uiManager.ToggleHintButton(false);
        _uiManager.ToggleUndoButton(false);
        _uiManager.ShowEndOfGameButton();
        _playerStats.CompletedGame(
            SelectedDifficulty,
            _elapsedTime,
            int.Parse(_timesSolvedText.text)
        );
        SavedGameData.PersistData();
    }

    private bool CheckLineOrColumnResult(int currentSum, int expectedResult, Block block)
    {
        if (
            SelectedDifficulty < Constants.Difficulty.Extreme
            || (
                SelectedDifficulty == Constants.Difficulty.Challenge
                && ActualDifficulty != Constants.Difficulty.Extreme
            )
        )
        {
            if (currentSum == expectedResult)
            {
                block
                    .GetNode()
                    .UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]
                    );
            }
            else
            {
                block
                    .GetNode()
                    .UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED]
                    );
            }
        }
        else if (
            (
                SelectedDifficulty == Constants.Difficulty.Challenge
                && ActualDifficulty == Constants.Difficulty.Extreme
            )
            || SelectedDifficulty == Constants.Difficulty.Extreme
        )
        {
            block
                .GetNode()
                .UpdateColor(
                    ColourManager.Instance.SelectedPalette().Colours[
                        Constants.COLOR_SOLUTION_NODE_NO_HINT
                    ]
                );
        }

        if (currentSum == expectedResult)
        {
            return true;
        }
        return false;
    }

    private void LogSolution()
    {
        string _solution = "";
        foreach (var solutionNumber in _solutionNumbers)
        {
            _solution += solutionNumber;
        }
        Debug.Log("Solution: " + _solution);
    }

    internal void ResetBoard(
        bool isExit,
        bool shouldClearSavedGame,
        bool resetChallengeActualDifficulty
    )
    {
        DOTween.Kill("NumberJump");
        for (int i = 0; i < _allNodes.Count; i++)
        {
            DestroyBlock(_allNodes[i].GetBlockInNode());
        }
        for (int i = 0; i < _allNodes.Count; i++)
        {
            Destroy(_allNodes[i].gameObject);
        }
        DestroyBlock(_firstRowResultBlock);
        DestroyBlock(_secondRowResultBlock);
        DestroyBlock(_thirdRowResultBlock);
        DestroyBlock(_firstColumnResultBlock);
        DestroyBlock(_secondColumnResultBlock);
        DestroyBlock(_thirdColumnResultBlock);
        _generatedNodesObject.transform.DetachChildren();
        _allNodes = new List<Node>();
        _indexesUsedForStartingPosition = new();
        _indexesUsedForSolution = new();
        _solutionNumbers = new();
        if (!isExit && SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo())
        {
            SavedGameData.GameInProgressData.UndoData.ClearUndoData();
            _uiManager.ToggleUndoButton(false);
        }
        if (shouldClearSavedGame)
        {
            SavedGameData.ClearInProgressSavedGame();
        }
        if (isExit)
        {
            _timer.StopTimer();
            _timesSolvedText.text = "0";
        }
        if (SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            if (resetChallengeActualDifficulty)
            {
                _timesSolvedText.text = "0";
            }
            ActualDifficulty = GetActualDifficulty();
        }
    }

    public void ResetTimesSolved()
    {
        _timesSolvedText.text = "0";
    }

    public bool UseHint()
    {
        ResetSelectedBlock();
        List<Node> correctNodes = new List<Node>();
        for (int i = 0; i < _solutionNumbers.Count; i++)
        {
            if (
                _solutionNumbers[i] == _allNodes[i].GetBlockInNode().Value
                && _allNodes[i].GetBlockInNode().IsInteractable()
            )
            {
                correctNodes.Add(_allNodes[i]);
            }
        }
        if (correctNodes.Count > 0)
        {
            int hintsLimit = Mathf.Min(
                correctNodes.Count,
                Constants.GetNumberOfHintsDisplayed(ActualDifficulty)
            );
            for (int i = 0; i <= hintsLimit - 1; i++)
            {
                int randomNodeIndex = Random.Range(0, correctNodes.Count);
                correctNodes[randomNodeIndex].UpdateColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]
                );
                CustomAnimation.SumIsCorrect(
                    correctNodes[randomNodeIndex].transform,
                    correctNodes[randomNodeIndex].name
                );
                correctNodes[randomNodeIndex].GetBlockInNode().ChangeInteraction(false);
                correctNodes.RemoveAt(randomNodeIndex);
            }
            _uiManager.InteractionPerformed(Constants.AudioClip.DropBlock, false);
            return true;
        }
        else
        {
            for (int i = 0; i < _solutionNumbers.Count; i++)
            {
                if (
                    _solutionNumbers[i] != _allNodes[i].GetBlockInNode().Value
                    && _allNodes[i].GetBlockInNode().IsInteractable()
                )
                {
                    _allNodes[i].UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED]
                    );
                    _ = _allNodes[i].GetBlockInNode().AnimateIncorrectSolution();
                    /*_allNodes[i].UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[
                            Constants.COLOR_NODE_NEUTRAL
                        ]
                    );*/
                }
            }
            _uiManager.InteractionPerformed(Constants.AudioClip.DropBlockUndo, false);
        }
        return false;
    }

    public void RemoveHints()
    {
        Color colorToUpdateTo = HasGameEnded()
            ? ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN]
            : ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_NODE_NEUTRAL];

        bool hasGameEnded = HasGameEnded();
        for (int i = 0; i < _solutionNumbers.Count; i++)
        {
            Color colorToUpdateToExtraValidation =
                !hasGameEnded && _allNodes[i].GetBlockInNode().IsSelected
                    ? ColourManager.Instance.SelectedPalette().Colours[
                        Constants.COLOR_SELECTED_NODE
                    ]
                    : colorToUpdateTo;
            if (_allNodes[i].GetBlockInNode().IsInteractable())
            {
                _allNodes[i].UpdateColor(colorToUpdateToExtraValidation);
                _allNodes[i].GetBlockInNode().UpdateTextColor();
            }
        }
        if (IsGameInProgress())
        {
            _firstRowResultBlock.UpdateTextColor();
            _secondRowResultBlock.UpdateTextColor();
            _thirdRowResultBlock.UpdateTextColor();
            _firstColumnResultBlock.UpdateTextColor();
            _secondColumnResultBlock.UpdateTextColor();
            _thirdColumnResultBlock.UpdateTextColor();
        }

        if (!HasGameEnded())
        {
            _uiManager.ToggleHintButton(true);
        }
    }

    private void DestroyBlock(Block block)
    {
        Destroy(block.gameObject);
        Destroy(block.GetNode().gameObject);
    }

    public Block GetSelectedBlock()
    {
        foreach (Node node in _allNodes)
        {
            if (node.GetBlockInNode().IsSelected)
            {
                return node.GetBlockInNode();
            }
        }
        return null;
    }

    public void ResetSelectedBlock()
    {
        foreach (Node node in _allNodes)
        {
            node.GetBlockInNode().IsSelected = false;
        }
    }

    internal bool SavedGameExists()
    {
        return SavedGameData != null
            && SavedGameData.GameInProgressData.GameNumbers != null
            && SavedGameData.GameInProgressData.GameNumbers.Count == 9;
    }

    public void StoreUndoData(Node firstNode, Node secondNode)
    {
        SavedGameData.GameInProgressData.UndoData.StoreMoveToUndo(firstNode.name, secondNode.name);
        _uiManager.ToggleUndoButton(true);
    }

    public void UndoLastMove()
    {
        if (SavedGameData.GameInProgressData.UndoData.ThereIsDataToUndo())
        {
            _ = Block.SwitchBlocksUndo(
                GameObject
                    .Find(
                        SavedGameData.GameInProgressData.UndoData.FirstNodes[
                            SavedGameData.GameInProgressData.UndoData.FirstNodes.Count - 1
                        ]
                    )
                    .GetComponent<Node>(),
                GameObject
                    .Find(
                        SavedGameData.GameInProgressData.UndoData.SecondNodes[
                            SavedGameData.GameInProgressData.UndoData.SecondNodes.Count - 1
                        ]
                    )
                    .GetComponent<Node>()
            );
            GameObject
                .Find(
                    SavedGameData.GameInProgressData.UndoData.FirstNodes[
                        SavedGameData.GameInProgressData.UndoData.FirstNodes.Count - 1
                    ]
                )
                .GetComponent<Node>()
                .GetBlockInNode()
                .ChangeInteraction(true);
            GameObject
                .Find(
                    SavedGameData.GameInProgressData.UndoData.SecondNodes[
                        SavedGameData.GameInProgressData.UndoData.SecondNodes.Count - 1
                    ]
                )
                .GetComponent<Node>()
                .GetBlockInNode()
                .ChangeInteraction(true);
            _uiManager.InteractionPerformed(Constants.AudioClip.DropBlockUndo, false);
            SavedGameData.GameInProgressData.UndoData.ClearMoveUndone();
            CheckResult(true);
        }
    }

    public bool IsGameInProgress()
    {
        if (_generatedNodesObject.transform.childCount == 9)
        {
            return true;
        }
        return false;
    }

    public bool HasGameEnded()
    {
        foreach (Transform node in _generatedNodesObject.transform)
        {
            if (node.gameObject.GetComponent<Node>().GetBlockInNode().IsInteractable())
            {
                return false;
            }
        }
        return true;
    }

    public async void StartGame(Canvas loadingCanvas)
    {
        Task<SaveGame> load = SaveGame.LoadSaveGame();
        await load;
        SavedGameData = load.Result;

        LocalizationManager.Read();
        LocalizationManager.Language = SavedGameData.SettingsData.LanguageSelected;
        if (!SavedGameData.PurchaseData.RemovedAds)
        {
            _adBanner.ShowBannerAd();
        }

        _uiManager = FindObjectOfType<UIManager>();
        _settingsHandler = FindObjectOfType<SettingsHandler>();

        _settingsHandler.LoadData(this);
        _playerStats.LoadData(this);
        _audioManager.PlayMusic();
        ColourManager.Instance.SelectPalette(SavedGameData.SettingsData.SelectedThemeIndex);
        var boardCenter = new Vector2(
            (float)(_width + 1) / 2 - 0.5f,
            (float)(_height + 2.68) / 2 - 1.02f
        );
        _gameBackground.transform.position = boardCenter;

        _uiManager.ShowMainMenu();
        FindObjectOfType<ColorHelper>().ApplyUpdates();
        _backgroundCamera.enabled = true;
        loadingCanvas.gameObject.SetActive(false);
    }

    /*     private void OnApplicationFocus(bool focusedOn)
        {
            if (!focusedOn)
            {
                Debug.Log("Save by lost focus");
                LastResortSaveGame();
            }
        } */

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Debug.Log("Save by pause");
            LastResortSaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Save by quit");
        LastResortSaveGame();
    }

    private void LastResortSaveGame()
    {
        if (IsGameInProgress())
        {
            if (!HasGameEnded())
            {
                if (SelectedDifficulty != Constants.Difficulty.Challenge)
                {
                    SavedGameData.UpdateInProgressSavedGame(
                        _generatedNodesObject,
                        _solutionNumbers,
                        SelectedDifficulty,
                        _timer.GetTimerValue()
                    );
                }
            }
            else
            {
                if (SavedGameData != null)
                {
                    SavedGameData.ClearInProgressSavedGame();
                }
            }
        }
        SavedGameData.PersistData();
    }

    internal void ResetSavedGameData()
    {
        SavedGameData = new SaveGame();
        SavedGameData.PersistData();
    }
}
