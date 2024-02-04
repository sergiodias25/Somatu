using UnityEngine;
using TMPro;
using static Constants;
using System;
using Assets.Scripts.SaveGame;

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
            case Difficulty.Easy:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.EasyStats
                );
                break;
            case Difficulty.Medium:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.MediumStats
                );
                break;
            case Difficulty.Hard:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.HardStats
                );
                break;
            case Difficulty.Extreme:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.ExtremeStats
                );
                break;
        }
        UpdateValues();
    }

    public void CompletedGame(Difficulty difficulty, double timeToComplete, int solvesCount)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _gameManager.SavedGameData.ManageTime(
                    timeToComplete,
                    ref _gameManager.SavedGameData.EasyStats
                );
                _gameManager.SavedGameData.IncrementGamesCompleted(
                    _gameManager.SavedGameData.EasyStats
                );
                break;
            case Difficulty.Medium:
                _gameManager.SavedGameData.ManageTime(
                    timeToComplete,
                    ref _gameManager.SavedGameData.MediumStats
                );
                _gameManager.SavedGameData.IncrementGamesCompleted(
                    _gameManager.SavedGameData.MediumStats
                );
                break;
            case Difficulty.Hard:
                _gameManager.SavedGameData.ManageTime(
                    timeToComplete,
                    ref _gameManager.SavedGameData.HardStats
                );
                _gameManager.SavedGameData.IncrementGamesCompleted(
                    _gameManager.SavedGameData.HardStats
                );
                break;
            case Difficulty.Extreme:
                _gameManager.SavedGameData.ManageTime(
                    timeToComplete,
                    ref _gameManager.SavedGameData.ExtremeStats
                );
                _gameManager.SavedGameData.IncrementGamesCompleted(
                    _gameManager.SavedGameData.ExtremeStats
                );
                break;
            case Difficulty.Challenge:
                _gameManager.SavedGameData.ManageChallengeSolves(solvesCount);
                _gameManager.SavedGameData.ManageChallengeTime(
                    timeToComplete,
                    ref _gameManager.SavedGameData.ChallengeStats
                );
                _gameManager.SavedGameData.IncrementGamesCompleted(
                    _gameManager.SavedGameData.ChallengeStats
                );
                break;
        }
        UpdateValues();
    }

    public void UsedHelp(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _gameManager.SavedGameData.IncrementHelpsUsed(_gameManager.SavedGameData.EasyStats);
                break;
            case Difficulty.Medium:
                _gameManager.SavedGameData.IncrementHelpsUsed(
                    _gameManager.SavedGameData.MediumStats
                );
                break;
            case Difficulty.Hard:
                _gameManager.SavedGameData.IncrementHelpsUsed(_gameManager.SavedGameData.HardStats);
                break;
            case Difficulty.Extreme:
                _gameManager.SavedGameData.IncrementHelpsUsed(
                    _gameManager.SavedGameData.ExtremeStats
                );
                break;
            case Difficulty.Challenge:
                _gameManager.SavedGameData.IncrementHelpsUsed(
                    _gameManager.SavedGameData.ChallengeStats
                );
                break;
        }
        UpdateValues();
    }

    public void UpdateValues()
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

        _gameManager.SavedGameData.PersistData();
    }
}
