using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public int SelectedColorsIndex = 0;

    public void ChangeTheme()
    {
        GradientBg gradientBg = FindObjectOfType<GradientBg>();
        SelectedColorsIndex = GetNextIndex(Constants.ColorPalettes.Length);
        gradientBg.UpdateTheme(Constants.ColorPalettes[SelectedColorsIndex]);
    }

    private int GetNextIndex(int size)
    {
        if (SelectedColorsIndex == size - 1)
        {
            return 0;
        }
        return SelectedColorsIndex + 1;
    }
}
