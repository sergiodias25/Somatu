using CandyCabinets.Components.Colour;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    GameManager _gameManager;

    [Header("Component")]
    [SerializeField]
    private TextMeshProUGUI _timerText;

    [Header("Settings")]
    private double _currentTime;
    private double _elapsedTime;
    private bool _isCountdown;
    private bool _isRunning;
    private double _timeLimitValue;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _isRunning = false;
    }

    public void Init(bool isChallenge, double timerValue)
    {
        Init();
        _isCountdown = isChallenge;
        _currentTime = isChallenge ? _timeLimitValue : timerValue;
    }

    public void Init(bool isChallenge)
    {
        Init();
        _isCountdown = isChallenge;
        _currentTime = isChallenge ? _timeLimitValue : 0f;
    }

    private void Init()
    {
        enabled = true;
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_LIGHT_TEXT
        ];
        _timeLimitValue = Constants.ChallengeTimeLimit;
        _isRunning = true;
    }

    public double GetTimerValue()
    {
        return _currentTime;
    }

    public void SetTimerValue(double timerValue)
    {
        _currentTime = timerValue;
    }

    void Update()
    {
        if (_isRunning)
        {
            _currentTime = _isCountdown
                ? _currentTime -= Time.deltaTime
                : _currentTime += Time.deltaTime;
            _elapsedTime += Time.deltaTime;
            if (_isCountdown && _currentTime <= 1.0f)
            {
                HandleTimerExpired();
            }
            UpdateTimerText();
        }
    }

    private void HandleTimerExpired()
    {
        _currentTime = 0.0f;
        UpdateTimerText();
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED];
        enabled = false;
        _isRunning = false;
        _gameManager.PuzzleFailed(_elapsedTime);
        _elapsedTime = 0.0f;
    }

    public void StopTimer()
    {
        _currentTime = 0f;
        _elapsedTime = 0.0f;
        UpdateTimerText();
        enabled = false;
        _isRunning = false;
    }

    public void RestartTimer()
    {
        StopTimer();
        UnpauseTimer();
    }

    private void UpdateTimerText()
    {
        _timerText.text = FormatTime(_currentTime);
    }

    public void PauseTimer()
    {
        enabled = false;
        _isRunning = false;
    }

    public void UnpauseTimer()
    {
        enabled = true;
        _isRunning = true;
    }

    public void AddPuzzleSolvedBonus()
    {
        _currentTime += Constants.ChallengePuzzleSolvedBonus;
    }

    public static string FormatTime(double timeValue)
    {
        int timeInSecondsInt = (int)timeValue; //We don't care about fractions of a second, so easy to drop them by just converting to an int
        int minutes = (int)(timeValue / 60); //Get total minutes
        int seconds = timeInSecondsInt - (minutes * 60); //Get seconds for display alongside minutes
        return minutes.ToString("D2") + ":" + seconds.ToString("D2"); //Create the string representation, where both seconds and minutes are at minimum 2 digits}
    }

    public void UpdateTextColor()
    {
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_DARK_TEXT
        ];
    }
}
