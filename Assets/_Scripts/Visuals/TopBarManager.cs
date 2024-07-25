using CandyCabinets.Components.Colour;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.CustomAnimation;

public class TopBarManager : MonoBehaviour
{
    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _shopButton;

    [SerializeField]
    private Button _profileButton;

    [SerializeField]
    private Button _settingsButton;

    private void MakeActive(Button button)
    {
        button.GetComponent<Image>().color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_DARK_TEXT
        ];
        button.GetComponent<Outline>().effectColor = ColourManager.Instance
            .SelectedPalette()
            .Colours[Constants.COLOR_BUTTON];
        button.transform.GetChild(0).GetComponent<Image>().color = ColourManager.Instance
            .SelectedPalette()
            .Colours[Constants.COLOR_BUTTON];
    }

    private void MakeInactive(Button button)
    {
        button.GetComponent<Image>().color = ColourManager.Instance.SelectedPalette().Colours[
            Constants.COLOR_BUTTON
        ];
        button.GetComponent<Outline>().effectColor = ColourManager.Instance
            .SelectedPalette()
            .Colours[Constants.COLOR_DARK_TEXT];
        button.transform.GetChild(0).GetComponent<Image>().color = ColourManager.Instance
            .SelectedPalette()
            .Colours[Constants.COLOR_DARK_TEXT];
    }

    public async void SelectHomeButton()
    {
        await CustomAnimation.ButtonClicked(_homeButton.transform);
        MakeActive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
    }

    public async void SelectShopButton()
    {
        await CustomAnimation.ButtonClicked(_shopButton.transform);
        MakeInactive(_homeButton);
        MakeActive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
    }

    public async void SelectSettingsButton()
    {
        await CustomAnimation.ButtonClicked(_settingsButton.transform);
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeActive(_settingsButton);
        MakeInactive(_profileButton);
    }

    public async void SelectProfileButton()
    {
        await CustomAnimation.ButtonClicked(_profileButton.transform);
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeActive(_profileButton);
    }

    public async void DeselectHomeButton()
    {
        await CustomAnimation.ButtonClicked(_homeButton.transform);
        MakeInactive(_homeButton);
    }

    public async void DeselectShopButton()
    {
        await CustomAnimation.ButtonClicked(_shopButton.transform);
        MakeInactive(_shopButton);
    }

    public async void DeselectSettingsButton()
    {
        await CustomAnimation.ButtonClicked(_settingsButton.transform);
        MakeInactive(_settingsButton);
    }

    public async void DeselectProfileButton()
    {
        await CustomAnimation.ButtonClicked(_profileButton.transform);
        MakeInactive(_profileButton);
    }

    public void DeselectAll()
    {
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
    }
}
