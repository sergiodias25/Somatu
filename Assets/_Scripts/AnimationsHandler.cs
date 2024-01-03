using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    [SerializeField]
    public Animator _settingsAnimator;

    [SerializeField]
    public Animator _statsAnimator;

    [SerializeField]
    public Animator _gameplayBarAnimator;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void ClickOnSettings()
    {
        if (
            _gameManager.IsGameInProgress()
            && _gameplayBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !_gameplayBarAnimator.IsInTransition(0)
            && _settingsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !_settingsAnimator.IsInTransition(0)
        )
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
        if (
            _settingsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !_settingsAnimator.IsInTransition(0)
        )
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }
    }

    public void ClickOnProfile()
    {
        _statsAnimator.SetTrigger("ToggleStats");

        if (_settingsAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowSettings"))
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }

        if (_gameplayBarAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowGameplayBar"))
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
    }

    public void RestoreGameplayBar()
    {
        if (!_gameplayBarAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowGameplayBar"))
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
        if (_settingsAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowSettings"))
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }
    }

    public void HideGameplayBar()
    {
        if (_gameplayBarAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowGameplayBar"))
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
    }
}
