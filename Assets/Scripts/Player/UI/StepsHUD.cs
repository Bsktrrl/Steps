using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UIHeartAnimator;

public class StepsHUD : Singleton<StepsHUD>
{
    [Header("Footprint animation and effect")]
    [Range(0.1f, 2f)] public float deactivateDuration = 0.35f;
    public float deactivateStartScale = 1.0f;
    public float deactivateEndScale = 0.85f;
    public Ease deactivateEase = Ease.InOutQuad;

    [Range(0.1f, 2f)] public float activateDuration = 0.8f;
    public float activateStartScale = 0.85f;
    public float activateOvershootScale = 1.15f;
    public float activateEndScale = 1.0f;
    [Tooltip("How much of the duration is spent going StartScale -> OvershootScale (0..1).")]
    [Range(0.05f, 0.95f)] public float activateOvershootPortion = 0.4f;
    public Ease activateEaseUp = Ease.Linear;
    public Ease activateEaseDown = Ease.OutQuad;

    [Tooltip("Extra intensity if you want the heart to feel 'more alive'. 1 = normal.")]
    [Range(0.2f, 2f)] public float crossfadeAlphaMultiplier = 1f;

    [Header("Fill Sprites")]
    [SerializeField] Sprite baseSpriteUsed;
    [SerializeField] Sprite fillSprite;
    [SerializeField, Range(0.05f, 2f)] float fillDuration = 1.9f;

    [Header("Footprint Frame Glow")]
    public Frame_GlowUp frameGlow_Steps;
    public Frame_GlowUp frameGlow_Numbers;
    public Frame_GlowUp frameGlow_NumbersSteps;

    [Header("stepsIconList")]
    [SerializeField] List<GameObject> stepsIconList = new List<GameObject>();

    public float footprint_SpawnTime = 0.1f;
    public float StepsDisplay_RespawnTime = 0.5f;
    public float StepsDisplay_CheckpointTime = 0.65f;
    
    [Header("Checkpoint Glow")]
    [SerializeField] float checkpointGlowHoldTime = 0.18f;

    public int stepCounter;

    private const int ExtraFootstepStartIndex = 7;

    private Coroutine[] _runningFill;
    private Coroutine _refreshRoutine;
    private Coroutine _shineRoutine;

    private Image[] _images;
    private RectTransform[] _rects;
    private UIHeartAnimator[] _animators;


    //--------------------


    private void Awake()
    {
        CacheReferences();

        if (!frameGlow_Steps)
            frameGlow_Steps = GetComponentInChildren<Frame_GlowUp>(true);
    }

