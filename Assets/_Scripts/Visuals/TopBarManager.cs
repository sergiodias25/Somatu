using CandyCabinets.Components.Colour;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.CustomAnimation;

public class TopBarManager : MonoBehaviour
{
    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _profileButton;

    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private GameObject _mainTitle;

    [SerializeField]
    private GameObject _settingsTitle;

    [SerializeField]
    private GameObject _profileTitle;

    private void Start()
    {
        CustomAnimation.ButtonLoad(_homeButton.transform.parent);
        CustomAnimation.ButtonLoad(_profileButton.transform.parent);
        CustomAnimation.ButtonLoad(_settingsButton.transform.parent);
        CustomAnimation.AnimateTitle(_mainTitle.transform);
    }

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
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_homeButton.transform);
    }

    public void SelectSettingsButton()
    {
        MakeInactive(_homeButton);
        MakeActive(_settingsButton);
        MakeInactive(_profileButton);
        CustomAnimation.AnimateTitle(_settingsTitle.transform);
        _ = CustomAnimation.ButtonClicked(_settingsButton.transform);
    }

    public void SelectProfileButton()
    {
        MakeInactive(_homeButton);
        MakeInactive(_settingsButton);
        MakeActive(_profileButton);
        CustomAnimation.AnimateTitle(_profileTitle.transform);
        _ = CustomAnimation.ButtonClicked(_profileButton.transform);
    }

    public async void DeselectHomeButton()
    {
        MakeInactive(_homeButton);
        await CustomAnimation.ButtonClicked(_homeButton.transform, false);
    }

    public async void DeselectSettingsButton()
    {
        MakeInactive(_settingsButton);
        await CustomAnimation.ButtonClicked(_settingsButton.transform, false);
    }

    public async void DeselectProfileButton()
    {
        MakeInactive(_profileButton);
        await CustomAnimation.ButtonClicked(_profileButton.transform, false);
    }

    public void DeselectAll()
    {
        MakeInactive(_homeButton);
        MakeInactive(_settingsButton);
        MakeInactive(_profileButton);
    }
}
