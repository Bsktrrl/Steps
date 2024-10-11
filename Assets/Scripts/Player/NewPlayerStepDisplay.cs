using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerStepDisplay : MonoBehaviour
{
    [Header("Step Display - Horizontal")]
    public GameObject stepDisplay_Horizontal_Front;
    public GameObject stepDisplay_Horizontal_Back;
    public GameObject stepDisplay_Horizontal_Left;
    public GameObject stepDisplay_Horizontal_Right;

    [Header("Step Display - Diagonal")]
    public GameObject stepDisplay_Diagonal_Front;
    public GameObject stepDisplay_Diagonal_Back;
    public GameObject stepDisplay_Diagonal_Left;
    public GameObject stepDisplay_Diagonal_Right;

    [Header("Step Display - Vertical")]
    public GameObject stepDisplay_Vertical_Front;
    public GameObject stepDisplay_Vertical_Back;
    public GameObject stepDisplay_Vertical_Left;
    public GameObject stepDisplay_Vertical_Right;


    //--------------------


    private void Start()
    {
        HideStepDisplays();
    }
    private void Update()
    {
        if (gameObject.GetComponent<NewPlayerMovement>().movementStates == MovementStates.Moving)
        {
            HideStepDisplays();
        }
        else
        {
            ActivateStepDisplays();
        }
    }


    //--------------------


    void ActivateStepDisplays()
    {
        //Front
        if (MainManager.Instance.canMove_Forward)
        {
            switch (MainManager.Instance.block_Vertical_InFront.blockType)
            {
                case BlockType.None:
                    stepDisplay_Horizontal_Front.SetActive(false);
                    stepDisplay_Diagonal_Front.SetActive(false);
                    stepDisplay_Vertical_Front.SetActive(false);
                    break;

                case BlockType.Cube:
                    stepDisplay_Horizontal_Front.SetActive(true);
                    break;
                case BlockType.Stair:
                    stepDisplay_Diagonal_Front.SetActive(true);
                    break;
                case BlockType.Ladder:
                    stepDisplay_Vertical_Front.SetActive(true);
                    break;

                default:
                    stepDisplay_Horizontal_Front.SetActive(false);
                    stepDisplay_Diagonal_Front.SetActive(false);
                    stepDisplay_Vertical_Front.SetActive(false);
                    break;
            }
        }
        
        //Back
        if (MainManager.Instance.canMove_Back)
        {
            switch (MainManager.Instance.block_Vertical_InBack.blockType)
            {
                case BlockType.None:
                    stepDisplay_Horizontal_Back.SetActive(false);
                    stepDisplay_Diagonal_Back.SetActive(false);
                    stepDisplay_Vertical_Back.SetActive(false);
                    break;

                case BlockType.Cube:
                    stepDisplay_Horizontal_Back.SetActive(true);
                    break;
                case BlockType.Stair:
                    stepDisplay_Diagonal_Back.SetActive(true);
                    break;
                case BlockType.Ladder:
                    stepDisplay_Vertical_Back.SetActive(true);
                    break;

                default:
                    stepDisplay_Horizontal_Back.SetActive(false);
                    stepDisplay_Diagonal_Back.SetActive(false);
                    stepDisplay_Vertical_Back.SetActive(false);
                    break;
            }
        }
        
        //Left
        if (MainManager.Instance.canMove_Left)
        {
            switch (MainManager.Instance.block_Vertical_ToTheLeft.blockType)
            {
                case BlockType.None:
                    stepDisplay_Horizontal_Left.SetActive(false);
                    stepDisplay_Diagonal_Left.SetActive(false);
                    stepDisplay_Vertical_Left.SetActive(false);
                    break;

                case BlockType.Cube:
                    stepDisplay_Horizontal_Left.SetActive(true);
                    break;
                case BlockType.Stair:
                    stepDisplay_Diagonal_Left.SetActive(true);
                    break;
                case BlockType.Ladder:
                    stepDisplay_Vertical_Left.SetActive(true);
                    break;

                default:
                    stepDisplay_Horizontal_Left.SetActive(false);
                    stepDisplay_Diagonal_Left.SetActive(false);
                    stepDisplay_Vertical_Left.SetActive(false);
                    break;
            }
        }
        
        //Right
        if (MainManager.Instance.canMove_Right)
        {
            switch (MainManager.Instance.block_Vertical_ToTheRight.blockType)
            {
                case BlockType.None:
                    stepDisplay_Horizontal_Right.SetActive(false);
                    stepDisplay_Diagonal_Right.SetActive(false);
                    stepDisplay_Vertical_Right.SetActive(false);
                    break;

                case BlockType.Cube:
                    stepDisplay_Horizontal_Right.SetActive(true);
                    break;
                case BlockType.Stair:
                    stepDisplay_Diagonal_Right.SetActive(true);
                    break;
                case BlockType.Ladder:
                    stepDisplay_Vertical_Right.SetActive(true);
                    break;

                default:
                    stepDisplay_Horizontal_Right.SetActive(false);
                    stepDisplay_Diagonal_Right.SetActive(false);
                    stepDisplay_Vertical_Right.SetActive(false);
                    break;
            }
        }
    }

    void HideStepDisplays()
    {
        stepDisplay_Horizontal_Front.SetActive(false);
        stepDisplay_Horizontal_Back.SetActive(false);
        stepDisplay_Horizontal_Left.SetActive(false);
        stepDisplay_Horizontal_Right.SetActive(false);

        stepDisplay_Diagonal_Front.SetActive(false);
        stepDisplay_Diagonal_Back.SetActive(false);
        stepDisplay_Diagonal_Left.SetActive(false);
        stepDisplay_Diagonal_Right.SetActive(false);

        stepDisplay_Vertical_Front.SetActive(false);
        stepDisplay_Vertical_Back.SetActive(false);
        stepDisplay_Vertical_Left.SetActive(false);
        stepDisplay_Vertical_Right.SetActive(false);
    }
}
