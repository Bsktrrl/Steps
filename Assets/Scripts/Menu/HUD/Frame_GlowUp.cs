using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Frame_GlowUp : MonoBehaviour
{
    [Header("Target Image (the frame Image on this GameObject)")]
    [SerializeField] private Image imageToGlowUp;

    [Header("Sprites")]
    [SerializeField] private Sprite spritePassive;
    [SerializeField] private Sprite spriteActive;

    [Header("Glow Up (Passive -> Active)")]
    [SerializeField, Range(0.05f, 2f)] private float startDuration = 0.6f;
    [SerializeField] private float startFromScale = 1.2f;
    [SerializeField] private float startToScale = 1.23f;

    [Header("Glow Down (Active -> Passive)")]
    [SerializeField, Range(0.05f, 2f)] private float endDuration = 0.6f;
    [SerializeField] private float endFromScale = 1.23f;
    [SerializeField] private float endToScale = 1.2f;

    private RectTransform _rt;
    private Image _overlay;
    private Coroutine _co;


    //--------------------


    private void Awake()
    {
        if (!imageToGlowUp) imageToGlowUp = GetComponent<Image>();

        if (!imageToGlowUp)
        {
            Debug.LogError($"{nameof(Frame_GlowUp)} on '{name}' needs an Image reference.", this);
            enabled = false;
            return;
        }

        _rt = imageToGlowUp.rectTransform;
        EnsureOverlay();
    }

    private void OnDisable()
    {
        if (_co != null)
        {
            StopCoroutine(_co);
            _co = null;
        }
    }


    //--------------------


    public void GlowUp()
    {
        if (!enabled) return;

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoGlowUp());
    }

    public void GlowDown()
    {
        if (!enabled) return;

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoGlowDown());
    }


    //--------------------


    private IEnumerator CoGlowUp()
    {
        EnsureOverlay();

        // Base is passive, overlay fades in active
        imageToGlowUp.sprite = spritePassive;
        SetAlpha(imageToGlowUp, 1f);

        _overlay.sprite = spriteActive;
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
        imageToGlowUp.sprite = spriteActive;
        SetAlpha(_overlay, 0f);
        _rt.localScale = Vector3.one * startToScale;

        _co = null;
    }

    private IEnumerator CoGlowDown()
    {
        EnsureOverlay();

        // Base is active, overlay fades in passive
        imageToGlowUp.sprite = spriteActive;
        SetAlpha(imageToGlowUp, 1f);

        _overlay.sprite = spritePassive;
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
        imageToGlowUp.sprite = spritePassive;
        SetAlpha(_overlay, 0f);
        _rt.localScale = Vector3.one * endToScale;

        _co = null;
    }

    private IEnumerator ScaleAndCrossfade(float fromScale, float toScale, float duration, float overlayAlphaFrom, float overlayAlphaTo, Func<float, float> ease)
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

        // Try find existing (in case duplicated objects)
        var existing = imageToGlowUp.transform.Find("ShineOverlay");
        if (existing != null)
        {
            _overlay = existing.GetComponent<Image>();
            _overlay.raycastTarget = false;
            CopyCommonImageSettings(imageToGlowUp, _overlay);
            SetAlpha(_overlay, 0f);
            return;
        }

        // Create overlay child
        var go = new GameObject("ShineOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(imageToGlowUp.transform, false);
        go.transform.SetAsLastSibling(); // render on top

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        _overlay = go.GetComponent<Image>();
        _overlay.raycastTarget = false;

        CopyCommonImageSettings(imageToGlowUp, _overlay);
        SetAlpha(_overlay, 0f);
    }

    private static void CopyCommonImageSettings(Image src, Image dst)
    {
        dst.preserveAspect = src.preserveAspect;
        dst.material = src.material;
        dst.type = src.type;
        dst.pixelsPerUnitMultiplier = src.pixelsPerUnitMultiplier;
        dst.useSpriteMesh = src.useSpriteMesh;
        dst.maskable = src.maskable;
    }

    private static void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 3f);
    private static float EaseInCubic(float t) => Mathf.Pow(Mathf.Clamp01(t), 3f);
}
