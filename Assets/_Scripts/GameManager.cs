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

    void Start()
    {
        var center = new Vector2((float)(_width + 1) / 2 - 0.5f, (float)(_height + 1.5) / 2 - 0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        _timesSolved.text = "0";
        _modeSelected.text = Constants.GameDifficulty.ToString();
        GenerateGrid(
            GenerateNumbersForLevel(Constants.GetNumbers(), Constants.GetRepeatedNumbersCount())
        );

        ApplyDifficultySettings();

        if (CheckResult(false))
        {
            ResetBoard();
            GenerateGrid(
                GenerateNumbersForLevel(Constants.GetNumbers(), Constants.GetRepeatedNumbersCount())
            );
        }
        ;
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

    private void ApplyDifficultySettings()
    {
        if (Constants.GameDifficulty == Constants.Difficulty.Insane) { }
        if (Constants.GameDifficulty >= Constants.Difficulty.Hard) { }
        if (Constants.GameDifficulty >= Constants.Difficulty.Medium)
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

        _firstRowResultBlock = GenerateResultBlock(3, 3, GetSolutionFirstRowSum());
        _secondRowResultBlock = GenerateResultBlock(3, 2, GetSolutionSecondRowSum());
        _thirdRowResultBlock = GenerateResultBlock(3, 1, GetSolutionThirdRowSum());
        _firstColumnResultBlock = GenerateResultBlock(0, 0, GetSolutionFirstColumnSum());
        _secondColumnResultBlock = GenerateResultBlock(1, 0, GetSolutionSecondColumnSum());
        _thirdColumnResultBlock = GenerateResultBlock(2, 0, GetSolutionThirdColumnSum());
        CheckResult(false);
        FindObjectOfType<Timer>().UnpauseTimer();
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

    public int GetFirstRowSum()
    {
        return _allNodes[2].GetBlockInNode().Value
            + _allNodes[5].GetBlockInNode().Value
            + _allNodes[8].GetBlockInNode().Value;
    }

    public int GetSecondRowSum()
    {
        return _allNodes[1].GetBlockInNode().Value
            + _allNodes[4].GetBlockInNode().Value
            + _allNodes[7].GetBlockInNode().Value;
    }

    public int GetThirdRowSum()
    {
        return _allNodes[0].GetBlockInNode().Value
            + _allNodes[3].GetBlockInNode().Value
            + _allNodes[6].GetBlockInNode().Value;
    }

    public int GetSolutionFirstRowSum()
    {
        return _solutionNumbers[2] + _solutionNumbers[5] + _solutionNumbers[8];
    }

    public int GetSolutionSecondRowSum()
    {
        return _solutionNumbers[1] + _solutionNumbers[4] + _solutionNumbers[7];
    }

    public int GetSolutionThirdRowSum()
    {
        return _solutionNumbers[0] + _solutionNumbers[3] + _solutionNumbers[6];
    }

    public int GetFirstColumnSum()
    {
        return _allNodes[2].GetBlockInNode().Value
            + _allNodes[1].GetBlockInNode().Value
            + _allNodes[0].GetBlockInNode().Value;
    }

    public int GetSecondColumnSum()
    {
        return _allNodes[3].GetBlockInNode().Value
            + _allNodes[4].GetBlockInNode().Value
            + _allNodes[5].GetBlockInNode().Value;
    }

    public int GetThirdColumnSum()
    {
        return _allNodes[6].GetBlockInNode().Value
            + _allNodes[7].GetBlockInNode().Value
            + _allNodes[8].GetBlockInNode().Value;
    }

    public int GetSolutionFirstColumnSum()
    {
        return _solutionNumbers[0] + _solutionNumbers[1] + _solutionNumbers[2];
    }

    public int GetSolutionSecondColumnSum()
    {
        return _solutionNumbers[3] + _solutionNumbers[4] + _solutionNumbers[5];
    }

    public int GetSolutionThirdColumnSum()
    {
        return _solutionNumbers[6] + _solutionNumbers[7] + _solutionNumbers[8];
    }

    internal bool CheckResult(bool isActionable)
    {
        bool firstRowCompleted = CheckLineOrColumnResult(
            GetFirstRowSum(),
            GetSolutionFirstRowSum(),
            _firstRowResultBlock
        );
        bool secondRowCompleted = CheckLineOrColumnResult(
            GetSecondRowSum(),
            GetSolutionSecondRowSum(),
            _secondRowResultBlock
        );
        bool thirdRowCompleted = CheckLineOrColumnResult(
            GetThirdRowSum(),
            GetSolutionThirdRowSum(),
            _thirdRowResultBlock
        );
        bool firstColumnCompleted = CheckLineOrColumnResult(
            GetFirstColumnSum(),
            GetSolutionFirstColumnSum(),
            _firstColumnResultBlock
        );
        bool secondColumnCompleted = CheckLineOrColumnResult(
            GetSecondColumnSum(),
            GetSolutionSecondColumnSum(),
            _secondColumnResultBlock
        );
        bool thirdColumnCompleted = CheckLineOrColumnResult(
            GetThirdColumnSum(),
            GetSolutionThirdColumnSum(),
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

        if (Constants.GameDifficulty < Constants.Difficulty.Medium)
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
        FindObjectOfType<Timer>().PauseTimer();
        FindObjectOfType<RestartButton>().ActivateRestartButton();
        _timesSolved.text = (int.Parse(_timesSolved.text) + 1).ToString();
    }

    private void CompletedFinalLevel()
    {
        FindObjectOfType<Timer>().StopTimer();
        _timesSolved.color = Color.green;
        FindObjectOfType<RestartButton>().HideRestartButton();
    }

    private bool CheckLineOrColumnResult(int currentSum, int expectedResult, Block block)
    {
        if (currentSum == expectedResult)
        {
            if (Constants.GameDifficulty < Constants.Difficulty.Hard)
            {
                block.UpdateColor(Constants.CorrectSumColor);
            }
            return true;
        }
        else
        {
            block.UpdateColor(Constants.IncorrectSumColor);
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

    internal void ResetBoard()
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
        if (Constants.GameDifficulty < Constants.Difficulty.Medium)
        {
            _correctBlocksCount.color = Constants.TextColor;
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
