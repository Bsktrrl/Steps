using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_NPC_ColliderTrigger : MonoBehaviour
{
    [SerializeField] Interactable_NPC parentScript;

    [SerializeField] bool isColliding;


    //--------------------


    private void FixedUpdate()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (DisableDialogue())
        {
            ButtonMessages.Instance.HideButtonMessage();
            parentScript.canInteract = false;
            GetComponent<BoxCollider>().enabled = false;

            return;
        }

        if (other.CompareTag("Player") && !Player_CeilingGrab.Instance.isCeilingGrabbing && !Player_CeilingGrab.Instance.isCeilingRotation)
        {
            Vector3 toNPC = (transform.position - other.transform.position);
            toNPC.y = 0; // Flatten to horizontal plane
            toNPC.Normalize();

            Vector3 playerFacing = Movement.Instance.lookDir;
            playerFacing.y = 0;
            playerFacing.Normalize();

            // Round direction to nearest 45° step
            Vector3 roundedToNPC = RoundTo45Degrees(toNPC);

            if (roundedToNPC == playerFacing && HasLineOfSightToNPC())
            {
                ButtonMessages.Instance.ShowButtonMessage(
                    ControlButtons.Down,
                    MessageManager.Instance.Show_Message(MessageManager.Instance.interact_Talk_Message)
                );
                parentScript.canInteract = true;
                CameraController.Instance.CM_Other = parentScript.NPCVirtualCamera;
            }
            else
            {
                ButtonMessages.Instance.HideButtonMessage();
                parentScript.canInteract = false;
                CameraController.Instance.CM_Other = null;
            }

        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DisableDialogue())
        {
            ButtonMessages.Instance.HideButtonMessage();
            parentScript.canInteract = false;
            GetComponent<BoxCollider>().enabled = false;

            return;
        }

        if (other.CompareTag("Player") && !Player_CeilingGrab.Instance.isCeilingGrabbing && !Player_CeilingGrab.Instance.isCeilingRotation && !PlayerManager.Instance.npcInteraction && !PlayerManager.Instance.pauseGame)
        {
            Vector3 toNPC = (transform.position - other.transform.position);
            toNPC.y = 0; // Flatten to horizontal plane
            toNPC.Normalize();

            Vector3 playerFacing = Movement.Instance.lookDir;
            playerFacing.y = 0;
            playerFacing.Normalize();

            // Round direction to nearest 45° step
            Vector3 roundedToNPC = RoundTo45Degrees(toNPC);

            if (roundedToNPC == playerFacing && HasLineOfSightToNPC())
            {
                ButtonMessages.Instance.ShowButtonMessage(
                    ControlButtons.Down,
                    MessageManager.Instance.Show_Message(MessageManager.Instance.interact_Talk_Message)
                );
                parentScript.canInteract = true;
                CameraController.Instance.CM_Other = parentScript.NPCVirtualCamera;
            }
            else
            {
                ButtonMessages.Instance.HideButtonMessage();
                parentScript.canInteract = false;
                CameraController.Instance.CM_Other = null;
            }

        }
        else
        {
            ButtonMessages.Instance.HideButtonMessage();
            parentScript.canInteract = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !Player_CeilingGrab.Instance.isCeilingGrabbing && !Player_CeilingGrab.Instance.isCeilingRotation)
        {
            ButtonMessages.Instance.HideButtonMessage();
            parentScript.canInteract = false;

            CameraController.Instance.CM_Other = null;
        }
        else
        {
            print("22. Enabled = true");
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    private Vector3 RoundTo45Degrees(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return Vector3.zero;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;

        float rad = snappedAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)).normalized;
    }


    //--------------------


    bool DisableDialogue()
    {
        GameObject hitObject;
        var hitType = Movement.Instance.PerformMovementRaycast(
            PlayerManager.Instance.player.transform.position,
            PlayerManager.Instance.playerBody.transform.forward,
            1f,
            out hitObject
        );

        // ADDED: block dialogue if there is no line of sight from player to this NPC
        if (!HasLineOfSightToNPC()) return true;

        return hitType == RaycastHitObjects.Fence
            || hitType == RaycastHitObjects.Ladder
            || hitType == RaycastHitObjects.LadderBlocker;
    }

    // --------------------
    // ADDED: Line-of-sight check from player to this NPC.
    // The NPC is on layer "NPC". We ignore the Player layer and the NPC layer so
    // only true occluders (e.g., fences, walls, ladder blockers) will block LOS.
    bool HasLineOfSightToNPC()
    {
        if (parentScript == null) return true; // fail-open if no NPC reference

        Transform playerTf = PlayerManager.Instance.player.transform;
        Transform npcTf = parentScript.transform;

        // Aim from chest height to chest height
        Vector3 origin = playerTf.position;
        Vector3 target = npcTf.position;

        Vector3 dir = target - origin;
        float dist = dir.magnitude;
        if (dist <= 0.001f) return true;
        dir /= dist;

        // Build a mask that excludes Player and NPC layers so we only hit occluders.
        int playerLayer = PlayerManager.Instance.player.layer;
        int npcLayer = LayerMask.NameToLayer("NPC");
        int excludeMask = (1 << playerLayer) | (1 << npcLayer);
        int occluderMask = ~excludeMask; // everything except player+npc

        // Raycast: if we hit anything before the NPC on occluderMask, LOS is blocked.
        // Triggers are ignored so thin trigger volumes don't count as walls.
        if (Physics.Raycast(origin, dir, dist, occluderMask, QueryTriggerInteraction.Ignore))
            return false;

        return true;
    }
}
