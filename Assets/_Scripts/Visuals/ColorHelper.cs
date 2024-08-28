using CandyCabinets.Components.Colour;
using UnityEngine;

public class ColorHelper : MonoBehaviour
{
    [SerializeField]
    private Material _titleMaterial;

    public void ApplyUpdates()
    {
        _titleMaterial.SetColor(
            "_OutlineColor",
            ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_SQUARE]
        );
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.UpdateTextColor();
        }
    }
}
