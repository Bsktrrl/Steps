using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    [SerializeField] float duration = 0.25f;
    [SerializeField] float blandShapeWeightValue = 0.70f;

    [SerializeField] SkinnedMeshRenderer numberMeshRenderer;
    [SerializeField] List<Mesh> numberMeshList;

    bool animation_isRunning = false;

    float startRot_X_Number;

    BlockInfo blockInfo;


    //--------------------


    private void Awake()
    {
        SetNumberColor();
        HideNumber();
    }
    private void Start()
    {
        blockInfo = GetComponentInParent<BlockInfo>();

        if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
            startRot_X_Number = 45;
        else
            startRot_X_Number = 0;
    }

    private void OnEnable()
    {
        CameraController.rotateCamera_End += UpdateRotation;
    }

    private void OnDisable()
    {
        CameraController.rotateCamera_End -= UpdateRotation;
    }


    //--------------------


    void DisplayNumber(int index)
    {
        numberMeshRenderer.sharedMesh = numberMeshList[index];

        StartCoroutine(NumberAnimation(numberMeshRenderer, duration));
    }
    public void ShowNumber()
    {
        //If Pushed
        if (PlayerManager.Instance.block_LookingAt_Vertical == gameObject && !PlayerManager.Instance.block_LookingAt_Vertical.GetComponent<Block_Pusher>() && PlayerManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed)
        {
            DisplayNumber(0);
        }

        //If in quicksand
        else if (Player_Quicksand.Instance.isInQuicksand && GetComponent<Block_Quicksand>())
        {
            DisplayNumber(Player_Quicksand.Instance.quicksandCounter);
        }

        //If in SwampWater
        else if (Player_SwampWater.Instance.isInSwampWater)
        {
            if (blockInfo.movementCost_Temp == 0)
                DisplayNumber(0);
            else if (blockInfo.movementCost_Temp == 1)
                DisplayNumber(0);
            else if (blockInfo.movementCost_Temp == 2)
                DisplayNumber(1);
            else if (blockInfo.movementCost_Temp == 3)
                DisplayNumber(2);
            else if (blockInfo.movementCost_Temp == 4)
                DisplayNumber(3);
            else if (blockInfo.movementCost_Temp == 5)
                DisplayNumber(4);
        }

        //If in Mud
        else if (Player_Mud.Instance.isInMud)
        {
            if (blockInfo.movementCost_Temp == 0)
                DisplayNumber(1);
            else if (blockInfo.movementCost_Temp == 1)
                DisplayNumber(2);
            else if (blockInfo.movementCost_Temp == 2)
                DisplayNumber(3);
            else if (blockInfo.movementCost_Temp == 3)
                DisplayNumber(4);
            else if (blockInfo.movementCost_Temp == 4)
                DisplayNumber(5);
            else if (blockInfo.movementCost_Temp == 5)
                DisplayNumber(6);
        }

        //Other
        else
        {
            DisplayNumber(blockInfo.movementCost_Temp);
        }

        UpdateRotation();
        numberMeshRenderer.gameObject.SetActive(true);
    }
    public void HideNumber()
    {
        numberMeshRenderer.gameObject.SetActive(false);
    }


    //--------------------


    private IEnumerator NumberAnimation(SkinnedMeshRenderer mesh, float time)
    {
        animation_isRunning = true;

        float elapsedTime = 0f;

        float currentValue = 0;

        while (elapsedTime < time)
        {
            currentValue = Mathf.Lerp(blandShapeWeightValue, 0, elapsedTime / time);
            mesh.SetBlendShapeWeight(0, currentValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentValue = 0;

        animation_isRunning = false;
    }


    //--------------------


    void SetNumberColor()
    {
        //Set color to the color given in BlockInfo
    }


    //--------------------


    public void UpdateRotation()
    {
        //If ceilingGrabbing
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //If the block is a Cube
            if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 180, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 0, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + -90, 0);
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 90, 0);
        }

        //If normal movement
        else
        {
            //If the block is a Stair
            if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        transform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        transform.localRotation = Quaternion.Euler(0, 0, 90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        transform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, 0, 0);

                    if (gameObject.transform.localRotation.eulerAngles.y == 0 || gameObject.transform.localRotation.eulerAngles.y == 360)
                        transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (gameObject.transform.localRotation.eulerAngles.y == 90)
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (gameObject.transform.localRotation.eulerAngles.y == -90 || gameObject.transform.localRotation.eulerAngles.y == 270)
                        transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (gameObject.transform.localRotation.eulerAngles.y == 180)
                        transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }

            //If the block is a Cube
            else
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 0, 0);

                    RotateBlockCheck(new Vector3(0, 0, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 180, 0);

                    RotateBlockCheck(new Vector3(0, 180, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + 90, 0);

                    RotateBlockCheck(new Vector3(0, 90, 0) + GetBlockOrientationWithCamera());
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    transform.localRotation = Quaternion.Euler(startRot_X_Number, -gameObject.transform.eulerAngles.y + -90, 0);

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
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(0, 0, 90))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, -90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(0, 0, 180))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 180 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(0, 0, -90))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 90 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 0))
            transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 0))
            transform.localRotation = Quaternion.Euler(180 + rotation.x, 180 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 0))
            transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 90))
            transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 90))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 90))
            transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, 180))
            transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, 180))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, 180))
            transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);

        else if (transform.rotation == Quaternion.Euler(90, 0, -90))
            transform.localRotation = Quaternion.Euler(-90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(180, 0, -90))
            transform.localRotation = Quaternion.Euler(0 + rotation.x, 0 + rotation.y, -90 + rotation.z);
        else if (transform.rotation == Quaternion.Euler(-90, 0, -90))
            transform.localRotation = Quaternion.Euler(90 + rotation.x, 0 + rotation.y, 0 + rotation.z);
    }


    //--------------------


    public void DestroyBlockStepCostDisplay()
    {
        CameraController.rotateCamera_Start -= UpdateRotation;

        Destroy(this);
    }
}
