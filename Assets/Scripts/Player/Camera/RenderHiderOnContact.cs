using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderHiderOnContact : MonoBehaviour
{
    [Header("Hiding Volume")]
    [Tooltip("Radius around this object within which objects can be considered for hiding.")]
    public float hideRadius = 0.5f;

    [Tooltip("Visual-only helper for debugging. Height (in world units) under this object that you conceptually think of as the 'fade band'. " +
             "This is just for gizmos now.")]
    public float smoothFadeHeight = 0.5f;

    [Tooltip("If true, draw gizmos in Scene view to visualize radius and fade height.")]
    public bool debugSphere = true;

    [Tooltip("Color of the debug gizmo volume.")]
    public Color debugColor = new Color(1f, 0f, 0f, 0.25f);

    [Header("Layer Filtering")]
    [Tooltip("Objects on these layers CAN be hidden.\n" +
             "Anything NOT in this mask will be ignored completely.")]
    public LayerMask hideableLayers = ~0;

    [Tooltip("Objects on these layers will NEVER be hidden, even if they overlap.\n" +
             "Useful to protect the player, important interactables, etc.")]
    public LayerMask ignoreLayers = 0;

    // Tracks which renderers we've currently overridden this/previous frames
    private readonly HashSet<Renderer> _currentlyHidden = new HashSet<Renderer>();

    // Tracks which renderers are still overlapping this frame and should remain hidden
    private readonly HashSet<Renderer> _seenThisFrame = new HashSet<Renderer>();

    // Stores each renderer's original shadow casting mode so we can restore it later
    private readonly Dictionary<Renderer, ShadowCastingMode> _originalCasting =
        new Dictionary<Renderer, ShadowCastingMode>();

    // Reused buffer to avoid allocations when restoring
    private static readonly List<Renderer> _toRestoreBuffer = new List<Renderer>(32);


    void LateUpdate()
    {
        Vector3 origin = transform.position;

        // grab everything in the full sphere
        Collider[] hits = Physics.OverlapSphere(
            origin,
            hideRadius,
            hideableLayers,
            QueryTriggerInteraction.Ignore
        );

        _seenThisFrame.Clear();

        // ---- NEW LOGIC ----
        // We only want to IGNORE (not hide) the very top 25% of the sphere.
        //
        // Sphere goes from (origin.y - hideRadius) to (origin.y + hideRadius).
        // The top 25% of that vertical range is the top quarter of the diameter.
        // That's everything above (origin.y + hideRadius * 0.5f).
        //
        // So:
        // targetY > cutoffY         -> DO NOT hide (too close to top cap)
        // targetY <= cutoffY        -> allowed to hide
        float cutoffY = (origin.y + hideRadius) * 1f;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider col = hits[i];
            if (col == null) continue;

            GameObject go = col.gameObject;

            // skip hard-ignored layers
            if (((1 << go.layer) & ignoreLayers.value) != 0)
                continue;

            // vertical position of this collider
            float targetY = col.bounds.center.y;

            // only hide things whose center is at/below cutoffY.
            // If it's above cutoffY, it's in that "top 25%" safe cap, leave it alone.
            //if (targetY > cutoffY)
            //    continue;

            // --- HIDE LOGIC (ShadowsOnly makes it invisible but still blocks light) ---
            Renderer[] rends = go.GetComponentsInChildren<Renderer>(includeInactive: false);
            for (int r = 0; r < rends.Length; r++)
            {
                Renderer rend = rends[r];
                if (rend == null)
                    continue;

                _seenThisFrame.Add(rend);

                if (!_currentlyHidden.Contains(rend))
                {
                    // Remember its original state so we can put it back
                    if (!_originalCasting.ContainsKey(rend))
                        _originalCasting[rend] = rend.shadowCastingMode;

                    // Make it invisible to the camera but still cast shadows / block light
                    rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                    // optional cosmetic tweak:
                    // rend.receiveShadows = false;

                    _currentlyHidden.Add(rend);
                }
            }
        }

        // restore anything that is no longer inside the "hide zone"
        if (_currentlyHidden.Count > 0)
        {
            var toRestore = _toRestoreBuffer;
            toRestore.Clear();

            foreach (var rend in _currentlyHidden)
            {
                if (!_seenThisFrame.Contains(rend))
                {
                    toRestore.Add(rend);
                }
            }

            for (int iRestore = 0; iRestore < toRestore.Count; iRestore++)
            {
                Renderer rend = toRestore[iRestore];
                if (rend != null)
                {
                    // Restore original shadow casting mode
                    if (_originalCasting.TryGetValue(rend, out var originalMode))
                    {
                        rend.shadowCastingMode = originalMode;
                    }
                    else
                    {
                        rend.shadowCastingMode = ShadowCastingMode.On;
                    }

                    // if you changed receiveShadows above, restore here
                    // rend.receiveShadows = true;
                }

                _currentlyHidden.Remove(rend);
            }
        }
    }


    void OnDisable()
    {
        RestoreAll();
    }

    void OnDestroy()
    {
        RestoreAll();
    }

    private void RestoreAll()
    {
        foreach (var rend in _currentlyHidden)
        {
            if (rend != null)
            {
                if (_originalCasting.TryGetValue(rend, out var originalMode))
                {
                    rend.shadowCastingMode = originalMode;
                }
                else
                {
                    rend.shadowCastingMode = ShadowCastingMode.On;
                }

                // rend.receiveShadows = true;
            }
        }

        _currentlyHidden.Clear();
        _seenThisFrame.Clear();
    }


    void OnDrawGizmosSelected()
    {
        if (!debugSphere)
            return;

        Gizmos.color = debugColor;

        // draw the detection sphere
        Gizmos.DrawWireSphere(transform.position, hideRadius);

        // draw helper lines:
        // 1. the "fade height" reference band under you
        float topY = transform.position.y;
        float bottomY = transform.position.y - smoothFadeHeight;

        Vector3 top = new Vector3(transform.position.x, topY, transform.position.z);
        Vector3 bottom = new Vector3(transform.position.x, bottomY, transform.position.z);

        Gizmos.DrawLine(top + Vector3.left * 0.1f, bottom + Vector3.left * 0.1f);
        Gizmos.DrawLine(top + Vector3.right * 0.1f, bottom + Vector3.right * 0.1f);
        Gizmos.DrawLine(top + Vector3.forward * 0.1f, bottom + Vector3.forward * 0.1f);
        Gizmos.DrawLine(top + Vector3.back * 0.1f, bottom + Vector3.back * 0.1f);

        // 2. show cutoffY for the "top 25% safe zone"
        float cutoffY = transform.position.y + hideRadius * 0.5f;
        Vector3 c = new Vector3(transform.position.x, cutoffY, transform.position.z);
        Gizmos.DrawLine(c + Vector3.left * hideRadius * 0.25f, c + Vector3.right * hideRadius * 0.25f);
        Gizmos.DrawLine(c + Vector3.forward * hideRadius * 0.25f, c + Vector3.back * hideRadius * 0.25f);
    }
}
