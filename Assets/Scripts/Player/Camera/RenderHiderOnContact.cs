using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderHiderOnContact : MonoBehaviour
{
    [Header("Hiding Volume")]
    [Tooltip("Radius around this object within which objects will be hidden.")]
    public float hideRadius = 0.5f;

    [Tooltip("If true, draw the radius as a wireframe sphere in the editor/game view (Gizmos).")]
    public bool debugSphere = true;

    [Tooltip("Color of the debug sphere gizmo.")]
    public Color debugColor = new Color(1f, 0f, 0f, 0.25f);

    [Header("Layer Filtering")]
    [Tooltip("Objects on these layers CAN be hidden.\n" +
             "Anything NOT in this mask will be ignored completely.")]
    public LayerMask hideableLayers = ~0;

    [Tooltip("Objects on these layers will NEVER be hidden, even if they overlap.\n" +
             "Useful to protect the player, important interactables, etc.")]
    public LayerMask ignoreLayers = 0;

    // Internal bookkeeping:
    // - currentlyHidden keeps all renderers we turned off this frame (and previous frames)
    //   so we can turn them back on later.
    // - thisFrameSeen is used to know which ones are STILL overlapping this frame.
    private readonly HashSet<Renderer> _currentlyHidden = new HashSet<Renderer>();
    private readonly HashSet<Renderer> _seenThisFrame = new HashSet<Renderer>();


    void LateUpdate()
    {
        
        // 1. Find all colliders inside hideRadius
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            hideRadius,
            hideableLayers,                       // only test layers we're ALLOWED to hide
            QueryTriggerInteraction.Ignore
        );

        _seenThisFrame.Clear();

        // 2. For each collider, skip if it's on an ignore layer, otherwise hide its renderers
        for (int i = 0; i < hits.Length; i++)
        {
            Collider col = hits[i];
            GameObject go = col.gameObject;

            // skip hard-ignored layers
            if (((1 << go.layer) & ignoreLayers.value) != 0)
                continue;

            // Grab all renderers on that object and its children
            // (We include children because usually a chunk of level geo
            // is multiple MeshRenderers under one parent.)
            Renderer[] rends = go.GetComponentsInChildren<Renderer>(includeInactive: false);

            for (int r = 0; r < rends.Length; r++)
            {
                Renderer rend = rends[r];
                if (rend == null)
                    continue;

                _seenThisFrame.Add(rend);

                // If it's not already hidden by us, hide it now
                if (!_currentlyHidden.Contains(rend))
                {
                    rend.enabled = false;
                    _currentlyHidden.Add(rend);
                }
            }
        }

        // 3. Anything we hid in previous frames but did NOT overlap this frame → restore
        // We'll build a temp list to avoid modifying the set while iterating
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

            for (int i = 0; i < toRestore.Count; i++)
            {
                Renderer rend = toRestore[i];
                if (rend != null)
                {
                    rend.enabled = true;
                }
                _currentlyHidden.Remove(rend);
            }
        }
    }

    // We reuse this list every frame to avoid GC
    private static readonly List<Renderer> _toRestoreBuffer = new List<Renderer>(32);


    void OnDisable()
    {
        // Safety: if this script is disabled, we MUST re-enable anything we hid
        RestoreAll();
    }

    void OnDestroy()
    {
        // Same safety if object is destroyed (camera removed, etc.)
        RestoreAll();
    }

    private void RestoreAll()
    {
        foreach (var rend in _currentlyHidden)
        {
            if (rend != null)
                rend.enabled = true;
        }
        _currentlyHidden.Clear();
        _seenThisFrame.Clear();
    }

    void OnDrawGizmosSelected()
    {
        if (!debugSphere)
            return;

        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, hideRadius);
    }
}
