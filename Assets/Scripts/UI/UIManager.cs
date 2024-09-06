using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI stepsText;

    [SerializeField] Image image_SwimSuit;
    [SerializeField] Image image_Flippers;
    [SerializeField] Image image_HikerGear;
    [SerializeField] Image image_LavaSuit;


    //--------------------


    private void Start()
    {
        PlayerStepCost.updateStepCounter += UpdateSteps;
        PlayerStats.updateStepMax += UpdateSteps;

        PlayerStats.updateCoins += UpdateCoins;
        PlayerStats.updateSwimsuit += Update_KeyItem_SwimSuit;
        PlayerStats.updateFlippers += Update_KeyItem_Flippers;
        PlayerStats.updateHikerGear += Update_KeyItem_HikerGear;
        PlayerStats.updateLavaSuit += Update_KeyItem_LavaSuit;
    }


    //--------------------


    void UpdateCoins()
    {
        coinText.text = "Coin: " + PlayerStats.Instance.collectables.coin;
    }
    void UpdateSteps()
    {
        stepsText.text = "Steps left: " + PlayerStats.Instance.stats.steps_Current;
    }

    void Update_KeyItem_SwimSuit()
    {
        if (PlayerStats.Instance.keyItems.SwimSuit)
        {
            image_SwimSuit.gameObject.SetActive(true);
        }
        else
        {
            image_SwimSuit.gameObject.SetActive(false);
        }
    }

    void Update_KeyItem_Flippers()
    {
        if (PlayerStats.Instance.keyItems.Flippers)
        {
            image_Flippers.gameObject.SetActive(true);
        }
        else
        {
            image_Flippers.gameObject.SetActive(false);
        }
    }
    void Update_KeyItem_HikerGear()
    {
        if (PlayerStats.Instance.keyItems.HikerGear)
        {
            image_HikerGear.gameObject.SetActive(true);
        }
        else
        {
            image_HikerGear.gameObject.SetActive(false);
        }
    }

    void Update_KeyItem_LavaSuit()
    {
        if (PlayerStats.Instance.keyItems.LavaSuit)
        {
            image_LavaSuit.gameObject.SetActive(true);
        }
        else
        {
            image_LavaSuit.gameObject.SetActive(false);
        }
    }
}
