using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UIHeartAnimator;

public class StepsHUD : Singleton<StepsHUD>
{
    [Header("Footprint animation and effect")]
    //[Header("Deactivate (Active -> Used)")]
    [Range(0.1f, 2f)] public float deactivateDuration = 0.35f;
    public float deactivateStartScale = 1.0f;
    public float deactivateEndScale = 0.85f;
    public Ease deactivateEase = Ease.InOutQuad;

    //[Header("Activate (Used -> Active)")]
    [Range(0.1f, 2f)] public float activateDuration = 0.8f;
    public float activateStartScale = 0.85f;
    public float activateOvershootScale = 1.15f;
    public float activateEndScale = 1.0f;
    [Tooltip("How much of the duration is spent going StartScale -> OvershootScale (0..1).")]
    [Range(0.05f, 0.95f)] public float activateOvershootPortion = 0.4f;
    public Ease activateEaseUp = Ease.Linear;
    public Ease activateEaseDown = Ease.OutQuad;

    //[Header("Sprite crossfade")]
    [Tooltip("Extra intensity if you want the heart to feel 'more alive'. 1 = normal.")]
    [Range(0.2f, 2f)] public float crossfadeAlphaMultiplier = 1f;

    //[Header("Fill Sprites")]
    [SerializeField] Sprite baseSpriteUsed;      // e.g. extraFootstep_Used (empty background)
    [SerializeField] Sprite fillSprite;          // the sprite that "fills up"
    [SerializeField, Range(0.05f, 2f)] float fillDuration = 1.9f;

    Coroutine[] _running;

    [Header("Footprint Frame Glow")]
    public Frame_GlowUp frameGlow_Steps;
    public Frame_GlowUp frameGlow_Numbers;
    public Frame_GlowUp frameGlow_NumbersSteps;

    [Header("stepsIconList")]
    [SerializeField] List<GameObject> stepsIconList = new List<GameObject>();

    public float footprint_SpawnTime = 0.1f;
    public float StepsDisplay_RespawnTime = 0.5f;
    public float StepsDisplay_CheckpointTime = 0.65f;

    public int stepCounter;


    //--------------------


    private void Awake()
    {
        _running = new Coroutine[stepsIconList.Count];

        // Auto-find a glow component in children if not assigned.
        if (!frameGlow_Steps) frameGlow_Steps = GetComponentInChildren<Frame_GlowUp>(true);
        if (!frameGlow_Numbers) frameGlow_Numbers = GetComponentInChildren<Frame_GlowUp>(true);
        if (!frameGlow_NumbersSteps) frameGlow_NumbersSteps = GetComponentInChildren<Frame_GlowUp>(true);
    }

    private void OnEnable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot += GetExtraFootprint;
        Movement.Action_StepTaken += UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate += UpdateStepsDisplay_Respawn;

