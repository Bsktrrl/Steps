using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockStepCostDisplay : MonoBehaviour
{
    [SerializeField] GameObject stepCostDisplay_Parent;
    [SerializeField] GameObject stepCostDisplay_Canvas;
    [SerializeField] GameObject stepCostText_Object;

    float startRot_X_StairCanvas;
    float startRot_Y_CubeCanvas;
    float startRot_Z_StairText;


    //--------------------


    private void Start()
    {
        Cameras.rotateCamera += UpdateRotation;

        if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            startRot_X_StairCanvas = 45 /*Quaternion.Euler(stepCostDisplay_Canvas.transform.localRotation.x, stepCostDisplay_Canvas.transform.localRotation.y, stepCostDisplay_Canvas.transform.localRotation.z).x*/;
        }
        else
        {
            startRot_X_StairCanvas = 90;
        }
        
        startRot_Y_CubeCanvas = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z).y;
        startRot_Z_StairText = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z).z;
        stepCostText_Object.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponent<BlockInfo>().stepCostText_Color;

        HideDisplay();
        UpdateRotation();
        UpdateStepCostTextValue();
    }


    //--------------------


    public void ShowDisplay()
    {
        stepCostDisplay_Parent.SetActive(true);
    }
    public void HideDisplay()
    {
        stepCostDisplay_Parent.SetActive(false);
    }


    //--------------------


    public void UpdateStepCostTextValue()
    {
        stepCostText_Object.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<BlockInfo>().movementCost.ToString();
    }


    //--------------------


    public void UpdateRotation()
    {
        //If the block is a Stair
        if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            if (Cameras.Instance.cameraState == CameraState.Forward)
            {
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_StairCanvas, 0, 0);
                stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, startRot_Z_StairText - startRot_Z_StairText/*0*/);
            }
            else if (Cameras.Instance.cameraState == CameraState.Backward)
            {
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_StairCanvas, 0, 0);
                stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, startRot_Z_StairText - startRot_Z_StairText/*180*/);

            }
            else if (Cameras.Instance.cameraState == CameraState.Left)
            {
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_StairCanvas, 0, 0);
                stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, startRot_Z_StairText - startRot_Z_StairText/*-90*/);

            }
            else if (Cameras.Instance.cameraState == CameraState.Right)
            {
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_StairCanvas, 0, 0);
                stepCostText_Object.transform.localRotation = Quaternion.Euler(0, 0, startRot_Z_StairText - startRot_Z_StairText/*90*/);

            }
        }

        //If the block is a Cube
        else
        {
            if (Cameras.Instance.cameraState == CameraState.Forward)
                stepCostDisplay_Canvas.transform.rotation = Quaternion.Euler(startRot_X_StairCanvas, -startRot_Y_CubeCanvas + 0, 0);
            else if (Cameras.Instance.cameraState == CameraState.Backward)
                stepCostDisplay_Canvas.transform.rotation = Quaternion.Euler(startRot_X_StairCanvas, -startRot_Y_CubeCanvas + 180, 0);
            else if (Cameras.Instance.cameraState == CameraState.Left)
                stepCostDisplay_Canvas.transform.rotation = Quaternion.Euler(startRot_X_StairCanvas, -startRot_Y_CubeCanvas + 90, 0);
            else if (Cameras.Instance.cameraState == CameraState.Right)
                stepCostDisplay_Canvas.transform.rotation = Quaternion.Euler(startRot_X_StairCanvas, -startRot_Y_CubeCanvas + -90, 0);
        }
    }
}
