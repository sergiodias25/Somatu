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
        if (_gameManager.IsGameInProgress())
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
        if (!_settingsAnimator.IsInTransition(0))
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }
    }

    public void ClickOnProfile()
    {
        _statsAnimator.SetTrigger("ToggleStats");
    }

    public void RestoreGameplayBar()
    {
        if (!IsInAnimationState(_gameplayBarAnimator, "ShowGameplayBar"))
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
        if (IsInAnimationState(_settingsAnimator, "ShowSettings"))
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }
    }

    public void HideGameplayBar()
    {
        if (IsInAnimationState(_gameplayBarAnimator, "ShowGameplayBar"))
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
    }

    private bool IsInAnimationState(Animator animator, string state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private void Update()
    {
        /*Debug.Log(
            IsInAnimationState(_settingsAnimator, "ShowSettings").ToString()
                + " "
                + _settingsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime.ToString()
                + " "
                + _settingsAnimator.IsInTransition(0)
        );*/

        if (
            _gameManager.IsGameInProgress()
            && IsInAnimationState(_gameplayBarAnimator, "ShowGameplayBar")
            && IsInAnimationState(_settingsAnimator, "ShowSettings")
            && !_settingsAnimator.IsInTransition(0)
        )
        {
            _settingsAnimator.SetTrigger("ToggleSettings");
        }
        if (
            _gameManager.IsGameInProgress()
            && !IsInAnimationState(_gameplayBarAnimator, "ShowGameplayBar")
            && !IsInAnimationState(_settingsAnimator, "ShowSettings")
            && !_gameplayBarAnimator.IsInTransition(0)
        )
        {
            _gameplayBarAnimator.SetTrigger("ToggleGameplay");
        }
    }
}
