using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // BOARD NUMBER COMBINATIONS
    public static List<int> NumbersForEasyMode = new List<int> { 1, 2, 3 };
    public static List<int> NumbersForMediumMode = new List<int> { 1, 2, 3, 4, 5 };
    public static List<int> NumbersForHardMode = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
    public static List<int> NumbersForExtremeMode = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public static int GetRepeatedNumbersCount(
        Difficulty selectedDifficulty,
        bool isHalfwayThroughCurrentDifficulty
    )
    {
        switch (selectedDifficulty)
        {
            case Difficulty.Easy:
                // repeats 7 or 8 numbers
                if (isHalfwayThroughCurrentDifficulty)
                {
                    return 7;
                }
                return 8;
            case Difficulty.Medium:
                // repeats 5 or 6 numbers
                if (isHalfwayThroughCurrentDifficulty)
                {
                    return 5;
                }
                return 6;
            case Difficulty.Hard:
                // repeats betweeen 3 to 4 numbers
                if (isHalfwayThroughCurrentDifficulty)
                {
                    return 3;
                }
                return 4;
            case Difficulty.Extreme:
                // repeats betweeen 0 to 1 number
                if (isHalfwayThroughCurrentDifficulty)
                {
                    return 0;
                }
                return 2;
        }
        return Random.Range(0, 10);
    }

    // GAME SETTINGS
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme,
        Challenge
    };

    public enum ControlMethod
    {
        Drag,
        DoubleClick
    }

    public static ControlMethod SelectedControlMethod = ControlMethod.Drag;

    // 1 second does not count
    public static double ChallengeTimeLimit = 5.0f;
    public static double ChallengePuzzleSolvedBonus = 3.0f;

    public static List<int> GetNumbers(Difficulty selectedDifficulty)
    {
        switch (selectedDifficulty)
        {
            case Difficulty.Easy:
                return NumbersForEasyMode;
            case Difficulty.Medium:
                return NumbersForMediumMode;
            case Difficulty.Hard:
                return NumbersForHardMode;
            case Difficulty.Extreme:
                return NumbersForExtremeMode;
        }
        return NumbersForExtremeMode;
    }

    public static int GetNumberOfSolvesToUnlockNextDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 2;
            case Difficulty.Medium:
                return 2;
            case Difficulty.Hard:
                return 2;
            case Difficulty.Extreme:
                return 2;
        }
        return -1;
    }

    public static int GetNumberOfSolvesToProgressInChallenge(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 2;
            case Difficulty.Medium:
                return 4;
            case Difficulty.Hard:
                return 6;
        }
        return -1;
    }

    // COLORS USED
    public static Color CorrectSumColor = new Color32(52, 235, 122, 255);
    public static Color IncorrectSumColor = new Color32(244, 113, 116, 255);
    public static Color SuccessBackgroundColor = new Color32(16, 173, 18, 255);
    public static Color InProgressBackgroundColor = new Color32(245, 245, 159, 255);
    public static Color NextLevelButtonEnabled = new Color32(144, 136, 130, 255);
    public static Color NextLevelButtonDisabled = new Color32(144, 136, 130, 0);
    public static Color TextColor = Color.white;
    public static Color SelectedBlock = Color.yellow;
    public static Color UnselectedBlock = Color.gray;

    public static Color[] BluesColors = { Color.blue, Color.blue, Color.cyan, Color.white };
    public static Color[] RedsColors = { Color.magenta, Color.magenta, Color.red, Color.white };
    public static Color[] GraysColors = { Color.gray, Color.gray, Color.black, Color.white };

    public static Color[][] ColorPalettes = { BluesColors, RedsColors, GraysColors };
}
