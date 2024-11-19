using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        UpdateCoinsUI();
        UpdateStepsUI();
    }
    private void Update()
    {
        UpdateStepDisplayUI();
    }


    //--------------------


    private void OnEnable()
    {
        PlayerStats.updateCoins += UpdateCoinsUI;
        PlayerStats.updateStepsMax += UpdateStepsUI;
    }

    private void OnDisable()
    {
        PlayerStats.updateCoins -= UpdateCoinsUI;
        PlayerStats.updateStepsMax -= UpdateStepsUI;
    }


    //--------------------


    void UpdateStepDisplayUI()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving)
        {
            stepCost_Forward_Text.text = "";
            stepCost_Backward_Text.text = "";
            stepCost_Right_Text.text = "";
            stepCost_Left_Text.text = "";

            return;
        }

        //If a key is held down, don't show the new Movement Costs
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)) { }
        else
        {
            StepDisplay();
        }

        //When a key is pressed up, show the new Movement Costs
        if (Input.GetKeyUp(KeyCode.W)
            || Input.GetKeyUp(KeyCode.S)
            || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.D))
        {
            StepDisplay();
        }
    }
    void StepDisplay()
    {
        if (PlayerManager.Instance.canMove_Forward)
        {
            if (PlayerManager.Instance.block_Horizontal_InFront.block == null && PlayerManager.Instance.block_Vertical_InFront.block == null)
                stepCost_Forward_Text.text = "";
            else
            {
                if (PlayerManager.Instance.block_Vertical_InFront.block != null)
                    if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                        stepCost_Forward_Text.text = PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().movementCost.ToString();
                    else
                        stepCost_Forward_Text.text = "";
                else
                    stepCost_Forward_Text.text = "";
            }
        }
        else
        {
            stepCost_Forward_Text.text = "";
        }

        if (PlayerManager.Instance.canMove_Back)
        {
            if (PlayerManager.Instance.block_Horizontal_InBack.block == null && PlayerManager.Instance.block_Vertical_InBack.block == null)
                stepCost_Backward_Text.text = "";
            else
            {
                if (PlayerManager.Instance.block_Vertical_InBack.block != null)
                    if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                        stepCost_Backward_Text.text = PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().movementCost.ToString();
                    else
                        stepCost_Backward_Text.text = "";
                else
                    stepCost_Backward_Text.text = "";
            }
        }
        else
        {
            stepCost_Backward_Text.text = "";
        }

        if (PlayerManager.Instance.canMove_Left)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheLeft.block == null && PlayerManager.Instance.block_Vertical_ToTheLeft.block == null)
                stepCost_Left_Text.text = "";
            else
            {
                if (PlayerManager.Instance.block_Vertical_ToTheLeft.block != null)
                    if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                        stepCost_Left_Text.text = PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().movementCost.ToString();
                    else
                        stepCost_Left_Text.text = "";
                else
                    stepCost_Left_Text.text = "";
            }

        }
        else
        {
            stepCost_Left_Text.text = "";
        }

        if (PlayerManager.Instance.canMove_Right)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheRight.block == null && PlayerManager.Instance.block_Vertical_ToTheRight.block == null)
                stepCost_Right_Text.text = "";
            else
            {
                if (PlayerManager.Instance.block_Vertical_ToTheRight.block != null)
                    if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                        stepCost_Right_Text.text = PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().movementCost.ToString();
                    else
                        stepCost_Right_Text.text = "";
                else
                    stepCost_Right_Text.text = "";
            }
        }
        else
            stepCost_Right_Text.text = "";
    }


    //--------------------


    void UpdateCoinsUI()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            coinText.text = "Coin: " + gameObject.GetComponent<PlayerStats>().stats.itemsGot.coin;
        }
    }
    public void UpdateStepsUI()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            stepsText.text = "Steps: " + gameObject.GetComponent<PlayerStats>().stats.steps_Current + "/" + gameObject.GetComponent<PlayerStats>().stats.steps_Max;
        }
    }
}
