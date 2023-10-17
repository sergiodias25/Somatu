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
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    private List<int> _indexesUsedForStartingPosition = new();
    private List<int> _indexesUsedForSolution = new();
    private List<int> _solutionNumbers = new();
    private List<Node> _allNodes = new List<Node>();
    private Block firstRowSolutionBlock;
    private Block secondRowSolutionBlock;
    private Block thirdRowSolutionBlock;
    private Block firstColumnSolutionBlock;
    private Block secondColumnSolutionBlock;
    private Block thirdColumnSolutionBlock;
    private int[] currentLevel;

    void Start()
    {
        currentLevel = Constants.numbersForLvl1;
        GenerateGrid(currentLevel);
    }

    public int[] GetCurrentLevel() {
        return currentLevel;
    }

    private int GenerateNumber(int[] numbers)
    {
        bool needsRandom = true;
        int randomized = -1;
        
        while (needsRandom == true) {
            randomized = UnityEngine.Random.Range(0, 9);
            if (!_indexesUsedForStartingPosition.Contains(randomized)) {
                needsRandom = false;
                _indexesUsedForStartingPosition.Add(randomized);
            }
        }
        needsRandom = true;
        return numbers[randomized];  
    }

    private void GenerateSolutionNumber(int[] numbers)
    {
        bool needsRandom = true;
        while (needsRandom == true) {
            int randomized = UnityEngine.Random.Range(0, 9);
            if (!_indexesUsedForSolution.Contains(randomized)) {
                needsRandom = false;
                _solutionNumbers.Add(numbers[randomized]);
                _indexesUsedForSolution.Add(randomized);
            }
        }
    }

    public void GenerateGrid(int[] numbers) {
        currentLevel = numbers;
        for (int i = 0; i < _width; i++) {
            for (int j = 1; j < _height + 1; j++) {
                var node = Instantiate(_nodePrefab, new Vector2(i, j), Quaternion.identity);
                var generatedNumber = GenerateNumber(numbers);
                Block generatedBLock = SpawnBlock(node, generatedNumber, true);
                GenerateSolutionNumber(numbers);
                node.SetName(i, j);
                node.SetBlockInNode(generatedBLock);
                _allNodes.Add(node);
            }
        }
        
        var center = new Vector2((float) (_width + 1) /2 - 0.5f,(float) (_height + 1) / 2 -0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        firstRowSolutionBlock = GenerateSolutionBlock(3, 3, GetSolutionFirstRowSum());
        secondRowSolutionBlock = GenerateSolutionBlock(3, 2, GetSolutionSecondRowSum());
        thirdRowSolutionBlock = GenerateSolutionBlock(3, 1, GetSolutionThirdRowSum());
        firstColumnSolutionBlock = GenerateSolutionBlock(0, 0, GetSolutionFirstColumnSum());
        secondColumnSolutionBlock = GenerateSolutionBlock(1, 0, GetSolutionSecondColumnSum());
        thirdColumnSolutionBlock = GenerateSolutionBlock(2, 0, GetSolutionThirdColumnSum());
        CheckResult();
        LogSolution();
    }

    private Block GenerateSolutionBlock(int x, int y, int numberValue)
    {
        var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
        Block generatedBLock = SpawnBlock(node, numberValue, false);
        node.SetName(x, y);
        node.SetBlockInNode(generatedBLock);
        return generatedBLock;
    }

    Block SpawnBlock(Node node, int value, bool interactible) {
        var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
        return block.Init(value, interactible, node);
    }

    public int GetFirstRowSum() {
        return _allNodes[2].GetBlockInNode().Value + _allNodes[5].GetBlockInNode().Value + _allNodes[8].GetBlockInNode().Value;
    }
    public int GetSecondRowSum() {
        return _allNodes[1].GetBlockInNode().Value + _allNodes[4].GetBlockInNode().Value + _allNodes[7].GetBlockInNode().Value;
    }
    public int GetThirdRowSum() {
        return _allNodes[0].GetBlockInNode().Value + _allNodes[3].GetBlockInNode().Value + _allNodes[6].GetBlockInNode().Value;
    }
    public int GetSolutionFirstRowSum() {
        return _solutionNumbers[2] + _solutionNumbers[5] + _solutionNumbers[8];
    }
    public int GetSolutionSecondRowSum() {
        return _solutionNumbers[1] + _solutionNumbers[4] + _solutionNumbers[7];
    }
    public int GetSolutionThirdRowSum() {
        return _solutionNumbers[0] + _solutionNumbers[3] + _solutionNumbers[6];
    }

    public int GetFirstColumnSum() {
        return _allNodes[2].GetBlockInNode().Value + _allNodes[1].GetBlockInNode().Value + _allNodes[0].GetBlockInNode().Value;
    }
    public int GetSecondColumnSum() {
        return _allNodes[3].GetBlockInNode().Value + _allNodes[4].GetBlockInNode().Value + _allNodes[5].GetBlockInNode().Value;
    }
    public int GetThirdColumnSum() {
        return _allNodes[6].GetBlockInNode().Value + _allNodes[7].GetBlockInNode().Value + _allNodes[8].GetBlockInNode().Value;
    }
    public int GetSolutionFirstColumnSum() {
        return _solutionNumbers[0] + _solutionNumbers[1] + _solutionNumbers[2];
    }
    public int GetSolutionSecondColumnSum() {
        return _solutionNumbers[3] + _solutionNumbers[4] + _solutionNumbers[5];
    }
    public int GetSolutionThirdColumnSum() {
        return _solutionNumbers[6] + _solutionNumbers[7] + _solutionNumbers[8];
    }

    internal void CheckResult()
    {
        bool firstRowCompleted = CheckLineOrColumnResult(GetFirstRowSum(), GetSolutionFirstRowSum(), firstRowSolutionBlock);
        bool secondRowCompleted = CheckLineOrColumnResult(GetSecondRowSum(), GetSolutionSecondRowSum(), secondRowSolutionBlock);
        bool thirdRowCompleted = CheckLineOrColumnResult(GetThirdRowSum(), GetSolutionThirdRowSum(), thirdRowSolutionBlock);
        bool firstColumnCompleted = CheckLineOrColumnResult(GetFirstColumnSum(), GetSolutionFirstColumnSum(), firstColumnSolutionBlock);
        bool secondColumnCompleted = CheckLineOrColumnResult(GetSecondColumnSum(), GetSolutionSecondColumnSum(), secondColumnSolutionBlock);
        bool thirdColumnCompleted = CheckLineOrColumnResult(GetThirdColumnSum(), GetSolutionThirdColumnSum(), thirdColumnSolutionBlock);

        if (firstRowCompleted && secondRowCompleted && thirdRowCompleted &&
            firstColumnCompleted && secondColumnCompleted && thirdColumnCompleted) {
            foreach (var node in _allNodes)
            {
                node.GetBlockInNode().DisableInteraction();
                node.GetBlockInNode()._sprite.color = Constants.successBackgroundColor;
            }
            FindObjectOfType<RestartButton>().ActivateRestartButton();
        }
    }

    private bool CheckLineOrColumnResult(int currentSum, int expectedResult, Block block) {
        if (currentSum == expectedResult)
        {
            block._sprite.color = Constants.successBackgroundColor;
            return true;
        }
        else
        {
            block._sprite.color = Constants.inProgressBackgroundColor;
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
        DestroyBlock(firstRowSolutionBlock);
        DestroyBlock(secondRowSolutionBlock);
        DestroyBlock(thirdRowSolutionBlock);
        DestroyBlock(firstColumnSolutionBlock);
        DestroyBlock(secondColumnSolutionBlock);
        DestroyBlock(thirdColumnSolutionBlock);
        _allNodes = new List<Node>();
        _indexesUsedForStartingPosition = new();
        _indexesUsedForSolution = new();
        _solutionNumbers = new();
        _allNodes = new List<Node>();
    }

    private void DestroyBlock(Block block) {
        Destroy(block.gameObject);
        Destroy(block.GetNode().gameObject);
    }
}
