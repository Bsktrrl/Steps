using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderHiderOnContact : Singleton<RenderHiderOnContact>
{
    [Header("Camera")]
    [Tooltip("Camera used for detection. If null, Camera.main is used.")]
    public Camera targetCamera;

    [Header("Front Detection")]
    [Tooltip("How far ahead to search for the front anchor block.")]
    public float frontCheckDistance = 0.85f;

    [Tooltip("Minimum radius of the sphere cast used to find the anchor block sooner when rotating.")]
    public float frontCheckRadius = 0.25f;

    [Tooltip("Offsets the cast start slightly backward so rotation catches nearby blocks earlier.")]
    public float frontCastBackOffset = 0.75f;

    [Tooltip("Use camera near clip / FOV to enlarge the cast radius when needed.")]
    public bool useDynamicCastRadius = true;

    [Tooltip("Multiplier for the near-clip-based dynamic cast radius.")]
    public float dynamicRadiusMultiplier = 0.6f;

    [Header("Grid")]
    [Tooltip("Size of one block in the grid.")]
    public float blockSize = 1f;

    [Tooltip("Half-size of the overlap box used to detect a block at a target cell.")]
    public Vector3 cellCheckExtents = new Vector3(0.4f, 0.4f, 0.4f);

    [Header("Hide Area")]
    [Tooltip("How many blocks left/right from center to hide. 2 = total width 5.")]
    public int horizontalRadius = 2;

    [Tooltip("How many blocks above the center to hide.")]
    public int verticalUpRadius = 1;

    [Tooltip("How many blocks below the center to hide.")]
    public int verticalDownRadius = 1;

    [Header("Layer Filtering")]
    [Tooltip("Objects on these layers CAN be hidden.")]
    public LayerMask hideableLayers = ~0;

    [Tooltip("Objects on these layers will NEVER be hidden.")]
    public LayerMask ignoreLayers = 0;

    [Header("Debug")]
    public bool debugDraw = true;
    public Color debugColor = Color.red;

    private readonly HashSet<Renderer> _currentlyHidden = new HashSet<Renderer>();
    private readonly HashSet<Renderer> _seenThisFrame = new HashSet<Renderer>();
    private readonly Dictionary<Renderer, ShadowCastingMode> _originalCasting = new Dictionary<Renderer, ShadowCastingMode>();
    private static readonly List<Renderer> _toRestoreBuffer = new List<Renderer>(64);

    private readonly List<Vector3> _debugCellCenters = new List<Vector3>(32);
    private Vector3 _debugFrontOrigin;
    private Vector3 _debugFrontEnd;
    private float _debugFrontRadius;
    private bool _debugFrontHit;

    [SerializeField] bool _freeCamActive = false;
    private Coroutine _freeCamCoroutine;

    private readonly Dictionary<Renderer, float> _lastSeenTime = new Dictionary<Renderer, float>();


    //--------------------


    void Awake()
    {
        // RenderHiderOnContact is attached to the physical camera,
        // so obtain that component directly instead of using Camera.main.
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();

        if (targetCamera == null)
        {
            Debug.LogError(
                "RenderHiderOnContact could not find a Camera component. " +
                "Assign Target Camera in the Inspector.",
                this
            );

            enabled = false;
        }
    }

    void Update()
    {
        if (_freeCamActive) return;
        if (GlueplantCamera.Instance.camera_isTraveling) { RestoreAll(); return; }

        RunHideCheck();
    }


    //--------------------


    public void FreeCamOn()
    {
        if (_freeCamCoroutine != null)
            StopCoroutine(_freeCamCoroutine);

        _freeCamCoroutine = StartCoroutine(FreeCamOn_Delay());
    }

    IEnumerator FreeCamOn_Delay()
    {
        yield return new WaitForSeconds(0.5f);

        if (_freeCamCoroutine == null)
            yield break;

        _freeCamActive = true;
        RestoreAll();

        _freeCamCoroutine = null;
    }

    public void FreeCamOff()
    {
        if (_freeCamCoroutine != null)
        {
            StopCoroutine(_freeCamCoroutine);
            _freeCamCoroutine = null;
        }

        _freeCamActive = false;
        RunHideCheck();

        //print("2. FreeCamOff(): " + _freeCamActive);
    }


    //--------------------


    void RunHideCheck()
    {
        if (targetCamera == null)
            return;

        _seenThisFrame.Clear();
        _debugCellCenters.Clear();

        Transform cam = targetCamera.transform;

        // Keep the origin slightly behind the lens so a block containing
        // the camera can still be detected, without reaching far behind it.
        float safeBackOffset = Mathf.Min(frontCastBackOffset, 0.1f);

        Vector3 castOrigin =
            cam.position - cam.forward * safeBackOffset;

        Vector3 castDirection = cam.forward;

        float castRadius = frontCheckRadius;

        if (useDynamicCastRadius)
        {
            float nearClipRadius =
                Mathf.Tan(
                    targetCamera.fieldOfView *
                    0.5f *
                    Mathf.Deg2Rad
                ) *
                targetCamera.nearClipPlane *
                dynamicRadiusMultiplier;

            castRadius = Mathf.Max(
                frontCheckRadius,
                nearClipRadius
            );
        }

        // A large sphere reaches into walls beside the camera.
        // This smaller cap focuses on the block obstructing the view.
        castRadius = Mathf.Clamp(
            castRadius,
            0.02f,
            0.1f
        );

        float castDistance =
            frontCheckDistance + safeBackOffset;

        Ray ray = new Ray(
            castOrigin,
            castDirection
        );

        _debugFrontOrigin = castOrigin;
        _debugFrontEnd =
            castOrigin + castDirection * castDistance;

        _debugFrontRadius = castRadius;
        _debugFrontHit = false;

        if (!Physics.SphereCast(
                ray,
                castRadius,
                out RaycastHit hit,
                castDistance,
                hideableLayers,
                QueryTriggerInteraction.Ignore))
        {
            RestoreNoLongerSeen();
            return;
        }

        if (hit.collider == null)
        {
            RestoreNoLongerSeen();
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        if (((1 << hitObject.layer) & ignoreLayers.value) != 0)
        {
            RestoreNoLongerSeen();
            return;
        }

        _debugFrontHit = true;
        _debugFrontEnd = hit.point;

        // Hide only the grid block that actually obstructs the camera.
        // Do not hide a rectangular area of neighbouring blocks.
        Vector3 obstructingCell =
            SnapToGrid(hit.collider.bounds.center);

        _debugCellCenters.Add(obstructingCell);

        HideBlockAtCell(obstructingCell);

        RestoreNoLongerSeen();
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / blockSize) * blockSize;
        float y = Mathf.Round(position.y / blockSize) * blockSize;
        float z = Mathf.Round(position.z / blockSize) * blockSize;
        return new Vector3(x, y, z);
    }

    Vector3 GetHorizontalSideAxis(Transform cam)
    {
        Vector3 right = cam.right;
        right.y = 0f;

        if (right.sqrMagnitude < 0.0001f)
            return Vector3.right;

        right.Normalize();

        float dotX = Mathf.Abs(Vector3.Dot(right, Vector3.right));
        float dotZ = Mathf.Abs(Vector3.Dot(right, Vector3.forward));

        if (dotX >= dotZ)
            return Vector3.right * Mathf.Sign(Vector3.Dot(right, Vector3.right));
        else
            return Vector3.forward * Mathf.Sign(Vector3.Dot(right, Vector3.forward));
    }

    void HideBlockAtCell(Vector3 cellCenter)
    {
        Collider[] hits = Physics.OverlapBox(
            cellCenter,
            cellCheckExtents,
            Quaternion.identity,
            hideableLayers,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < hits.Length; i++)
        {
            Collider col = hits[i];

            if (col == null)
                continue;

            GameObject go = col.gameObject;

            if (((1 << go.layer) & ignoreLayers.value) != 0)
                continue;

            Renderer[] renderers =
                go.GetComponentsInChildren<Renderer>(
                    includeInactive: false
                );

            for (int r = 0; r < renderers.Length; r++)
            {
                Renderer rend = renderers[r];

                if (rend == null)
                    continue;

                _seenThisFrame.Add(rend);
                _lastSeenTime[rend] = Time.unscaledTime;

                if (_currentlyHidden.Contains(rend))
                    continue;

                if (!_originalCasting.ContainsKey(rend))
                {
                    _originalCasting[rend] =
                        rend.shadowCastingMode;
                }

                rend.shadowCastingMode =
                    ShadowCastingMode.ShadowsOnly;

                _currentlyHidden.Add(rend);
            }
        }
    }

    void RestoreNoLongerSeen()
    {
        const float restoreDelay = 0.15f;

        if (_currentlyHidden.Count == 0)
            return;

        _toRestoreBuffer.Clear();

        foreach (Renderer rend in _currentlyHidden)
        {
            if (rend == null)
            {
                _toRestoreBuffer.Add(rend);
                continue;
            }

            if (_seenThisFrame.Contains(rend))
                continue;

            if (!_lastSeenTime.TryGetValue(
                    rend,
                    out float lastSeenTime))
            {
                _toRestoreBuffer.Add(rend);
                continue;
            }

            // Keep the block hidden briefly when the cast crosses a
            // collider or grid-cell boundary. This prevents flickering.
            if (Time.unscaledTime - lastSeenTime >= restoreDelay)
            {
                _toRestoreBuffer.Add(rend);
            }
        }

        for (int i = 0; i < _toRestoreBuffer.Count; i++)
        {
            Renderer rend = _toRestoreBuffer[i];

            if (rend != null)
            {
                if (_originalCasting.TryGetValue(
                        rend,
                        out ShadowCastingMode originalMode))
                {
                    rend.shadowCastingMode = originalMode;
                }
                else
                {
                    rend.shadowCastingMode =
                        ShadowCastingMode.On;
                }
            }

            _currentlyHidden.Remove(rend);
            _lastSeenTime.Remove(rend);
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

    void RestoreAll()
    {
        foreach (Renderer rend in _currentlyHidden)
        {
            if (rend == null)
                continue;

            if (_originalCasting.TryGetValue(
                    rend,
                    out ShadowCastingMode originalMode))
            {
                rend.shadowCastingMode = originalMode;
            }
            else
            {
                rend.shadowCastingMode =
                    ShadowCastingMode.On;
            }
        }

        _currentlyHidden.Clear();
        _seenThisFrame.Clear();
        _lastSeenTime.Clear();
        _debugCellCenters.Clear();
        _debugFrontHit = false;
    }

    void OnDrawGizmos()
    {
        if (!debugDraw || _freeCamActive)
            return;

        Gizmos.color = _debugFrontHit ? debugColor : Color.gray;
        Gizmos.DrawLine(_debugFrontOrigin, _debugFrontEnd);
        Gizmos.DrawWireSphere(_debugFrontEnd, _debugFrontRadius);

        Gizmos.color = debugColor;
        for (int i = 0; i < _debugCellCenters.Count; i++)
        {
            Gizmos.DrawWireCube(_debugCellCenters[i], cellCheckExtents * 2f);
        }
    }
}