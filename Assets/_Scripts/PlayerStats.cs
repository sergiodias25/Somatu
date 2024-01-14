using UnityEngine;
using TMPro;
using static Constants;
using System;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _easyGamesPlayedText;

    [SerializeField]
    private TextMeshPro _easyGamesCompletedText;

    [SerializeField]
    private TextMeshPro _easyTimeFastestText;

    [SerializeField]
    private TextMeshPro _easyTimeAverageText;

    [SerializeField]
    private TextMeshPro _easyHelpsUsedText;

    [SerializeField]
    private TextMeshPro _mediumGamesPlayedText;

    [SerializeField]
    private TextMeshPro _mediumGamesCompletedText;

    [SerializeField]
    private TextMeshPro _mediumTimeFastestText;

    [SerializeField]
    private TextMeshPro _mediumTimeAverageText;

    [SerializeField]
    private TextMeshPro _mediumHelpsUsedText;

    [SerializeField]
    private TextMeshPro _hardGamesPlayedText;

    [SerializeField]
    private TextMeshPro _hardGamesCompletedText;

    [SerializeField]
    private TextMeshPro _hardTimeFastestText;

    [SerializeField]
    private TextMeshPro _hardTimeAverageText;

    [SerializeField]
    private TextMeshPro _hardHelpsUsedText;

    [SerializeField]
    private TextMeshPro _extremeGamesPlayedText;

    [SerializeField]
    private TextMeshPro _extremeGamesCompletedText;

    [SerializeField]
    private TextMeshPro _extremeTimeFastestText;

    [SerializeField]
    private TextMeshPro _extremeTimeAverageText;

    [SerializeField]
    private TextMeshPro _extremeHelpsUsedText;

    [SerializeField]
    private TextMeshPro _challengeGamesPlayedText;

    [SerializeField]
    private TextMeshPro _challengeSolvesMaximumText;

    [SerializeField]
    private TextMeshPro _challengeSolvesAverageText;

    [SerializeField]
    private TextMeshPro _challengeTimeLongestText;

    [SerializeField]
    private TextMeshPro _challengeTimeAverageText;

    [SerializeField]
    private TextMeshPro _challengeHelpsUsedText;

    GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void StartedGame(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _gameManager.SavedGameData.EasyStats.GamesPlayed++;
                break;
            case Difficulty.Médio:
                _gameManager.SavedGameData.MediumStats.GamesPlayed++;
                break;
            case Difficulty.Difícil:
                _gameManager.SavedGameData.HardStats.GamesPlayed++;
                break;
            case Difficulty.Extremo:
                _gameManager.SavedGameData.ExtremeStats.GamesPlayed++;
                break;
        }
        UpdateValues();
    }

    public void CompletedGame(Difficulty difficulty, double timeToComplete, int solvesCount)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                ManageTime(timeToComplete, ref _gameManager.SavedGameData.EasyStats);
                _gameManager.SavedGameData.EasyStats.GamesCompleted++;
                break;
            case Difficulty.Médio:
                ManageTime(timeToComplete, ref _gameManager.SavedGameData.MediumStats);
                _gameManager.SavedGameData.MediumStats.GamesCompleted++;
                break;
            case Difficulty.Difícil:
                ManageTime(timeToComplete, ref _gameManager.SavedGameData.HardStats);
                _gameManager.SavedGameData.HardStats.GamesCompleted++;
                break;
            case Difficulty.Extremo:
                ManageTime(timeToComplete, ref _gameManager.SavedGameData.ExtremeStats);
                _gameManager.SavedGameData.ExtremeStats.GamesCompleted++;
                break;
            case Difficulty.Desafio:
                ManageChallengeSolves(solvesCount, ref _gameManager.SavedGameData.ChallengeStats);
                ManageChallengeTime(timeToComplete, ref _gameManager.SavedGameData.ChallengeStats);
                _gameManager.SavedGameData.ChallengeStats.GamesCompleted++;
                break;
        }
        UpdateValues();
    }

    private void ManageChallengeSolves(int solvesCount, ref SavedGameData.ModeStats challengeStats)
    {
        if (solvesCount > challengeStats.SolveCountBest)
        {
            challengeStats.SolveCountBest = solvesCount;
        }

        if (challengeStats.GamesCompleted == 0)
        {
            challengeStats.SolveCountAverage = solvesCount;
        }
        else
        {
            challengeStats.SolveCountAverage =
                ((challengeStats.GamesCompleted * challengeStats.SolveCountAverage) + solvesCount)
                / (challengeStats.GamesCompleted + 1);
        }
    }

    public void UsedHelp(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _gameManager.SavedGameData.EasyStats.HelpsUsed++;
                break;
            case Difficulty.Médio:
                _gameManager.SavedGameData.MediumStats.HelpsUsed++;
                break;
            case Difficulty.Difícil:
                _gameManager.SavedGameData.HardStats.HelpsUsed++;
                break;
            case Difficulty.Extremo:
                _gameManager.SavedGameData.ExtremeStats.HelpsUsed++;
                break;
            case Difficulty.Desafio:
                _gameManager.SavedGameData.ChallengeStats.HelpsUsed++;
                break;
        }
        UpdateValues();
    }

    private void ManageTime(double timeToComplete, ref SavedGameData.ModeStats playerStats)
    {
        CheckFastestTime(timeToComplete, ref playerStats.TimeBest);
        CalculateAverageTime(
            timeToComplete,
            ref playerStats.TimeAverage,
            playerStats.GamesCompleted
        );
    }

    private void ManageChallengeTime(double timeToComplete, ref SavedGameData.ModeStats playerStats)
    {
        CheckLongestTime(timeToComplete, ref playerStats.TimeBest);
        CalculateAverageTime(
            timeToComplete,
            ref playerStats.TimeAverage,
            playerStats.GamesCompleted
        );
    }

    private void CheckLongestTime(double timeToComplete, ref double previousBestTime)
    {
        if (previousBestTime == 0.0 || timeToComplete > previousBestTime)
        {
            previousBestTime = timeToComplete;
        }
    }

    private void CalculateAverageTime(
        double timeToComplete,
        ref double previousTimeAverage,
        int gamesCompleted
    )
    {
        if (gamesCompleted == 0)
        {
            previousTimeAverage = timeToComplete;
        }
        else
        {
            previousTimeAverage =
                ((gamesCompleted * previousTimeAverage) + timeToComplete) / (gamesCompleted + 1);
        }
    }

    private void CheckFastestTime(double timeToComplete, ref double previousTimeFastest)
    {
        if (previousTimeFastest == 0.0 || timeToComplete < previousTimeFastest)
        {
            previousTimeFastest = timeToComplete;
        }
    }

    private void UpdateValues()
    {
        _easyGamesPlayedText.text = _gameManager.SavedGameData.EasyStats.GamesPlayed.ToString();
        _easyGamesCompletedText.text =
            _gameManager.SavedGameData.EasyStats.GamesCompleted.ToString();
        _easyTimeFastestText.text = Timer.FormatTime(_gameManager.SavedGameData.EasyStats.TimeBest);
        _easyTimeAverageText.text = Timer.FormatTime(
            _gameManager.SavedGameData.EasyStats.TimeAverage
        );
        _easyHelpsUsedText.text = _gameManager.SavedGameData.EasyStats.HelpsUsed.ToString();

        _mediumGamesPlayedText.text = _gameManager.SavedGameData.MediumStats.GamesPlayed.ToString();
        _mediumGamesCompletedText.text =
            _gameManager.SavedGameData.MediumStats.GamesCompleted.ToString();
        _mediumTimeFastestText.text = Timer.FormatTime(
            _gameManager.SavedGameData.MediumStats.TimeBest
        );
        _mediumTimeAverageText.text = Timer.FormatTime(
            _gameManager.SavedGameData.MediumStats.TimeAverage
        );
        _mediumHelpsUsedText.text = _gameManager.SavedGameData.MediumStats.HelpsUsed.ToString();

        _hardGamesPlayedText.text = _gameManager.SavedGameData.HardStats.GamesPlayed.ToString();
        _hardGamesCompletedText.text =
            _gameManager.SavedGameData.HardStats.GamesCompleted.ToString();
        _hardTimeFastestText.text = Timer.FormatTime(_gameManager.SavedGameData.HardStats.TimeBest);
        _hardTimeAverageText.text = Timer.FormatTime(
            _gameManager.SavedGameData.HardStats.TimeAverage
        );
        _hardHelpsUsedText.text = _gameManager.SavedGameData.HardStats.HelpsUsed.ToString();

        _extremeGamesPlayedText.text =
            _gameManager.SavedGameData.ExtremeStats.GamesPlayed.ToString();
        _extremeGamesCompletedText.text =
            _gameManager.SavedGameData.ExtremeStats.GamesCompleted.ToString();
        _extremeTimeFastestText.text = Timer.FormatTime(
            _gameManager.SavedGameData.ExtremeStats.TimeBest
        );
        _extremeTimeAverageText.text = Timer.FormatTime(
            _gameManager.SavedGameData.ExtremeStats.TimeAverage
        );
        _extremeHelpsUsedText.text = _gameManager.SavedGameData.ExtremeStats.HelpsUsed.ToString();

        _challengeGamesPlayedText.text =
            _gameManager.SavedGameData.ChallengeStats.GamesCompleted.ToString();
        _challengeSolvesMaximumText.text =
            _gameManager.SavedGameData.ChallengeStats.SolveCountBest.ToString();
        _challengeSolvesAverageText.text = Math.Round(
                _gameManager.SavedGameData.ChallengeStats.SolveCountAverage,
                2
            )
            .ToString();
        _challengeTimeAverageText.text = Timer.FormatTime(
            _gameManager.SavedGameData.ChallengeStats.TimeAverage
        );
        _challengeTimeLongestText.text = Timer.FormatTime(
            _gameManager.SavedGameData.ChallengeStats.TimeBest
        );
        _challengeHelpsUsedText.text =
            _gameManager.SavedGameData.ChallengeStats.HelpsUsed.ToString();
    }
}
