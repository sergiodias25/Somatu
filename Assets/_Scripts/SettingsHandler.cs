using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public int SelectedColorsIndex = 0;

    [SerializeField]
    public Animator SettingsBarAnimator;

    [SerializeField]
    public Animator GameBarAnimator;

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

    public void HandleTopBarsSlide()
    {
        if (
            SettingsBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !SettingsBarAnimator.IsInTransition(0)
            && GameBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !GameBarAnimator.IsInTransition(0)
        )
        {
            SettingsBarAnimator.SetTrigger("Toggle");
            GameBarAnimator.SetTrigger("Toggle");
        }
    }
}
