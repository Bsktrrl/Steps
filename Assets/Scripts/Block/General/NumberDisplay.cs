using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Tooltip("Offset above the cube surface")]
    public float offsetAboveSurface = 0.1f;

    public float localStartHeight = 0;

    //--------------------

    // Cached references
    Transform cachedTransform;
    Transform cachedParent;
    Transform numberChildTransform;
    GameObject numberChildObject;
    Transform cameraAnchorTransform;
    EffectBlockInfo parentEffectBlockInfo;
    Block_Quicksand parentQuicksandBlock;

    Coroutine numberAnimationCoroutine;

    // Cached state to avoid unnecessary work every frame
    float lastCameraAnchorY = float.MinValue;
    float lastBlockLocalY = float.MinValue;
    bool lastIsCeilingGrabbing;
    CameraRotationState lastCameraRotationState;
    bool hasRotationStateBeenInitialized = false;

    private void Awake()
    {
        cachedTransform = transform;

        CacheReferencesIfNeeded();

        HideNumber();
    }

    private void Start()
    {
        CacheReferencesIfNeeded();

        if (blockInfo == null)
        {
            return;
        }

        SetObjectRenderer();
        SetPropertyBlock();

        SetNumberColors(SetNumberColor_MoreOrLess(blockInfo.movementCost));

        if (blockInfo.blockType == BlockType.Stair)
        {
            cachedTransform.localPosition = new Vector3(0, 0.2f + 0.02f, -0.4f + 0.02f);
            cachedTransform.localRotation = Quaternion.Euler(45, 0, 0);

            if (numberChildTransform != null)
                numberChildTransform.localPosition = new Vector3(0, 0.56f, 0.05f);
        }
        else if (blockInfo.blockType == BlockType.Slope)
        {
            cachedTransform.localPosition = new Vector3(0, 0.2f + 0.02f, -0.4f + 0.02f);
            cachedTransform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        else
        {
            //numberChildTransform.localPosition = new Vector3(0, 0.48f, 0);
        }

        UpdateRotation();
        ResetRotationTracking();
        GetBlockOrientationWithCamera(true);
    }

    private void Update()
    {
        GetBlockOrientationWithCamera();
    }

    private void OnEnable()
    {
        CacheReferencesIfNeeded();

        CameraController.Action_RotateCamera_End += UpdateRotation;
        Player_CeilingGrab.Action_raycastCeiling += UpdateRotation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished += UpdateRotation;
    }

    private void OnDisable()
    {
        CameraController.Action_RotateCamera_End -= UpdateRotation;
        Player_CeilingGrab.Action_raycastCeiling -= UpdateRotation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished -= UpdateRotation;
    }

    //--------------------

    void SetObjectRenderer()
    {
        objectRenderers.Clear();

        for (int i = 0; i < cachedTransform.childCount; i++)
        {
            Transform child = cachedTransform.GetChild(i);
            SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                objectRenderers.Add(skinnedMeshRenderer);
            }
        }
    }

    void SetPropertyBlock()
    {
        propertyBlocks.Clear();

        // Initialize property blocks and get original colors
        for (int i = 0; i < objectRenderers.Count; i++)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            objectRenderers[i].GetPropertyBlock(block);
            propertyBlocks.Add(block);
        }
    }

    //--------------------

    public void ShowNumber()
    {
        CacheReferencesIfNeeded();

        if (blockInfo == null) return;

        if (cachedParent == null) return;

        // If a Teleporter, don't show the number at all
        if (parentEffectBlockInfo != null && parentEffectBlockInfo.effectBlock_Teleporter_isAdded)
        {
            return;
        }

        // If in quicksand
        if (Player_Quicksand.Instance.isInQuicksand && parentQuicksandBlock != null)
        {
            DisplayNumber(Player_Quicksand.Instance.quicksandCounter);
        }

        // If in SwampWater
        else if (Player_SwampWater.Instance.isInSwampWater)
        {
            if (blockInfo.movementCost_Temp == -1)
                DisplayNumber(-2);
            else if (blockInfo.movementCost_Temp == 0)
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

        // If in Mud
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

        // Other
        else
        {
            //If this block is a Water Block and the player cannot swim
            if (gameObject.transform.parent.gameObject.GetComponent<BlockInfo>() && gameObject.transform.parent.gameObject.GetComponent<BlockInfo>().blockElement == BlockElement.Water
                && !PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel
                && !PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank && !PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank
                && !PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)
            {
                DisplayNumber(-3);
            }
            else
            {
                DisplayNumber(blockInfo.movementCost_Temp);
            }
        }

        UpdateRotation();
        ResetRotationTracking();

        if (numberMeshRenderer != null)
            numberMeshRenderer.gameObject.SetActive(true);

        GetBlockOrientationWithCamera(true);
    }

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
        else if (value == -3)
        {
            value = 12;
        }
        else if (value <= -4)
        {
            return;
        }

        if (value < 0 || value >= numberMeshList.Count)
            return;

        numberMeshRenderer.sharedMesh = numberMeshList[value];

        if (numberAnimationCoroutine != null)
        {
            StopCoroutine(numberAnimationCoroutine);
        }

        numberAnimationCoroutine = StartCoroutine(NumberAnimation(numberMeshRenderer, duration));
    }

    public void HideNumber()
    {
        if (numberMeshRenderer != null)
            numberMeshRenderer.gameObject.SetActive(false);

        ResetRotationTracking();
    }

    //--------------------

    IEnumerator NumberAnimation(SkinnedMeshRenderer mesh, float time)
    {
        float elapsedTime = 0f;
        float currentValue = blandShapeWeightValue;

        while (elapsedTime < time)
        {
            currentValue = Mathf.Lerp(blandShapeWeightValue, 0, elapsedTime / time);
            mesh.SetBlendShapeWeight(0, currentValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mesh.SetBlendShapeWeight(0, 0);
        numberAnimationCoroutine = null;
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

        //-3 = X - Cannot walk
        if (moveCost > blockInfo.movementCost_Temp || moveCost == -3)
        {
            return BlockManager.Instance.expensive_TextColor * tempTintColor;
        }
        else if (moveCost < blockInfo.movementCost_Temp)
        {
            return BlockManager.Instance.cheap_TextColor * tempTintColor;
        }
        else if (moveCost == blockInfo.movementCost_Temp)
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

        return blockInfo.stepCostText_Color * tempTintColor;
    }


    //--------------------

    public void UpdateRotation()
    {
        CacheReferencesIfNeeded();

        if (blockInfo == null)
        {
            return;
        }

        if (cachedTransform == null)
        {
            return;
        }

        bool isAscendTarget = IsAscendTarget();
        bool isCeilingGrabTarget = IsCeilingGrabTarget();
        bool isCurrentlyCeilingGrabbing = Player_CeilingGrab.Instance != null &&
                                          Player_CeilingGrab.Instance.isCeilingGrabbing;

        // PRIORITY: Ascend visuals should win over CeilingGrab visuals.
        if (isAscendTarget)
        {
            if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
                RotateBlockCheck_Stair();
            else
                PositionOnTopOfParentCube();
        }
        else if (isCeilingGrabTarget || isCurrentlyCeilingGrabbing)
        {
            PositionOnBottomOfParentCube();
        }
        else
        {
            if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
                RotateBlockCheck_Stair();
            else
                PositionOnTopOfParentCube();
        }

        ResetRotationTracking();
        GetBlockOrientationWithCamera(true);
    }

    void RotateBlockCheck_Stair()
    {
        ////[0, 0, 0] - [0, 180, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
        //    numberChildTransform.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

        ////[0, 90, 0] - [0, 90, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 90, 0))
        //    numberChildTransform.gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);

        ////[0, 180, 0] - [0, 0, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 180, 0))
        //    numberChildTransform.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        ////[0, -90, 0] - [0, -90, 0]
        //if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, -90, 0))
        //    numberChildTransform.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    void RotateBlockCheck_Cube()
    {
        //[0, 0, 0] - [0, 0, 0]
        if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, 0);

        //[0, 0, 90] - [0, 0, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, -90);
        //[0, 0, 180] - [0, 0, 180]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, 180);
        //[0, 0, -90] - [0, 0, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, 90);

        //[90, 0, 0] - [-90, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(-90, 0, 0);
        //[180, 0, 0] - [180, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, 0);
        //[-90, 0, 0] - [90, 0, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 0)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(90, 0, 0);

        //[90, 0, 90] - [0, 90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(0, 90, -90);
        //[180, 0, 90] - [180, 0, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, 90);
        //[-90, 0, 90] - [0, -90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(0, -90, -90);

        //[90, 0, 180] - [90, 90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(90, 90, -90);
        //[180, 0, 180] - [0, 180, 0]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(0, 180, 0);
        //[-90, 0, 180] - [-90, -90, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 180)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(-90, -90, -90);

        //[90, 0, -90] - [0, -90, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(0, -90, 90);
        //[180, 0, -90] - [180, 0, -90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, -90);
        //[-90, 0, -90] - [0, 90, 90]
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, -90))
            cachedTransform.localRotation = Quaternion.Euler(0, 90, 90);
    }

    void RotateBlockCheck_Cube_CeilingGrab()
    {
        //[0, 0, 0] 
        if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, 0);

        //[0, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, 90);
        //[0, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(0, 180, 0);
        //[0, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(0, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(180, 0, -90);

        //[90, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(90, 0, 0);
        //[180, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, 0);
        //[-90, 0, 0] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 0)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 0))
            cachedTransform.localRotation = Quaternion.Euler(-90, 0, 0);

        //[90, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(0, -90, -90);
        //[180, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, -90);
        //[-90, 0, 90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 90))
            cachedTransform.localRotation = Quaternion.Euler(180, -90, 90);

        //[90, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(-90, 90, 90);
        //[180, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(180, 180, 0);
        //[-90, 0, 180] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 180)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 180))
            cachedTransform.localRotation = Quaternion.Euler(90, 90, -90);

        //[90, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(90, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(0, 90, 90);
        //[180, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(180, 0, 270))
            cachedTransform.localRotation = Quaternion.Euler(0, 0, 90);
        //[-90, 0, -90] 
        else if (blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, -90)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(-90, 0, 270)
                 || blockInfo.gameObject.transform.rotation == Quaternion.Euler(270, 0, -90))
            cachedTransform.localRotation = Quaternion.Euler(0, -90, 90);
    }

    void GetBlockOrientationWithCamera()
    {
        GetBlockOrientationWithCamera(false);
    }

    void GetBlockOrientationWithCamera(bool forceUpdate)
    {
        if (numberChildObject == null || !numberChildObject.activeInHierarchy)
            return;

        if (blockInfo == null || cameraController == null || cameraAnchorTransform == null)
            return;

        float cameraY = cameraAnchorTransform.localEulerAngles.y;

        // Important:
        // Use world rotation, not local rotation.
        // Burned/swapped blocks are parented under the original block,
        // so localEulerAngles can be different from the actual visible block rotation.
        float blockY = blockInfo.transform.eulerAngles.y;

        bool isCeilingGrabbing = Player_CeilingGrab.Instance.isCeilingGrabbing;
        CameraRotationState cameraState = cameraController.cameraRotationState;

        if (!forceUpdate && hasRotationStateBeenInitialized)
        {
            if (Mathf.Approximately(cameraY, lastCameraAnchorY) &&
                Mathf.Approximately(blockY, lastBlockLocalY) &&
                isCeilingGrabbing == lastIsCeilingGrabbing &&
                cameraState == lastCameraRotationState)
            {
                return;
            }
        }

        lastCameraAnchorY = cameraY;
        lastBlockLocalY = blockY;
        lastIsCeilingGrabbing = isCeilingGrabbing;
        lastCameraRotationState = cameraState;
        hasRotationStateBeenInitialized = true;

        //-----

        if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
        {
            int roundedBlockY = Mathf.RoundToInt(blockY) % 360;
            if (roundedBlockY < 0) roundedBlockY += 360;

            if (roundedBlockY == 0)
                numberRotation = Quaternion.Euler(0f, 180f + cameraY, 0f);
            else if (roundedBlockY == 180)
                numberRotation = Quaternion.Euler(0f, 0f + cameraY, 0f);
            else if (roundedBlockY == 90)
                numberRotation = Quaternion.Euler(0f, 90f + cameraY, 0f);
            else if (roundedBlockY == 270)
                numberRotation = Quaternion.Euler(0f, -90f + cameraY, 0f);
            else
                numberRotation = Quaternion.Euler(0f, 180f + cameraY, 0f);

            numberChildTransform.localRotation = numberRotation;
        }
        else
        {
            float yRotationOffset;

            if (isCeilingGrabbing)
            {
                if (cameraState == CameraRotationState.Forward || cameraState == CameraRotationState.Backward)
                    yRotationOffset = 180f;
                else
                    yRotationOffset = 0f;
            }
            else
            {
                yRotationOffset = 180f;
            }

            numberRotation = Quaternion.Euler(0f, yRotationOffset + cameraY, 0f);
            numberChildTransform.localRotation = numberRotation;
        }
    }

    //--------------------

    public void DestroyBlockStepCostDisplay()
    {
        CameraController.Action_RotateCamera_Start -= UpdateRotation;

        Destroy(this);
    }

    //--------------------

    public void PositionOnTopOfParentCube()
    {
        if (cachedTransform.parent == null)
        {
            Debug.LogWarning("NumberDisplay has no parent to align with.");
            return;
        }

        Transform parent = cachedTransform.parent;

        // Use parent's up direction to find top in world space
        Vector3 topDirection = parent.up;

        // Use parent's Y-scale as height (assuming cube is upright)
        float cubeHeight = parent.localScale.y;

        // Compute world position for the top center of the cube
        Vector3 worldTopPosition = Vector3.zero;
        if (parent.gameObject.GetComponent<Block_Snow>())
        {
            worldTopPosition = parent.position + topDirection * (cubeHeight / 2f + offsetAboveSurface - 0.6f + 0.0075f) + (Vector3.up * localStartHeight);
        }
        else
        {
            worldTopPosition = parent.position + topDirection * (cubeHeight / 2f + offsetAboveSurface - 0.6f + 0.0075f);
        }

        // Apply the world position
        cachedTransform.position = worldTopPosition;

        // Pipe blocks should force local Y to -0.1
        BlockInfo parentBlockInfo = parent.GetComponent<BlockInfo>();
        if (blockInfo != null && blockInfo.blockElement == BlockElement.Pipe)
        {
            Vector3 localPos = cachedTransform.localPosition;
            localPos.y = -0.115f;
            cachedTransform.localPosition = localPos;
        }

        // Keep the number upright in world space
        cachedTransform.rotation = Quaternion.identity;
    }

    public void PositionOnBottomOfParentCube()
    {
        if (cachedTransform.parent == null)
        {
            Debug.LogWarning("NumberDisplay has no parent to align with.");
            return;
        }

        Transform parent = cachedTransform.parent;

        // Get the parent's "down" direction
        Vector3 bottomDirection = -parent.up;

        // Use parent's Y scale for height
        float cubeHeight = parent.localScale.y;

        // World position at bottom of the cube
        Vector3 worldBottomPosition = parent.position + bottomDirection * (cubeHeight / 2f + offsetAboveSurface - 0.6f + 0.0075f) + (Vector3.up * localStartHeight);

        // Move the number object to the bottom in world space
        cachedTransform.position = worldBottomPosition;

        // Pipe blocks should force local Y to -0.1
        BlockInfo parentBlockInfo = parent.GetComponent<BlockInfo>();
        if (blockInfo != null && blockInfo.blockElement == BlockElement.Pipe)
        {
            Vector3 localPos = cachedTransform.localPosition;
            localPos.y = -0.115f;
            cachedTransform.localPosition = localPos;
        }

        // Make the number face downward in world space
        cachedTransform.up = Vector3.down;
    }

    void ResetRotationTracking()
    {
        hasRotationStateBeenInitialized = false;
        lastCameraAnchorY = float.MinValue;
        lastBlockLocalY = float.MinValue;
        lastIsCeilingGrabbing = false;
        lastCameraRotationState = default;
    }



    #region Helpers

    bool IsAscendTarget()
    {
        return Movement.Instance != null &&
               cachedParent != null &&
               Movement.Instance.moveToBlock_Ascend != null &&
               Movement.Instance.moveToBlock_Ascend.canMoveTo &&
               Movement.Instance.moveToBlock_Ascend.targetBlock == cachedParent.gameObject;
    }

    bool IsCeilingGrabTarget()
    {
        return Player_CeilingGrab.Instance != null &&
               cachedParent != null &&
               Player_CeilingGrab.Instance.ceilingGrabBlock == cachedParent.gameObject;
    }

    void CacheReferencesIfNeeded()
    {
        if (cachedTransform == null)
        {
            cachedTransform = transform;
        }

        if (cachedParent != cachedTransform.parent)
        {
            cachedParent = cachedTransform.parent;
            parentEffectBlockInfo = null;
        }

        if (numberChildTransform == null && cachedTransform.childCount > 0)
        {
            numberChildTransform = cachedTransform.GetChild(0);
            numberChildObject = numberChildTransform.gameObject;
        }

        if (blockInfo == null)
        {
            blockInfo = GetComponentInParent<BlockInfo>();
        }

        if (player_BlockDetector == null)
        {
            player_BlockDetector = FindObjectOfType<Player_BlockDetector>();
        }

        if (cameraController == null)
        {
            cameraController = FindObjectOfType<CameraController>();
        }

        if (cameraAnchorTransform == null && cameraController != null && cameraController.cameraAnchor != null)
        {
            cameraAnchorTransform = cameraController.cameraAnchor.transform;
        }

        if (parentEffectBlockInfo == null && cachedParent != null)
        {
            parentEffectBlockInfo = cachedParent.GetComponent<EffectBlockInfo>();
        }

        if (parentQuicksandBlock == null)
        {
            parentQuicksandBlock = GetComponentInParent<Block_Quicksand>();
        }
    }

    #endregion
}