using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Stat Text")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI stepsText;

    [Header("Equipment Images")]
    [SerializeField] Image image_SwimSuit;
    [SerializeField] Image image_Flippers;
    [SerializeField] Image image_HikerGear;
    [SerializeField] Image image_LavaSuit;

    [Header("StepCost Display")]
    [SerializeField] TextMeshProUGUI stepCost_Forward_Text;
    [SerializeField] TextMeshProUGUI stepCost_Backward_Text;
    [SerializeField] TextMeshProUGUI stepCost_Left_Text;
    [SerializeField] TextMeshProUGUI stepCost_Right_Text;


    //--------------------


    private void Start()
    {
        //PlayerStepCost.updateStepCounter += UpdateStepsUI;
        Player_Stats.updateStepMax += UpdateStepsUI;

        Player_Stats.updateCoins += UpdateCoinsUI;
        Player_Stats.updateSwimsuit += Update_KeyItem_SwimSuitUI;
        Player_Stats.updateFlippers += Update_KeyItem_FlippersUI;
        Player_Stats.updateHikerGear += Update_KeyItem_HikerGearUI;
        Player_Stats.updateLavaSuit += Update_KeyItem_LavaSuitUI;

        Player_Movement.updateStepDisplay += UpdateStepDisplayUI;

        UpdateCoinsUI();
        UpdateStepsUI();
    }
    private void Update()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Still)
        {
            UpdateStepDisplayUI();
        }
    }


    //--------------------


    void UpdateCoinsUI()
    {
        coinText.text = "Coin: " + Player_Stats.Instance.collectables.coin;
    }
    void UpdateStepsUI()
    {
        stepsText.text = "Steps left: " + Player_Stats.Instance.stats.steps_Current;
    }
    void UpdateStepDisplayUI()
    {
        if (MainManager.Instance.canMove_Forward)
        {
            if (MainManager.Instance.block_Horizontal_InFront.block == null && MainManager.Instance.block_Vertical_InFront.block == null)
                stepCost_Forward_Text.text = "";
            else
            {
                stepCost_Forward_Text.text = BlockManager.Instance.GetMovementCost(MainManager.Instance.block_Vertical_InFront.blockElement).ToString();
            }
        }
        else
        {
            stepCost_Forward_Text.text = "";
        }

        if (MainManager.Instance.canMove_Back)
        {
            if (MainManager.Instance.block_Horizontal_InBack.block == null && MainManager.Instance.block_Vertical_InBack.block == null)
                stepCost_Backward_Text.text = "";
            else
            {
                stepCost_Backward_Text.text = BlockManager.Instance.GetMovementCost(MainManager.Instance.block_Vertical_InBack.blockElement).ToString();
            }
        }
        else
        {
            stepCost_Backward_Text.text = "";
        }

        if (MainManager.Instance.canMove_Left)
        {
            if (MainManager.Instance.block_Horizontal_ToTheLeft.block == null && MainManager.Instance.block_Vertical_ToTheLeft.block == null)
                stepCost_Left_Text.text = "";
            else
            {
                stepCost_Left_Text.text = BlockManager.Instance.GetMovementCost(MainManager.Instance.block_Vertical_ToTheLeft.blockElement).ToString();
            }

        }
        else
        {
            stepCost_Left_Text.text = "";
        }

        if (MainManager.Instance.canMove_Right)
        {
            if (MainManager.Instance.block_Horizontal_ToTheRight.block == null && MainManager.Instance.block_Vertical_ToTheRight.block == null)
                stepCost_Right_Text.text = "";
            else
            {
                stepCost_Right_Text.text = BlockManager.Instance.GetMovementCost(MainManager.Instance.block_Vertical_ToTheRight.blockElement).ToString();
            }
        }
        else
            stepCost_Right_Text.text = "";
    }


    void Update_KeyItem_SwimSuitUI()
    {
        if (Player_Stats.Instance.upgrades.SwimSuit)
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
        if (Player_Stats.Instance.upgrades.Flippers)
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
        if (Player_Stats.Instance.upgrades.HikerGear)
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
        if (Player_Stats.Instance.upgrades.LavaSuit)
        {
            image_LavaSuit.gameObject.SetActive(true);
        }
        else
        {
            image_LavaSuit.gameObject.SetActive(false);
        }
    }
}
