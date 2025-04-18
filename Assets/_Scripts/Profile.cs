using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField]
    private Button _arrowBack;

    [SerializeField]
    private Button _arrowForward;

    [SerializeField]
    private TextMeshProUGUI _modeText;

    private int _selectedDifficulty = 0;

    [SerializeField]
    private Button _easyButton;

    [SerializeField]
    private Button _moderateButton;

    [SerializeField]
    private Button _hardButton;

    [SerializeField]
    private Button _extremeButton;

    [SerializeField]
    private Button _challengeButton;

    private void OnEnable()
    {
        _selectedDifficulty = (int)FindObjectOfType<GameManager>().SelectedDifficulty;
        UpdateModeText();
        ChangeContentDisplayed();
    }

    public void MoveBackward()
    {
        if (_selectedDifficulty == 0)
        {
            _selectedDifficulty = 4;
        }
        else
        {
            _selectedDifficulty -= 1;
        }
        UpdateModeText();
        ChangeContentDisplayed();
    }

    public void MoveForward()
    {
        if (_selectedDifficulty == 4)
        {
            _selectedDifficulty = 0;
        }
        else
        {
            _selectedDifficulty += 1;
        }
        UpdateModeText();
        ChangeContentDisplayed();
    }

    private void ChangeContentDisplayed()
    {
        switch (_selectedDifficulty)
        {
            case 0:
                _easyButton.Invoke("Press", 0);
                break;
            case 1:
                _moderateButton.Invoke("Press", 0);
                break;
            case 2:
                _hardButton.Invoke("Press", 0);
                break;
            case 3:
                _extremeButton.Invoke("Press", 0);
                break;
            case 4:
                _challengeButton.Invoke("Press", 0);
                break;
            default:
                break;
        }
    }

    private void UpdateModeText()
    {
        switch (_selectedDifficulty)
        {
            case 0:
                _modeText.text = LocalizationManager.Localize("mode-easy");
                break;
            case 1:
                _modeText.text = LocalizationManager.Localize("mode-medium");
                break;
            case 2:
                _modeText.text = LocalizationManager.Localize("mode-hard");
                break;
            case 3:
                _modeText.text = LocalizationManager.Localize("mode-extreme");
                break;
            case 4:
                _modeText.text = LocalizationManager.Localize("mode-challenge");
                break;
            default:
                break;
        }
    }
}
