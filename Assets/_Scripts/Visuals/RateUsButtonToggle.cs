using UnityEngine;
using UnityEngine.UI;

public class RateUsButtonToggle : MonoBehaviour
{
    [SerializeField]
    private Button _rateUsButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (Object.FindObjectOfType<GameManager>().SavedGameData.PurchaseData.HasRatedGame)
        {
            _rateUsButton.gameObject.SetActive(false);
        }
    }
}
