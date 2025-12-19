using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Image targetImage;

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;     // “original”
    [SerializeField] private Sprite activeSprite;   // “during ability”

    [Header("Appearing")]
    [SerializeField, Min(0.0001f)] private float appearDuration = 0.25f;
    [SerializeField] private float appearStartScale = 0f;
    [SerializeField] private float appearOvershootScale = 1.2f;
    [SerializeField] private float appearEndScale = 1f;
    [SerializeField, Range(0.05f, 0.95f)] private float appearOvershootPortion = 0.65f;
    [SerializeField] private Ease appearEaseUp = Ease.OutBack;
    [SerializeField] private Ease appearEaseDown = Ease.OutCubic;

    [Header("Activating (Idle -> Active)")]
    [SerializeField, Min(0.0001f)] private float activateDuration = 0.20f;
    [SerializeField] private float activateStartScale = 1f;
    [SerializeField] private float activateEndScale = 1.2f;
    [SerializeField] private Ease activateEase = Ease.OutCubic;

    [Header("Deactivating (Active -> Idle)")]
    [SerializeField, Min(0.0001f)] private float deactivateDuration = 0.18f;
    [SerializeField] private float deactivateEndScale = 1f; // starts from Activating end (1.2)
    [SerializeField] private Ease deactivateEase = Ease.InCubic;

    [Header("Timing")]
    [SerializeField] private bool useUnscaledTime = true;

    private RectTransform _rt;
    private Image _overlay;
    private Coroutine _co;

    public bool isActive;


    public enum Ease
    {
        Linear,
        InQuad, OutQuad, InOutQuad,
        InCubic, OutCubic, InOutCubic,
        OutBack
    }

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

    /// <summary>
    /// Appearing: scale 0 -> 1.2 -> 1 with easing (no sprite crossfade unless you set idleSprite).
    /// </summary>
    public void Appearing()
    {
        isActive = true;

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoAppearing());
    }

    /// <summary>
    /// Activating: crossfade idle -> active while scaling 1 -> 1.2 (no overshoot).
    /// </summary>
    public void Activating()
    {
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoActivating());
    }

    /// <summary>
    /// Deactivating: crossfade active -> idle while scaling from current (typically 1.2) -> 1 (no overshoot).
    /// </summary>
    public void Deactivating()
    {
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoDeactivating());
    }

    private IEnumerator CoAppearing()
    {
        EnsureOverlay();

        // Commit base sprite (optional, but usually you want it set)
        if (idleSprite) targetImage.sprite = idleSprite;
        SetAlpha(targetImage, 1f);
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * appearStartScale;

        float upTime = Mathf.Max(0.0001f, appearDuration * appearOvershootPortion);
        float downTime = Mathf.Max(0.0001f, appearDuration - upTime);

        yield return ScaleOnly(appearStartScale, appearOvershootScale, upTime, appearEaseUp);
        yield return ScaleOnly(appearOvershootScale, appearEndScale, downTime, appearEaseDown);

        _rt.localScale = Vector3.one * appearEndScale;
        _co = null;
    }

    private IEnumerator CoActivating()
    {
        EnsureOverlay();

        // Base shows idle, overlay fades in active
        if (idleSprite) targetImage.sprite = idleSprite;
        SetAlpha(targetImage, 1f);

        if (activeSprite) _overlay.sprite = activeSprite;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * activateStartScale;

        yield return ScaleAndOverlayFade(
            fromScale: activateStartScale,
            toScale: activateEndScale,
            duration: activateDuration,
            ease: activateEase,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f
        );

        // Commit final: base becomes active, overlay hidden
        if (activeSprite) targetImage.sprite = activeSprite;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * activateEndScale;
        _co = null;
    }

    private IEnumerator CoDeactivating()
    {
        EnsureOverlay();

        // Start from “whatever scale we’re currently at” (typically 1.2 after Activating)
        float startScale = _rt.localScale.x;

        // Base shows active, overlay fades in idle
        if (activeSprite) targetImage.sprite = activeSprite;
        SetAlpha(targetImage, 1f);

        if (idleSprite) _overlay.sprite = idleSprite;
        SetAlpha(_overlay, 0f);

        yield return ScaleAndOverlayFade(
            fromScale: startScale,
            toScale: deactivateEndScale,
            duration: deactivateDuration,
            ease: deactivateEase,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f
        );

        // Commit final: base becomes idle, overlay hidden
        if (idleSprite) targetImage.sprite = idleSprite;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * deactivateEndScale;
        _co = null;
    }

    // ---- Helpers ----

    private IEnumerator ScaleOnly(float fromScale, float toScale, float duration, Ease ease)
    {
        float t = 0f;
        duration = Mathf.Max(0.0001f, duration);

        while (t < duration)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float u = Mathf.Clamp01(t / duration);
            float e = ApplyEase(ease, u);

            float s = Mathf.LerpUnclamped(fromScale, toScale, e);
            _rt.localScale = Vector3.one * s;

            yield return null;
        }

        _rt.localScale = Vector3.one * toScale;
    }

    private IEnumerator ScaleAndOverlayFade(
        float fromScale, float toScale,
        float duration, Ease ease,
        float overlayAlphaFrom, float overlayAlphaTo
    )
    {
        float t = 0f;
        duration = Mathf.Max(0.0001f, duration);

        while (t < duration)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float u = Mathf.Clamp01(t / duration);
            float e = ApplyEase(ease, u);

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

        // Reuse if already exists
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

        // Match common settings
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
