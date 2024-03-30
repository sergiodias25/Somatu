using System.Collections.Generic;
using UnityEngine;

public static class Constants
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

    // 1 second does not count
    public static double ChallengeTimeLimit = 15.0f;
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

    public static int GetNumberOfSolvesToUnlockNextDifficulty(Difficulty? difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 1;
            case Difficulty.Medium:
                return 1;
            case Difficulty.Hard:
                return 1;
            case Difficulty.Extreme:
                return 1;
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

    public static int GetNumberOfHintsDisplayed(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 3;
            case Difficulty.Medium:
                return 2;
            case Difficulty.Hard:
                return 1;
            case Difficulty.Extreme:
                return 1;
        }
        return -1;
    }

    /* COLORS USED
    0 - Central and top background
    1 - Dark text
    2 - Neutral block
    3 - green block
    4 - red block
    5 - selected block
    6 - no hint solution block
    7 - Clear text
    8 - Buttons
    9 - Icons
     */

    // COLORS
    public static int COLOR_BACKGROUND = 0;
    public static int COLOR_LIGHT_TEXT = 1;
    public static int COLOR_BUTTON = 2;
    public static int COLOR_GREEN = 3;
    public static int COLOR_RED = 4;
    public static int COLOR_SELECTED_NODE = 5;
    public static int COLOR_SOLUTION_NODE_NO_HINT = 6;
    public static int COLOR_DARK_TEXT = 7;
    public static int COLOR_NODE_NEUTRAL = 8;
    public static int COLOR_CLOUD = 9;

    /*
        1st - bottom left
        2nd - bottom right
        3rd - top right
        4th - top left
    */
    public static Color[] DayPalette =
    {
        new Color32(0xA7, 0xE7, 0xF2, 0xFF),
        new Color32(0xBD, 0xED, 0xF5, 0xFF),
        new Color32(0x92, 0xE2, 0xEF, 0xFF),
        new Color32(0x7C, 0xDC, 0xEB, 0xFF)
    };
    public static Color[] NightPalette =
    {
        new Color32(0x00, 0x1D, 0x5C, 0xFF),
        new Color32(0x00, 0x1D, 0x5C, 0xFF),
        new Color32(0x00, 0x1D, 0x5C, 0xFF),
        new Color32(0x00, 0x1D, 0x5C, 0xFF),
    };
    public static Color[] SunrisePalette = { Color.gray, Color.gray, Color.white, Color.white };
    public static Color[] SunsetPalette = { Color.yellow, Color.yellow, Color.white, Color.black };

    public static Color[][] ColorPalettes =
    {
        DayPalette,
        NightPalette,
        SunrisePalette,
        SunsetPalette
    };
}
