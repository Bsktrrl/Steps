using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
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
        MainManager.updateUI += UpdateUI;

        UpdateCoins();
        UpdateSteps();
    }


    //--------------------


    void UpdateUI()
    {
        UpdateCoins();
        UpdateSteps();

        Update_KeyItem_SwimSuit();
        Update_KeyItem_Flippers();
        Update_KeyItem_HikerGear();
        Update_KeyItem_LavaSuit();
    }
    void UpdateCoins()
    {
        coinText.text = "Coin: " + MainManager.Instance.collectables.coin;
    }
    void UpdateSteps()
    {
        stepsText.text = "Steps left: " + MainManager.Instance.playerStats.stepsToUse;
    }

    void Update_KeyItem_SwimSuit()
    {
        if (MainManager.Instance.keyItems.SwimSuit)
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
        if (MainManager.Instance.keyItems.Flippers)
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
        if (MainManager.Instance.keyItems.HikerGear)
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
        if (MainManager.Instance.keyItems.LavaSuit)
        {
            image_LavaSuit.gameObject.SetActive(true);
        }
        else
        {
            image_LavaSuit.gameObject.SetActive(false);
        }
    }
}
