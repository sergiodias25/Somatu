using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    [SerializeField]
    public Animator _statsAnimator;
    private Timer _timer;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
    }

    public void ClickOnProfile()
    {
        _statsAnimator.SetTrigger("ToggleStats");
        _timer.ToggleTimer();
    }

    public void RestoreGameplayBar()
    {
        HideStats();
    }

    public void HideStats()
    {
        HidePanelAnimation(_statsAnimator, "ShowStats", "ToggleStats");
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
