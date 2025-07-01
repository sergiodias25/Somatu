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
    private GameObject image;
    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = FindObjectOfType<GameManager>();
        ValidateLockDisplay();
    }

    private void ValidateLockDisplay()
    {
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
                        image.SetActive(true);
                        break;
                    }
                    image.SetActive(false);
                    break;
                case DifficultyValue.Hard:
                    if (!_gameManager.SavedGameData.IsDifficultyUnlocked(Constants.Difficulty.Hard))
                    {
                        image.SetActive(true);
                        break;
                    }
                    image.SetActive(false);
                    break;
                case DifficultyValue.Extreme:
                    if (
                        !_gameManager.SavedGameData.IsDifficultyUnlocked(
                            Constants.Difficulty.Extreme
                        )
                    )
                    {
                        image.SetActive(true);
                        break;
                    }
                    image.SetActive(false);
                    break;
                case DifficultyValue.Challenge:
                    if (
                        !_gameManager.SavedGameData.IsDifficultyUnlocked(
                            Constants.Difficulty.Challenge
                        )
                    )
                    {
                        image.SetActive(true);
                        break;
                    }
                    image.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}
