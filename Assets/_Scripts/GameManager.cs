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
    private static readonly int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private List<int> _numbersUsed = new();
    private List<int> _solutionNumbers = new();
    private List<Node> _allNodes = new List<Node>();

    void Start()
    {
        GenerateGrid();
    }

    private int GenerateNumber()
    {
        bool needsRandom = true;
        int randomized = 0;
        
        while (needsRandom == true) {
            randomized = UnityEngine.Random.Range(1, 10);
            if (!_numbersUsed.Contains(randomized)) {
                needsRandom = false;
                _numbersUsed.Add(randomized);
            }
        }
        return randomized;  
    }

    private void GenerateSolutionNumber()
    {
        bool needsRandom = true;
        while (needsRandom == true) {
            int randomized = UnityEngine.Random.Range(1, 10);
            if (!_solutionNumbers.Contains(randomized)) {
                needsRandom = false;
                _solutionNumbers.Add(randomized);
            }
        }
    }

    void GenerateGrid() {
        for (int i = 0; i < _width; i++) {
            for (int j = 0; j < _height; j++) {
                var node = Instantiate(_nodePrefab, new Vector2(i, j), Quaternion.identity);
                var generatedNumber = GenerateNumber();
                Block generatedBLock = SpawnBlock(node, generatedNumber, true);
                GenerateSolutionNumber();
                node.SetName(i, j);
                node.SetBlockInNode(generatedBLock);
                _allNodes.Add(node);
            }
        }
        
        var center = new Vector2((float) (_width + 1) /2 - 0.5f,(float) (_height + 1) / 2 -0.5f);
        // var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        // board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        GenerateSolutionBlock(3, 2, GetSolutionFirstRowSum());
        GenerateSolutionBlock(3, 1, GetSolutionSecondRowSum());
        GenerateSolutionBlock(3, 0, GetSolutionThirdRowSum());
        GenerateSolutionBlock(0, -1, GetSolutionFirstColumnSum());
        GenerateSolutionBlock(1, -1, GetSolutionSecondColumnSum());
        GenerateSolutionBlock(2, -1, GetSolutionThirdColumnSum());
    }

    private void GenerateSolutionBlock(int x, int y, int numberValue)
    {
        var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
        Block generatedBLock = SpawnBlock(node, numberValue, false);
        node.SetName(x, y);
        node.SetBlockInNode(generatedBLock);
    }

    Block SpawnBlock(Node node, int value, bool interactible) {
        var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
        return block.Init(value, interactible);
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
        if (GetFirstRowSum() == GetSolutionFirstRowSum()) {
            GameObject.Find("Node_3_2").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_3_2").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }
        if (GetSecondRowSum() == GetSolutionSecondRowSum()) {
            GameObject.Find("Node_3_1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_3_1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }
        if (GetThirdRowSum() == GetSolutionThirdRowSum()) {
            GameObject.Find("Node_3_0").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_3_0").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }
        if (GetFirstColumnSum() == GetSolutionFirstColumnSum()) {
            GameObject.Find("Node_0_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_0_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }
        if (GetSecondColumnSum() == GetSolutionSecondColumnSum()) {
            GameObject.Find("Node_1_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_1_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }
        if (GetThirdColumnSum() == GetSolutionThirdColumnSum()) {
            GameObject.Find("Node_2_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(16, 173, 18, 255);
        } else {
            GameObject.Find("Node_2_-1").GetComponent<Node>().GetBlockInNode()._sprite.color = new Color32(245, 245, 159, 255);
        }

        if (GetFirstRowSum() == GetSolutionFirstRowSum() &&
        GetSecondRowSum() == GetSolutionSecondRowSum() &&
        GetThirdRowSum() == GetSolutionThirdRowSum() &&
        GetFirstColumnSum() == GetSolutionFirstColumnSum() &&
        GetSecondColumnSum() == GetSolutionSecondColumnSum() &&
        GetThirdColumnSum() == GetSolutionThirdColumnSum()
        ) {
            EditorUtility.DisplayDialog("Game has ended", "WINNER!", "Fácil!");
        }
    }
}
