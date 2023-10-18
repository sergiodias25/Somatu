using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]
    private TextMeshProUGUI _timerText;

    [Header("Settings")]
    private double _currentTime;
    private bool _isCountdown;
    private bool _hasLimit;
    private double _timeLimitValue;

    void Start()
    {
        _timeLimitValue = 0f;
        _currentTime = 3f;
        _isCountdown = true;
        _hasLimit = true;
    }

    void Update()
    {
        _currentTime = _isCountdown ? _currentTime -= Time.deltaTime : _currentTime += Time.deltaTime;
        if (_hasLimit && ((_isCountdown && _currentTime <= _timeLimitValue + 1f) || (!_isCountdown && _currentTime <= _timeLimitValue + 1f))) {
            _currentTime = _timeLimitValue;
            UpdateTimerText();
            _timerText.color = Color.red;
            enabled = false;
            // Time limit reached
        }
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int timeInSecondsInt = (int)_currentTime;  //We don't care about fractions of a second, so easy to drop them by just converting to an int
        int minutes = (int)(_currentTime / 60);  //Get total minutes
        int seconds = timeInSecondsInt - (minutes * 60);  //Get seconds for display alongside minutes
        _timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");  //Create the string representation, where both seconds and minutes are at minimum 2 digits
    }
}
