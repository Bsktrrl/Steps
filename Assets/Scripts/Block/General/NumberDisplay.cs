using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NumberDisplay : MonoBehaviour
{
    BlockInfo blockInfo;

    [HideInInspector] public float duration = 0.2f;
    float blandShapeWeightValue = 60;

    [SerializeField] SkinnedMeshRenderer numberMeshRenderer;
    [SerializeField] List<Mesh> numberMeshList;

    float startRot_X_Number;


    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Start()
    {
        blockInfo = GetComponentInParent<BlockInfo>();

        SetObjectRenderer();
        SetPropertyBlock();

        SetNumberColors(SetNumberColor_MoreOrLess(blockInfo.movementCost));
        HideNumber();

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


    void SetObjectRenderer()
    {
        //Set objectRenderers
        for (int i = 0; i < transform.childCount; i++)
        {
            //for (int j = 0; j < transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials.Length; j++)
            //{
            //    if (transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials[j])
            //    {
            //        objectRenderers.Add(transform.GetChild(i).GetComponent<SkinnedMeshRenderer>());
            //    }
            //}

            if (transform.GetChild(i).GetComponent<SkinnedMeshRenderer>())
            {
                objectRenderers.Add(transform.GetChild(i).GetComponent<SkinnedMeshRenderer>());
            }
        }
    }
    void SetPropertyBlock()
    {
        // Initialize property blocks and get original colors
        for (int i = 0; i < objectRenderers.Count; i++)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            objectRenderers[i].GetPropertyBlock(block);
            propertyBlocks.Add(block);
        }
    }


    //--------------------


    void DisplayNumber(int value)
    {
        numberMeshRenderer.sharedMesh = numberMeshList[value];

        SetNumberColors(SetNumberColor_MoreOrLess(value));

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


    IEnumerator NumberAnimation(SkinnedMeshRenderer mesh, float time)
    {
        float elapsedTime = 0f;

        float currentValue = 0;

        while (elapsedTime < time)
        {
            currentValue = Mathf.Lerp(blandShapeWeightValue, 0, elapsedTime / time);
            mesh.SetBlendShapeWeight(0, currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        mesh.SetBlendShapeWeight(0, currentValue);
    }


    //--------------------


    void SetNumberColors(Color movementCostColor)
    {
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Set the original color in the MaterialPropertyBlock
            propertyBlocks[i].SetColor("_BaseColor", movementCostColor);

            // Apply the MaterialPropertyBlock to the renderer
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }
    public Color SetNumberColor_MoreOrLess(float moveCost)
    {
        Color tempTintColor = new Color();

        if (blockInfo.colorTint_isActive)
        {
            tempTintColor = Color.white * blockInfo.tintValue;
        }
        else
        {
            tempTintColor = Color.white;
        }

        if (moveCost == blockInfo.movementCost_Temp)
        {
            if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            {
                if (blockInfo.stepCostText_ColorUnder.a == 0 || (blockInfo.stepCostText_ColorUnder.r == 0 && blockInfo.stepCostText_ColorUnder.g == 0 && blockInfo.stepCostText_ColorUnder.b == 0))
                    return blockInfo.stepCostText_Color * tempTintColor;
                else
                    return blockInfo.stepCostText_ColorUnder * tempTintColor;
            }
            else
            {
                return blockInfo.stepCostText_Color * tempTintColor;
            }
        }
        else if (moveCost < blockInfo.movementCost_Temp)
        {
            return BlockManager.Instance.cheap_TextColor * tempTintColor;
        }
        else if (moveCost > blockInfo.movementCost_Temp)
        {
            return BlockManager.Instance.expensive_TextColor * tempTintColor;
        }

        return blockInfo.stepCostText_Color * tempTintColor;
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
