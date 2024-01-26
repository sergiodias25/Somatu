using System.Collections.Generic;
using UnityEngine;

public class SaveGame
{
    public Constants.Difficulty? _unlockedDifficulty = Constants.Difficulty.Extreme;
    private int _timesBeatenCurrentDifficulty = 0;
    public int HelpsAvailable = 0;
    public GameInProgress GameInProgressData;
    public ModeStats EasyStats;
    public ModeStats MediumStats;
    public ModeStats HardStats;
    public ModeStats ExtremeStats;
    public ModeStats ChallengeStats;

    public class GameInProgress
    {
        public List<int> GameNumbers { get; private set; }
        public List<int> SolutionNumbers { get; private set; }
        public Constants.Difficulty? Difficulty { get; private set; }
        public double TimerValue { get; private set; }
        public Undo UndoData { get; private set; }

        public GameInProgress()
        {
            GameNumbers = new List<int>();
            SolutionNumbers = new List<int>();
            Difficulty = null;
            UndoData = new Undo();
        }

        public void ClearInProgressSavedGame()
        {
            GameNumbers = new List<int>();
            SolutionNumbers = new List<int>();
            Difficulty = null;
            TimerValue = 0.0;
            UndoData = new Undo();
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
                Node node = generatedNodesObject.transform
                    .GetChild(i)
                    .gameObject.GetComponent<Node>();
                if (GameNumbers.Count == 9 && GameNumbers[i] != node.GetBlockInNode().Value)
                {
                    GameNumbers[i] = node.GetBlockInNode().Value;
                }
                else if (GameNumbers.Count == i)
                {
                    GameNumbers.Add(node.GetBlockInNode().Value);
                }
                if (SolutionNumbers.Count == 9 && SolutionNumbers[i] != solutionNumbers[i])
                {
                    SolutionNumbers[i] = solutionNumbers[i];
                }
                else if (SolutionNumbers.Count == i)
                {
                    SolutionNumbers.Add(solutionNumbers[i]);
                }
            }

            Difficulty = difficulty;
            TimerValue = timerValue;
        }

        public class Undo
        {
            public List<string> FirstNodes;
            public List<string> SecondNodes;

            public Undo()
            {
                FirstNodes = new List<string>();
                SecondNodes = new List<string>();
            }

            public void ClearMoveUndone()
            {
                if (ThereIsDataToUndo())
                {
                    FirstNodes.RemoveAt(FirstNodes.Count - 1);
                    SecondNodes.RemoveAt(SecondNodes.Count - 1);
                }
            }

            public void ClearUndoData()
            {
                if (ThereIsDataToUndo())
                {
                    FirstNodes.Clear();
                    SecondNodes.Clear();
                }
            }

            public bool ThereIsDataToUndo()
            {
                return FirstNodes.Count > 0 && SecondNodes.Count > 0;
            }

            internal void StoreMoveToUndo(string firstNode, string secondNode)
            {
                if (!ThereIsDataToUndo())
                {
                    FirstNodes = new List<string>();
                    SecondNodes = new List<string>();
                }
                FirstNodes.Add(firstNode);
                SecondNodes.Add(secondNode);
            }
        }
    }

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

    public SaveGame()
    {
        GameInProgressData = new GameInProgress();
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

    public bool IsHalfwayThroughCurrentDifficulty(Constants.Difficulty difficulty)
    {
        if (
            (difficulty < _unlockedDifficulty)
            || (
                _timesBeatenCurrentDifficulty
                >= (Constants.GetNumberOfSolvesToUnlockNextDifficulty(difficulty) / 2)
            )
        )
        {
            return true;
        }
        return false;
    }
}