        Block_Checkpoint.Action_CheckPointEntered += UpdateStepsDisplay_Checkpoint;
    }

    private void OnDisable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot -= GetExtraFootprint;
        Movement.Action_StepTaken -= UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate -= UpdateStepsDisplay_Respawn;

        Block_Checkpoint.Action_CheckPointEntered -= UpdateStepsDisplay_Checkpoint;
    }

    //--------------------

    public void UpdateStepsDisplay_Walking()
    {
        if (Movement.Instance.blockStandingOn != null &&
            (Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>() && !Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_SpawnPoint_isAdded)
            || (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair))
        {
            for (int i = 0; i < 10; i++)
            {
                UpdateFootprints(i);
            }
        }
    }

    void UpdateFootprints(int index)
    {
        //Check passive extra footprints
        if (PlayerStats.Instance.stats.steps_Max < index + 1)
        {
            stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Passive;
            stepsIconList[index].GetComponent<RectTransform>().localScale = new Vector3(deactivateEndScale, deactivateEndScale, deactivateEndScale);
        }
        else if (PlayerStats.Instance.stats.steps_Current >= index + 1)
        {
            if (!stepsIconList[index].GetComponent<UIHeartAnimator>().isActive)
            {
                stepsIconList[index].GetComponent<UIHeartAnimator>().Activate();
            }
        }
        else
        {
            if (stepsIconList[index].GetComponent<UIHeartAnimator>().isActive)
            {
                stepsIconList[index].GetComponent<UIHeartAnimator>().Deactivate();
            }
        }

        if (Movement.Instance.blockStandingOn &&
            Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>() &&
            Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_MushroomCircle_isAdded)
        {
            StartCoroutine(FootstepsFrameShine_InOut(0.25f));
        }
    }

    //--------------------

    void GetExtraFootprint()
    {
        for (int i = 7; i < 10; i++)
        {
            if (PlayerStats.Instance.stats.steps_Max < i + 1)
            {
                print("1. MaxStep < " + (i + 1) + " | MaxSteps: " + PlayerStats.Instance.stats.steps_Max);
                stepsIconList[i].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Passive;
                stepsIconList[i].GetComponent<RectTransform>().localScale = new Vector3(deactivateEndScale, deactivateEndScale, deactivateEndScale);
            }
            else
            {
                print("2. MaxStep >= " + (i + 1) + " | MaxSteps: " + PlayerStats.Instance.stats.steps_Max);

                //If steps current are high as extra steps (above 7), don't do anything
                if (stepsIconList[i].GetComponent<Image>().sprite != StepsDisplay.Instance.extraFootstep_Passive)
                {
                    // do nothing
                }
                else if (PlayerStats.Instance.stats.steps_Current >= i + 1)
                {
                    // do nothing
                }
                else
                {
                    FillFootstep(i);
                    stepsIconList[i].GetComponent<RectTransform>().localScale = new Vector3(deactivateEndScale, deactivateEndScale, deactivateEndScale);
                }
            }
        }
    }

    public void FillFootstep(int index)
    {
        // Set the base sprite (your existing behavior)
        stepsIconList[index].GetComponent<Image>().sprite = baseSpriteUsed;

        // Start fill animation on the overlay
        if (_running[index] != null) StopCoroutine(_running[index]);
        _running[index] = StartCoroutine(CoFillBottomToTop(stepsIconList[index].GetComponent<Image>(), fillSprite, fillDuration));
    }

    private IEnumerator CoFillBottomToTop(Image baseImage, Sprite overlaySprite, float duration)
    {
        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowUp();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowUp();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowUp();

        Image overlay = EnsureFillOverlay(baseImage);

        overlay.sprite = overlaySprite;
        overlay.type = Image.Type.Filled;
        overlay.fillMethod = Image.FillMethod.Vertical;
        overlay.fillOrigin = (int)Image.OriginVertical.Bottom;
        overlay.fillClockwise = true;

        overlay.fillAmount = 0f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / duration);

            // non-linear (ease out): fast early, slower near the end
            float eased = 1f - Mathf.Pow(1f - u, 3f);

            overlay.fillAmount = eased;

            yield return null;
        }

        overlay.fillAmount = 1f;

        baseImage.sprite = StepsDisplay.Instance.extraFootstep_Used;

        overlay.gameObject.SetActive(false);

        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowDown();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowDown();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowDown();
    }

    private Image EnsureFillOverlay(Image baseImage)
    {
        // Find existing overlay
        Transform existing = baseImage.transform.Find("FillOverlay");
        if (existing != null) return existing.GetComponent<Image>();

        // Create child overlay
        var go = new GameObject("FillOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(baseImage.transform, false);

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        var overlay = go.GetComponent<Image>();
        overlay.raycastTarget = false;

        // Match some common display settings
        overlay.preserveAspect = baseImage.preserveAspect;
        overlay.material = baseImage.material;

        // IMPORTANT: Ensure it renders on top of the base image
        go.transform.SetAsLastSibling();

        return overlay;
    }

    //--------------------

    public void UpdateStepsDisplay_Respawn()
    {
        StartCoroutine(UpdateFootprintDelay(StepsDisplay_RespawnTime, footprint_SpawnTime));
    }

    public void UpdateStepsDisplay_Checkpoint()
    {
        StartCoroutine(UpdateFootprintDelay(StepsDisplay_CheckpointTime, footprint_SpawnTime));
    }

    IEnumerator UpdateFootprintDelay(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay);

        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowUp();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowUp();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowUp();

        for (int i = 0; i < 10; i++)
        {
            if (stepsIconList[i].GetComponent<Image>().sprite == StepsDisplay.Instance.normalFootstep_Active)
            {
                continue;
            }
            else
            {
                yield return new WaitForSeconds(waitTime);
                UpdateFootprints(i);
            }
        }

        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowDown();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowDown();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowDown();
    }

    //--------------------

    IEnumerator FootstepsFrameShine_InOut(float waitTime)
    {
        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowUp();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowUp();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowUp();

        yield return new WaitForSeconds(waitTime);

        if (frameGlow_Steps.gameObject.activeInHierarchy)
            frameGlow_Steps?.GlowDown();
        if (frameGlow_Numbers.gameObject.activeInHierarchy)
            frameGlow_Numbers?.GlowDown();
        if (frameGlow_NumbersSteps.gameObject.activeInHierarchy)
            frameGlow_NumbersSteps?.GlowDown();
    }
}
