using System.Collections.Generic;
using UnityEngine;

public class SavedGameData
{
    public List<int> _gameNumbersInProgress;
    public List<int> _solutionNumbersInProgress;
    public Constants.Difficulty? _savedGameDifficulty = null;
    public double _timerValue;
    public Constants.Difficulty? _unlockedDifficulty = Constants.Difficulty.Challenge;
    private int _timesBeatenCurrentDifficulty = 0;

    public ModeStats EasyStats;
    public ModeStats MediumStats;
    public ModeStats HardStats;
    public ModeStats ExtremeStats;
    public ModeStats ChallengeStats;
    public int HelpsAvailable = 0;

    public class ModeStats
    {
        public int GamesPlayed;
        public int GamesCompleted;
        public double TimeBest;
        public double TimeAverage;
        public int SolveCountBest;
        public double SolveCountAverage;
        public int HelpsUsed;

        public ModeStats()
        {
            GamesPlayed = 0;
            GamesCompleted = 0;
            TimeBest = 0;
            TimeAverage = 0;
            SolveCountBest = 0;
            SolveCountAverage = 0.0;
            HelpsUsed = 0;
        }
    }

    public SavedGameData()
    {
        _gameNumbersInProgress = new List<int>();
        _solutionNumbersInProgress = new List<int>();
        EasyStats = new ModeStats();
        MediumStats = new ModeStats();
        HardStats = new ModeStats();
        ExtremeStats = new ModeStats();
        ChallengeStats = new ModeStats();
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
            difficulty != Constants.Difficulty.Challenge
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
        _timerValue = 0.0;
    }

    public void UpdateInProgressSavedGame(
        GameObject generatedNodesObject,
        List<int> solutionNumbers,
        Constants.Difficulty difficulty,
        double timerValue
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
        _timerValue = timerValue;
    }
}
