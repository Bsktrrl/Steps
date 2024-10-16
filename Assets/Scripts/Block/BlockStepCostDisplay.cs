using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockStepCostDisplay : MonoBehaviour
{
    [SerializeField] GameObject stepCostDisplay_Parent;
    [SerializeField] GameObject stepCostDisplay_Canvas;
    [SerializeField] GameObject stepCostText_Object;

    float startRot_X_Canvas;
    float startRot_Y_CubeCanvas;
    float startRot_Z_StairText;


    //--------------------


    private void Start()
    {
        Cameras.rotateCamera += UpdateRotation;

        if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair)
            startRot_X_Canvas = 45;
        else
            startRot_X_Canvas = 90;
        
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
        //Only rotate available blocks
        //for (int i = 0; i < gameObject.GetComponent<BlockInfo>().propertyBlocks.Count; i++)
        //{
        //    if (gameObject.GetComponent<BlockInfo>().propertyBlocks[i].GetColor("_BaseColor") != Color.white * BlockManager.Instance.materialDarkeningValue)
        //    {
        //        return;
        //    }
        //}

        //If the block is a Stair
        if (gameObject.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            if (Cameras.Instance.cameraState == CameraState.Forward)
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
            else if (Cameras.Instance.cameraState == CameraState.Backward)
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
            else if (Cameras.Instance.cameraState == CameraState.Left)
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
            else if (Cameras.Instance.cameraState == CameraState.Right)
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

        //If the block is a Cube - Complete
        else
        {
            if (Cameras.Instance.cameraState == CameraState.Forward)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 0, 0);
            else if (Cameras.Instance.cameraState == CameraState.Backward)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 180, 0);
            else if (Cameras.Instance.cameraState == CameraState.Left)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + 90, 0);
            else if (Cameras.Instance.cameraState == CameraState.Right)
                stepCostDisplay_Canvas.transform.localRotation = Quaternion.Euler(startRot_X_Canvas, -gameObject.transform.eulerAngles.y + -90, 0);
        }
    }
}
