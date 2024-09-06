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
        PlayerStepCost.updateStepCounter += UpdateStepsUI;
        PlayerStats.updateStepMax += UpdateStepsUI;

        PlayerStats.updateCoins += UpdateCoinsUI;
        PlayerStats.updateSwimsuit += Update_KeyItem_SwimSuitUI;
        PlayerStats.updateFlippers += Update_KeyItem_FlippersUI;
        PlayerStats.updateHikerGear += Update_KeyItem_HikerGearUI;
        PlayerStats.updateLavaSuit += Update_KeyItem_LavaSuitUI;

        UpdateCoinsUI();
        UpdateStepsUI();
    }


    //--------------------


    void UpdateCoinsUI()
    {
        coinText.text = "Coin: " + PlayerStats.Instance.collectables.coin;
    }
    void UpdateStepsUI()
    {
        stepsText.text = "Steps left: " + PlayerStats.Instance.stats.steps_Current;
    }

    void Update_KeyItem_SwimSuitUI()
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

    void Update_KeyItem_FlippersUI()
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
    void Update_KeyItem_HikerGearUI()
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

    void Update_KeyItem_LavaSuitUI()
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
