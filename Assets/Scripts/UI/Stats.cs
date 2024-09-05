using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI stepsText;


    //--------------------


    private void Start()
    {
        MainManager.updateUI += UpdateUI;

        UpdateCoins();
        UpdateSteps();
    }


    //--------------------


    void UpdateUI()
    {
        UpdateCoins();
        UpdateSteps();
    }
    void UpdateCoins()
    {
        coinText.text = "Coin: " + MainManager.Instance.collectables.coin;
    }
    void UpdateSteps()
    {
        stepsText.text = "Steps left: " + MainManager.Instance.playerStats.stepsToUse;
    }
}
