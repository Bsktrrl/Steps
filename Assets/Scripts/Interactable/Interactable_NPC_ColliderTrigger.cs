using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable_NPC_ColliderTrigger : MonoBehaviour
{
    [SerializeField] Interactable_NPC parentScript;

    private static readonly HashSet<Interactable_NPC_ColliderTrigger> overlaps = new();
    private static readonly HashSet<Interactable_NPC> checkedNPCs = new();
    private readonly HashSet<Collider> playerColliders = new();
    private BoxCollider sideCollider;

    internal static bool HasOverlaps => overlaps.Count > 0;

    private void Awake()
    {
        sideCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) => TrackOverlap(other);

    private void OnTriggerStay(Collider other) => TrackOverlap(other);

    private void TrackOverlap(Collider other)
    {
        if (!isActiveAndEnabled || playerColliders.Contains(other) || !other.CompareTag("Player"))
            return;

        playerColliders.Add(other);
        overlaps.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerColliders.Remove(other) && playerColliders.Count == 0)
            overlaps.Remove(this);
    }

    private void OnDisable()
    {
        playerColliders.Clear();
        overlaps.Remove(this);
    }

    private static bool HasNoLiveOverlap(Interactable_NPC_ColliderTrigger trigger)
    {
        if (!trigger || !trigger.isActiveAndEnabled)
            return true;

        if (trigger.sideCollider && trigger.sideCollider.enabled)
        {
            trigger.playerColliders.RemoveWhere(collider => !collider || !collider.enabled || !collider.gameObject.activeInHierarchy);
            Bounds triggerBounds = trigger.sideCollider.bounds;
            foreach (Collider collider in trigger.playerColliders)
                if (triggerBounds.Intersects(collider.bounds))
                    return false;
        }

        // Disabled or teleported colliders may disappear without an exit callback.
        trigger.playerColliders.Clear();
        return true;
    }

    internal static Interactable_NPC FindTalkTarget(PlayerManager player, Movement movement, Interactable_NPC previousTarget)
    {
        overlaps.RemoveWhere(HasNoLiveOverlap);
        if (!HasOverlaps)
            return null;

        Vector3 origin = player.player.transform.position;
        Vector3 facing = player.playerBody.transform.forward;
        facing.y = 0f;
        facing = RoundTo45Degrees(facing);
        if (facing == Vector3.zero)
            return null;

        Interactable_NPC bestNPC = null;
        float bestDistance = float.PositiveInfinity;
        bool checkedFacingObstruction = false;
        foreach (var trigger in overlaps)
        {
            Interactable_NPC npc = trigger.parentScript;
            if (!npc || !npc.isActiveAndEnabled || npc.isInteracting || !checkedNPCs.Add(npc))
                continue;

            Vector3 toNPC = npc.transform.position - origin;
            Vector3 horizontal = new Vector3(toNPC.x, 0f, toNPC.z);
            if (RoundTo45Degrees(horizontal) != facing)
                continue;

            float distance = toNPC.sqrMagnitude;
            if (bestNPC && (distance > bestDistance ||
                (distance == bestDistance && npc != previousTarget &&
                 (bestNPC == previousTarget || npc.GetInstanceID() > bestNPC.GetInstanceID()))))
                continue;

            if (!checkedFacingObstruction)
            {
                checkedFacingObstruction = true;
                var hitType = movement.PerformMovementRaycast(origin, player.playerBody.transform.forward, 1f, out _);
                if (hitType == RaycastHitObjects.Fence || hitType == RaycastHitObjects.Ladder ||
                    hitType == RaycastHitObjects.LadderBlocker)
                    break;
            }

            if (HasLineOfSight(origin, toNPC, player.player.layer))
            {
                bestNPC = npc;
                bestDistance = distance;
            }
        }

        checkedNPCs.Clear();
        return bestNPC;
    }

    private static Vector3 RoundTo45Degrees(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return Vector3.zero;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float radians = Mathf.Round(angle / 45f) * 45f * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0f, Mathf.Cos(radians)).normalized;
    }

    private static bool HasLineOfSight(Vector3 origin, Vector3 toNPC, int playerLayer)
    {
        float distance = toNPC.magnitude;
        if (distance <= 0.001f)
            return true;

        int npcLayer = LayerMask.NameToLayer("NPC");
        int occluderMask = ~((1 << playerLayer) | (1 << npcLayer));
        return !Physics.Raycast(origin, toNPC / distance, distance, occluderMask, QueryTriggerInteraction.Ignore);
    }
}
