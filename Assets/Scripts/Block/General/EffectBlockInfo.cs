using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class EffectBlockInfo : MonoBehaviour
{
    private EffectBlockManager effectBlockManager;

    [Header("Effect visual parent (optional)")]
    [Tooltip("If empty, auto-uses child named 'Number_Parent'. If not found, uses this transform.")]
    [SerializeField] private Transform effectVisualParent;

    [Header("Editor behavior")]
    [Tooltip("If true, removes duplicates so only 1 per type remains (Editor only).")]
    [SerializeField] private bool removeDuplicatesInEditor = true;

    [Header("Is Added (synced from children)")]
    public bool effectBlock_SpawnPoint_isAdded;
    public bool effectBlock_Teleporter_isAdded;
    [SerializeField] private bool effectBlock_Moveable_isAdded;
    public bool effectBlock_MushroomCircle_isAdded;

    private void Awake()
    {
        ApplyMovementCostFromComponents();
        SyncIsAddedFlagsFromChildren();

        if (Application.isPlaying) return;

#if UNITY_EDITOR
        ResolveManagerEditor();
        EditorRefresh();
#endif
    }

    private void OnEnable()
    {
        ApplyMovementCostFromComponents();
        SyncIsAddedFlagsFromChildren();

        if (Application.isPlaying && !effectBlock_SpawnPoint_isAdded && !effectBlock_Teleporter_isAdded && !effectBlock_Moveable_isAdded && !effectBlock_MushroomCircle_isAdded)
        {
            this.gameObject.GetComponent<EffectBlockInfo>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<EffectBlockInfo>().enabled = true;
        }

        if (Application.isPlaying) return;

#if UNITY_EDITOR
        ResolveManagerEditor();
        EditorRefresh();
#endif
    }

    private void Update()
    {
        // Runtime-safe: keep bools correct. No spawning here.
        SyncIsAddedFlagsFromChildren();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyMovementCostFromComponents();
        SyncIsAddedFlagsFromChildren();

        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        EditorApplication.delayCall -= DelayedRefresh;
        EditorApplication.delayCall += DelayedRefresh;
    }

    private void DelayedRefresh()
    {
        if (!this) return;
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        ResolveManagerEditor();
        EditorRefresh();
    }
#endif

    // --------------------
    // Movement cost (runtime + editor)
    // --------------------
    private void ApplyMovementCostFromComponents()
    {
        var info = GetComponent<BlockInfo>();
        if (!info) return;

        int? forcedCost = null;

        if (GetComponent<Block_MushroomCircle>())
            forcedCost = -1;
        else if (GetComponent<Block_Checkpoint>()
              || GetComponent<Block_RefillSteps>()
              || GetComponent<Block_Pusher>()
              || GetComponent<Block_Teleport>())
            forcedCost = 0;

        if (forcedCost.HasValue)
        {
            info.movementCost = forcedCost.Value;
            info.movementCost_Temp = forcedCost.Value;
        }
    }

    // --------------------
    // Where visuals live
    // --------------------
    private Transform GetEffectParent()
    {
        if (effectVisualParent) return effectVisualParent;

        var t = transform.Find("Number_Parent");
        if (t) return t;

        return transform;
    }

    // --------------------
    // Sync isAdded flags from actual children
    // --------------------
    private void SyncIsAddedFlagsFromChildren()
    {
        effectBlock_SpawnPoint_isAdded = HasType(EffectVisualType.SpawnPoint);
        effectBlock_Teleporter_isAdded = HasType(EffectVisualType.Teleporter);
        effectBlock_Moveable_isAdded = HasType(EffectVisualType.Moveable);
        effectBlock_MushroomCircle_isAdded = HasType(EffectVisualType.MushroomCircle);
    }

    private bool HasType(EffectVisualType type)
    {
        Transform parent = GetEffectParent();

        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var marker = child.GetComponent<EffectVisualMarker>();
            if (marker != null && marker.type == type)
                return true;
        }

        return false;
    }

