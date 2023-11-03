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
            gameManager.ResetBoard();
            gameManager.GenerateGrid(
                GameManager.GenerateNumbersForLevel(
                    Constants.GetNumbers(),
                    Constants.GetRepeatedNumbersCount()
                )
            );
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
}
