using System.Collections.Generic;
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

    public struct UndoMoveData
    {
        public List<Node> firstNodes;
        public List<Node> secondNodes;

        public bool IsUndoEnabled()
        {
            return ThereIsDataToUndo();
        }

        public void ClearMoveUndone()
        {
            if (ThereIsDataToUndo())
            {
                firstNodes.RemoveAt(firstNodes.Count - 1);
                secondNodes.RemoveAt(secondNodes.Count - 1);
            }
        }

        public void ClearUndoData()
        {
            firstNodes.Clear();
            secondNodes.Clear();
        }

        public bool ThereIsDataToUndo()
        {
            return firstNodes != null
                && firstNodes.Count > 0
                && secondNodes != null
                && secondNodes.Count > 0;
        }

        internal void StoreMoveToUndo(Node firstNode, Node secondNode)
        {
            if (!ThereIsDataToUndo())
            {
                firstNodes = new List<Node>();
                secondNodes = new List<Node>();
            }
            firstNodes.Add(firstNode);
            secondNodes.Add(secondNode);
        }
    }

    public SavedGameData _savedGameData;
    public UndoMoveData _undoMoveData;
    private PlayerStats _playerStats;

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
        _uiManager = FindObjectOfType<UIManager>();
        _animationsHandler = FindObjectOfType<AnimationsHandler>();
        _audioManager.PlayMusic();
        var center = new Vector2((float)(_width + 1) / 2 - 0.5f, (float)(_height + 3.2) / 2 - 0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        _savedGameData = new SavedGameData();
        _playerStats = FindObjectOfType<PlayerStats>();
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
        _modeSelected.text = SelectedDifficulty.ToString();

        GenerateGrid(
            GenerateNumbersForLevel(
                Constants.GetNumbers(ActualDifficulty),
                Constants.GetRepeatedNumbersCount(ActualDifficulty)
            ),
            loadGame
        );
        _timer.Init(
            SelectedDifficulty == Constants.Difficulty.Desafio,
            _savedGameData != null ? _savedGameData._timerValue : 0d
        );
    }

    public Constants.Difficulty GetActualDifficulty()
    {
        if (SelectedDifficulty != Constants.Difficulty.Desafio)
        {
            return SelectedDifficulty;
        }
        int _timesBeaten = int.Parse(_timesSolvedText.text);
        if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Fácil)
        )
        {
            return Constants.Difficulty.Fácil;
        }
        else if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Médio)
        )
        {
            return Constants.Difficulty.Médio;
        }
        else if (
            _timesBeaten
            < Constants.GetNumberOfSolvesToProgressInChallenge(Constants.Difficulty.Difícil)
        )
        {
            return Constants.Difficulty.Difícil;
        }
        return Constants.Difficulty.Extremo;
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
                if (loadGame && _savedGameData._gameNumbersInProgress.Count > 0)
                {
                    generatedNumber = _savedGameData._gameNumbersInProgress[_countTracker];
                    _solutionNumbers.Add(_savedGameData._solutionNumbersInProgress[_countTracker]);
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
        if (loadGame && _savedGameData._gameNumbersInProgress.Count > 0)
        {
            SelectedDifficulty = (Constants.Difficulty)_savedGameData._savedGameDifficulty;
            _modeSelected.text = SelectedDifficulty.ToString();
            _timer.SetTimerValue(_savedGameData._timerValue);
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
        if (SelectedDifficulty != Constants.Difficulty.Desafio)
        {
            _savedGameData.UpdateInProgressSavedGame(
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

        _savedGameData.IncrementTimesBeaten(SelectedDifficulty);
        _timesSolvedText.text = (int.Parse(_timesSolvedText.text) + 1).ToString();
        _uiManager.ToggleUndoButton(false);
        _uiManager.ToggleHelpButton(false);
        _undoMoveData.ClearUndoData();

        if (SelectedDifficulty == Constants.Difficulty.Desafio)
        {
            _timer.AddPuzzleSolvedBonus();
        }
        else
        {
            _timer.PauseTimer();
            _savedGameData.ClearInProgressSavedGame();
            _playerStats.CompletedGame(SelectedDifficulty, _timer.GetTimerValue(), -1);
        }
        _uiManager.ShowGameplayButtons();
        _audioManager.PlaySFX(_audioManager.PuzzleSolved);

        if (SelectedDifficulty == Constants.Difficulty.Desafio)
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
            SelectedDifficulty < Constants.Difficulty.Extremo
            || (
                SelectedDifficulty == Constants.Difficulty.Desafio
                && ActualDifficulty != Constants.Difficulty.Extremo
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
                SelectedDifficulty == Constants.Difficulty.Desafio
                && ActualDifficulty == Constants.Difficulty.Extremo
            )
            || SelectedDifficulty == Constants.Difficulty.Extremo
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
        if (_undoMoveData.ThereIsDataToUndo())
        {
            _undoMoveData.ClearUndoData();
        }
        if (shouldClearSavedGame)
        {
            _savedGameData.ClearInProgressSavedGame();
        }
        if (isExit)
        {
            _timer.StopTimer();
            _timesSolvedText.text = "0";
        }
        if (SelectedDifficulty == Constants.Difficulty.Desafio)
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
        return _savedGameData != null
            && _savedGameData._gameNumbersInProgress != null
            && _savedGameData._gameNumbersInProgress.Count == 9;
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
        _undoMoveData.StoreMoveToUndo(firstNode, secondNode);
        _uiManager.ToggleUndoButton(true);
    }

    public void UndoLastMove()
    {
        Block.SwitchNodes(
            _undoMoveData.firstNodes[_undoMoveData.firstNodes.Count - 1],
            _undoMoveData.secondNodes[_undoMoveData.secondNodes.Count - 1]
        );
        _audioManager.PlaySFX(_audioManager.DropBlockUndo);
        _undoMoveData.ClearMoveUndone();
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
