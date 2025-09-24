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
        InteractWithObject();
    }


    //--------------------


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