#if UNITY_EDITOR
    // --------------------
    // Editor-only spawning and dedupe
    // --------------------
    private void EditorRefresh()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        var info = GetComponent<BlockInfo>();
        if (!info) return;
        if (info.blockType != BlockType.Cube && info.blockType != BlockType.Slab) return;

        if (!effectBlockManager) ResolveManagerEditor();
        if (!effectBlockManager) return;

        if (removeDuplicatesInEditor)
            EnforceOnePerType();

        EnsureVisualsForPresentComponents();

        // After any changes, ensure bools are correct.
        SyncIsAddedFlagsFromChildren();

        MarkSceneDirtyIfNeeded();
    }

    private void EnsureVisualsForPresentComponents()
    {
        if (GetComponent<Block_Checkpoint>())
            EnsureOne(effectBlockManager.effectBlock_SpawnPoint_Prefab, EffectVisualType.SpawnPoint);

        if (GetComponent<Block_RefillSteps>())
            EnsureOne(effectBlockManager.effectBlock_RefillSteps_Prefab, EffectVisualType.RefillSteps);

        if (GetComponent<Block_Pusher>())
            EnsureOne(effectBlockManager.effectBlock_Pusher_Prefab, EffectVisualType.Pusher);

        if (GetComponent<Block_Teleport>())
            EnsureOne(effectBlockManager.effectBlock_Teleporter_Prefab, EffectVisualType.Teleporter);

        if (GetComponent<Block_Moveable>())
            EnsureOne(effectBlockManager.effectBlock_Moveable_Prefab, EffectVisualType.Moveable);

        if (GetComponent<Block_MushroomCircle>())
            EnsureOne(effectBlockManager.effectBlock_MushroomCircle_Prefab, EffectVisualType.MushroomCircle);
    }

    private void EnsureOne(GameObject prefab, EffectVisualType type)
    {
        if (!prefab) return;
        if (HasType(type)) return;

        Transform parent = GetEffectParent();

        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        if (!instance) return;

        Undo.RegisterCreatedObjectUndo(instance, $"Create {type} effect visual");

        // Ensure marker exists and correct
        var marker = instance.GetComponent<EffectVisualMarker>();
        if (!marker) marker = Undo.AddComponent<EffectVisualMarker>(instance);
        marker.type = type;

        // ✅ Force it to be the last child in the container
        instance.transform.SetAsLastSibling();
    }

    private void EnforceOnePerType()
    {
        EnforceOne(EffectVisualType.SpawnPoint);
        EnforceOne(EffectVisualType.RefillSteps);
        EnforceOne(EffectVisualType.Pusher);
        EnforceOne(EffectVisualType.Teleporter);
        EnforceOne(EffectVisualType.Moveable);
        EnforceOne(EffectVisualType.MushroomCircle);
    }

    private void EnforceOne(EffectVisualType type)
    {
        Transform parent = GetEffectParent();
        Transform keeper = null;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
            var marker = child.GetComponent<EffectVisualMarker>();
            if (marker == null || marker.type != type) continue;

            if (keeper == null) keeper = child;
            else Undo.DestroyObjectImmediate(child.gameObject);
        }
    }

    private void ResolveManagerEditor()
    {
        if (effectBlockManager) return;

#if UNITY_2023_1_OR_NEWER
        effectBlockManager = FindAnyObjectByType<EffectBlockManager>(FindObjectsInactive.Include);
#else
        effectBlockManager = FindObjectOfType<EffectBlockManager>();
#endif

        if (!effectBlockManager)
        {
            var all = Resources.FindObjectsOfTypeAll<EffectBlockManager>();
            if (all != null && all.Length > 0)
                effectBlockManager = all[0];
        }
    }

    private void MarkSceneDirtyIfNeeded()
    {
        if (!gameObject.scene.IsValid()) return;
        if (!gameObject.scene.isLoaded) return;

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
#endif
}