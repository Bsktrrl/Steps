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
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                if (MainManager.Instance.block_Horizontal_InFront != null && Cameras.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InFront);
                else if (MainManager.Instance.block_Horizontal_InBack != null && Cameras.Instance.directionFacing == Vector3.back)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InBack);
                else if (MainManager.Instance.block_Horizontal_ToTheLeft != null && Cameras.Instance.directionFacing == Vector3.left)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheLeft);
                else if (MainManager.Instance.block_Horizontal_ToTheRight != null && Cameras.Instance.directionFacing == Vector3.right)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraState.Backward:
                if (MainManager.Instance.block_Horizontal_InBack != null && Cameras.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InFront);
                else if (MainManager.Instance.block_Horizontal_InFront != null && Cameras.Instance.directionFacing == Vector3.back)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InBack);
                else if (MainManager.Instance.block_Horizontal_ToTheRight != null && Cameras.Instance.directionFacing == Vector3.left)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheLeft);
                else if (MainManager.Instance.block_Horizontal_ToTheLeft != null && Cameras.Instance.directionFacing == Vector3.right)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraState.Left:
                if (MainManager.Instance.block_Horizontal_ToTheLeft != null && Cameras.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InFront);
                else if (MainManager.Instance.block_Horizontal_ToTheRight != null && Cameras.Instance.directionFacing == Vector3.back)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InBack);
                else if (MainManager.Instance.block_Horizontal_InFront != null && Cameras.Instance.directionFacing == Vector3.left)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheLeft);
                else if (MainManager.Instance.block_Horizontal_InBack != null && Cameras.Instance.directionFacing == Vector3.right)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheRight);
                else
                {
                    canInteract = false;
                    interactableObject = null;
                }
                break;
            case CameraState.Right:
                if (MainManager.Instance.block_Horizontal_ToTheRight != null && Cameras.Instance.directionFacing == Vector3.forward)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InFront);
                else if (MainManager.Instance.block_Horizontal_ToTheLeft != null && Cameras.Instance.directionFacing == Vector3.back)
                    PerformInteraction(MainManager.Instance.block_Horizontal_InBack);
                else if (MainManager.Instance.block_Horizontal_InBack != null && Cameras.Instance.directionFacing == Vector3.left)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheLeft);
                else if (MainManager.Instance.block_Horizontal_InFront != null && Cameras.Instance.directionFacing == Vector3.right)
                    PerformInteraction(MainManager.Instance.block_Horizontal_ToTheRight);
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
            if (detectedBlockInfo.block.GetComponent<InteractableObject>())
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

    void InteractWithObject()
    {
        //Don't be able to perform interaction if the Object is Interacted With
        if (isInteracting) { return;  }

        if (canInteract && interactableObject)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                if (interactableObject.GetComponent<InteractableObject>().oneTimeInteractableObject)
                {
                    if (interactableObject.GetComponent<InteractableObject>().timesInteractedWith <= 0)
                    {
                        interactableObject.GetComponent<InteractableObject>().Interact();
                        isInteracting = true;
                    }
                }
                else
                {
                    interactableObject.GetComponent<InteractableObject>().Interact();
                    isInteracting = true;
                }
            }
        }
    }
}
