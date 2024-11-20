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

    bool canRun;


    //--------------------


    private void Update()
    {
        if (!canRun) { return; }

        SetPlayerStatsDisplay();
    }
    private void OnEnable()
    {
        SetPlayerStatsDisplay();
        //SaveLoad_PlayerStats.playerStats_hasLoaded += HasLoaded;
    }
    private void OnDisable()
    {
        //SaveLoad_PlayerStats.playerStats_hasLoaded -= HasLoaded;
    }

    //void HasLoaded()
    //{
    //    canRun = true;
    //}


    //--------------------


    void SetPlayerStatsDisplay()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            stepsAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.steps_Max.ToString();
            coinAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.coin.ToString();
            collectableAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.collectable.ToString();
        }
    }
}
