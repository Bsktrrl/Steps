using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block_Weak : MonoBehaviour
{
    public static event Action Action_WalkedOnCrackedIce;
    public static event Action Action_WalkedOffCrackedIce;

    [SerializeField] GameObject originalBlock;
    [SerializeField] GameObject newBlock;

    [SerializeField] GameObject IceCracked_Parent;
    [SerializeField] GameObject IceCracked_Parent_InScene;

    // One effect instance per cracked-ice tile, so rapidly gliding blocks
    // do not restart a shared particle system on every new tile.
    [SerializeField] private CrackedIceScript moveableIceEffectPrefab;
    [SerializeField] private float moveableEffectLifetime = 2.5f;

    public bool isSteppedOn;

    private bool isDissolving;
    private bool pendingBreakFromJump;
    private CrackedIceScript moveableIceEffectInstance;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += DisolveBlock;
        Movement.Action_isJumping += CheckIfJumpingFromBlock;
        Movement.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= DisolveBlock;
        Movement.Action_isJumping -= CheckIfJumpingFromBlock;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous == gameObject)
        {
            isSteppedOn = true;
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Ice && Movement.Instance.blockStandingOn.GetComponent<Block_Weak>())
            {
                IceBreakingEffect_Manager.Instance.CrackIce_Start();
            }

            if (!isDissolving && !pendingBreakFromJump)
            {
                ResetBlock();
            }
        }
    }

    void CheckIfJumpingFromBlock()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            return;

        if (Movement.Instance.currentMoveStartBlock == gameObject)
        {
            pendingBreakFromJump = true;
        }
    }

    void DisolveBlock()
    {
        if ((!isSteppedOn && !pendingBreakFromJump) ||
            Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            return;
        }

        pendingBreakFromJump = false;
        StartDissolving(false);
    }

    // Called when a movable block arrives above this cracked-ice tile.
    public void CrackFromMoveableBlock()
    {
        if (!isActiveAndEnabled || isDissolving || moveableIceEffectPrefab == null)
            return;

        if (moveableIceEffectInstance != null)
            return;

        moveableIceEffectInstance = Instantiate(
            moveableIceEffectPrefab,
            transform.position,
            transform.rotation
        );

        moveableIceEffectInstance.Crack();
    }

    // Called when a movable block leaves this cracked ice.
    public void BreakFromMoveableBlock()
    {
        if (!isActiveAndEnabled)
            return;

        StartDissolving(true);
    }

    private void StartDissolving(bool fromMoveableBlock)
    {
        if (isDissolving)
            return;

        isDissolving = true;
        StartCoroutine(WaitBeforeDisolveBlock(0.145f, fromMoveableBlock));
    }

    IEnumerator WaitBeforeDisolveBlock(float waitTime, bool fromMoveableBlock)
    {
        Vector3 breakPosition = transform.position;
        Quaternion breakRotation = transform.rotation;

        if (fromMoveableBlock && moveableIceEffectPrefab != null)
        {
            // Use this tile's own effect instance instead of reusing the
            // effect manager's shared instance for consecutive quick steps.
            if (moveableIceEffectInstance == null)
            {
                moveableIceEffectInstance = Instantiate(
                    moveableIceEffectPrefab,
                    breakPosition,
                    breakRotation
                );
            }

            moveableIceEffectInstance.Break();
            Destroy(moveableIceEffectInstance.gameObject,
                Mathf.Max(0.1f, moveableEffectLifetime));
            moveableIceEffectInstance = null;
        }
        else
        {
            // Preserve existing player behavior (and fallback if no
            // movable-specific effect prefab has been assigned).
            IceBreakingEffect_Manager.Instance.BreakIce_Start(breakPosition, breakRotation);
        }

        yield return new WaitForSeconds(waitTime);

        if (originalBlock)
        {
            originalBlock = Instantiate(newBlock, breakPosition, Quaternion.identity);
            Player_BurnChanging.Instance.AddBurnedBlockToList(originalBlock);
        }

        gameObject.SetActive(false);

        isSteppedOn = false;
        isDissolving = false;
        pendingBreakFromJump = false;

        if (!Movement.Instance.isIceGliding)
        {
            Movement.Instance.UpdateAvailableMovementBlocks();
        }
    }

    public void ResetBlock()
    {
        StopAllCoroutines();

        isSteppedOn = false;
        isDissolving = false;
        pendingBreakFromJump = false;

        // An effect waiting for its break can be removed on reset.
        if (moveableIceEffectInstance != null)
        {
            Destroy(moveableIceEffectInstance.gameObject);
            moveableIceEffectInstance = null;
        }

        gameObject.SetActive(true);

        if (originalBlock != null)
        {
            Destroy(originalBlock);
            originalBlock = null;
        }
    }
}
