using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockStepCostDisplay : MonoBehaviour
{
    public GameObject stepCostDisplay_Parent;
    public GameObject stepCostDisplay_Canvas;
    [SerializeField] GameObject stepCostText_Object;

    float startRot_X_Canvas;
    float startRot_Y_CubeCanvas;
    float startRot_Z_StairText;


    //--------------------


    private void Start()
    {
        if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair || gameObject.GetComponent<BlockInfo>().blockType == BlockType.Slope)
            startRot_X_Canvas = 45;
        else
            startRot_X_Canvas = 90;
        
        startRot_Y_CubeCanvas = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z).y;
        startRot_Z_StairText = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z).z;

        HideDisplay();
        UpdateRotation();
        UpdateStepCostTextValue(gameObject.GetComponent<BlockInfo>().movementCost);
        stepCostText_Object.GetComponent<TextMeshProUGUI>().color = GetComponent<BlockInfo>().SetTextColor(gameObject.GetComponent<BlockInfo>().movementCost);
    }


    //--------------------


    private void OnEnable()
    {
        CameraController.rotateCamera_End += UpdateRotation;
    }

    private void OnDisable()
    {
        CameraController.rotateCamera_End -= UpdateRotation;
    }


    //--------------------


    public void ShowDisplay()
    {
        //If Pushed
        if (PlayerManager.Instance.block_LookingAt_Vertical == gameObject && !PlayerManager.Instance.block_LookingAt_Vertical.GetComponent<Block_Pusher>() && PlayerManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed)
            SetMovementCost(0);

        //If in quicksand
        else if (Player_Quicksand.Instance.isInQuicksand && GetComponent<Block_Quicksand>())
        {
            SetMovementCost(Player_Quicksand.Instance.quicksandCounter);
        }

        //If in SwampWater
        else if (Player_SwampWater.Instance.isInSwampWater)
        {
            if (GetComponent<BlockInfo>().movementCost_Temp == 0)
                 SetMovementCost(0);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 1)
                SetMovementCost(0);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 2)
                SetMovementCost(1);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 3)
                SetMovementCost(2);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 4)
                SetMovementCost(3);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 5)
                SetMovementCost(4);
        }

        //If in Mud
        else if (Player_Mud.Instance.isInMud)
        {
            if (GetComponent<BlockInfo>().movementCost_Temp == 0)
                SetMovementCost(1);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 1)
                SetMovementCost(2);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 2)
                SetMovementCost(3);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 3)
                SetMovementCost(4);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 4)
                SetMovementCost(5);
            else if (GetComponent<BlockInfo>().movementCost_Temp == 5)
                SetMovementCost(6);
        }

        //Other
        else
            SetMovementCost(gameObject.GetComponent<BlockInfo>().movementCost_Temp);

        UpdateRotation();
        UpdatePosition();
        UpdateColor();

        stepCostDisplay_Parent.SetActive(true);
    }
    void SetMovementCost(int value)
    {
        gameObject.GetComponent<BlockInfo>().movementCost = value;
        UpdateStepCostTextValue(gameObject.GetComponent<BlockInfo>().movementCost);

        stepCostText_Object.GetComponent<TextMeshProUGUI>().color = GetComponent<BlockInfo>().SetTextColor(gameObject.GetComponent<BlockInfo>().movementCost);
    }

    public void HideDisplay()
    {
        stepCostDisplay_Parent.SetActive(false);
    }


    //--------------------


    public void UpdateStepCostTextValue(float moveCost)
    {
        //stepCostText_Object.GetComponent<TextMeshProUGUI>().color = GetComponent<BlockInfo>().SetTextColor(moveCost);
        stepCostText_Object.GetComponent<TextMeshProUGUI>().text = moveCost.ToString();
    }


    //--------------------


    public void UpdateRotation()
    {
        //If ceilingGrabbing
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //If the block is a Cube
            if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 180, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 0, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + -90, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 90, 0);
        }

        //If normal movement
        else
        {
            //If the block is a Stair
            if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair || gameObject.GetComponent<BlockInfo>().blockType == BlockType.Slope)
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }

            //If the block is a Cube
            else
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 0, 0);

                    RotateBlockCheck(new Vector3(0, 0, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 180, 0);

                    RotateBlockCheck(new Vector3(0, 180, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 90, 0);

                    RotateBlockCheck(new Vector3(0, 90, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + -90, 0);

                    RotateBlockCheck(new Vector3(0, -90, 0) + GetBlockOrientationWithCamera());
                }
            }
        }
    }
    Vector3 GetBlockOrientationWithCamera()
    {
        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                return new Vector3(0, 0, 0);
            case CameraRotationState.Backward:
                return new Vector3(0, 180, 0);
            case CameraRotationState.Left:
                return new Vector3(0, -90, 0);
            case CameraRotationState.Right:
                return new Vector3(0, 90, 0);

            default:
                break;
        }

        return Vector3.zero;
    }
    void RotateBlockCheck(Vector3 rotation)
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(0, 0, 90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, -90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(0, 0, 180))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 180 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(0, 0, -90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 90 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 0))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 0))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(180 + rotation.x, 180 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 0))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 180))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 180))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 180))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, -90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, -90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, -90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, -90))
            stepCostDisplay_Parent.transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
    }

    public void UpdatePosition()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            if (!GetComponent<Block_Snow>() && GetComponent<BlockInfo>().blockType != BlockType.Stair && GetComponent<BlockInfo>().blockType != BlockType.Slope)
                stepCostDisplay_Canvas.transform.localPosition = new Vector3(0, -0.55f, 0);
        }
        else
        {
            if (!GetComponent<Block_Snow>() && GetComponent<BlockInfo>().blockType != BlockType.Stair && GetComponent<BlockInfo>().blockType != BlockType.Slope)
                stepCostDisplay_Canvas.transform.localPosition = new Vector3(0, 0.55f, 0);
        }
    }
    void UpdateColor()
    {
        stepCostText_Object.GetComponent<TextMeshProUGUI>().color = GetComponent<BlockInfo>().SetTextColor(gameObject.GetComponent<BlockInfo>().movementCost);
    }


    //--------------------


    public void DestroyBlockStepCostDisplay()
    {
        CameraController.rotateCamera_Start -= UpdateRotation;

        Destroy(this);
    }
}
