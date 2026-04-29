using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_BurnTransforming : MonoBehaviour
{
    [Header("New Block When Burned")]
    [SerializeField] GameObject burningBlock;

    [Header("Other Parameters")]
    [SerializeField] GameObject burningBlock_InScene;
    public bool isSteppedOn;

    private bool isBurningBlock;

    private Renderer[] originalRenderers;
    private Collider[] originalColliders;


    //--------------------


    private void Awake()
    {
        originalRenderers = GetComponentsInChildren<Renderer>();
        originalColliders = GetComponentsInChildren<Collider>();
    }

    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += BurnBlock;
        Movement.Action_RespawnPlayerEarly += ResetBlock;

        Player_Burning.Action_PlayerStartedBurning += BurnBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= BurnBlock;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;

        Player_Burning.Action_PlayerStartedBurning -= BurnBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (burningBlock_InScene != null) { return; }

        if (Movement.Instance.blockStandingOn == gameObject)
            isSteppedOn = true;
        else
        {
            ResetBlock();
        }
    }

    void BurnBlock()
    {
        if (!Player_Burning.Instance.isBurning || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (burningBlock_InScene != null) { return; }

        if (isBurningBlock) { return; }

        if (!isSteppedOn && Movement.Instance.blockStandingOn != gameObject) { return; }

        StartCoroutine(WaitBeforeBurningBlock(0.005f));
    }

    IEnumerator WaitBeforeBurningBlock(float waitTime)
    {
        isBurningBlock = true;

        yield return new WaitForSeconds(waitTime);

        if (burningBlock_InScene != null)
        {
            isBurningBlock = false;
            yield break;
        }

        if (Movement.Instance.blockStandingOn != gameObject)
        {
            isBurningBlock = false;
            yield break;
        }

        burningBlock_InScene = Instantiate(burningBlock, transform.position, transform.rotation);
        burningBlock_InScene.transform.SetParent(transform, true);

        Movement.Instance.blockStandingOn = burningBlock_InScene;

        HideOriginalBlock();

        isSteppedOn = false;
        isBurningBlock = false;

        Movement.Instance.UpdateAvailableMovementBlocks();
    }

    public void ResetBlock()
    {
        isSteppedOn = false;
        isBurningBlock = false;

        GameObject blockToRemove = burningBlock_InScene;

        if (blockToRemove != null)
        {
            if (Movement.Instance.blockStandingOn == blockToRemove)
            {
                Movement.Instance.blockStandingOn = gameObject;
            }

            Destroy(blockToRemove);
            burningBlock_InScene = null;
        }

        ShowOriginalBlock();

        // Important:
        // Do NOT call Movement.Instance.UpdateAvailableMovementBlocks() here.
        // ResetBlock can be called from Movement.Action_isSwitchingBlocks.
        // Calling UpdateAvailableMovementBlocks here can create an infinite loop.
    }

    void HideOriginalBlock()
    {
        for (int i = 0; i < originalRenderers.Length; i++)
        {
            if (originalRenderers[i] != null)
            {
                originalRenderers[i].enabled = false;
            }
        }

        for (int i = 0; i < originalColliders.Length; i++)
        {
            if (originalColliders[i] != null)
            {
                originalColliders[i].enabled = false;
            }
        }
    }

    void ShowOriginalBlock()
    {
        for (int i = 0; i < originalRenderers.Length; i++)
        {
            if (originalRenderers[i] != null)
            {
                originalRenderers[i].enabled = true;
            }
        }

        for (int i = 0; i < originalColliders.Length; i++)
        {
            if (originalColliders[i] != null)
            {
                originalColliders[i].enabled = true;
            }
        }
    }
}