using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // BOARD NUMBER COMBINATIONS
    public static int[] numbersForLvl1 = { 1, 1, 1, 1, 1, 1, 1, 1, 2 };
    public static int[] numbersForLvl2 = { 1, 1, 1, 1, 1, 1, 1, 2, 3 };
    public static int[] numbersForLvl3 = { 1, 1, 1, 1, 1, 1, 2, 3, 4 };
    public static int[] numbersForLvl4 = { 1, 1, 1, 1, 1, 2, 3, 4, 5 };
    public static int[] numbersForLvl5 = { 1, 1, 1, 1, 2, 3, 4, 5, 6 };
    public static int[] numbersForLvl6 = { 1, 1, 1, 2, 3, 4, 5, 6, 7 };
    public static int[] numbersForLvl7 = { 1, 1, 2, 3, 4, 5, 6, 7, 8 };
    public static int[] numbersForLvl8 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // GAME SETTINGS
    public static int[] starterLevel = numbersForLvl6;
    public static int[] endLevel = numbersForLvl8;

    // COLORS USED
    public static Color successBackgroundColor = new Color32(16, 173, 18, 255);
    public static Color inProgressBackgroundColor = new Color32(245, 245, 159, 255);
    public static Color nextLevelButtonEnabled = new Color32(144, 136, 130, 255);
    public static Color nextLevelButtonDisabled = new Color32(144, 136, 130, 0);
    public static Color textColor = Color.white;
}
