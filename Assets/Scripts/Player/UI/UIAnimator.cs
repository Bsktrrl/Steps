
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Image targetImage;

    [Header("Inspector Presets (optional)")]
    [SerializeField] private AnimConfig startConfig = AnimConfig.DefaultStart();
    [SerializeField] private AnimConfig endConfig = AnimConfig.DefaultEnd();

    private RectTransform _rt;
    private Image _overlay;
    private Coroutine _running;


    //--------------------


    // Same ease set as earlier
    public enum Ease
    {
        Linear,
        InQuad, OutQuad, InOutQuad,
        InCubic, OutCubic, InOutCubic,
        OutBack
    }

    [Serializable]
    public struct AnimConfig
    {
        [Header("Timing")]
        [Min(0.0001f)] public float duration;
        public bool useUnscaledTime;

        [Header("Sprites (optional crossfade)")]
        public Sprite fromSprite; // what the base image starts showing
        public Sprite toSprite;   // what the overlay fades to (then gets committed)

        [Header("Fade")]
        [Range(0f, 1f)] public float fromAlpha; // base alpha (usually 1)
        [Range(0f, 1f)] public float toAlpha;   // base alpha (usually 1)
        [Range(0f, 2f)] public float overlayAlphaMultiplier; // 1 = normal

        [Header("Scale (supports overshoot pop)")]
        public float startScale;      // e.g. 0 or 0.8 or 1
        public float overshootScale;  // e.g. 1.2 (ignored if overshoot disabled)
        public float endScale;        // e.g. 1

        public bool useOvershoot;
        [Range(0.05f, 0.95f)] public float overshootPortion; // how much time in phase 1

        [Header("Easing")]
        public Ease easeUp;     // start -> overshoot
        public Ease easeDown;   // overshoot -> end (or single-phase)

        public static AnimConfig DefaultStart()
        {
            return new AnimConfig
            {
                duration = 0.20f,
                useUnscaledTime = true,

                fromSprite = null,
                toSprite = null,

                fromAlpha = 1f,
                toAlpha = 1f,
                overlayAlphaMultiplier = 1f,

                startScale = 1f,
                useOvershoot = true,
                overshootScale = 1.2f,
                endScale = 1f,
                overshootPortion = 0.65f,

                easeUp = Ease.OutBack,
                easeDown = Ease.OutCubic
            };
        }

        public static AnimConfig DefaultEnd()
        {
            return new AnimConfig
            {
                duration = 0.18f,
                useUnscaledTime = true,

                fromSprite = null,
                toSprite = null,

                fromAlpha = 1f,
                toAlpha = 1f,
                overlayAlphaMultiplier = 1f,

                startScale = 1.2f,
                useOvershoot = false,
                overshootScale = 1.2f,
                endScale = 1f,
                overshootPortion = 0.65f,

                easeUp = Ease.InCubic,
                easeDown = Ease.InCubic
            };
        }
    }


    //--------------------


    private void Reset()
    {
        targetImage = GetComponent<Image>();
    }

    private void Awake()
    {
        if (!targetImage) targetImage = GetComponent<Image>();
        _rt = targetImage.rectTransform;
        EnsureOverlay();
    }


    //--------------------


    // --- Easy-to-use entry points (Inspector presets) ---
    public void Anim_Start() => Play(startConfig);
    public void Anim_End() => Play(endConfig);


    //--------------------


    // --- Same-parameter versions (both take AnimConfig) ---
    public void Anim_Start(AnimConfig config) => Play(config);
    public void Anim_End(AnimConfig config) => Play(config);


    //--------------------


    private void Play(AnimConfig config)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoPlay(config));
    }

    private IEnumerator CoPlay(AnimConfig config)
    {
        EnsureOverlay();

        // Base setup
        if (config.fromSprite != null) targetImage.sprite = config.fromSprite;
        SetAlpha(targetImage, config.fromAlpha);

        // Overlay setup (only meaningful if toSprite exists; otherwise overlay just stays invisible)
        if (config.toSprite != null)
        {
            _overlay.sprite = config.toSprite;
            SetAlpha(_overlay, 0f);
        }
        else
        {
            SetAlpha(_overlay, 0f);
        }

        // Scale start
        _rt.localScale = Vector3.one * config.startScale;

        // Animate
        if (config.useOvershoot)
        {
            float upTime = Mathf.Max(0.0001f, config.duration * config.overshootPortion);
            float downTime = Mathf.Max(0.0001f, config.duration - upTime);

            // Phase 1: start -> overshoot, overlay fades in (if sprite provided)
            yield return ScaleFadePhase(
                config, config.startScale, config.overshootScale, upTime,
                config.easeUp,
                overlayA0: 0f,
                overlayA1: (config.toSprite != null) ? 1f : 0f
            );

            // Phase 2: overshoot -> end, keep overlay at full (if used)
            yield return ScaleFadePhase(
                config, config.overshootScale, config.endScale, downTime,
                config.easeDown,
                overlayA0: (config.toSprite != null) ? 1f : 0f,
                overlayA1: (config.toSprite != null) ? 1f : 0f
            );
        }
        else
        {
            // Single phase: start -> end, overlay fades in (if sprite provided)
            yield return ScaleFadePhase(
                config, config.startScale, config.endScale, config.duration,
                config.easeDown,
                overlayA0: 0f,
                overlayA1: (config.toSprite != null) ? 1f : 0f
            );
        }

        // Commit sprite (so we end with a clean single image)
        if (config.toSprite != null)
        {
            targetImage.sprite = config.toSprite;
            SetAlpha(_overlay, 0f);
        }

        // Commit base alpha/end scale
        SetAlpha(targetImage, config.toAlpha);
        _rt.localScale = Vector3.one * config.endScale;

        _running = null;
    }

    private IEnumerator ScaleFadePhase(
        AnimConfig config,
        float fromScale, float toScale, float duration,
        Ease ease,
        float overlayA0, float overlayA1
    )
    {
        float t = 0f;
        duration = Mathf.Max(0.0001f, duration);

        while (t < duration)
        {
            t += config.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float u = Mathf.Clamp01(t / duration);
            float e = ApplyEase(ease, u);

            // scale
            float s = Mathf.LerpUnclamped(fromScale, toScale, e);
            _rt.localScale = Vector3.one * s;

            // overlay alpha (only matters if overlay is being used)
            float oa = Mathf.LerpUnclamped(overlayA0, overlayA1, e) * config.overlayAlphaMultiplier;
            SetAlpha(_overlay, Mathf.Clamp01(oa));

            yield return null;
        }

        _rt.localScale = Vector3.one * toScale;
        float finalA = overlayA1 * config.overlayAlphaMultiplier;
        SetAlpha(_overlay, Mathf.Clamp01(finalA));
    }

    private void EnsureOverlay()
    {
        if (_overlay) return;

        // If already exists (prefab duplication etc.)
        var existing = targetImage.transform.Find("UI_Overlay");
        if (existing != null)
        {
            _overlay = existing.GetComponent<Image>();
            _overlay.raycastTarget = false;
            SetAlpha(_overlay, 0f);
            return;
        }

        // Create overlay child
        var go = new GameObject("UI_Overlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(targetImage.transform, false);
        go.transform.SetAsLastSibling(); // render on top

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        _overlay = go.GetComponent<Image>();
        _overlay.raycastTarget = false;

        // Match some visual settings
        _overlay.preserveAspect = targetImage.preserveAspect;
        _overlay.material = targetImage.material;
        _overlay.type = targetImage.type;

        SetAlpha(_overlay, 0f);
    }

    private static void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private static float ApplyEase(Ease ease, float t)
    {
        t = Mathf.Clamp01(t);

        switch (ease)
        {
            case Ease.Linear: return t;

            case Ease.InQuad: return t * t;
            case Ease.OutQuad: return 1f - (1f - t) * (1f - t);
            case Ease.InOutQuad:
                return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

            case Ease.InCubic: return t * t * t;
            case Ease.OutCubic: return 1f - Mathf.Pow(1f - t, 3f);
            case Ease.InOutCubic:
                return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;

            case Ease.OutBack:
                const float c1 = 1.70158f;
                const float c3 = c1 + 1f;
                return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

            default:
                return t;
        }
    }
}
