using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<GameObject> flameEffectObjectList;

    [Header("Effect Animation")]
    [SerializeField] private float flameIgniteScaleUpDuration = 0.3f;
    [SerializeField] private float flameIgniteSettleDuration = 0.2f;
    [SerializeField] private float flameExtinguishDuration = 0.3f;
    [SerializeField] private float flameIgniteOvershootScale = 1.25f;
    [SerializeField] private float flameActiveScale = 1f;

    private Coroutine burnDelayCoroutine;
    private Coroutine checkForLavaDelayCoroutine;
    private Coroutine flameEffectAnimationCoroutine;

    private bool flameableCounterWasResetThisStep;

    private Dictionary<GameObject, Vector3> flameEffectOriginalScales = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        CacheOriginalFlameEffectScales();
    }

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

        // Remove burning when standing on water
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
            {
                RemoveFlameable();
                return;
            }
        }

        if (flameableCounterWasResetThisStep)
        {
            flameableCounterWasResetThisStep = false;
            return;
        }

        flameableStepCounter += 1;

        // Remove burning after 5 steps
        if (flameableStepCounter > 5)
        {
            RemoveFlameable();
            return;
        }
    }

    private void AddFlameable()
    {
        if (isBurning)
        {
            flameableStepCounter = 0;
            flameableCounterWasResetThisStep = true;

            if (!AnyFlameEffectIsActive())
            {
                StartFlameEffectIgniteAnimation();
            }

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
        flameableCounterWasResetThisStep = false;

        StartFlameEffectIgniteAnimation();

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
        flameableCounterWasResetThisStep = false;

        StartFlameEffectExtinguishAnimation();
    }

    private void StartFlameEffectIgniteAnimation()
    {
        if (flameEffectAnimationCoroutine != null)
        {
            StopCoroutine(flameEffectAnimationCoroutine);
        }

        flameEffectAnimationCoroutine = StartCoroutine(AnimateFlameEffectIn());
    }

    private void StartFlameEffectExtinguishAnimation()
    {
        if (flameEffectAnimationCoroutine != null)
        {
            StopCoroutine(flameEffectAnimationCoroutine);
        }

        flameEffectAnimationCoroutine = StartCoroutine(AnimateFlameEffectOut());
    }

    private IEnumerator AnimateFlameEffectIn()
    {
        CacheOriginalFlameEffectScales();

        SetFlameEffectsScale(0f);
        SetFlameEffectsActive(true);
        PlayFlameParticles();

        yield return ScaleFlameEffects(0f, flameIgniteOvershootScale, flameIgniteScaleUpDuration);
        yield return ScaleFlameEffects(flameIgniteOvershootScale, flameActiveScale, flameIgniteSettleDuration);

        SetFlameEffectsScale(flameActiveScale);

        flameEffectAnimationCoroutine = null;
    }

    private IEnumerator AnimateFlameEffectOut()
    {
        CacheOriginalFlameEffectScales();

        StopFlameParticles();

        Dictionary<GameObject, Vector3> startScales = new Dictionary<GameObject, Vector3>();

        if (flameEffectObjectList != null)
        {
            for (int i = 0; i < flameEffectObjectList.Count; i++)
            {
                GameObject flameObject = flameEffectObjectList[i];

                if (flameObject == null) { continue; }

                startScales[flameObject] = flameObject.transform.localScale;
            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < flameExtinguishDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / flameExtinguishDuration);
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            if (flameEffectObjectList != null)
            {
                for (int i = 0; i < flameEffectObjectList.Count; i++)
                {
                    GameObject flameObject = flameEffectObjectList[i];

                    if (flameObject == null) { continue; }

                    if (!startScales.ContainsKey(flameObject)) { continue; }

                    flameObject.transform.localScale = Vector3.Lerp(
                        startScales[flameObject],
                        Vector3.zero,
                        easedT
                    );
                }
            }

            yield return null;
        }

        SetFlameEffectsScale(0f);
        SetFlameEffectsActive(false);

        flameEffectAnimationCoroutine = null;
    }

    private IEnumerator ScaleFlameEffects(float fromScale, float toScale, float duration)
    {
        if (duration <= 0f)
        {
            SetFlameEffectsScale(toScale);
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            float currentScale = Mathf.Lerp(fromScale, toScale, easedT);

            SetFlameEffectsScale(currentScale);

            yield return null;
        }

        SetFlameEffectsScale(toScale);
    }

    private void CacheOriginalFlameEffectScales()
    {
        if (flameEffectObjectList == null) { return; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            GameObject flameObject = flameEffectObjectList[i];

            if (flameObject == null) { continue; }

            if (flameEffectOriginalScales.ContainsKey(flameObject)) { continue; }

            if (flameObject.transform.localScale.sqrMagnitude <= 0.0001f)
            {
                flameEffectOriginalScales.Add(flameObject, Vector3.one);
            }
            else
            {
                flameEffectOriginalScales.Add(flameObject, flameObject.transform.localScale);
            }
        }
    }

    private void SetFlameEffectsScale(float scaleMultiplier)
    {
        if (flameEffectObjectList == null) { return; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            GameObject flameObject = flameEffectObjectList[i];

            if (flameObject == null) { continue; }

            Vector3 originalScale = GetOriginalFlameEffectScale(flameObject);

            flameObject.transform.localScale = originalScale * scaleMultiplier;
        }
    }

    private Vector3 GetOriginalFlameEffectScale(GameObject flameObject)
    {
        if (flameObject == null)
        {
            return Vector3.one;
        }

        if (!flameEffectOriginalScales.ContainsKey(flameObject))
        {
            if (flameObject.transform.localScale.sqrMagnitude <= 0.0001f)
            {
                flameEffectOriginalScales.Add(flameObject, Vector3.one);
            }
            else
            {
                flameEffectOriginalScales.Add(flameObject, flameObject.transform.localScale);
            }
        }

        return flameEffectOriginalScales[flameObject];
    }

    private void SetFlameEffectsActive(bool active)
    {
        if (flameEffectObjectList == null) { return; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            if (flameEffectObjectList[i] == null) { continue; }

            flameEffectObjectList[i].SetActive(active);
        }
    }

    private bool AnyFlameEffectIsActive()
    {
        if (flameEffectObjectList == null) { return false; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            if (flameEffectObjectList[i] == null) { continue; }

            if (flameEffectObjectList[i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void PlayFlameParticles()
    {
        if (flameEffectObjectList == null) { return; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            GameObject flameObject = flameEffectObjectList[i];

            if (flameObject == null) { continue; }

            ParticleSystem[] particleSystems = flameObject.GetComponentsInChildren<ParticleSystem>(true);

            for (int j = 0; j < particleSystems.Length; j++)
            {
                if (particleSystems[j] == null) { continue; }

                particleSystems[j].Play(true);
            }
        }
    }

    private void StopFlameParticles()
    {
        if (flameEffectObjectList == null) { return; }

        for (int i = 0; i < flameEffectObjectList.Count; i++)
        {
            GameObject flameObject = flameEffectObjectList[i];

            if (flameObject == null) { continue; }

            ParticleSystem[] particleSystems = flameObject.GetComponentsInChildren<ParticleSystem>(true);

            for (int j = 0; j < particleSystems.Length; j++)
            {
                if (particleSystems[j] == null) { continue; }

                particleSystems[j].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + checkOffset, burnDistance);
    }
}