    private void Start()
    {
        RefreshAllFootprintsImmediate(false);
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


    private void CacheReferences()
    {
        int count = stepsIconList.Count;

        _runningFill = new Coroutine[count];
        _images = new Image[count];
        _rects = new RectTransform[count];
        _animators = new UIHeartAnimator[count];

        for (int i = 0; i < count; i++)
        {
            if (stepsIconList[i] == null)
                continue;

            _images[i] = stepsIconList[i].GetComponent<Image>();
            _rects[i] = stepsIconList[i].GetComponent<RectTransform>();
            _animators[i] = stepsIconList[i].GetComponent<UIHeartAnimator>();
        }
    }

    private bool HasRequiredReferences()
    {
        return PlayerStats.Instance != null &&
               PlayerStats.Instance.stats != null &&
               StepsDisplay.Instance != null;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 &&
               index < stepsIconList.Count &&
               _images != null &&
               index < _images.Length &&
               _images[index] != null;
    }

    private bool IsExtraFootstep(int index)
    {
        return index >= ExtraFootstepStartIndex;
    }

    private Sprite GetUsedSprite(int index)
    {
        return IsExtraFootstep(index)
            ? StepsDisplay.Instance.extraFootstep_Used
            : StepsDisplay.Instance.normalFootstep_Used;
    }

    private Sprite GetActiveSprite(int index)
    {
        return IsExtraFootstep(index)
            ? StepsDisplay.Instance.extraFootstep_Active
            : StepsDisplay.Instance.normalFootstep_Active;
    }

    private bool IsStandingOnMushroomCircle()
    {
        return Movement.Instance != null &&
               Movement.Instance.blockStandingOn != null &&
               Movement.Instance.blockStandingOn.TryGetComponent(out EffectBlockInfo effectInfo) &&
               effectInfo.effectBlock_MushroomCircle_isAdded;
    }


    //--------------------


    public void UpdateStepsDisplay_Walking()
    {
        // Checkpoints have their own delayed refill timing.
        // Do not update immediately when the player steps onto one.
        if (IsStandingOnCheckpoint())
            return;

        RefreshAllFootprintsImmediate(true);

        if (IsStandingOnMushroomCircle())
            StartFootstepsFrameShine(0.25f);
    }

    public void RefreshAllFootprintsImmediate(bool allowAnimations = false)
    {
        if (!HasRequiredReferences())
            return;

        for (int i = 0; i < stepsIconList.Count; i++)
        {
            UpdateFootprints(i, allowAnimations);
        }
    }

    void UpdateFootprints(int index)
    {
        UpdateFootprints(index, true);
    }

    void UpdateFootprints(int index, bool allowAnimations)
    {
        if (!IsValidIndex(index))
            return;

        int stepsMax = PlayerStats.Instance.stats.steps_Max;
        int stepsCurrent = PlayerStats.Instance.stats.steps_Current;

        if (stepsMax < index + 1)
        {
            ApplyPassiveState(index);
        }
        else if (stepsCurrent >= index + 1)
        {
            ApplyActiveState(index, allowAnimations);
        }
        else
        {
            ApplyUsedState(index, allowAnimations);
        }
    }

    private void ApplyPassiveState(int index)
    {
        StopFillAnimation(index);

        if (_animators[index] != null)
            _animators[index].ForceSetHidden();
        else
            ApplyDirectVisual(index, StepsDisplay.Instance.extraFootstep_Passive, deactivateEndScale, false);
    }

    private void ApplyUsedState(int index, bool allowAnimations)
    {
        StopFillAnimation(index);

        if (_animators[index] != null)
        {
            bool shouldAnimate = allowAnimations && _animators[index].isActive;

            if (shouldAnimate)
                _animators[index].Deactivate();
            else
                _animators[index].ForceSetUsed();
        }
        else
        {
            ApplyDirectVisual(index, GetUsedSprite(index), deactivateEndScale, false);
        }
    }

    private void ApplyActiveState(int index, bool allowAnimations)
    {
        StopFillAnimation(index);

        if (_animators[index] != null)
        {
            bool shouldAnimate = allowAnimations && !_animators[index].isActive;

            if (shouldAnimate)
                _animators[index].Activate();
            else
                _animators[index].ForceSetActive();
        }
        else
        {
            ApplyDirectVisual(index, GetActiveSprite(index), activateEndScale, true);
        }
    }

    private void ApplyDirectVisual(int index, Sprite sprite, float scale, bool active)
    {
        if (!IsValidIndex(index))
            return;

        _images[index].sprite = sprite;

        if (_rects[index] != null)
            _rects[index].localScale = Vector3.one * scale;

        if (_animators[index] != null)
        {
            _animators[index].isActive = active;
            _animators[index].StopAnimationAndHideOverlay();
        }

        HideFillOverlay(index);
    }

    private void StopFillAnimation(int index)
    {
        if (!IsValidIndex(index))
            return;

        if (_runningFill[index] != null)
        {
            StopCoroutine(_runningFill[index]);
            _runningFill[index] = null;
        }

        if (_animators[index] != null)
            _animators[index].StopAnimationAndHideOverlay();

        HideFillOverlay(index);
    }

    private void HideFillOverlay(int index)
    {
        if (!IsValidIndex(index))
            return;

        Transform overlay = _images[index].transform.Find("FillOverlay");
        if (overlay != null)
            overlay.gameObject.SetActive(false);
    }


    //--------------------


    void GetExtraFootprint()
    {
        for (int i = ExtraFootstepStartIndex; i < stepsIconList.Count; i++)
        {
            if (PlayerStats.Instance.stats.steps_Max < i + 1)
            {
                ApplyPassiveState(i);
            }
            else
            {
                if (_images[i] != null &&
                    _images[i].sprite == StepsDisplay.Instance.extraFootstep_Passive &&
                    PlayerStats.Instance.stats.steps_Current < i + 1)
                {
                    FillFootstep(i);

                    if (_rects[i] != null)
                        _rects[i].localScale = Vector3.one * deactivateEndScale;
                }
                else
                {
                    UpdateFootprints(i, false);
                }
            }
        }
    }

    public void FillFootstep(int index)
    {
        if (!IsValidIndex(index))
            return;

        StopFillAnimation(index);

        _images[index].sprite = baseSpriteUsed;

        if (_runningFill[index] != null)
            StopCoroutine(_runningFill[index]);

        _runningFill[index] = StartCoroutine(CoFillBottomToTop(_images[index], fillSprite, fillDuration, index));
    }

    private IEnumerator CoFillBottomToTop(Image baseImage, Sprite overlaySprite, float duration, int index)
    {
        GlowUpAll();

        Image overlay = EnsureFillOverlay(baseImage);

        overlay.gameObject.SetActive(true);
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
            float eased = 1f - Mathf.Pow(1f - u, 3f);

            overlay.fillAmount = eased;
            yield return null;
        }

        overlay.fillAmount = 1f;

        baseImage.sprite = StepsDisplay.Instance.extraFootstep_Used;
        overlay.gameObject.SetActive(false);

        _runningFill[index] = null;

        GlowDownAll();
    }

