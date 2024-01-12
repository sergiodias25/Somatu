using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayTextManager : MonoBehaviour
{
    [SerializeField]
    private Text _easyTabText;

    [SerializeField]
    private Text _mediumTabText;

    [SerializeField]
    private Text _hardTabText;

    [SerializeField]
    private Text _extremeTabText;

    [SerializeField]
    private Text _challengeTabText;

    private void Awake()
    {
        _easyTabText.text = Constants.Difficulty.Fácil.ToString();
        _mediumTabText.text = Constants.Difficulty.Médio.ToString();
        _hardTabText.text = Constants.Difficulty.Difícil.ToString();
        _extremeTabText.text = Constants.Difficulty.Extremo.ToString();
        _challengeTabText.text = Constants.Difficulty.Desafio.ToString();
    }
}
