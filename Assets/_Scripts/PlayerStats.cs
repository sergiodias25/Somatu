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
    int _easyGamesPlayed = 0;
    int _easyGamesCompleted = 0;
    double _easyTimeFastest = 0.0;
    double _easyTimeAverage = 0.0;
    int _easyHelpsUsed = 0;

    public void StartedGame(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _easyGamesPlayed++;
                break;
            case Difficulty.Médio:
            case Difficulty.Difícil:
            case Difficulty.Extremo:
            case Difficulty.Desafio:
                break;
        }
        UpdateValues();
    }

    public void CompletedGame(Difficulty difficulty, double timeToComplete)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _easyGamesCompleted++;
                break;
            case Difficulty.Médio:
            case Difficulty.Difícil:
            case Difficulty.Extremo:
            case Difficulty.Desafio:
                break;
        }
        ManageTime(difficulty, timeToComplete);
        UpdateValues();
    }

    public void UsedHelp(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _easyHelpsUsed++;
                break;
            case Difficulty.Médio:
            case Difficulty.Difícil:
            case Difficulty.Extremo:
            case Difficulty.Desafio:
                break;
        }
        UpdateValues();
    }

    private void ManageTime(Difficulty difficulty, double timeToComplete)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                CheckFastestTime(timeToComplete, ref _easyTimeFastest);
                CalculateAverageTime(timeToComplete, ref _easyTimeAverage, _easyGamesCompleted);
                break;
            case Difficulty.Médio:
            case Difficulty.Difícil:
            case Difficulty.Extremo:
            case Difficulty.Desafio:
                break;
        }
    }

    private void CalculateAverageTime(
        double timeToComplete,
        ref double previousTimeAverage,
        int gamesCompleted
    )
    {
        if (previousTimeAverage == 0.0)
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
        _easyGamesPlayedText.text = _easyGamesPlayed.ToString();
        _easyGamesCompletedText.text = _easyGamesCompleted.ToString();
        _easyTimeFastestText.text = Timer.FormatTime(_easyTimeFastest);
        _easyTimeAverageText.text = Timer.FormatTime(_easyTimeAverage);
        _easyHelpsUsedText.text = _easyHelpsUsed.ToString();
    }
}
