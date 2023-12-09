using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _width = 3;

    private int _height = 3;

    [SerializeField]
    private Node _nodePrefab;

    [SerializeField]
    private Block _blockPrefab;

    [SerializeField]
    private SpriteRenderer _boardPrefab;

    [SerializeField]
    private TextMeshProUGUI _timesSolved;

    [SerializeField]
    private TextMeshProUGUI _modeSelected;

    [SerializeField]
    private TextMeshProUGUI _correctBlocksCount;
    private Timer _timer;
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

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
        _audioManager.PlayMusic();
        var center = new Vector2((float)(_width + 1) / 2 - 0.5f, (float)(_height + 3.2) / 2 - 0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
    }

    public void Init(Constants.Difficulty selectedDifficulty)
    {
        SelectedDifficulty = selectedDifficulty;
        _timesSolved.text = "0";
        _modeSelected.text = SelectedDifficulty.ToString();

        GenerateGrid(
            GenerateNumbersForLevel(
                Constants.GetNumbers(SelectedDifficulty),
                Constants.GetRepeatedNumbersCount(SelectedDifficulty)
            )
        );
        _timer.Init(SelectedDifficulty == Constants.Difficulty.Desafio);
        ApplyDifficultySettings(SelectedDifficulty);
    }

    public static List<int> GenerateNumbersForLevel(List<int> possibleValues, int repeatedCount)
    {
        List<int> currentPossibleValues = new List<int>(possibleValues);
        List<int> result = new();
        int randomized = UnityEngine.Random.Range(0, currentPossibleValues.Count);
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
            int nextNumber = UnityEngine.Random.Range(0, currentPossibleValues.Count);
            result.Add(currentPossibleValues[nextNumber]);
            currentPossibleValues.RemoveAt(nextNumber);
        }
        return result;
    }

    public static Color ChangeAlpha(Color originalColor, float newAlpha)
    {
        var newColor = originalColor;
        newColor.a = newAlpha;
        return newColor;
    }

    private void ApplyDifficultySettings(Constants.Difficulty selectedDifficulty)
    {
        if (selectedDifficulty == Constants.Difficulty.Extremo) { }
        if (selectedDifficulty >= Constants.Difficulty.Difícil) { }
        if (selectedDifficulty >= Constants.Difficulty.Médio)
        {
            TextMeshProUGUI _correctCountLabel = GameObject
                .Find("CorrectCountLabel")
                .GetComponent<TextMeshProUGUI>();
            _correctCountLabel.color = ChangeAlpha(_correctCountLabel.color, 0f);

            TextMeshProUGUI _correctCountText = GameObject
                .Find("CorrectCount")
                .GetComponent<TextMeshProUGUI>();
            _correctCountText.color = ChangeAlpha(_correctCountText.color, 0f);
        }
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

    public void GenerateGrid(List<int> numbers)
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 1; j < _height + 1; j++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(i, j), Quaternion.identity);
                var generatedNumber = GenerateNumber(numbers);
                Block generatedBLock = SpawnBlock(node, generatedNumber, true);
                GenerateSolutionNumber(numbers);
                node.Init(i, j);
                node.SetBlockInNode(generatedBLock);
                node.transform.SetParent(GameObject.Find("GeneratedNodes").transform);
                _allNodes.Add(node);
            }
        }

        _firstRowResultBlock = GenerateResultBlock(3, 3, GetSolutionGroupSum(2, 5, 8));
        _secondRowResultBlock = GenerateResultBlock(3, 2, GetSolutionGroupSum(1, 4, 7));
        _thirdRowResultBlock = GenerateResultBlock(3, 1, GetSolutionGroupSum(0, 3, 6));
        _firstColumnResultBlock = GenerateResultBlock(0, 0, GetSolutionGroupSum(0, 1, 2));
        _secondColumnResultBlock = GenerateResultBlock(1, 0, GetSolutionGroupSum(3, 4, 5));
        _thirdColumnResultBlock = GenerateResultBlock(2, 0, GetSolutionGroupSum(6, 7, 8));
        if (CheckResult(false))
        {
            ResetBoard(false);
            GenerateGrid(numbers);
        }
        LogSolution();
    }

    private Block GenerateResultBlock(int x, int y, int numberValue)
    {
        var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
        Block generatedBLock = SpawnBlock(node, numberValue, false);
        node.Init(x, y);
        node.SetBlockInNode(generatedBLock);
        node.transform.SetParent(GameObject.Find("SolutionNodes").transform);
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

        int _correctCount = 0;
        for (int i = 0; i < _allNodes.Count; i++)
        {
            if (_allNodes[i].GetBlockInNode().Value == _solutionNumbers[i])
            {
                _correctCount += 1;
            }
        }

        if (SelectedDifficulty < Constants.Difficulty.Médio)
        {
            int _previousCorrectCount = int.Parse(_correctBlocksCount.text);
            _correctBlocksCount.text = _correctCount.ToString();
            if (!isActionable || _previousCorrectCount == _correctCount)
            {
                _correctBlocksCount.color = Color.white;
            }
            else if (_previousCorrectCount > _correctCount)
            {
                _correctBlocksCount.color = Color.red;
            }
            else if (_previousCorrectCount < _correctCount)
            {
                _correctBlocksCount.color = Color.cyan;
            }

            if (_correctCount == _allNodes.Count)
            {
                _correctBlocksCount.color = Color.green;
            }
        }

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
                if (SelectedDifficulty == Constants.Difficulty.Desafio)
                {
                    ResetBoard(false);
                    GenerateGrid(
                        GenerateNumbersForLevel(
                            Constants.GetNumbers(SelectedDifficulty),
                            Constants.GetRepeatedNumbersCount(SelectedDifficulty)
                        )
                    );
                }
                _timesSolved.text = (int.Parse(_timesSolved.text) + 1).ToString();
            }
            return true;
        }
        return false;
    }

    private void DoEndGameActions()
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().DisableInteraction();
            node.GetBlockInNode().UpdateColor(Constants.CorrectSumColor);
        }
        _firstRowResultBlock.UpdateColor(Constants.CorrectSumColor);
        _secondRowResultBlock.UpdateColor(Constants.CorrectSumColor);
        _thirdRowResultBlock.UpdateColor(Constants.CorrectSumColor);
        _firstColumnResultBlock.UpdateColor(Constants.CorrectSumColor);
        _secondColumnResultBlock.UpdateColor(Constants.CorrectSumColor);
        _thirdColumnResultBlock.UpdateColor(Constants.CorrectSumColor);
        if (SelectedDifficulty == Constants.Difficulty.Desafio)
        {
            _timer.AddPuzzleSolvedBOnus();
        }
        else
        {
            _timer.PauseTimer();
        }
        FindObjectOfType<UIManager>().ShowGameplayButtons();
        _audioManager.PlaySFX(_audioManager.PuzzleSolved);
    }

    public void PuzzleFailed()
    {
        foreach (var node in _allNodes)
        {
            node.GetBlockInNode().DisableInteraction();
            node.GetBlockInNode().UpdateColor(Constants.IncorrectSumColor);
        }
        _audioManager.PlaySFX(_audioManager.PuzzleSolved);
    }

    private bool CheckLineOrColumnResult(int currentSum, int expectedResult, Block block)
    {
        if (SelectedDifficulty < Constants.Difficulty.Extremo)
        {
            if (currentSum == expectedResult)
            {
                block.UpdateColor(Constants.CorrectSumColor);
            }
            else
            {
                block.UpdateColor(Constants.IncorrectSumColor);
            }
        }
        else
        {
            block.UpdateColor(Constants.InProgressBackgroundColor);
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

    internal void ResetBoard(bool isExit)
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
        _allNodes = new List<Node>();
        _indexesUsedForStartingPosition = new();
        _indexesUsedForSolution = new();
        _solutionNumbers = new();
        _allNodes = new List<Node>();
        if (SelectedDifficulty < Constants.Difficulty.Médio)
        {
            _correctBlocksCount.color = Constants.TextColor;
        }
        if (isExit)
        {
            _timer.StopTimer();
            _timesSolved.text = "0";
        }
    }

    public void ResetTimesSolved()
    {
        _timesSolved.text = "0";
    }

    public void ShowHints()
    {
        for (int i = 0; i < _solutionNumbers.Count; i++)
        {
            if (_solutionNumbers[i] == _allNodes[i].GetBlockInNode().Value)
            {
                _allNodes[i].UpdateColor(Constants.CorrectSumColor);
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
}
