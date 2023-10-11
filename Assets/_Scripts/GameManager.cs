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
    private List<int> _numbersUsed = new List<int>();
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
            }
            _numbersUsed.Add(randomized);
        }
        return randomized;  
    }

    void GenerateGrid() {
        for (int i = 0; i < _width; i++) {
            for (int j = 0; j > _height * -1; j--) {
                var node = Instantiate(_nodePrefab, new Vector2(i, j * -1), Quaternion.identity);
                /*if (i == _width - 1 || j == _height - 1) {
                    break;
                }*/
                Block generatedBLock = SpawnBlock(node, GenerateNumber());
                node.SetName(i, j);
                node.SetBlockInNode(generatedBLock);
                _allNodes.Add(node);
            }
        }
        
        var center = new Vector2((float) _width /2 - 0.5f,(float) _height / 2 -0.5f);
        var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
    }

    Block SpawnBlock(Node node, int value) {
        var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
        return block.Init(value);
    }
   
}
