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
            .Colours[Constants.COLOR_SELECTED_NODE];
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
        MakeActive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_homeButton.transform);
    }

    public async void SelectShopButton()
    {
        MakeInactive(_homeButton);
        MakeActive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_shopButton.transform);
    }

    public async void SelectSettingsButton()
    {
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeActive(_settingsButton);
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_settingsButton.transform);
    }

    public async void SelectProfileButton()
    {
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeActive(_profileButton);
        await CustomAnimation.ButtonClicked(_profileButton.transform);
    }

    public async void DeselectHomeButton()
    {
        MakeInactive(_homeButton);
        await CustomAnimation.ButtonClicked(_homeButton.transform);
    }

    public async void DeselectShopButton()
    {
        MakeInactive(_shopButton);
        await CustomAnimation.ButtonClicked(_shopButton.transform);
    }

    public async void DeselectSettingsButton()
    {
        MakeInactive(_settingsButton);
        await CustomAnimation.ButtonClicked(_settingsButton.transform);
    }

    public async void DeselectProfileButton()
    {
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_profileButton.transform);
    }

    public void DeselectAll()
    {
        MakeInactive(_homeButton);
        MakeInactive(_shopButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
    }
}
