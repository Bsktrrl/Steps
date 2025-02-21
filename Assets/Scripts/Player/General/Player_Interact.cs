using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : Singleton<Player_Interact>
{
    public bool canInteract;
    public GameObject interactableObject;

    public bool isInteracting;


    //--------------------


    private void Update()
    {
        CheckForInteractableObject();

        InteractWithObject();
    }


    //--------------------


    void CheckForInteractableObject()
    {
        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (PlayerManager.Instance.block_Horizontal_InFront != null && CameraController.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InFront);
                else if (PlayerManager.Instance.block_Horizontal_InBack != null && CameraController.Instance.directionFacing == Vector3.back)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InBack);
                else if (PlayerManager.Instance.block_Horizontal_ToTheLeft != null && CameraController.Instance.directionFacing == Vector3.left)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheLeft);
                else if (PlayerManager.Instance.block_Horizontal_ToTheRight != null && CameraController.Instance.directionFacing == Vector3.right)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraRotationState.Backward:
                if (PlayerManager.Instance.block_Horizontal_InBack != null && CameraController.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InFront);
                else if (PlayerManager.Instance.block_Horizontal_InFront != null && CameraController.Instance.directionFacing == Vector3.back)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InBack);
                else if (PlayerManager.Instance.block_Horizontal_ToTheRight != null && CameraController.Instance.directionFacing == Vector3.left)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheLeft);
                else if (PlayerManager.Instance.block_Horizontal_ToTheLeft != null && CameraController.Instance.directionFacing == Vector3.right)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraRotationState.Left:
                if (PlayerManager.Instance.block_Horizontal_ToTheLeft != null && CameraController.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InFront);
                else if (PlayerManager.Instance.block_Horizontal_ToTheRight != null && CameraController.Instance.directionFacing == Vector3.back)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InBack);
                else if (PlayerManager.Instance.block_Horizontal_InFront != null && CameraController.Instance.directionFacing == Vector3.left)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheLeft);
                else if (PlayerManager.Instance.block_Horizontal_InBack != null && CameraController.Instance.directionFacing == Vector3.right)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraRotationState.Right:
                if (PlayerManager.Instance.block_Horizontal_ToTheRight != null && CameraController.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InFront);
                else if (PlayerManager.Instance.block_Horizontal_ToTheLeft != null && CameraController.Instance.directionFacing == Vector3.back)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_InBack);
                else if (PlayerManager.Instance.block_Horizontal_InBack != null && CameraController.Instance.directionFacing == Vector3.left)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheLeft);
                else if (PlayerManager.Instance.block_Horizontal_InFront != null && CameraController.Instance.directionFacing == Vector3.right)
                    PerformInteraction(PlayerManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;

            default:
                break;
        }
    }
    void PerformInteraction(DetectedBlockInfo detectedBlockInfo)
    {
        if (detectedBlockInfo.block != null)
        {
            if (detectedBlockInfo.block.GetComponent<Interactable_Object>())
            {
                canInteract = true;
                interactableObject = detectedBlockInfo.block;
            }
        }
        else
        {
            canInteract = false;
            interactableObject = null;
        }
    }

    public void InteractWithObject()
    {
        //Don't be able to perform interaction if the Object is Interacted With
        if (isInteracting) { return;  }

        if (canInteract && interactableObject)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                if (interactableObject.GetComponent<Interactable_Object>().oneTimeInteractableObject)
                {
                    if (interactableObject.GetComponent<Interactable_Object>().timesInteractedWith <= 0)
                    {
                        interactableObject.GetComponent<Interactable_Object>().Interact();
                        isInteracting = true;
                    }
                }
                else
                {
                    interactableObject.GetComponent<Interactable_Object>().Interact();
                    isInteracting = true;
                }
            }
        }
    }
}
