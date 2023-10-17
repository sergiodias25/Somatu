using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private void Start() {
        enabled = false;
    }

    public void ActivateRestartButton() {
        enabled = true;
    }
    private void OnMouseDown()
    {
        if (enabled) {
           //FindObjectOfType<GameManager>().GenerateGrid(Constants.numbersForLvl2);
        }
    }

}
