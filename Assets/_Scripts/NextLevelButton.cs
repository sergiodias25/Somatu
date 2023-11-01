using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private SpriteRenderer _button;

    private void Start()
    {
        HideRestartButton();
    }

    public void ActivateRestartButton()
    {
        ShowButton();
    }

    private void OnMouseDown()
    {
        if (enabled)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            int[] nextLevel = CalculateNextLevel(gameManager);
            gameManager.ResetBoard();
            gameManager.GenerateGrid(nextLevel);
            HideRestartButton();
        }
    }

    public void HideRestartButton()
    {
        _text.enabled = false;
        _text.alpha = 0f;
        enabled = false;
        _button.color = Constants.NextLevelButtonDisabled;
    }

    private void ShowButton()
    {
        _text.alpha = 1f;
        _text.enabled = true;
        enabled = true;
        _button.color = Constants.NextLevelButtonEnabled;
    }

    private int[] CalculateNextLevel(GameManager gameManager)
    {
        int[] currentLevel = gameManager.GetCurrentLevel();
        if (currentLevel == Constants.NumbersForLvl1)
        {
            return Constants.NumbersForLvl2;
        }
        if (currentLevel == Constants.NumbersForLvl2)
        {
            return Constants.NumbersForLvl3;
        }
        if (currentLevel == Constants.NumbersForLvl3)
        {
            return Constants.NumbersForLvl4;
        }
        if (currentLevel == Constants.NumbersForLvl4)
        {
            return Constants.NumbersForLvl5;
        }
        if (currentLevel == Constants.NumbersForLvl5)
        {
            return Constants.NumbersForLvl6;
        }
        if (currentLevel == Constants.NumbersForLvl6)
        {
            return Constants.NumbersForLvl7;
        }
        if (currentLevel == Constants.NumbersForLvl7)
        {
            return Constants.NumbersForLvl8;
        }
        return Constants.NumbersForLvl1;
    }
}
