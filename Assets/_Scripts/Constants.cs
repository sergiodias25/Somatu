using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // BOARD NUMBER COMBINATIONS
    public static List<int> NumbersForEasyMode = new List<int> { 1, 2, 3 };
    public static List<int> NumbersForMediumMode = new List<int> { 1, 2, 3, 4, 5 };
    public static List<int> NumbersForHardMode = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // GAME SETTINGS
    public enum Difficulty
    {
        Fácil,
        Médio,
        Difícil,
        Extremo,
        Desafio
    };

    public enum ControlMethod
    {
        Drag,
        DoubleClick
    }

    public static ControlMethod SelectedControlMethod = ControlMethod.Drag;

    public static double ChallengeTimeLimit = 10.0f;
    public static double ChallengePuzzleSolvedBonus = 10.0f;

    public static List<int> GetNumbers(Difficulty selectedDifficulty)
    {
        switch (selectedDifficulty)
        {
            case Difficulty.Fácil:
            case Difficulty.Desafio:
                return NumbersForEasyMode;
            case Difficulty.Médio:
                return NumbersForMediumMode;
            case Difficulty.Difícil:
            case Difficulty.Extremo:
                return NumbersForHardMode;
        }
        return NumbersForHardMode;
    }

    public static int GetRepeatedNumbersCount(Difficulty selectedDifficulty)
    {
        switch (selectedDifficulty)
        {
            case Difficulty.Fácil:
            case Difficulty.Desafio:
                // repeats betweeen 7 to 8 numbers
                return Random.Range(7, 9);
            case Difficulty.Médio:
                // repeats betweeen 5 to 6 numbers
                return Random.Range(5, 7);
            case Difficulty.Difícil:
                // repeats betweeen 0 to 4 numbers
                return Random.Range(0, 3);
            case Difficulty.Extremo:
                // repeats betweeen 0 to 1 number
                return Random.Range(0, 3);
        }
        return Random.Range(0, 10);
    }

    public static int GetNumberOfSolvesToUnlockNextDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Fácil:
                return 1;
            case Difficulty.Médio:
                return 1;
            case Difficulty.Difícil:
                return 1;
            case Difficulty.Extremo:
                return 1;
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
