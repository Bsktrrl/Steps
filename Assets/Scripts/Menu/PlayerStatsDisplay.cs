using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [Header("Amount Display")]
    [SerializeField] TextMeshProUGUI stepsAmountDisplay;
    [SerializeField] TextMeshProUGUI essenceAmountDisplay;
    [SerializeField] TextMeshProUGUI skinAmountDisplay;


    //--------------------


    private void Update()
    {
        //SetPlayerStatsDisplay();
    }

    private void OnEnable()
    {
        //SetPlayerStatsDisplay();
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
                    essenceAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.essence_Max.ToString() + "[" + gameObject.GetComponent<PlayerStats>().stats.itemsGot.essence_Current.ToString() + "]";
                else
                    essenceAmountDisplay.text = 0.ToString();

                if (gameObject.GetComponent<PlayerStats>().stats.itemsGot != null)
                    skinAmountDisplay.text = gameObject.GetComponent<PlayerStats>().stats.itemsGot.skin.ToString();
                else
                    skinAmountDisplay.text = 0.ToString();
            } 
        }
    }
}
