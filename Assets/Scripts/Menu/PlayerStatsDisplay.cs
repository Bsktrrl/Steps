using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [Header("Amount Display")]
    [SerializeField] TextMeshProUGUI stepsAmountDisplay;
    [SerializeField] TextMeshProUGUI coinAmountDisplay;
    [SerializeField] TextMeshProUGUI collectableAmountDisplay;


    //--------------------


    private void Update()
    {
        SetPlayerStatsDisplay();
    }

    private void OnEnable()
    {
        SetPlayerStatsDisplay();
    }
    private void OnDisable()
    {

    }


    //--------------------


    void SetPlayerStatsDisplay()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.steps_Max > 0)
                    stepsAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.steps_Max.ToString();
                else
                    stepsAmountDisplay.text = 0.ToString();

                if (gameObject.GetComponent<PlayerStats>().stats.itemsGot != null)
                    coinAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.coin.ToString();
                else
                    coinAmountDisplay.text = 0.ToString();

                if (gameObject.GetComponent<PlayerStats>().stats.itemsGot != null)
                    collectableAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.collectable.ToString();
                else
                    collectableAmountDisplay.text = 0.ToString();
            } 
        }
    }
}
