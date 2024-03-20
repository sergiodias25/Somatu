using UnityEngine;
using UnityEngine.UI;

public class ShowLockImageInButton : MonoBehaviour
{
    private enum DifficultyValue
    {
        Unlocked = 0,
        Moderate = 1,
        Hard = 2,
        Extreme = 3,
        Challenge = 4
    }

    [SerializeField]
    private DifficultyValue value;

    [SerializeField]
    private Image image;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (value != DifficultyValue.Unlocked)
        {
            switch (value)
            {
                case DifficultyValue.Moderate:
                    if (
                        !_gameManager.SavedGameData.IsDifficultyUnlocked(
                            Constants.Difficulty.Medium
                        )
                    )
                    {
                        image.enabled = true;
                    }
                    break;
                case DifficultyValue.Hard:
                    if (!_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Hard))
                    {
                        image.enabled = true;
                    }
                    break;
                case DifficultyValue.Extreme:
                    if (
                        !_gameManager.SavedGameData.IsDifficultyUnlocked(
                            Constants.Difficulty.Extreme
                        )
                    )
                    {
                        image.enabled = true;
                    }
                    break;
                case DifficultyValue.Challenge:
                    if (
                        !_gameManager.SavedGameData.IsDifficultyUnlocked(
                            Constants.Difficulty.Challenge
                        )
                    )
                    {
                        image.enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
