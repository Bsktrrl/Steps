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


    //--------------------


    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
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

        Vector3 castOrigin = cam.position - cam.forward * frontCastBackOffset;
        Vector3 castDirection = cam.forward;

        float castRadius = frontCheckRadius;
        if (useDynamicCastRadius)
        {
            float nearClipRadius =
                Mathf.Tan(targetCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) *
                targetCamera.nearClipPlane *
                dynamicRadiusMultiplier;

            castRadius = Mathf.Max(frontCheckRadius, nearClipRadius);
        }

        Ray ray = new Ray(castOrigin, castDirection);

        _debugFrontOrigin = ray.origin;
        _debugFrontEnd = ray.origin + ray.direction * frontCheckDistance;
        _debugFrontRadius = castRadius;
        _debugFrontHit = false;

        if (!Physics.SphereCast(
                ray,
                castRadius,
                out RaycastHit hit,
                frontCheckDistance,
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

        GameObject frontObject = hit.collider.gameObject;

        if (((1 << frontObject.layer) & ignoreLayers.value) != 0)
        {
            RestoreNoLongerSeen();
            return;
        }

        _debugFrontHit = true;
        _debugFrontEnd = hit.point;

        Vector3 anchorCenter = SnapToGrid(hit.collider.bounds.center);
        Vector3 sideAxis = GetHorizontalSideAxis(cam);
        Vector3 upAxis = Vector3.up;

        for (int y = verticalUpRadius; y >= -verticalDownRadius; y--)
        {
            for (int x = -horizontalRadius; x <= horizontalRadius; x++)
            {
                Vector3 cellCenter =
                    anchorCenter +
                    sideAxis * (x * blockSize) +
                    upAxis * (y * blockSize);

                _debugCellCenters.Add(cellCenter);
                HideBlockAtCell(cellCenter);
            }
        }

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

            //Mesh Renderer
            Renderer[] rends = go.GetComponentsInChildren<Renderer>(includeInactive: false);
            for (int r = 0; r < rends.Length; r++)
            {
                Renderer rend = rends[r];
                if (rend == null)
                    continue;

                _seenThisFrame.Add(rend);

                if (!_currentlyHidden.Contains(rend))
                {
                    if (!_originalCasting.ContainsKey(rend))
                        _originalCasting[rend] = rend.shadowCastingMode;

                    rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    _currentlyHidden.Add(rend);
                }
            }
        }
    }

    void RestoreNoLongerSeen()
    {
        if (_currentlyHidden.Count == 0)
            return;

        _toRestoreBuffer.Clear();

        foreach (Renderer rend in _currentlyHidden)
        {
            if (!_seenThisFrame.Contains(rend))
                _toRestoreBuffer.Add(rend);
        }

        for (int i = 0; i < _toRestoreBuffer.Count; i++)
        {
            Renderer rend = _toRestoreBuffer[i];
            if (rend != null)
            {
                if (_originalCasting.TryGetValue(rend, out ShadowCastingMode originalMode))
                    rend.shadowCastingMode = originalMode;
                else
                    rend.shadowCastingMode = ShadowCastingMode.On;
            }

            _currentlyHidden.Remove(rend);
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
            if (rend != null)
            {
                if (_originalCasting.TryGetValue(rend, out ShadowCastingMode originalMode))
                    rend.shadowCastingMode = originalMode;
                else
                    rend.shadowCastingMode = ShadowCastingMode.On;
            }
        }

        _currentlyHidden.Clear();
        _seenThisFrame.Clear();
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