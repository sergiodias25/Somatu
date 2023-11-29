using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    [SerializeField]
    public Animator _settingsAnimator;

    public void HandleTopBarsSlide()
    {
        _settingsAnimator.SetTrigger("Toggle");
    }

    public void RestoreGameplayBar()
    {
        if (
            _settingsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !_settingsAnimator.IsInTransition(0)
            && _settingsAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowSettings")
        )
        {
            _settingsAnimator.SetTrigger("Toggle");
        }
    }
}
