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
    private Timer _timer;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timer = FindObjectOfType<Timer>();
    }

    public void ClickOnSettings()
    {
        if (_gameManager.IsGameInProgress())
        {
            HideGameplayBar();
        }
        HideStats();
        ShowPanelAnimation(_settingsAnimator, "ShowSettings", "ToggleSettings");
    }

    public void ClickOnProfile()
    {
        _statsAnimator.SetTrigger("ToggleStats");
        _timer.ToggleTimer();
        HideSettings();
    }

    public void RestoreGameplayBar()
    {
        ShowPanelAnimation(_gameplayBarAnimator, "ShowGameplayBar", "ToggleGameplay");
        HideSettings();
        HideStats();
    }

    public void HideGameplayBar()
    {
        HidePanelAnimation(_gameplayBarAnimator, "ShowGameplayBar", "ToggleGameplay");
    }

    public void HideStats()
    {
        HidePanelAnimation(_statsAnimator, "ShowStats", "ToggleStats");
    }

    public void HideSettings()
    {
        HidePanelAnimation(_settingsAnimator, "ShowSettings", "ToggleSettings");
    }

    public void HidePanelAnimation(Animator animator, string activeStatusName, string triggerName)
    {
        if (IsInAnimationState(animator, activeStatusName))
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void ShowPanelAnimation(Animator animator, string activeStatusName, string triggerName)
    {
        if (!IsInAnimationState(animator, activeStatusName))
        {
            animator.SetTrigger(triggerName);
        }
    }

    private bool IsInAnimationState(Animator animator, string state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private void Update()
    {
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
