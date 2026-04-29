using System;
using System.Collections;
using UnityEngine;

public class Player_Burning : Singleton<Player_Burning>
{
    public static Action Action_PlayerStartedBurning;

    [Header("Burning State")]
    public bool isBurning;
    public int flameableStepCounter;

    [Header("Burn Settings")]
    [SerializeField] private float burnDistance = 0.7f;
    [SerializeField] private Vector3 checkOffset = Vector3.zero;
    [SerializeField] private LayerMask lavaCheckMask = ~0;

    [Header("Effects")]
    [SerializeField] private GameObject flameEffectObject;

    private Coroutine burnDelayCoroutine;
    private Coroutine checkForLavaDelayCoroutine;

    private void OnEnable()
    {
        Movement.Action_StepTaken += CheckForNearbyLava;
        Movement.Action_StepTaken += CheckFlameableCounter;
        Movement.Action_RespawnPlayer += RemoveFlameable;

        Movement.Action_RespawnPlayer += CheckForNearbyLavaDelayed;
        DataManager.Action_dataHasLoaded += CheckForNearbyLavaDelayed;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= CheckForNearbyLava;
        Movement.Action_StepTaken -= CheckFlameableCounter;
        Movement.Action_RespawnPlayer -= RemoveFlameable;

        Movement.Action_RespawnPlayer -= CheckForNearbyLavaDelayed;
        DataManager.Action_dataHasLoaded -= CheckForNearbyLavaDelayed;
    }

    private void CheckForNearbyLava()
    {
        if (IsCloseEnoughToLava())
        {
            AddFlameable();
        }
    }

    private void CheckForNearbyLavaDelayed()
    {
        if (checkForLavaDelayCoroutine != null)
        {
            StopCoroutine(checkForLavaDelayCoroutine);
        }

        checkForLavaDelayCoroutine = StartCoroutine(DelayCheckForNearbyLava());
    }

    private IEnumerator DelayCheckForNearbyLava()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        CheckForNearbyLava();

        checkForLavaDelayCoroutine = null;
    }

    private bool IsCloseEnoughToLava()
    {
        Vector3 checkPosition = transform.position + checkOffset;

        Collider[] nearbyColliders = Physics.OverlapSphere(
            checkPosition,
            burnDistance,
            lavaCheckMask,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider collider in nearbyColliders)
        {
            BlockInfo blockInfo = collider.GetComponent<BlockInfo>();

            if (blockInfo == null)
            {
                blockInfo = collider.GetComponentInParent<BlockInfo>();
            }

            if (blockInfo == null)
            {
                continue;
            }

            if (blockInfo.blockElement != BlockElement.Lava)
            {
                continue;
            }

            float distanceToLava = Vector3.Distance(
                checkPosition,
                collider.ClosestPoint(checkPosition)
            );

            if (distanceToLava <= burnDistance)
            {
                return true;
            }
        }

        return false;
    }

    private void CheckFlameableCounter()
    {
        if (!isBurning)
        {
            return;
        }

        flameableStepCounter += 1;

        // Remove burning after 5 steps
        if (flameableStepCounter > 5)
        {
            RemoveFlameable();
            return;
        }

        // Remove burning when standing on water
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
            {
                RemoveFlameable();
            }
        }
    }

    private void AddFlameable()
    {
        if (isBurning)
        {
            return;
        }

        if (burnDelayCoroutine != null)
        {
            StopCoroutine(burnDelayCoroutine);
        }

        burnDelayCoroutine = StartCoroutine(DelayFlammable());
    }

    private IEnumerator DelayFlammable()
    {
        yield return new WaitForEndOfFrame();

        isBurning = true;
        flameableStepCounter = 0;

        if (flameEffectObject != null)
        {
            flameEffectObject.SetActive(true);
        }

        Action_PlayerStartedBurning?.Invoke();

        burnDelayCoroutine = null;
    }

    private void RemoveFlameable()
    {
        if (burnDelayCoroutine != null)
        {
            StopCoroutine(burnDelayCoroutine);
            burnDelayCoroutine = null;
        }

        if (!isBurning)
        {
            return;
        }

        isBurning = false;
        flameableStepCounter = 0;

        if (flameEffectObject != null)
        {
            flameEffectObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + checkOffset, burnDistance);
    }
}