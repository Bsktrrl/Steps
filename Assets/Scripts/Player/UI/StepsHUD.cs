using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    [Header("Footprint Frame")]
    [SerializeField] Image frameImage;

    [SerializeField] Sprite framePassive;
    [SerializeField] Sprite frameActive;

    [SerializeField, Range(0.05f, 2f)] float startDuration = 0.6f;
    [SerializeField] float startFromScale = 1.2f;
    [SerializeField] float startToScale = 1.23f;

    [SerializeField, Range(0.05f, 2f)] float endDuration = 0.6f;
    [SerializeField] float endFromScale = 1.23f;
    [SerializeField] float endToScale = 1.2f;

    RectTransform _rt;
    Image _overlay;
    Coroutine _co;


    [Header("stepsIconList")]
    [SerializeField] List<GameObject> stepsIconList = new List<GameObject>();

    float footprintSpawnTime = 0.1f;

    bool firstTimeFrameActivates;



    //--------------------


    private void Awake()
    {
        _running = new Coroutine[stepsIconList.Count];

        if (!frameImage) frameImage = GetComponent<Image>();
        _rt = frameImage.rectTransform;
        EnsureOverlay();
    }

    private void OnEnable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot += GetExtraFootprint;
        Movement.Action_StepTaken += UpdateStepsDisplay_Walking;
        //Block_MushroomCircle.Action_MushroomCircleEntered += UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate += UpdateStepsDisplay_Respawn;

        //DataManager.Action_dataHasLoaded += UpdateStepsDisplay_Checkpoint;
        Block_Checkpoint.Action_CheckPointEntered += UpdateStepsDisplay_Checkpoint;
        //Block_RefillSteps.Action_RefillStepsEntered += UpdateStepsDisplay_Respawn;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot -= GetExtraFootprint;
        Movement.Action_StepTaken -= UpdateStepsDisplay_Walking;
        //Block_MushroomCircle.Action_MushroomCircleEntered -= UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate -= UpdateStepsDisplay_Respawn;

        //DataManager.Action_dataHasLoaded -= UpdateStepsDisplay_Checkpoint;
        Block_Checkpoint.Action_CheckPointEntered -= UpdateStepsDisplay_Checkpoint;
        //Block_RefillSteps.Action_RefillStepsEntered -= UpdateStepsDisplay_Respawn;
    }


    //--------------------


    public void UpdateStepsDisplay_Walking()
    {
        if (Movement.Instance.blockStandingOn && (Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>() && !Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_SpawnPoint_isAdded)
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

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_MushroomCircle_isAdded)
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
                print("1. MaxStep < " + (i + 1) + " | MaxSteps: "+ PlayerStats.Instance.stats.steps_Max);
                stepsIconList[i].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Passive;
                stepsIconList[i].GetComponent<RectTransform>().localScale = new Vector3(deactivateEndScale, deactivateEndScale, deactivateEndScale);
            }
            else
            {
                print("2. MaxStep >= " + (i + 1) + " | MaxSteps: " + PlayerStats.Instance.stats.steps_Max);
                //If steps current are high as extra steps (above 7), don't do anything
                if (stepsIconList[i].GetComponent<Image>().sprite != StepsDisplay.Instance.extraFootstep_Passive)
                {
                    
                }
                else if (PlayerStats.Instance.stats.steps_Current >= i + 1)
                {
                    
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
        FootstepsFrameShine_Start();

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

        FootstepsFrameShine_End();
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
        //print("1. UpdateStepsDisplay_Respawn");
        StartCoroutine(UpdateFootprintDelay(0.5f, footprintSpawnTime));
    }
    public void UpdateStepsDisplay_Checkpoint()
    {
        //print("2. UpdateStepsDisplay_Checkpoint");
        StartCoroutine(UpdateFootprintDelay(0.65f, footprintSpawnTime));
    }
    IEnumerator UpdateFootprintDelay(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay);

        FootstepsFrameShine_Start();

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

        FootstepsFrameShine_End();
    }


    //--------------------


    public void FootstepsFrameShine_Start()
    {
        //if (firstTimeFrameActivates)
        //{
        //    if (_co != null) StopCoroutine(_co);
        //    _co = StartCoroutine(CoShineStart());
        //}

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoShineStart());
    }
    public void FootstepsFrameShine_End()
    {
        //if (firstTimeFrameActivates)
        //{
        //    if (_co != null) StopCoroutine(_co);
        //    _co = StartCoroutine(CoShineEnd());
        //}

        //firstTimeFrameActivates = true;

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoShineEnd());
    }
    IEnumerator FootstepsFrameShine_InOut(float WaitTime)
    {
        FootstepsFrameShine_Start();

        yield return new WaitForSeconds(WaitTime);

        FootstepsFrameShine_End();
    }

    private IEnumerator CoShineStart()
    {
        EnsureOverlay();

        // Base is passive, overlay fades in active
        frameImage.sprite = framePassive;
        SetAlpha(frameImage, 1f);

        _overlay.sprite = frameActive;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * startFromScale;

        yield return ScaleAndCrossfade(
            fromScale: startFromScale,
            toScale: startToScale,
            duration: startDuration,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f,
            ease: EaseOutCubic
        );

        // Commit final state
        frameImage.sprite = frameActive;
        SetAlpha(_overlay, 0f);
        _rt.localScale = Vector3.one * startToScale;

        _co = null;
    }
    private IEnumerator CoShineEnd()
    {
        EnsureOverlay();

        // Base is active, overlay fades in passive
        frameImage.sprite = frameActive;
        SetAlpha(frameImage, 1f);

        _overlay.sprite = framePassive;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * endFromScale;

        yield return ScaleAndCrossfade(
            fromScale: endFromScale,
            toScale: endToScale,
            duration: endDuration,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f,
            ease: EaseInCubic
        );

        // Commit final state
        frameImage.sprite = framePassive;
        SetAlpha(_overlay, 0f);
        _rt.localScale = Vector3.one * endToScale;

        _co = null;
    }

    private IEnumerator ScaleAndCrossfade(
        float fromScale,
        float toScale,
        float duration,
        float overlayAlphaFrom,
        float overlayAlphaTo,
        System.Func<float, float> ease
    )
    {
        float t = 0f;
        duration = Mathf.Max(0.0001f, duration);

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / duration);
            float e = ease(u);

            float s = Mathf.LerpUnclamped(fromScale, toScale, e);
            _rt.localScale = Vector3.one * s;

            float a = Mathf.LerpUnclamped(overlayAlphaFrom, overlayAlphaTo, e);
            SetAlpha(_overlay, Mathf.Clamp01(a));

            yield return null;
        }

        _rt.localScale = Vector3.one * toScale;
        SetAlpha(_overlay, Mathf.Clamp01(overlayAlphaTo));
    }
    private void EnsureOverlay()
    {
        if (_overlay) return;

        // Try find existing (in case you duplicated objects)
        var existing = frameImage.transform.Find("ShineOverlay");
        if (existing != null)
        {
            _overlay = existing.GetComponent<Image>();
            _overlay.raycastTarget = false;
            SetAlpha(_overlay, 0f);
            return;
        }

        // Create overlay child
        var go = new GameObject("ShineOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(frameImage.transform, false);
        go.transform.SetAsLastSibling(); // render on top

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        _overlay = go.GetComponent<Image>();
        _overlay.raycastTarget = false;

        // Match display settings
        _overlay.preserveAspect = frameImage.preserveAspect;
        _overlay.material = frameImage.material;
        _overlay.type = frameImage.type;

        SetAlpha(_overlay, 0f);
    }
    private static void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    // Non-linear easing
    private static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 3f);
    private static float EaseInCubic(float t) => Mathf.Pow(Mathf.Clamp01(t), 3f);
}
