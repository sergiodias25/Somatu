using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            if (swipeIsConsiderable(startTouchPosition, endTouchPosition))
            {
                processSwipe(endTouchPosition - startTouchPosition);
            }
        }
    }

    private bool swipeIsConsiderable(Vector2 startTouchPosition, Vector2 endTouchPosition)
    {
        float minSwipeDistance = .015f;
        if (
            (
                Mathf.Abs(startTouchPosition.normalized.x - endTouchPosition.normalized.x)
                < minSwipeDistance
            )
            || (
                Mathf.Abs(startTouchPosition.normalized.y - endTouchPosition.normalized.y)
                < minSwipeDistance
            )
        )
        {
            return false;
        }
        return true;
    }

    private void processSwipe(Vector2 inputVector)
    {
        if (inputVector.x > 0)
        {
            RightSwipe();
        }
        else
        {
            LeftSwipe();
        }
        if (inputVector.y > 0)
        {
            UpSwipe();
        }
        else
        {
            DownSwipe();
        }
    }

    private void UpSwipe() { }

    private void DownSwipe() { }

    private void LeftSwipe()
    {
        FindObjectOfType<Profile>().MoveBackward();
        FindObjectOfType<UIManager>().InteractionPerformed(Constants.AudioClip.MenuInteraction);
    }

    private void RightSwipe()
    {
        FindObjectOfType<Profile>().MoveForward();
        FindObjectOfType<UIManager>().InteractionPerformed(Constants.AudioClip.MenuInteraction);
    }
}
