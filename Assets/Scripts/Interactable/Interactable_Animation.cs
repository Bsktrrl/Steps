using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Animation : MonoBehaviour
{
    [Header("Components")]
    public Animator chestAnimator;
    public AudioSource openSound;

    [Header("Animation Name")]
    public string AnimationName_StartInteraction;
    public string AnimationName_EndInteraction;

    bool isInteractedWith = false;


    //--------------------


    void Start()
    {
        //Ensure Animator is assigned
        if (chestAnimator == null)
        {
            chestAnimator = GetComponent<Animator>();
        }

        //Ensure AudioSource is assigned
        if (openSound == null)
        {
            openSound = GetComponent<AudioSource>();
        }
    }


    //--------------------


    public void StartAnimation()
    {
        if (!isInteractedWith)
        {
            if (AnimationName_StartInteraction != "")
            {
                //Perform the animation
                chestAnimator.SetTrigger(AnimationName_StartInteraction);

                //Play sound, if any
                if (openSound != null)
                    openSound.Play();

                isInteractedWith = true;
            }
        }
        else
        {
            if (AnimationName_EndInteraction != "")
            {
                //Perform the animation
                chestAnimator.SetTrigger(AnimationName_EndInteraction);

                //Play sound, if any
                if (openSound != null)
                    openSound.Play();

                isInteractedWith = false;
            }
        }
    }
}
