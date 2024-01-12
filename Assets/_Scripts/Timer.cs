using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]
    private TextMeshProUGUI _timerText;

    [Header("Settings")]
    private double _currentTime;
    GameManager _gameManager;
    private bool _isCountdown;
    private bool _isRunning;
    private bool _hasLimit;
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
        _hasLimit = isChallenge;
        _currentTime = isChallenge ? _timeLimitValue : timerValue;
    }

    public void Init(bool isChallenge)
    {
        Init();
        _isCountdown = isChallenge;
        _hasLimit = isChallenge;
        _currentTime = isChallenge ? _timeLimitValue : 0f;
    }

    private void Init()
    {
        enabled = true;
        _timerText.color = Color.white;
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
            if (
                _hasLimit
                && (
                    (_isCountdown && _currentTime <= 0.1f)
                    || (!_isCountdown && _currentTime <= _timeLimitValue + 1f)
                )
            )
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
        _timerText.color = Color.red;
        enabled = false;
        _isRunning = false;
        _gameManager.PuzzleFailed();
    }

    public void StopTimer()
    {
        _currentTime = 0f;
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

    public void ToggleTimer()
    {
        if (_isRunning)
        {
            PauseTimer();
        }
        else if (!_gameManager.HasGameEnded())
        {
            UnpauseTimer();
        }
    }

    public void AddPuzzleSolvedBOnus()
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
}
