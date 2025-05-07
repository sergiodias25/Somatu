using UnityEngine;
using TMPro;
using static Constants;
using System;
using Assets.Scripts.AnalyticsEvent;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _easyGamesPlayedText;

    [SerializeField]
    private TextMeshProUGUI _easyGamesCompletedText;

    [SerializeField]
    private TextMeshProUGUI _easyTimeFastestText;

    [SerializeField]
    private TextMeshProUGUI _easyTimeAverageText;

    [SerializeField]
    private TextMeshProUGUI _easyHelpsUsedText;

    [SerializeField]
    private TextMeshProUGUI _mediumGamesPlayedText;

    [SerializeField]
    private TextMeshProUGUI _mediumGamesCompletedText;

    [SerializeField]
    private TextMeshProUGUI _mediumTimeFastestText;

    [SerializeField]
    private TextMeshProUGUI _mediumTimeAverageText;

    [SerializeField]
    private TextMeshProUGUI _mediumHelpsUsedText;

    [SerializeField]
    private TextMeshProUGUI _hardGamesPlayedText;

    [SerializeField]
    private TextMeshProUGUI _hardGamesCompletedText;

    [SerializeField]
    private TextMeshProUGUI _hardTimeFastestText;

    [SerializeField]
    private TextMeshProUGUI _hardTimeAverageText;

    [SerializeField]
    private TextMeshProUGUI _hardHelpsUsedText;

    [SerializeField]
    private TextMeshProUGUI _extremeGamesPlayedText;

    [SerializeField]
    private TextMeshProUGUI _extremeGamesCompletedText;

    [SerializeField]
    private TextMeshProUGUI _extremeTimeFastestText;

    [SerializeField]
    private TextMeshProUGUI _extremeTimeAverageText;

    [SerializeField]
    private TextMeshProUGUI _extremeHelpsUsedText;

    [SerializeField]
    private TextMeshProUGUI _challengeGamesPlayedText;

    [SerializeField]
    private TextMeshProUGUI _challengeSolvesMaximumText;

    [SerializeField]
    private TextMeshProUGUI _challengeSolvesAverageText;

    [SerializeField]
    private TextMeshProUGUI _challengeTimeLongestText;

    [SerializeField]
    private TextMeshProUGUI _challengeTimeAverageText;

    [SerializeField]
    private TextMeshProUGUI _challengeHelpsUsedText;

    GameManager _gameManager;

    public void LoadData(GameManager gameManager)
    {
        _gameManager = gameManager;
        UpdateValues();
    }

    public void StartedGame(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.EasyStats
                );
                ClassicPlayed.SendAnalyticsEvent(difficulty.ToString());
                break;
            case Difficulty.Medium:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.MediumStats
                );
                ClassicPlayed.SendAnalyticsEvent(difficulty.ToString());
                break;
            case Difficulty.Hard:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.HardStats
                );
                ClassicPlayed.SendAnalyticsEvent(difficulty.ToString());
                break;
            case Difficulty.Extreme:
                _gameManager.SavedGameData.IncrementGamesPlayed(
                    _gameManager.SavedGameData.ExtremeStats
                );
                ClassicPlayed.SendAnalyticsEvent(difficulty.ToString());
                break;
            case Difficulty.Challenge:
                ChallengePlayed.SendAnalyticsEvent();
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
                GameFinished.SendAnalyticsEvent(
                    difficulty.ToString(),
                    Math.Round(timeToComplete, 2)
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
                GameFinished.SendAnalyticsEvent(
                    difficulty.ToString(),
                    Math.Round(timeToComplete, 2)
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
                GameFinished.SendAnalyticsEvent(
                    difficulty.ToString(),
                    Math.Round(timeToComplete, 2)
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
                GameFinished.SendAnalyticsEvent(
                    difficulty.ToString(),
                    Math.Round(timeToComplete, 2)
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
                GameFinished.SendAnalyticsEvent(
                    difficulty.ToString(),
                    Math.Round(timeToComplete, 2),
                    solvesCount
                );
                break;
        }
        UpdateValues();
    }

    public void UsedHint(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _gameManager.SavedGameData.IncrementHintsUsedClassic(
                    _gameManager.SavedGameData.EasyStats
                );
                break;
            case Difficulty.Medium:
                _gameManager.SavedGameData.IncrementHintsUsedClassic(
                    _gameManager.SavedGameData.MediumStats
                );
                break;
            case Difficulty.Hard:
                _gameManager.SavedGameData.IncrementHintsUsedClassic(
                    _gameManager.SavedGameData.HardStats
                );
                break;
            case Difficulty.Extreme:
                _gameManager.SavedGameData.IncrementHintsUsedClassic(
                    _gameManager.SavedGameData.ExtremeStats
                );
                break;
            case Difficulty.Challenge:
                _gameManager.SavedGameData.IncrementHintsUsedChallenge(
                    _gameManager.SavedGameData.ChallengeStats
                );
                break;
        }
        HintUsed.SendAnalyticsEvent(difficulty.ToString());
        UpdateValues();
    }

    public void UpdateValues()
    {
        _easyGamesPlayedText.text = _gameManager.SavedGameData.EasyStats.GamesPlayed.ToString();
        _easyGamesCompletedText.text =
            _gameManager.SavedGameData.EasyStats.GamesCompleted.ToString();
        _easyTimeFastestText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.EasyStats.TimeBest
        );
        _easyTimeAverageText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.EasyStats.TimeAverage
        );
        _easyHelpsUsedText.text = _gameManager.SavedGameData.EasyStats.HintsUsed.ToString();

        _mediumGamesPlayedText.text = _gameManager.SavedGameData.MediumStats.GamesPlayed.ToString();
        _mediumGamesCompletedText.text =
            _gameManager.SavedGameData.MediumStats.GamesCompleted.ToString();
        _mediumTimeFastestText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.MediumStats.TimeBest
        );
        _mediumTimeAverageText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.MediumStats.TimeAverage
        );
        _mediumHelpsUsedText.text = _gameManager.SavedGameData.MediumStats.HintsUsed.ToString();

        _hardGamesPlayedText.text = _gameManager.SavedGameData.HardStats.GamesPlayed.ToString();
        _hardGamesCompletedText.text =
            _gameManager.SavedGameData.HardStats.GamesCompleted.ToString();
        _hardTimeFastestText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.HardStats.TimeBest
        );
        _hardTimeAverageText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.HardStats.TimeAverage
        );
        _hardHelpsUsedText.text = _gameManager.SavedGameData.HardStats.HintsUsed.ToString();

        _extremeGamesPlayedText.text =
            _gameManager.SavedGameData.ExtremeStats.GamesPlayed.ToString();
        _extremeGamesCompletedText.text =
            _gameManager.SavedGameData.ExtremeStats.GamesCompleted.ToString();
        _extremeTimeFastestText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.ExtremeStats.TimeBest
        );
        _extremeTimeAverageText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.ExtremeStats.TimeAverage
        );
        _extremeHelpsUsedText.text = _gameManager.SavedGameData.ExtremeStats.HintsUsed.ToString();

        _challengeGamesPlayedText.text =
            _gameManager.SavedGameData.ChallengeStats.GamesCompleted.ToString();
        if (_gameManager.SavedGameData.ChallengeStats.GamesCompleted == 0)
        {
            _challengeSolvesMaximumText.text = "0";
        }
        else if (
            _gameManager.SavedGameData.ChallengeStats.GamesCompleted > 0
            && _gameManager.SavedGameData.ChallengeStats.SolveCountBest == 0
        )
        {
            _challengeSolvesMaximumText.text = "1";
        }
        else
        {
            _challengeSolvesMaximumText.text = (
                _gameManager.SavedGameData.ChallengeStats.SolveCountBest + 1
            ).ToString();
        }
        _challengeSolvesAverageText.text = Math.Round(
                _gameManager.SavedGameData.ChallengeStats.SolveCountAverage,
                2
            )
            .ToString();
        _challengeTimeAverageText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.ChallengeStats.TimeAverage
        );
        _challengeTimeLongestText.text = Timer.FormatTimeForText(
            _gameManager.SavedGameData.ChallengeStats.TimeBest
        );
        _challengeHelpsUsedText.text =
            _gameManager.SavedGameData.ChallengeStats.HintsUsed.ToString();

        //_gameManager.SavedGameData.PersistData();
    }
}
