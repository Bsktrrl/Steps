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

            if (roundedToNPC == playerFacing)
            {
                ButtonMessages.Instance.ShowButtonMessage(ControlButtons.Down, MessageManager.Instance.Show_Message(MessageManager.Instance.interact_Talk_Message));
                parentScript.canInteract = true;

                CameraController.Instance.focusVirtualCamera = parentScript.NPCVirtualCamera;
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
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

            if (roundedToNPC == playerFacing)
            {
                ButtonMessages.Instance.ShowButtonMessage(ControlButtons.Down, MessageManager.Instance.Show_Message(MessageManager.Instance.interact_Talk_Message));
                parentScript.canInteract = true;

                CameraController.Instance.focusVirtualCamera = parentScript.NPCVirtualCamera;
            }
            else
            {
                ButtonMessages.Instance.HideButtonMessage();
                parentScript.canInteract = false;

                CameraController.Instance.focusVirtualCamera = null;
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

            CameraController.Instance.focusVirtualCamera = null;
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
}
