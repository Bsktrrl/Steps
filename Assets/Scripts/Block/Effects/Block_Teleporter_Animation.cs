using System.Collections;
using UnityEngine;

public class Block_Teleporter_Animation : MonoBehaviour
{
    [Header("Wait Before Scaling")]
    [SerializeField] float scale_Wait = 0.07f;

    [Header("Scale Speed")]
    [SerializeField] float scale_Speed = 2.5f;

    [Header("Scale dimensions")]
    [SerializeField] bool scaleX;
    [SerializeField] bool scaleY;
    [SerializeField] bool scaleZ;

    [Header("Scale Size")]
    [SerializeField] float scale_Min = 1f;
    [SerializeField] float scale_Max = 1.4f;
    [SerializeField] float scale_Up_End = 1.2f;
    [SerializeField] float scale_ReturnSpeed = 1.5f;

    [Header("Debug")]
    [SerializeField] bool debugLogs = false;

    private Coroutine scaleCoroutine;

    private Block_Teleport ownTeleporterRoot;
    private Block_Teleporter_Animation linkedPortalAnimation;

    private bool currentlyScaledUp;


    private void Awake()
    {
        ownTeleporterRoot = GetComponentInParent<Block_Teleport>();
    }

    private void OnEnable()
    {
        Movement.Action_StepTaken += RefreshScaleFromPlayerStandingBlock;
        Movement.Action_LandedFromFalling += RefreshScaleFromPlayerStandingBlock;
        Block_Teleport.Action_EndTeleport += RefreshScaleFromPlayerStandingBlock;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= RefreshScaleFromPlayerStandingBlock;
        Movement.Action_LandedFromFalling -= RefreshScaleFromPlayerStandingBlock;
        Block_Teleport.Action_EndTeleport -= RefreshScaleFromPlayerStandingBlock;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6) return; // Player layer

        if (debugLogs)
            Debug.Log($"{name} OnTriggerEnter. Scaling pair up immediately.", gameObject);

        SetLinkedPairScaledUp(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6) return;

        if (debugLogs)
            Debug.Log($"{name} OnTriggerExit ignored. Scale-down is handled by Movement/blockStandingOn.", gameObject);

        // Important:
        // Do NOT scale down here.
        // During teleport, trigger exit happens too early and causes the visible dip.
    }


    private void RefreshScaleFromPlayerStandingBlock()
    {
        bool playerIsOnThisPair = IsPlayerStandingOnThisTeleporterPair();

        if (debugLogs)
            Debug.Log($"{name} RefreshScaleFromPlayerStandingBlock: {playerIsOnThisPair}", gameObject);

        SetLinkedPairScaledUp(playerIsOnThisPair);
    }


    private bool IsPlayerStandingOnThisTeleporterPair()
    {
        if (ownTeleporterRoot == null)
            ownTeleporterRoot = GetComponentInParent<Block_Teleport>();

        if (ownTeleporterRoot == null)
            return false;

        if (Movement.Instance == null)
            return false;

        GameObject blockStandingOn = Movement.Instance.blockStandingOn;

        if (blockStandingOn == null)
            return false;

        GameObject thisTeleporter = ownTeleporterRoot.gameObject;
        GameObject linkedTeleporter = ownTeleporterRoot.newLandingSpot;

        return blockStandingOn == thisTeleporter || blockStandingOn == linkedTeleporter;
    }


    private void SetLinkedPairScaledUp(bool scaledUp)
    {
        ApplyScaleState(scaledUp);

        Block_Teleporter_Animation linkedAnimation = GetLinkedPortalAnimation();

        if (linkedAnimation != null && linkedAnimation != this)
            linkedAnimation.ApplyScaleState(scaledUp);
    }


    private void ApplyScaleState(bool scaledUp)
    {
        // This prevents restarting the same animation again and again.
        if (currentlyScaledUp == scaledUp)
            return;

        currentlyScaledUp = scaledUp;

        if (scaledUp)
            ScaleUp();
        else
            ScaleDown();
    }


    private Block_Teleporter_Animation GetLinkedPortalAnimation()
    {
        if (linkedPortalAnimation != null)
            return linkedPortalAnimation;

        if (ownTeleporterRoot == null)
            ownTeleporterRoot = GetComponentInParent<Block_Teleport>();

        if (ownTeleporterRoot == null)
            return null;

        if (ownTeleporterRoot.newLandingSpot == null)
            return null;

        linkedPortalAnimation =
            ownTeleporterRoot.newLandingSpot.GetComponentInChildren<Block_Teleporter_Animation>(true);

        return linkedPortalAnimation;
    }


    private void ScaleUp()
    {
        StartNewScaleCoroutine(ScaleUpRoutine());
    }

    private void ScaleDown()
    {
        StartNewScaleCoroutine(ScaleDownRoutine());
    }


    private void StartNewScaleCoroutine(IEnumerator routine)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }

        scaleCoroutine = StartCoroutine(routine);
    }


    private IEnumerator ScaleUpRoutine()
    {
        float timer = 0f;

        while (timer < scale_Wait)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 maxScale = GetTargetScale(scale_Max);

        while (Vector3.Distance(transform.localScale, maxScale) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                maxScale,
                scale_Speed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = maxScale;

        Vector3 endScale = GetTargetScale(scale_Up_End);

        while (Vector3.Distance(transform.localScale, endScale) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                endScale,
                scale_ReturnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = endScale;
        scaleCoroutine = null;
    }


    private IEnumerator ScaleDownRoutine()
    {
        Vector3 minScale = GetTargetScale(scale_Min);

        while (Vector3.Distance(transform.localScale, minScale) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                minScale,
                scale_Speed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = minScale;
        scaleCoroutine = null;
    }


    private Vector3 GetTargetScale(float targetValue)
    {
        Vector3 targetScale = transform.localScale;

        if (scaleX) targetScale.x = targetValue;
        if (scaleY) targetScale.y = targetValue;
        if (scaleZ) targetScale.z = targetValue;

        return targetScale;
    }
}