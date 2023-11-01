using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // BOARD NUMBER COMBINATIONS
    public static int[] NumbersForLvl1 = { 1, 1, 1, 1, 1, 1, 1, 1, 2 };
    public static int[] NumbersForLvl2 = { 1, 1, 1, 1, 1, 1, 1, 2, 3 };
    public static int[] NumbersForLvl3 = { 1, 1, 1, 1, 1, 1, 2, 3, 4 };
    public static int[] NumbersForLvl4 = { 1, 1, 1, 1, 1, 2, 3, 4, 5 };
    public static int[] NumbersForLvl5 = { 1, 1, 1, 1, 2, 3, 4, 5, 6 };
    public static int[] NumbersForLvl6 = { 1, 1, 1, 2, 3, 4, 5, 6, 7 };
    public static int[] NumbersForLvl7 = { 1, 1, 2, 3, 4, 5, 6, 7, 8 };
    public static int[] NumbersForLvl8 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // GAME SETTINGS

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Insane
    };

    public enum ControlMethod
    {
        Drag,
        DoubleClick
    }

    public static int[] StarterLevel = NumbersForLvl8;
    public static int[] LastLevel = NumbersForLvl8;
    public static Difficulty GameDifficulty = Difficulty.Easy;
    public static ControlMethod SelectedControlMethod = ControlMethod.Drag;

    // COLORS USED
    public static Color SuccessBackgroundColor = new Color32(16, 173, 18, 255);
    public static Color InProgressBackgroundColor = new Color32(245, 245, 159, 255);
    public static Color NextLevelButtonEnabled = new Color32(144, 136, 130, 255);
    public static Color NextLevelButtonDisabled = new Color32(144, 136, 130, 0);
    public static Color TextColor = Color.white;
    public static Color SelectedBlock = Color.yellow;
    public static Color UnselectedBlock = Color.gray;
}
