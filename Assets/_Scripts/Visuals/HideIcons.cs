using System;
using System.Globalization;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

namespace GridSum.Assets._Scripts.Visuals
{
    public class HideIcons : MonoBehaviour
    {
        [SerializeField]
        private GameObject _sunriseIcon;

        [SerializeField]
        private GameObject _sunsetIcon;

        [SerializeField]
        private TextMeshProUGUI _sunriseText;

        [SerializeField]
        private TextMeshProUGUI _sunsetText;

        void OnEnable()
        {
            Process();
        }

        public void Process()
        {
            if (FindObjectOfType<GameManager>().SavedGameData.PurchaseData.SunriseTheme)
            {
                _sunriseIcon.SetActive(false);
                _sunriseText.text = LocalizationManager.Localize("btn-shop-sunrisetheme-purchased");
            }
            else
            {
                _sunriseIcon.SetActive(true);
                _sunriseText.text = LocalizationManager.Localize("btn-shop-sunrisetheme");
            }
            if (FindObjectOfType<GameManager>().SavedGameData.PurchaseData.SunsetTheme)
            {
                _sunsetIcon.SetActive(false);
                _sunsetText.text = LocalizationManager.Localize("btn-shop-sunsettheme-purchased");
            }
            else
            {
                _sunsetIcon.SetActive(true);
                _sunsetText.text = LocalizationManager.Localize("btn-shop-sunsettheme");
            }
        }
    }
}
