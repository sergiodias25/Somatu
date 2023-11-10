using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField]
    private RawImage backgroundImage;

    void Update()
    {
        backgroundImage.uvRect = new Rect(
            backgroundImage.uvRect.position + new Vector2(0.00f, 0f) * Time.deltaTime,
            backgroundImage.uvRect.size
        );
    }
}
