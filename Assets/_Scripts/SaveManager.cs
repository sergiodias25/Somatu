using System.Collections.Generic;
using UnityEngine;

public class SavedGameData
{
    public List<int> _gameNumbersInProgress;
    public List<int> _solutionNumbersInProgress;
    public Constants.Difficulty? _savedGameDifficulty = null;
    public Constants.Difficulty? _unlockedDifficulty = Constants.Difficulty.Fácil;
    private int _timesBeatenCurrentDifficulty = 0;

    public SavedGameData()
    {
        _gameNumbersInProgress = new List<int>();
        _solutionNumbersInProgress = new List<int>();
    }

    public void IncrementTimesBeaten(Constants.Difficulty difficulty)
    {
        if (difficulty == _unlockedDifficulty)
        {
            _timesBeatenCurrentDifficulty++;
        }
        UnlockNextLevel(difficulty);
    }

    public bool IsDifficultyUnlocked(Constants.Difficulty difficulty)
    {
        return difficulty <= _unlockedDifficulty;
    }

    public void UnlockNextLevel(Constants.Difficulty difficulty)
    {
        if (
            difficulty != Constants.Difficulty.Desafio
            && difficulty >= _unlockedDifficulty
            && _timesBeatenCurrentDifficulty
                >= Constants.GetNumberOfSolvesToUnlockNextDifficulty(difficulty)
        )
        {
            _unlockedDifficulty++;
            _timesBeatenCurrentDifficulty = 0;
        }
    }

    public void ClearInProgressSavedGame()
    {
        _gameNumbersInProgress = new List<int>();
        _solutionNumbersInProgress = new List<int>();
        _savedGameDifficulty = null;
    }

    public void UpdateInProgressSavedGame(
        GameObject generatedNodesObject,
        List<int> solutionNumbers,
        Constants.Difficulty difficulty
    )
    {
        for (int i = 0; i < generatedNodesObject.transform.childCount; i++)
        {
            Node node = generatedNodesObject.transform.GetChild(i).gameObject.GetComponent<Node>();
            if (
                _gameNumbersInProgress.Count == 9
                && _gameNumbersInProgress[i] != node.GetBlockInNode().Value
            )
            {
                _gameNumbersInProgress[i] = node.GetBlockInNode().Value;
            }
            else if (_gameNumbersInProgress.Count == i)
            {
                _gameNumbersInProgress.Add(node.GetBlockInNode().Value);
            }
            if (
                _solutionNumbersInProgress.Count == 9
                && _solutionNumbersInProgress[i] != solutionNumbers[i]
            )
            {
                _solutionNumbersInProgress[i] = solutionNumbers[i];
            }
            else if (_solutionNumbersInProgress.Count == i)
            {
                _solutionNumbersInProgress.Add(solutionNumbers[i]);
            }
        }

        _savedGameDifficulty = difficulty;
    }
}
