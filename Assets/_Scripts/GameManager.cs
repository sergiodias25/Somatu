using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _width = 3;

    private int _height = 3;

    [SerializeField]
    private Node _nodePrefab;

    [SerializeField]
    private Block _blockPrefab;

    /*
    [SerializeField]
    private SpriteRenderer _boardPrefab;
    */
    [SerializeField]
    private TextMeshProUGUI _timesSolvedText;

    [SerializeField]
    private TextMeshProUGUI _modeSelected;

    [SerializeField]
    private GameObject _generatedNodesObject;
    private Timer _timer;
    private UIManager _uiManager;
    private AnimationsHandler _animationsHandler;
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
    public SavedGameData SavedGameData;
    private PlayerStats _playerStats;

    void Start()
    {
        LocalizationManager.Read();
        LocalizationManager.Language = "English";
        //FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);
        // This way you can subscribe to LocalizationChanged event.
        //LocalizationManager.OnLocalizationChanged += () => FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);

        var center = new Vector2((float)(_width + 1) / 2 - 0.5f, (float)(_height + 3.2) / 2 - 0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        SavedGameData = new SavedGameData();

        _playerStats = FindObjectOfType<PlayerStats>();
        _audioManager = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
        _uiManager = FindObjectOfType<UIManager>();
        _animationsHandler = FindObjectOfType<AnimationsHandler>();
        _audioManager.PlayMusic();
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
        _modeSelected.text = LocalizationManager.Localize(
            "mode-" + SelectedDifficulty.ToString().ToLower()
        );

        GenerateGrid(
            GenerateNumbersForLevel(
                Constants.GetNumbers(ActualDifficulty),
                Constants.GetRepeatedNumbersCount(ActualDifficulty)
            ),
            loadGame
        );
        _timer.Init(
            SelectedDifficulty == Constants.Difficulty.Challenge,
            SavedGameData != null ? SavedGameData._timerValue : 0d
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
                Node node = Instantiate(_nodePrefab, new Vector2(i, j), Quaternion.identity);
                int generatedNumber;
                if (loadGame && SavedGameData._gameNumbersInProgress.Count > 0)
                {
                    generatedNumber = SavedGameData._gameNumbersInProgress[_countTracker];
                    _solutionNumbers.Add(SavedGameData._solutionNumbersInProgress[_countTracker]);
                }
                else
                {
                    generatedNumber = GenerateNumber(numbers);
                    GenerateSolutionNumber(numbers);
                }
                Block generatedBLock = SpawnBlock(node, generatedNumber, true);
                node.Init(i, j, generatedBLock, "GeneratedNodes");
                _allNodes.Add(node);
                _countTracker += 1;
            }
        }
        if (loadGame && SavedGameData._gameNumbersInProgress.Count > 0)
        {
            SelectedDifficulty = (Constants.Difficulty)SavedGameData._savedGameDifficulty;
            _modeSelected.text = LocalizationManager.Localize(
                "mode-" + SelectedDifficulty.ToString().ToLower()
            );
            _timer.SetTimerValue(SavedGameData._timerValue);
        }

        _firstRowResultBlock = GenerateResultBlock(3, 3, GetSolutionGroupSum(2, 5, 8));
        _secondRowResultBlock = GenerateResultBlock(3, 2, GetSolutionGroupSum(1, 4, 7));
        _thirdRowResultBlock = GenerateResultBlock(3, 1, GetSolutionGroupSum(0, 3, 6));
        _firstColumnResultBlock = GenerateResultBlock(0, 0, GetSolutionGroupSum(0, 1, 2));
        _secondColumnResultBlock = GenerateResultBlock(1, 0, GetSolutionGroupSum(3, 4, 5));
        _thirdColumnResultBlock = GenerateResultBlock(2, 0, GetSolutionGroupSum(6, 7, 8));
        if (CheckResult(false))
        {
            ResetBoard(false, true, false);
            GenerateGrid(numbers, false);
        }
        _uiManager.ToggleHelpButton(true);
        _animationsHandler.RestoreGameplayBar();
        LogSolution();
    }

    private Block GenerateResultBlock(int x, int y, int numberValue)
    {
        var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
        Block generatedBLock = SpawnBlock(node, numberValue, false);
        node.Init(x, y, generatedBLock, "SolutionNodes");
        return generatedBLock;
    }

    Block SpawnBlock(Node node, int value, bool interactible)
    {
        var block = Instantiate(_blockPrefab, node.transform.position, Quaternion.identity);
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
        //FindObjectOfType<AdRewarded>().LoadAd();

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
                DoEndGameActions();
            }
            return true;
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

    private void DoEndGameActions()
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().DisableInteraction();
            node.UpdateColor(Constants.CorrectSumColor);
        }
        _firstRowResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);
        _secondRowResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);
        _thirdRowResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);
        _firstColumnResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);
        _secondColumnResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);
        _thirdColumnResultBlock.GetNode().UpdateColor(Constants.CorrectSumColor);

        SavedGameData.IncrementTimesBeaten(SelectedDifficulty);
        SavedGameData.HelpsAvailable++;
        _timesSolvedText.text = (int.Parse(_timesSolvedText.text) + 1).ToString();
        _uiManager.ToggleUndoButton(false);
        _uiManager.ToggleHelpButton(false);
        SavedGameData.UndoMoveNodesData.ClearUndoData();

        if (SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            _timer.AddPuzzleSolvedBonus();
        }
        else
        {
            _timer.PauseTimer();
            SavedGameData.ClearInProgressSavedGame();
            _playerStats.CompletedGame(SelectedDifficulty, _timer.GetTimerValue(), -1);
        }
        _uiManager.ShowGameplayButtons();
        _audioManager.PlaySFX(_audioManager.PuzzleSolved);

        if (SelectedDifficulty == Constants.Difficulty.Challenge)
        {
            ResetBoard(false, false, false);
            GenerateGrid(
                GenerateNumbersForLevel(
                    Constants.GetNumbers(ActualDifficulty),
                    Constants.GetRepeatedNumbersCount(ActualDifficulty)
                ),
                false
            );
        }
    }

    public void PuzzleFailed(double _elapsedTime)
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().DisableInteraction();
            node.UpdateColor(Constants.IncorrectSumColor);
        }
        _audioManager.PlaySFX(_audioManager.PuzzleSolved);
        _uiManager.ToggleHelpButton(false);
        _playerStats.CompletedGame(
            SelectedDifficulty,
            _elapsedTime,
            int.Parse(_timesSolvedText.text)
        );
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
                block.GetNode().UpdateColor(Constants.CorrectSumColor);
            }
            else
            {
                block.GetNode().UpdateColor(Constants.IncorrectSumColor);
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
            block.GetNode().UpdateColor(Constants.InProgressBackgroundColor);
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
        if (!isExit && SavedGameData.UndoMoveNodesData.ThereIsDataToUndo())
        {
            SavedGameData.UndoMoveNodesData.ClearUndoData();
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

    public void ShowHints()
    {
        for (int i = 0; i < _solutionNumbers.Count; i++)
        {
            if (_solutionNumbers[i] == _allNodes[i].GetBlockInNode().Value)
            {
                _allNodes[i].UpdateColor(Constants.CorrectSumColor);
            }
            else
            {
                _allNodes[i].UpdateColor(Constants.IncorrectSumColor);
            }
        }
    }

    public void RemoveHints()
    {
        for (int i = 0; i < _solutionNumbers.Count; i++)
        {
            _allNodes[i].UpdateColor(Constants.UnselectedBlock);
        }
        _uiManager.ToggleHelpButton(true);
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
            && SavedGameData._gameNumbersInProgress != null
            && SavedGameData._gameNumbersInProgress.Count == 9;
    }

    public void ResetAllBlocksOpacity()
    {
        for (int i = 0; i < _allNodes.Count; i++)
        {
            Block.UpdateOpacity(_allNodes[i].GetBlockInNode(), 1f);
        }
    }

    public void StoreUndoData(Node firstNode, Node secondNode)
    {
        SavedGameData.UndoMoveNodesData.StoreMoveToUndo(firstNode.name, secondNode.name);
        _uiManager.ToggleUndoButton(true);
    }

    public void UndoLastMove()
    {
        Block.SwitchNodes(
            GameObject
                .Find(
                    SavedGameData.UndoMoveNodesData.firstNodes[
                        SavedGameData.UndoMoveNodesData.firstNodes.Count - 1
                    ]
                )
                .GetComponent<Node>(),
            GameObject
                .Find(
                    SavedGameData.UndoMoveNodesData.secondNodes[
                        SavedGameData.UndoMoveNodesData.secondNodes.Count - 1
                    ]
                )
                .GetComponent<Node>()
        );
        _audioManager.PlaySFX(_audioManager.DropBlockUndo);
        SavedGameData.UndoMoveNodesData.ClearMoveUndone();
        CheckResult(true);
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
        if (
            _generatedNodesObject.transform.childCount == 0
            || !_generatedNodesObject.transform
                .GetChild(0)
                .gameObject.GetComponent<Node>()
                .GetBlockInNode()
                .IsInteractable()
        )
        {
            return true;
        }
        return false;
    }
}
