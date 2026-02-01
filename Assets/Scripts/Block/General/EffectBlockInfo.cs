using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class EffectBlockInfo : MonoBehaviour
{
    private Coroutine _pollRoutine;

    private EffectBlockManager effectBlockManager;

    [Header("Is Added")]
    public bool effectBlock_SpawnPoint_isAdded;
    [SerializeField] private bool effectBlock_RefillSteps_isAdded;
    [SerializeField] private bool effectBlock_Pusher_isAdded;
    public bool effectBlock_Teleporter_isAdded;
    [SerializeField] private bool effectBlock_Moveable_isAdded;
    public bool effectBlock_MushroomCircle_isAdded;

    [Header("Child List")]
    [SerializeField] private List<GameObject> blockEffectHolding_List = new List<GameObject>();

    // --------------------

    private void Awake()
    {
        // In edit mode Awake can be called a lot; keep it safe.
        ResolveManager();

        // Your original call (kept), but note: your original sets isAdded=true
        // and then the check immediately returns. I’m keeping behavior but fixing that below.
        CheckEffectBlockInChildRecursively(transform);

        StartPollingIfNeeded();
    }

    private void OnEnable()
    {
        Block_Snow.Action_SnowSetup_End += AdjustPosition_Snow;

        ResolveManager();
        StartPollingIfNeeded();
    }

    private void OnDisable()
    {
        Block_Snow.Action_SnowSetup_End -= AdjustPosition_Snow;

        StopPolling();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        ResolveManager();

        // If you add/remove effect components in inspector,
        // we want to re-attempt without Update().
        StartPollingIfNeeded();
    }
#endif

    // --------------------
    // Polling (replaces Update)
    // --------------------

    private void StartPollingIfNeeded()
    {
        if (!ShouldPoll()) return;

        if (_pollRoutine == null && gameObject.activeInHierarchy)
        {
            _pollRoutine = StartCoroutine(PollUntilSpawned());
        }
    }

    private void StopPolling()
    {
        if (_pollRoutine != null)
        {
            StopCoroutine(_pollRoutine);
            _pollRoutine = null;
        }
    }

    private IEnumerator PollUntilSpawned()
    {
        // Keep trying until success or until the object becomes irrelevant.
        while (ShouldPoll())
        {
            // Equivalent to your SetWaitTime + counter >= waitTime behavior
            float waitTime = Random.Range(0f, 1f);

#if UNITY_EDITOR
            // In edit mode, WaitForSeconds is not reliable; we use editor delay.
            if (!Application.isPlaying)
            {
                double start = EditorApplication.timeSinceStartup;
                while (!Application.isPlaying && EditorApplication.timeSinceStartup - start < waitTime)
                    yield return null;
            }
            else
#endif
            {
                yield return new WaitForSeconds(waitTime);
            }

            ResolveManager();

            // Run one pass of your checks
            TrySpawnEffectOnce();

            // If spawned now, stop forever.
            if (HasAnyEffectBlockChild())
                break;
        }

        _pollRoutine = null;
    }

    private bool ShouldPoll()
    {
        // Only trigger on Cubes and Slabs
        var info = GetComponent<BlockInfo>();
        if (!info) return false;

        if (info.blockType != BlockType.Cube && info.blockType != BlockType.Slab) return false;

        // Already have the visual child => no need to poll
        if (HasAnyEffectBlockChild()) return false;

        // If flags say something is already added, no need to poll
        // (this matches your Update early returns)
        if (effectBlock_SpawnPoint_isAdded) return false;
        if (effectBlock_RefillSteps_isAdded) return false;
        if (effectBlock_Pusher_isAdded) return false;
        if (effectBlock_Teleporter_isAdded) return false;
        if (effectBlock_Moveable_isAdded) return false;
        if (effectBlock_MushroomCircle_isAdded) return false;

        // Otherwise: we still need to try.
        return true;
    }

    private void TrySpawnEffectOnce()
    {
        // Exactly your Update "if(counter>=waitTime){...}" body,
        // but without counter/time logic.
        CheckForEffectBlockUpdate_SpawnPoint();
        CheckForEffectBlockUpdate_RefillSteps();
        CheckForEffectBlockUpdate_Pusher();
        CheckForEffectBlockUpdate_Teleporter();
        CheckForEffectBlockUpdate_Moveable();
        CheckForEffectBlockUpdate_MushroomCircle();
    }

    // --------------------
    // Your original method (IMPORTANT FIX)
    // --------------------
    // Your original sets effectBlock_*_isAdded = true BEFORE calling CheckForEffectBlockUpdate_*,
    // but those methods early-return if isAdded is true, so nothing spawns.
    //
    // I’m adjusting this to: "if component exists, attempt spawn (without pre-setting isAdded)".
    //
    // If your intention was “mark as already added if component exists AND child exists”, then
    // you should set isAdded based on HasAnyEffectBlockChild() instead.
    void CheckEffectBlockInChildRecursively(Transform parent)
    {
        // If you truly want this to reflect "already has a visual child"
        // then set flags only if child exists:
        if (HasAnyEffectBlockChild())
        {
            // Optional: you could try to detect which one, but leaving it simple:
            // (You can remove this block entirely if you don’t need it.)
            return;
        }

        // Otherwise, do NOT set isAdded before trying to spawn.
        if (GetComponent<Block_Checkpoint>())
        {
            CheckForEffectBlockUpdate_SpawnPoint();
        }
        else if (GetComponent<Block_RefillSteps>())
        {
            CheckForEffectBlockUpdate_RefillSteps();
        }
        else if (GetComponent<Block_Pusher>())
        {
            CheckForEffectBlockUpdate_Pusher();
        }
        else if (GetComponent<Block_Teleport>())
        {
            CheckForEffectBlockUpdate_Teleporter();
        }
        else if (GetComponent<Block_Moveable>())
        {
            CheckForEffectBlockUpdate_Moveable();
        }
        else if (GetComponent<Block_MushroomCircle>())
        {
            CheckForEffectBlockUpdate_MushroomCircle();
        }
    }

    // --------------------
    // Manager resolution
    // --------------------
    private void ResolveManager()
    {
        if (effectBlockManager) return;

#if UNITY_2023_1_OR_NEWER
        effectBlockManager = FindAnyObjectByType<EffectBlockManager>(FindObjectsInactive.Include);
#else
        effectBlockManager = FindObjectOfType<EffectBlockManager>();
#endif

#if UNITY_EDITOR
        // Editor fallback: finds inactive objects too
        if (!effectBlockManager)
        {
            var all = Resources.FindObjectsOfTypeAll<EffectBlockManager>();
            if (all != null && all.Length > 0)
                effectBlockManager = all[0];
        }
#endif
    }

    // --------------------
    // Your original check methods (with one safety guard added)
    // --------------------

    void CheckForEffectBlockUpdate_SpawnPoint()
    {
        if (!GetComponent<Block_Checkpoint>()) return;

        ChangeMovementCost(0);

        if (!effectBlockManager) return;
        if (!effectBlockManager.effectBlock_SpawnPoint_Prefab) return;
        if (effectBlock_SpawnPoint_isAdded) return;

        effectBlock_SpawnPoint_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_SpawnPoint_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }

    void CheckForEffectBlockUpdate_RefillSteps()
    {
        if (!GetComponent<Block_RefillSteps>()) return;

        ChangeMovementCost(0);

        if (!effectBlockManager) return;
        if (!effectBlockManager.effectBlock_RefillSteps_Prefab) return;
        if (effectBlock_RefillSteps_isAdded) return;

        effectBlock_RefillSteps_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_RefillSteps_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }

    void CheckForEffectBlockUpdate_Pusher()
    {
        if (!GetComponent<Block_Pusher>()) return;

        ChangeMovementCost(0);

        if (!effectBlockManager) return;
        if (!effectBlockManager.effectBlock_Pusher_Prefab) return;
        if (effectBlock_Pusher_isAdded) return;

        effectBlock_Pusher_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_Pusher_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }

    void CheckForEffectBlockUpdate_Teleporter()
    {
        if (!GetComponent<Block_Teleport>()) return;

        ChangeMovementCost(0);

        if (!effectBlockManager) return;
        if (!effectBlockManager.effectBlock_Teleporter_Prefab) return;
        if (effectBlock_Teleporter_isAdded) return;

        effectBlock_Teleporter_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_Teleporter_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }

    void CheckForEffectBlockUpdate_Moveable()
    {
        if (!GetComponent<Block_Moveable>()) return;

        if (!effectBlockManager) return;
        if (effectBlockManager.effectBlock_Moveable_Prefab == null) return;
        if (effectBlock_Moveable_isAdded) return;

        effectBlock_Moveable_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_Moveable_Prefab);

        AdjustPosition();
        ChangeColor();
    }

    void CheckForEffectBlockUpdate_MushroomCircle()
    {
        if (!GetComponent<Block_MushroomCircle>()) return;

        ChangeMovementCost(-1);

        if (!effectBlockManager) return;
        if (!effectBlockManager.effectBlock_MushroomCircle_Prefab) return;
        if (effectBlock_MushroomCircle_isAdded) return;

        effectBlock_MushroomCircle_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_MushroomCircle_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(-1);
    }

    // --------------------
    // Instantiate (kept)
    // --------------------
    void InstantiateEffectBlock(GameObject effectBlock)
    {
        if (HasAnyEffectBlockChild()) return;

        GameObject instance = Instantiate(effectBlock, transform);
        blockEffectHolding_List.Add(instance);

        // Special case for teleporter
        var teleport = GetComponent<Block_Teleport>();
        if (teleport)
        {
            teleport.SetupTeleporter();
        }

        RemoveDuplicateEffectBlocks();
    }

    // --------------------
    // Your other helpers (kept)
    // --------------------

    void ChangeMovementCost(int cost)
    {
        var info = GetComponent<BlockInfo>();
        if (!info) return;

        info.movementCost_Temp = cost;
        info.movementCost = cost;
    }

    void ChangeColor()
    {
        // (left commented as you had it)
    }

    void AdjustPosition()
    {
        // (left commented as you had it)
    }

    void AdjustPosition_Snow()
    {
        // (left commented as you had it)
    }

    bool HasAnyEffectBlockChild()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EffectBlock_Reference>()) return true;
        }
        return false;
    }

    void RemoveDuplicateEffectBlocks()
    {
        var seen = new HashSet<GameObject>();
        for (int i = blockEffectHolding_List.Count - 1; i >= 0; i--)
        {
            var obj = blockEffectHolding_List[i];
            if (!obj || seen.Contains(obj))
            {
                if (obj) DestroyImmediate(obj);
                blockEffectHolding_List.RemoveAt(i);
            }
            else
            {
                seen.Add(obj);
            }
        }
    }
}