    private Image EnsureFillOverlay(Image baseImage)
    {
        Transform existing = baseImage.transform.Find("FillOverlay");
        if (existing != null)
            return existing.GetComponent<Image>();

        GameObject go = new GameObject("FillOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(baseImage.transform, false);

        RectTransform rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        Image overlay = go.GetComponent<Image>();
        overlay.raycastTarget = false;
        overlay.preserveAspect = baseImage.preserveAspect;
        overlay.material = baseImage.material;

        go.transform.SetAsLastSibling();

        return overlay;
    }


    //--------------------


    public void UpdateStepsDisplay_Respawn()
    {
        RestartRefreshRoutine(StepsDisplay_RespawnTime, footprint_SpawnTime);
    }

    public void UpdateStepsDisplay_Checkpoint()
    {
        if (_refreshRoutine != null)
            StopCoroutine(_refreshRoutine);

        _refreshRoutine = StartCoroutine(UpdateCheckpointFootprintsDelay());
    }
    private IEnumerator UpdateCheckpointFootprintsDelay()
    {
        yield return new WaitForSeconds(StepsDisplay_CheckpointTime);

        GlowUpAll();

        // Let the glow begin easing in before the icons change.
        yield return new WaitForSeconds(0.05f);

        // Checkpoint refill should happen all at once.
        RefreshAllFootprintsImmediate(true);

        // Keep the frame visible for a short moment so the in/out feels smoother.
        yield return new WaitForSeconds(checkpointGlowHoldTime);

        GlowDownAll();

        _refreshRoutine = null;
    }

    private void RestartRefreshRoutine(float startDelay, float waitTime)
    {
        if (_refreshRoutine != null)
            StopCoroutine(_refreshRoutine);

        _refreshRoutine = StartCoroutine(UpdateFootprintDelay(startDelay, waitTime));
    }

    IEnumerator UpdateFootprintDelay(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay);

        GlowUpAll();

        for (int i = 0; i < stepsIconList.Count; i++)
        {
            UpdateFootprints(i, true);
            yield return new WaitForSeconds(waitTime);
        }

        GlowDownAll();
        _refreshRoutine = null;
    }


    //--------------------


    private void StartFootstepsFrameShine(float waitTime)
    {
        if (_shineRoutine != null)
            StopCoroutine(_shineRoutine);

        _shineRoutine = StartCoroutine(FootstepsFrameShine_InOut(waitTime));
    }

    IEnumerator FootstepsFrameShine_InOut(float waitTime)
    {
        GlowUpAll();

        yield return new WaitForSeconds(waitTime);

        GlowDownAll();
        _shineRoutine = null;
    }

    private void GlowUpAll()
    {
        SafeGlowUp(frameGlow_Steps);
        SafeGlowUp(frameGlow_Numbers);
        SafeGlowUp(frameGlow_NumbersSteps);
    }

    private void GlowDownAll()
    {
        SafeGlowDown(frameGlow_Steps);
        SafeGlowDown(frameGlow_Numbers);
        SafeGlowDown(frameGlow_NumbersSteps);
    }

    private void SafeGlowUp(Frame_GlowUp glow)
    {
        if (glow != null && glow.gameObject.activeInHierarchy)
            glow.GlowUp();
    }

    private void SafeGlowDown(Frame_GlowUp glow)
    {
        if (glow != null && glow.gameObject.activeInHierarchy)
            glow.GlowDown();
    }

    #region Helpers

    private bool IsStandingOnCheckpoint()
    {
        return Movement.Instance != null &&
               Movement.Instance.blockStandingOn != null &&
               Movement.Instance.blockStandingOn.GetComponent<Block_Checkpoint>() != null;
    }

    #endregion
}