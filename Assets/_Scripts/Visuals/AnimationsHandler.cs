using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    [SerializeField]
    public Animator _settingsAnimator;

    [SerializeField]
    public Animator _statsAnimator;
    private Timer _timer;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
    }

    public void ClickOnSettings()
    {
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
        HideSettings();
        HideStats();
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
}
