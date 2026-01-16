using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartAnimator : MonoBehaviour
{
    [Header("References")]
    public Image targetImage;

    [Header("Sprites")]
    public Sprite activeSprite;
    public Sprite usedSprite;
    public Sprite hiddenSprite;

    [SerializeField] int extraStep_Number;
    public bool isActive;

    private RectTransform _rt;
    private Image _overlay;              // child image used for crossfade
    private Coroutine _running;


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
    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetStartFootprints;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetStartFootprints;
    }


    //--------------------


    void SetStartFootprints()
    {
        if ((extraStep_Number == 1 && PlayerStats.Instance.stats.steps_Max >= 8) || (extraStep_Number == 2 && PlayerStats.Instance.stats.steps_Max >= 9) || (extraStep_Number == 3 && PlayerStats.Instance.stats.steps_Max >= 10))
        {
            targetImage.sprite = usedSprite;
        }
        else if (extraStep_Number > 0)
        {
            targetImage.sprite = hiddenSprite;
        }
        else
        {
            targetImage.sprite = usedSprite;
        }
    }


    //--------------------


    public void Activate()
    {
        if (StepsHUD.Instance.frameGlow_Numbers.gameObject.activeInHierarchy) return;
        if (!StepsHUD.Instance.frameGlow_Steps.gameObject.activeInHierarchy && !StepsHUD.Instance.frameGlow_NumbersSteps.gameObject.activeInHierarchy && !StepsHUD.Instance.frameGlow_Numbers.gameObject.activeInHierarchy) return;

        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoActivate());
    }
    public void Deactivate()
    {
        if (StepsHUD.Instance.frameGlow_Numbers.gameObject.activeInHierarchy) return;
        if (!StepsHUD.Instance.frameGlow_Steps.gameObject.activeInHierarchy && !StepsHUD.Instance.frameGlow_NumbersSteps.gameObject.activeInHierarchy && !StepsHUD.Instance.frameGlow_Numbers.gameObject.activeInHierarchy) return;

        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoDeactivate());
    }

    private IEnumerator CoActivate()
    {
        isActive = true;

        EnsureOverlay();

        // Base sprite stays "used"; overlay fades in "active".
        targetImage.sprite = usedSprite;
        SetAlpha(targetImage, 1f);

        _overlay.sprite = activeSprite;
        SetAlpha(_overlay, 0f);

        // Start scale
        _rt.localScale = Vector3.one * StepsHUD.Instance.activateStartScale;

        float upTime = Mathf.Max(0.0001f, StepsHUD.Instance.activateDuration * StepsHUD.Instance.activateOvershootPortion);
        float downTime = Mathf.Max(0.0001f, StepsHUD.Instance.activateDuration - upTime);

        // Phase 1: start -> overshoot (fade active in)
        yield return ScaleAndFade(
            fromScale: StepsHUD.Instance.activateStartScale,
            toScale: StepsHUD.Instance.activateOvershootScale,
            duration: upTime,
            scaleEase: StepsHUD.Instance.activateEaseUp,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f
        );

        // Phase 2: overshoot -> end (keep active fully visible)
        yield return ScaleAndFade(
            fromScale: StepsHUD.Instance.activateOvershootScale,
            toScale: StepsHUD.Instance.activateEndScale,
            duration: downTime,
            scaleEase: StepsHUD.Instance.activateEaseDown,
            overlayAlphaFrom: 1f,
            overlayAlphaTo: 1f
        );

        // Commit final state: main sprite becomes active, overlay hidden.
        targetImage.sprite = activeSprite;
        SetAlpha(targetImage, 1f);
        SetAlpha(_overlay, 0f);

        _running = null;
    }

    private IEnumerator CoDeactivate()
    {
        isActive = false;

        EnsureOverlay();

        // Base sprite stays "active"; overlay fades in "used" on top, ending with base swapped.
        targetImage.sprite = activeSprite;
        SetAlpha(targetImage, 1f);

        _overlay.sprite = usedSprite;
        SetAlpha(_overlay, 0f);

        _rt.localScale = Vector3.one * StepsHUD.Instance.deactivateStartScale;

        yield return ScaleAndFade(
            fromScale: StepsHUD.Instance.deactivateStartScale,
            toScale: StepsHUD.Instance.deactivateEndScale,
            duration: StepsHUD.Instance.deactivateDuration,
            scaleEase: StepsHUD.Instance.deactivateEase,
            overlayAlphaFrom: 0f,
            overlayAlphaTo: 1f
        );

        // Commit final state: main sprite becomes used, overlay hidden.
        targetImage.sprite = usedSprite;
        SetAlpha(targetImage, 1f);
        SetAlpha(_overlay, 0f);

        _running = null;
    }

    private IEnumerator ScaleAndFade(float fromScale, float toScale, float duration, Ease scaleEase, float overlayAlphaFrom, float overlayAlphaTo)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // UI polish should ignore timescale pauses
            float u = Mathf.Clamp01(t / duration);

            float eased = ApplyEase(scaleEase, u);

            float s = Mathf.LerpUnclamped(fromScale, toScale, eased);
            _rt.localScale = Vector3.one * s;

            float a = Mathf.LerpUnclamped(overlayAlphaFrom, overlayAlphaTo, eased) * StepsHUD.Instance.crossfadeAlphaMultiplier;
            SetAlpha(_overlay, Mathf.Clamp01(a));

            yield return null;
        }

        // Snap end
        _rt.localScale = Vector3.one * toScale;
        SetAlpha(_overlay, Mathf.Clamp01(overlayAlphaTo * StepsHUD.Instance.crossfadeAlphaMultiplier));
    }

    private void EnsureOverlay()
    {
        if (_overlay) return;

        // Create a child image that sits exactly on top.
        var go = new GameObject("SpriteOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(targetImage.transform, false);

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        _overlay = go.GetComponent<Image>();
        _overlay.raycastTarget = false;

        // Match settings
        _overlay.type = targetImage.type;
        _overlay.preserveAspect = targetImage.preserveAspect;
        _overlay.material = targetImage.material;

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
                // “Pop” style overshoot
                const float c1 = 1.70158f;
                const float c3 = c1 + 1f;
                return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

            default:
                return t;
        }
    }
}

// Simple ease options you can tweak in inspector.
public enum Ease
{
    Linear,
    InQuad, OutQuad, InOutQuad,
    InCubic, OutCubic, InOutCubic,
    OutBack
}