using UnityEngine;
using TMPro;
using static Constants;

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
                _gameManager._savedGameData.EasyStats.GamesPlayed++;
                break;
            case Difficulty.Médio:
                _gameManager._savedGameData.MediumStats.GamesPlayed++;
                break;
            case Difficulty.Difícil:
                _gameManager._savedGameData.HardStats.GamesPlayed++;
                break;
            case Difficulty.Extremo:
                _gameManager._savedGameData.ExtremeStats.GamesPlayed++;
                break;
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
                _gameManager._savedGameData.EasyStats.GamesCompleted++;
                ManageTime(timeToComplete, ref _gameManager._savedGameData.EasyStats);
                break;
            case Difficulty.Médio:
                _gameManager._savedGameData.MediumStats.GamesCompleted++;
                ManageTime(timeToComplete, ref _gameManager._savedGameData.MediumStats);
                break;
            case Difficulty.Difícil:
                _gameManager._savedGameData.HardStats.GamesCompleted++;
                ManageTime(timeToComplete, ref _gameManager._savedGameData.HardStats);
                break;
            case Difficulty.Extremo:
                _gameManager._savedGameData.ExtremeStats.GamesCompleted++;
                ManageTime(timeToComplete, ref _gameManager._savedGameData.ExtremeStats);
                break;
            case Difficulty.Desafio:
                break;
        }
        UpdateValues();
    }

    public void UsedHelp(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                _gameManager._savedGameData.EasyStats.HelpsUsed++;
                break;
            case Difficulty.Médio:
                _gameManager._savedGameData.MediumStats.HelpsUsed++;
                break;
            case Difficulty.Difícil:
                _gameManager._savedGameData.HardStats.HelpsUsed++;
                break;
            case Difficulty.Extremo:
                _gameManager._savedGameData.ExtremeStats.HelpsUsed++;
                break;
            case Difficulty.Desafio:
                break;
        }
        UpdateValues();
    }

    private void ManageTime(double timeToComplete, ref SavedGameData.PlayerStats playerStats)
    {
        CheckFastestTime(timeToComplete, ref playerStats.TimeFastest);
        CalculateAverageTime(
            timeToComplete,
            ref playerStats.TimeAverage,
            playerStats.GamesCompleted
        );
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
        _easyGamesPlayedText.text = _gameManager._savedGameData.EasyStats.GamesPlayed.ToString();
        _easyGamesCompletedText.text =
            _gameManager._savedGameData.EasyStats.GamesCompleted.ToString();
        _easyTimeFastestText.text = Timer.FormatTime(
            _gameManager._savedGameData.EasyStats.TimeFastest
        );
        _easyTimeAverageText.text = Timer.FormatTime(
            _gameManager._savedGameData.EasyStats.TimeAverage
        );
        _easyHelpsUsedText.text = _gameManager._savedGameData.EasyStats.HelpsUsed.ToString();

        _mediumGamesPlayedText.text =
            _gameManager._savedGameData.MediumStats.GamesPlayed.ToString();
        _mediumGamesCompletedText.text =
            _gameManager._savedGameData.MediumStats.GamesCompleted.ToString();
        _mediumTimeFastestText.text = Timer.FormatTime(
            _gameManager._savedGameData.MediumStats.TimeFastest
        );
        _mediumTimeAverageText.text = Timer.FormatTime(
            _gameManager._savedGameData.MediumStats.TimeAverage
        );
        _mediumHelpsUsedText.text = _gameManager._savedGameData.MediumStats.HelpsUsed.ToString();

        _hardGamesPlayedText.text = _gameManager._savedGameData.HardStats.GamesPlayed.ToString();
        _hardGamesCompletedText.text =
            _gameManager._savedGameData.HardStats.GamesCompleted.ToString();
        _hardTimeFastestText.text = Timer.FormatTime(
            _gameManager._savedGameData.HardStats.TimeFastest
        );
        _hardTimeAverageText.text = Timer.FormatTime(
            _gameManager._savedGameData.HardStats.TimeAverage
        );
        _hardHelpsUsedText.text = _gameManager._savedGameData.HardStats.HelpsUsed.ToString();

        _extremeGamesPlayedText.text =
            _gameManager._savedGameData.ExtremeStats.GamesPlayed.ToString();
        _extremeGamesCompletedText.text =
            _gameManager._savedGameData.ExtremeStats.GamesCompleted.ToString();
        _extremeTimeFastestText.text = Timer.FormatTime(
            _gameManager._savedGameData.ExtremeStats.TimeFastest
        );
        _extremeTimeAverageText.text = Timer.FormatTime(
            _gameManager._savedGameData.ExtremeStats.TimeAverage
        );
        _extremeHelpsUsedText.text = _gameManager._savedGameData.ExtremeStats.HelpsUsed.ToString();
    }
}
