using UnityEngine;

namespace GridSum.Assets._Scripts.Visuals
{
    public class HideIcons : MonoBehaviour
    {
        [SerializeField]
        private GameObject _sunriseIcon;

        [SerializeField]
        private GameObject _sunsetIcon;

        void OnEnable()
        {
            if (FindObjectOfType<GameManager>().SavedGameData.PurchaseData.SunriseTheme)
            {
                _sunriseIcon.SetActive(false);
            }
            else
            {
                _sunriseIcon.SetActive(true);
            }
            if (FindObjectOfType<GameManager>().SavedGameData.PurchaseData.SunsetTheme)
            {
                _sunsetIcon.SetActive(false);
            }
            else
            {
                _sunsetIcon.SetActive(true);
            }
        }
    }
}
