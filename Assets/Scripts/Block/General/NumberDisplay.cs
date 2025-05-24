using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NumberDisplay : MonoBehaviour
{
    BlockInfo blockInfo;
    Player_BlockDetector player_BlockDetector;
    CameraController cameraController;

    [HideInInspector] public float duration = 0.2f;
    float blandShapeWeightValue = 60;

    [SerializeField] SkinnedMeshRenderer numberMeshRenderer;
    [SerializeField] List<Mesh> numberMeshList;

    Quaternion numberRotation;

    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Awake()
    {
        HideNumber();
    }
    private void Start()
    {
        blockInfo = GetComponentInParent<BlockInfo>();
        player_BlockDetector = FindObjectOfType<Player_BlockDetector>();
        cameraController = FindObjectOfType<CameraController>();

        SetObjectRenderer();
        SetPropertyBlock();

        SetNumberColors(SetNumberColor_MoreOrLess(blockInfo.movementCost));

        if (blockInfo.blockType == BlockType.Stair)
        {
            transform.localPosition = new Vector3(0, 0.2f, -0.4f);
            transform.localRotation = Quaternion.Euler(45, 0, 0);

            transform.GetChild(0).localPosition = new Vector3(0, 0.56f, 0.05f);
        }
        else if (blockInfo.blockType == BlockType.Slope)
        {
            transform.localPosition = new Vector3(0, 0.2f, -0.4f);
            transform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        else
        {
            //transform.GetChild(0).transform.localPosition = new Vector3(0, 0.48f, 0);
        }

        if (player_BlockDetector)
        {
            player_BlockDetector.RaycastSetup();
        }

        UpdateRotation();
    }
    private void Update()
    {
        GetBlockOrientationWithCamera();
    }

    private void OnEnable()
    {
        CameraController.Action_RotateCamera_End += UpdateRotation;
    }

    private void OnDisable()
    {
        CameraController.Action_RotateCamera_End -= UpdateRotation;
    }


    //--------------------


    void SetObjectRenderer()
    {
        //Set objectRenderers
        for (int i = 0; i < transform.childCount; i++)
        {
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
        blockInfo.movementCost = value;
        SetNumberColors(SetNumberColor_MoreOrLess(value));

        if (value == -1)
        {
            value = 10;
        }
        else if (value == -2)
        {
            value = 11;
        }
        else if (value <= -3)
        {
            return;
        }

        numberMeshRenderer.sharedMesh = numberMeshList[value];
        StartCoroutine(NumberAnimation(numberMeshRenderer, duration));
    }
    public void ShowNumber()
    {
        //If Pushed
        if (PlayerManager.Instance.block_LookingAt_Vertical == gameObject && PlayerManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed)
        {
            print("1. Pusher");

            DisplayNumber(0);
        }

        //If in quicksand
        else if (Player_Quicksand.Instance.isInQuicksand && GetComponentInParent<Block_Quicksand>())
        {
            DisplayNumber(Player_Quicksand.Instance.quicksandCounter);
        }

        //If in SwampWater
        else if (Player_SwampWater.Instance.isInSwampWater)
        {
            if (blockInfo.movementCost_Temp == -1)
                DisplayNumber(-2);
            else if(blockInfo.movementCost_Temp == 0)
                DisplayNumber(-1);
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
            else if (blockInfo.movementCost_Temp == 6)
                DisplayNumber(7);
            else if (blockInfo.movementCost_Temp == 7)
                DisplayNumber(8);
            else if (blockInfo.movementCost_Temp == 8)
                DisplayNumber(9);
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
            RotateBlockCheck_Cube_CeilingGrab();
        }

        //If normal movement
        else
        {
            //If the block is a Stair or Slope
            if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
            {
                RotateBlockCheck_Stair();
            }

            //If the block is a Cube or Slab
            else
            {
                RotateBlockCheck_Cube();
            }
        }
    }
    void RotateBlockCheck_Stair()
    {
        ////[0, 0, 0] - [0, 180, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
        //    transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

        ////[0, 90, 0] - [0, 90, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 90, 0))
        //    transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);

        ////[0, 180, 0] - [0, 0, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 180, 0))
        //    transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        ////[0, -90, 0] - [0, -90, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, -90, 0))
        //    transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }
    void RotateBlockCheck_Cube()
    {
        //[0, 0, 0] - [0, 0, 0]
        if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        //[0, 0, 90] - [0, 0, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 90))
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        //[0, 0, 180] - [0, 0, 180]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 180))
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        //[0, 0, -90] - [0, 0, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 270))
            transform.localRotation = Quaternion.Euler(0, 0, 90);

        //[90, 0, 0] - [-90, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 0))
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        //[180, 0, 0] - [180, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0))
            transform.localRotation = Quaternion.Euler(180, 0, 0);
        //[-90, 0, 0] - [90, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 0)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 0))
            transform.localRotation = Quaternion.Euler(90, 0, 0);

        //[90, 0, 90] - [0, 90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 90))
            transform.localRotation = Quaternion.Euler(0, 90, -90);
        //[180, 0, 90] - [180, 0, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 90))
            transform.localRotation = Quaternion.Euler(180, 0, 90);
        //[-90, 0, 90] - [0, -90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 90))
            transform.localRotation = Quaternion.Euler(0, -90, -90);

        //[90, 0, 180] - [90, 90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 180))
            transform.localRotation = Quaternion.Euler(90, 90, -90);
        //[180, 0, 180] - [0, 180, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 180))
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        //[-90, 0, 180] - [-90, -90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 180)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 180))
            transform.localRotation = Quaternion.Euler(-90, -90, -90);

        //[90, 0, -90] - [0, -90, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 270))
            transform.localRotation = Quaternion.Euler(0, -90, 90);
        //[180, 0, -90] - [180, 0, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 270))
            transform.localRotation = Quaternion.Euler(180, 0, -90);
        //[-90, 0, -90] - [0, 90, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, -90))
            transform.localRotation = Quaternion.Euler(0, 90, 90);
    }
    void RotateBlockCheck_Cube_CeilingGrab()
    {
        //[0, 0, 0] 
        if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
            transform.localRotation = Quaternion.Euler(180, 0, 0);

        //[0, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 90))
            transform.localRotation = Quaternion.Euler(180, 0, 90);
        //[0, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 180))
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        //[0, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 270))
            transform.localRotation = Quaternion.Euler(180, 0, -90);

        //[90, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 0))
            transform.localRotation = Quaternion.Euler(90, 0, 0);
        //[180, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0))
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        //[-90, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 0)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 0))
            transform.localRotation = Quaternion.Euler(-90, 0, 0);

        //[90, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 90))
            transform.localRotation = Quaternion.Euler(0, -90, -90);
        //[180, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 90))
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        //[-90, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 90))
            transform.localRotation = Quaternion.Euler(180, -90, 90);

        //[90, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 180))
            transform.localRotation = Quaternion.Euler(-90, 90, 90);
        //[180, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 180))
            transform.localRotation = Quaternion.Euler(180, 180, 0);
        //[-90, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 180)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 180))
            transform.localRotation = Quaternion.Euler(90, 90, -90);

        //[90, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 270))
            transform.localRotation = Quaternion.Euler(0, 90, 90);
        //[180, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 270))
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        //[-90, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, -90))
            transform.localRotation = Quaternion.Euler(0, -90, 90);
    }
    void GetBlockOrientationWithCamera()
    {
        if (!transform.GetChild(0).gameObject.activeInHierarchy) { return; }


        //-----

        if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
        {
            if (blockInfo.transform.localEulerAngles.y == 0)
                numberRotation = Quaternion.Euler(0, 180 + cameraController.cameraAnchor.transform.localEulerAngles.y, 0);
            else if (blockInfo.transform.localEulerAngles.y == 180)
                numberRotation = Quaternion.Euler(0, 0 + cameraController.cameraAnchor.transform.localEulerAngles.y, 0);
            else if (blockInfo.transform.localEulerAngles.y == 90)
                numberRotation = Quaternion.Euler(0, 90 + cameraController.cameraAnchor.transform.localEulerAngles.y, 0);
            else if (blockInfo.transform.localEulerAngles.y == -90
                     || blockInfo.transform.localEulerAngles.y == 270)
                numberRotation = Quaternion.Euler(0, -90 + cameraController.cameraAnchor.transform.localEulerAngles.y, 0);

            transform.GetChild(0).transform.localRotation = numberRotation;
        }

        //If the block is a Cube or Slab
        else
        {
            if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            {
                if (cameraController.cameraRotationState == CameraRotationState.Forward || cameraController.cameraRotationState == CameraRotationState.Backward)
                {
                    numberRotation = Quaternion.Euler(transform.GetChild(0).transform.localRotation.x, transform.GetChild(0).transform.localRotation.y + 180 + cameraController.cameraAnchor.transform.localEulerAngles.y, transform.GetChild(0).transform.localRotation.z);
                    transform.GetChild(0).transform.localRotation = numberRotation;
                }
                else
                {
                    numberRotation = Quaternion.Euler(transform.GetChild(0).transform.localRotation.x, transform.GetChild(0).transform.localRotation.y + cameraController.cameraAnchor.transform.localEulerAngles.y, transform.GetChild(0).transform.localRotation.z);
                    transform.GetChild(0).transform.localRotation = numberRotation;
                }
            }
            else
            {
                numberRotation = Quaternion.Euler(transform.GetChild(0).transform.localRotation.x, transform.GetChild(0).transform.localRotation.y + 180 + cameraController.cameraAnchor.transform.localEulerAngles.y, transform.GetChild(0).transform.localRotation.z);
                transform.GetChild(0).transform.localRotation = numberRotation;
            }
        }
    }


    //--------------------


    public void DestroyBlockStepCostDisplay()
    {
        CameraController.Action_RotateCamera_Start -= UpdateRotation;

        Destroy(this);
    }
}
