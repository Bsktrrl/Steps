using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    BlockInfo blockInfo;
    CameraController cameraController;

    // Shared scene reference. The first visible NumberDisplay finds the camera;
    // later displays reuse the same reference.
    static CameraController sharedCameraController;

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

    private bool numberIsVisible;
    private int lastDisplayedRawValue = int.MinValue;
    private int lastDisplayedMeshIndex = int.MinValue;

    // Lazy setup / event state
    private bool numberDisplayIsSetup;
    private bool eventsAreSubscribed;

    public bool NumberDisplayIsSetup => numberDisplayIsSetup;


    //--------------------


    /// <summary>
    /// Performs the one-time setup that used to happen in Awake() and Start().
    /// Safe to call more than once.
    ///
    /// Call this after the NumberDisplay GameObject is activated and before
    /// ShowNumber(). ShowNumber() also calls it automatically as a safety net.
    /// </summary>
    public void SetupNumberDisplay()
    {
        if (numberDisplayIsSetup)
        {
            return;
        }

        CacheLocalReferencesIfNeeded();

        if (blockInfo == null)
        {
            Debug.LogWarning(
                $"NumberDisplay on '{name}' could not find a parent BlockInfo.",
                this);

            return;
        }

        if (numberMeshRenderer == null)
        {
            Debug.LogWarning(
                $"NumberDisplay on '{name}' has no Number Mesh Renderer assigned.",
                this);

            return;
        }

        if (numberMeshList == null || numberMeshList.Count == 0)
        {
            Debug.LogWarning(
                $"NumberDisplay on '{name}' has no number meshes assigned.",
                this);

            return;
        }

        SetObjectRenderer();
        SetPropertyBlock();
        ApplyBlockSpecificLayout();

        // Keep the visual hidden until ShowNumber() is called.
        SetNumberVisualActive(false);

        numberIsVisible = false;
        lastDisplayedRawValue = int.MinValue;
        lastDisplayedMeshIndex = int.MinValue;
        ResetRotationTracking();

        numberDisplayIsSetup = true;

        // A hidden display does not need Update().
        // Public methods can still be called while this component is disabled.
        enabled = false;
    }

    private void Update()
    {
        // Only visible displays should normally have this component enabled.
        if (!numberDisplayIsSetup || !numberIsVisible)
        {
            return;
        }

        GetBlockOrientationWithCamera();
    }


    //--------------------


    private void OnEnable()
    {
        // Intentionally no reference searches or setup here.
        // SetActive(true) can therefore remain very cheap.
        if (!numberDisplayIsSetup)
        {
            // Prevent hundreds of unused NumberDisplay components from
            // receiving Update() while waiting for first use.
            enabled = false;
            return;
        }

        if (numberIsVisible)
        {
            SubscribeToEvents();
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }


    //--------------------


    void ApplyBlockSpecificLayout()
    {
        if (blockInfo.blockType == BlockType.Stair)
        {
            cachedTransform.localPosition =
                new Vector3(0f, 0.22f, -0.38f);

            cachedTransform.localRotation =
                Quaternion.Euler(45f, 0f, 0f);

            if (numberChildTransform != null)
            {
                numberChildTransform.localPosition =
                    new Vector3(0f, 0.56f, 0.05f);
            }
        }
        else if (blockInfo.blockType == BlockType.Slope)
        {
            cachedTransform.localPosition =
                new Vector3(0f, 0.22f, -0.38f);

            cachedTransform.localRotation =
                Quaternion.Euler(45f, 0f, 0f);
        }
    }

    void SetNumberVisualActive(bool shouldBeActive)
    {
        if (numberMeshRenderer != null)
        {
            numberMeshRenderer.gameObject.SetActive(shouldBeActive);
        }
    }

    void SubscribeToEvents()
    {
        if (eventsAreSubscribed)
        {
            return;
        }

        CameraController.Action_RotateCamera_End += UpdateRotation;
        Player_CeilingGrab.Action_raycastCeiling += UpdateRotation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished += UpdateRotation;

        eventsAreSubscribed = true;
    }

    void UnsubscribeFromEvents()
    {
        if (!eventsAreSubscribed)
        {
            return;
        }

        CameraController.Action_RotateCamera_End -= UpdateRotation;
        Player_CeilingGrab.Action_raycastCeiling -= UpdateRotation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished -= UpdateRotation;

        eventsAreSubscribed = false;
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
        SetupNumberDisplay();

        if (!numberDisplayIsSetup || blockInfo == null || cachedParent == null)
        {
            return;
        }

        RefreshRuntimeReferencesIfNeeded();

        // If a Teleporter, do not show the number at all.
        if (parentEffectBlockInfo != null &&
            parentEffectBlockInfo.effectBlock_Teleporter_isAdded)
        {
            HideNumber();
            return;
        }

        // Enable this component only while the number is visible.
        if (!enabled)
        {
            enabled = true;
        }

        SubscribeToEvents();

        // Make sure the temporary cost is updated before showing the number.
        blockInfo.ApplyTemporaryMovementCostModifiers();

        int displayValue;

        if (Player_Quicksand.Instance != null &&
            Player_Quicksand.Instance.isInQuicksand &&
            parentQuicksandBlock != null)
        {
            displayValue = Player_Quicksand.Instance.quicksandCounter;
        }
        else
        {
            bool cannotEnterWater =
                blockInfo.blockElement == BlockElement.Water &&
                !PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel &&
                !PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel &&
                !PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank &&
                !PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank &&
                !PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers &&
                !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers;

            displayValue = cannotEnterWater
                ? -3
                : blockInfo.GetMovementCost_ForDisplay();
        }

        DisplayNumber(displayValue);

        SetNumberVisualActive(true);
        numberIsVisible = true;

        UpdateRotation();
    }

    void DisplayNumber(int value)
    {
        int rawValue = value;
        int meshIndex = value;

        if (meshIndex == -1)
        {
            meshIndex = 10;
        }
        else if (meshIndex == -2)
        {
            meshIndex = 11;
        }
        else if (meshIndex == -3)
        {
            meshIndex = 12;
        }
        else if (meshIndex <= -4)
        {
            return;
        }

        if (meshIndex < 0 || meshIndex >= numberMeshList.Count)
            return;

        bool sameNumberAlreadyVisible =
            numberIsVisible &&
            lastDisplayedRawValue == rawValue &&
            lastDisplayedMeshIndex == meshIndex &&
            numberMeshRenderer != null &&
            numberMeshRenderer.gameObject.activeInHierarchy;

        // Always keep color correct, because temporary effects can change tint/color.
        SetNumberColors(SetNumberColor_MoreOrLess(rawValue));

        // If the same number is already visible, do NOT restart the appearance animation.
        // This is what prevents flickering when elevators cause repeated visual refreshes.
        if (sameNumberAlreadyVisible)
            return;

        lastDisplayedRawValue = rawValue;
        lastDisplayedMeshIndex = meshIndex;

        numberMeshRenderer.sharedMesh = numberMeshList[meshIndex];

        if (numberAnimationCoroutine != null)
        {
            StopCoroutine(numberAnimationCoroutine);
        }

        numberAnimationCoroutine = StartCoroutine(NumberAnimation(numberMeshRenderer, duration));
    }

    public void HideNumber()
    {
        bool visualAlreadyHidden =
            numberMeshRenderer == null ||
            !numberMeshRenderer.gameObject.activeInHierarchy;

        if (!numberIsVisible && visualAlreadyHidden)
        {
            UnsubscribeFromEvents();

            if (enabled)
            {
                enabled = false;
            }

            return;
        }

        if (numberAnimationCoroutine != null)
        {
            StopCoroutine(numberAnimationCoroutine);
            numberAnimationCoroutine = null;
        }

        SetNumberVisualActive(false);

        numberIsVisible = false;
        lastDisplayedRawValue = int.MinValue;
        lastDisplayedMeshIndex = int.MinValue;

        ResetRotationTracking();
        UnsubscribeFromEvents();

        // This removes Update() overhead while the display is hidden.
        if (enabled)
        {
            enabled = false;
        }
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
        if (!numberDisplayIsSetup)
        {
            SetupNumberDisplay();
        }

        if (blockInfo == null)
        {
            return Color.white;
        }

        float originalCost = blockInfo.movementCost_Temp_Base;

        Color tempTintColor;

        if (blockInfo.colorTint_isActive)
        {
            tempTintColor = Color.white * blockInfo.tintValue;
        }
        else
        {
            tempTintColor = Color.white;
        }

        // -3 = X - Cannot walk
        if (moveCost > originalCost || moveCost == -3)
        {
            return BlockManager.Instance.expensive_TextColor * tempTintColor;
        }
        else if (moveCost < originalCost)
        {
            return BlockManager.Instance.cheap_TextColor * tempTintColor;
        }
        else
        {
            if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            {
                if (blockInfo.stepCostText_ColorUnder.a == 0 ||
                    (blockInfo.stepCostText_ColorUnder.r == 0 &&
                     blockInfo.stepCostText_ColorUnder.g == 0 &&
                     blockInfo.stepCostText_ColorUnder.b == 0))
                {
                    return blockInfo.stepCostText_Color * tempTintColor;
                }
                else
                {
                    return blockInfo.stepCostText_ColorUnder * tempTintColor;
                }
            }

            return blockInfo.stepCostText_Color * tempTintColor;
        }
    }


    //--------------------

    public void UpdateRotation()
    {
        if (!numberDisplayIsSetup)
        {
            SetupNumberDisplay();
        }

        if (!numberDisplayIsSetup)
        {
            return;
        }

        CacheLocalReferencesIfNeeded();
        RefreshRuntimeReferencesIfNeeded();

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

        bool isCeilingGrabbing =
            Player_CeilingGrab.Instance != null &&
            Player_CeilingGrab.Instance.isCeilingGrabbing;

        CameraRotationState cameraState =
            cameraController.cameraRotationState;

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
        UnsubscribeFromEvents();
        Destroy(this);
    }


    //--------------------


    public void PositionOnTopOfParentCube()
    {
        if (!numberDisplayIsSetup)
        {
            SetupNumberDisplay();
        }

        if (cachedTransform == null || cachedTransform.parent == null)
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
        if (!numberDisplayIsSetup)
        {
            SetupNumberDisplay();
        }

        if (cachedTransform == null || cachedTransform.parent == null)
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

    void CacheLocalReferencesIfNeeded()
    {
        if (cachedTransform == null)
        {
            cachedTransform = transform;
        }

        if (cachedParent != cachedTransform.parent)
        {
            cachedParent = cachedTransform.parent;

            // Parent-dependent references must be refreshed after reparenting.
            blockInfo = null;
            parentEffectBlockInfo = null;
            parentQuicksandBlock = null;
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

        if (parentEffectBlockInfo == null && cachedParent != null)
        {
            parentEffectBlockInfo =
                cachedParent.GetComponent<EffectBlockInfo>();
        }

        if (parentQuicksandBlock == null)
        {
            parentQuicksandBlock =
                GetComponentInParent<Block_Quicksand>();
        }
    }

    void RefreshRuntimeReferencesIfNeeded()
    {
        if (cameraController == null)
        {
            if (sharedCameraController == null)
            {
                sharedCameraController =
                    FindObjectOfType<CameraController>();
            }

            cameraController = sharedCameraController;
        }

        if (cameraAnchorTransform == null &&
            cameraController != null &&
            cameraController.cameraAnchor != null)
        {
            cameraAnchorTransform =
                cameraController.cameraAnchor.transform;
        }
    }

    #endregion

}