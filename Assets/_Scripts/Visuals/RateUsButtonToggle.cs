using UnityEngine;
using UnityEngine.UI;

public class RateUsButtonToggle : MonoBehaviour
{
    [SerializeField]
    private Button _rateUsButton;

    [SerializeField]
    private Button _closeButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (Object.FindObjectOfType<GameManager>().SavedGameData.PurchaseData.HasRatedGame)
        {
            _rateUsButton.gameObject.SetActive(false);
        }

        if (
            Object
                .FindObjectOfType<GameManager>()
                .SavedGameData.PurchaseData.TimesClosedSupportUsPopup
            >= Constants.MaxTimesCloseRemoveAdsPopup
        )
        {
            _closeButton.gameObject.SetActive(false);
            Object
                .FindObjectOfType<GameManager>()
                .SavedGameData.PurchaseData.TimesClosedSupportUsPopup = 0;
        }
        else
        {
            _closeButton.gameObject.SetActive(true);
        }
    }
}
