using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Object : MonoBehaviour
{
    [Header("Object Parameters")]
    public bool oneTimeInteractableObject;
    public int timesInteractedWith;
    public float animationWaitTime = 1;


    //--------------------


    public void Interact()
    {
        timesInteractedWith += 1;

        //If no Animation
        if (gameObject.GetComponent<Interactable_Animation>().AnimationName_StartInteraction == ""
            && gameObject.GetComponent<Interactable_Animation>().AnimationName_EndInteraction == "")
        {
            GetStuff();
        }

        //If Object has Animation
        else
        {
            gameObject.GetComponent<Interactable_Animation>().StartAnimation();
        }

        //Perform Animation
        StartCoroutine(AnimationWaitTime(animationWaitTime));
    }

    IEnumerator AnimationWaitTime(float wait)
    {
        yield return new WaitForSeconds(wait);

        if (timesInteractedWith % 2 == 1)
        {
            GetStuff();
        }

        Player_Interact.Instance.isInteracting = false;
    }

    void GetStuff()
    {
        //Get any available Items
        if (GetComponent<Interactable_GetItem>())
            GetComponent<Interactable_GetItem>().GetItems();

        //Get any available Abilities
        if (GetComponent<Interactable_GetAbility>())
            GetComponent<Interactable_GetAbility>().GetAbility();
    }
}
