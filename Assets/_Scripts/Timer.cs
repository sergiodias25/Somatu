using Assets.Scripts.CustomAnimation;
using CandyCabinets.Components.Colour;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    GameManager _gameManager;

    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private TextMeshProUGUI _timeRewardText;

    [SerializeField]
    private Image _clockIcon;

    [SerializeField]
    private GameObject _timerGroup;

    [Header("Values")]
    private double _currentTime;
    private double _elapsedTime;
    public double LastElapsedTime;
    private bool _isCountdown;
    private bool _isRunning;
    private double _timeLimitValue;
    private bool _isAnimating = false;
    private double _lastChallengeStartTime;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _isRunning = false;
    }

    public void Init(bool isChallenge, double timerValue)
    {
        Init();
        _isCountdown = isChallenge;
        if (_isCountdown && _isAnimating)
        {
            _isAnimating = false;
        }
        _lastChallengeStartTime = isChallenge ? _timeLimitValue : 0f;
        _currentTime = isChallenge ? _timeLimitValue : timerValue;
    }

    public void Init(bool isChallenge)
    {
        Init();
        _isCountdown = isChallenge;
        _currentTime = isChallenge ? _timeLimitValue : 0f;
        _lastChallengeStartTime = isChallenge ? _timeLimitValue : 0f;
    }

    private void Init()
    {
        enabled = true;
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_LIGHT_TEXT
        ];
        _clockIcon.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_LIGHT_TEXT
        ];
        _timerGroup.transform.DOScale(Vector3.one, .1f);
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
            UpdateTimerText();
            if (_isCountdown && _currentTime <= 0.0f)
            {
                HandleTimerExpired();
            }

            if (
                !_isAnimating
                && _isCountdown
                && _isRunning
                && _currentTime < Constants.ChallengeAnimatedTimeThreshold
            )
            {
                _isAnimating = true;
                AnimateTimerRunningOut();
            }
            if (
                _isAnimating
                && _isCountdown
                && _isRunning
                && _currentTime > Constants.ChallengeAnimatedTimeThreshold
            )
            {
                StopAnimatingTimerRunningOut();
            }
        }
    }

    private void AnimateTimerRunningOut()
    {
        AnimateTimerColor();
        AnimateTimerGroupScale();
    }

    private void StopAnimatingTimerRunningOut()
    {
        _isAnimating = false;
        FindObjectOfType<AudioManager>().StopSFX();
        DOTween.Kill("AnimateTimerSize");
        DOTween.Kill("AnimateTimerColor");
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED];
        _clockIcon.color = ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED];
        _timerGroup.transform.DOScale(Vector3.one, .1f);
    }

    private void AnimateTimerGroupScale()
    {
        if (_isAnimating && _isCountdown && _isRunning)
        {
            _timerGroup.transform
                .DOScale(Vector3.one, .5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    AudioManager audioManager = FindObjectOfType<AudioManager>();
                    audioManager.PlaySFX(Constants.AudioClip.TimerTicking);
                    _timerGroup.transform
                        .DOScale(new Vector3(1.25f, 1.25f, 1.25f), .5f)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            AnimateTimerGroupScale();
                        });
                })
                .SetId("AnimateTimerSize");
        }
    }

    private void AnimateTimerColor()
    {
        if (_isAnimating && _isCountdown && _isRunning)
        {
            _timerText
                .DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT],
                    0.5f
                )
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _timerText
                        .DOColor(
                            ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED],
                            0.5f
                        )
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            AnimateTimerColor();
                        });
                })
                .SetId("AnimateTimerColor");

            _clockIcon
                .DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT],
                    0.5f
                )
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _clockIcon
                        .DOColor(
                            ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED],
                            0.5f
                        )
                        .SetUpdate(true);
                })
                .SetId("AnimateTimerColor");
        }
    }

    private void HandleTimerExpired()
    {
        _currentTime = 0.0f;
        StopAnimatingTimerRunningOut();
        UpdateTimerText();
        _timerText.rectTransform.localScale = Vector3.one;
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED];
        _clockIcon.color = ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_RED];
        enabled = false;
        _isRunning = false;
        _gameManager.PuzzleFailed(_elapsedTime);
        _timerText.text = FormatTime(_elapsedTime);
        LastElapsedTime = _elapsedTime;
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
        if (_isAnimating && _isCountdown)
        {
            FindObjectOfType<AudioManager>().StopSFX();
            _isAnimating = false;
        }
    }

    public void UnpauseTimer()
    {
        enabled = true;
        _isRunning = true;
    }

    public void AddPuzzleSolvedBonus(Constants.Difficulty actualDifficulty)
    {
        PauseTimer();
        double _elapsedTimeInSolvedPuzzle = _lastChallengeStartTime - _currentTime;
        _timeRewardText.gameObject.SetActive(true);
        Vector3 originalPosition = _timeRewardText.transform.localPosition;
        int timeGained = GetTimeBonus(_elapsedTimeInSolvedPuzzle, actualDifficulty);
        _timeRewardText.text = "+" + timeGained.ToString() + "s";

        CustomAnimation
            .AnimateTimeReward(
                _timeRewardText.transform,
                _timerGroup.transform,
                _clockIcon,
                _timerText
            )
            .OnComplete(() =>
            {
                _clockIcon.DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT],
                    0.15f
                );
                _timerText.DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT],
                    0.15f
                );

                _timeRewardText.gameObject.SetActive(false);
                _timeRewardText.transform.localScale = new Vector3(0f, 0f, 0f);
                _timeRewardText.rectTransform.anchoredPosition = originalPosition;

                if (_gameManager.IsGameInProgress())
                {
                    _currentTime += timeGained;
                    _lastChallengeStartTime = _currentTime;
                    UnpauseTimer();
                }
            });
    }

    private int GetTimeBonus(
        double elapsedTimeInSolvedPuzzle,
        Constants.Difficulty actualDifficulty
    )
    {
        int timeGainedBase = 0;
        int timeGainedExtra = 0;
        int extraCalc;
        switch (actualDifficulty)
        {
            case Constants.Difficulty.Easy:
                timeGainedBase = (int)(Constants.ChallengeBonusThresholdTimeEasy / 2);
                extraCalc = (int)(
                    Constants.ChallengeBonusThresholdTimeEasy - elapsedTimeInSolvedPuzzle
                );
                timeGainedExtra = extraCalc > 0 ? extraCalc : 0;
                break;
            case Constants.Difficulty.Medium:
                timeGainedBase = (int)(Constants.ChallengeBonusThresholdTimeMedium / 2);
                extraCalc = (int)(
                    Constants.ChallengeBonusThresholdTimeMedium - elapsedTimeInSolvedPuzzle
                );
                timeGainedExtra = extraCalc > 0 ? extraCalc : 0;
                break;
            case Constants.Difficulty.Hard:
                timeGainedBase = (int)(Constants.ChallengeBonusThresholdTimeHard / 2);
                extraCalc = (int)(
                    Constants.ChallengeBonusThresholdTimeHard - elapsedTimeInSolvedPuzzle
                );
                timeGainedExtra = extraCalc > 0 ? extraCalc : 0;
                break;
            case Constants.Difficulty.Extreme:
                timeGainedBase = (int)(Constants.ChallengeBonusThresholdTimeExtreme / 2);
                extraCalc = (int)(
                    Constants.ChallengeBonusThresholdTimeExtreme - elapsedTimeInSolvedPuzzle
                );
                timeGainedExtra = extraCalc > 0 ? extraCalc : 0;
                break;
        }
        return timeGainedBase + timeGainedExtra;
    }

    public static string FormatTime(double timeValue)
    {
        int timeInSecondsInt = (int)timeValue; //We don't care about fractions of a second, so easy to drop them by just converting to an int
        int minutes = (int)(timeValue / 60); //Get total minutes
        int seconds = timeInSecondsInt - (minutes * 60); //Get seconds for display alongside minutes
        return minutes.ToString("D2") + ":" + seconds.ToString("D2"); //Create the string representation, where both seconds and minutes are at minimum 2 digits}
    }

    public static string FormatTimeForText(double timeValue)
    {
        int timeInSecondsInt = (int)timeValue; //We don't care about fractions of a second, so easy to drop them by just converting to an int
        int minutes = (int)(timeValue / 60); //Get total minutes
        int seconds = timeInSecondsInt - (minutes * 60); //Get seconds for display alongside minutes
        return minutes.ToString("D2") + "m:" + seconds.ToString("D2") + "s"; //Create the string representation, where both seconds and minutes are at minimum 2 digits}
    }

    public void UpdateTextColor()
    {
        _timerText.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_LIGHT_TEXT
        ];
        _clockIcon.color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_LIGHT_TEXT
        ];
    }
}
