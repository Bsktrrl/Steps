using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Timeline;

public class Player_GraplingHook : Singleton<Player_GraplingHook>
{
    [Header("Grappling Distance")]
    [SerializeField] float grapplingDistance = 5;

    [Header("Red Dot Object")]
    [SerializeField] GameObject redDot_Parent;
    [SerializeField] GameObject redDot;
    GameObject redDotSceneObject;

    RaycastHit hit;

    Vector3 endPoint;
    public LineRenderer lineRenderer;

    public bool isGrapplingHooking;

    Vector3 endDestination;

    [SerializeField] GameObject blockUnderPlayer_Old;
    [SerializeField] GameObject blockUnderPlayer_New;


    //--------------------


    private void Start()
    {
        redDotSceneObject = Instantiate(redDot);
        redDotSceneObject.transform.parent = redDot_Parent.transform;
        redDotSceneObject.SetActive(false);

        lineRenderer.positionCount = 2;
        EndLineRenderer();
    }
    private void Update()
    {
        if (isGrapplingHooking)
        {
            PerformGrapplingMovement();
        }
    }


    //--------------------


    public void StartRaycastGrappling()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { EndLineRenderer(); return; }
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) { return; }

        UngoingRaycastGrappling();
    }
    public void StopRaycastGrappling()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { EndLineRenderer(); return; }

        EndLineRenderer();
        ResetRaycastBlockUnder();

        redDotSceneObject.SetActive(false);
    }
    public void UngoingRaycastGrappling()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { EndLineRenderer(); return; }
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) { return; }

        endPoint = transform.position + (Player_BlockDetector.Instance.lookDir * grapplingDistance) + (-Player_BlockDetector.Instance.lookDir * 0.05f);

        if (Physics.Raycast(transform.position, Player_BlockDetector.Instance.lookDir, out hit, grapplingDistance))
        {
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    endPoint = hit.point + (-Player_BlockDetector.Instance.lookDir * 0.05f);
                    redDotSceneObject.transform.position = endPoint;

                    redDotSceneObject.transform.rotation = Quaternion.LookRotation(hit.normal);

                    redDotSceneObject.SetActive(true);

                    RunLineReader();

                    return;
                }
            }
        }

        RunLineReader();
        redDotSceneObject.SetActive(false);
    }

    void RunLineReader()
    {
        //Beam Start
        lineRenderer.SetPosition(0, transform.position);

        //Beam End
        lineRenderer.SetPosition(1, endPoint);
    }
    void EndLineRenderer()
    {
        //Beam Start
        lineRenderer.SetPosition(0, transform.position);

        //Beam End
        lineRenderer.SetPosition(1, transform.position);

        redDotSceneObject.SetActive(false);
    }


    //--------------------


    public bool CheckIfCanGrapple()
    {
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook || PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
        {
            if (redDotSceneObject.activeInHierarchy)
            {
                //Check if the block forward-under the targetBlock can be standing on (Raycast down from hit.point - 0.5 to get the block)
                if (Physics.Raycast(endPoint + (-Player_BlockDetector.Instance.lookDir * 0.25f), Vector3.down, out hit, 1))
                {
                    if (hit.transform.gameObject)
                    {
                        if (hit.transform.gameObject.GetComponent<BlockInfo>())
                        {
                            //Set the block forward-under as the target position
                            endDestination = hit.transform.gameObject.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

                            RaycastDown_Old();

                            return true;
                        }
                    }
                }
            }
        }
        
        return false;
    }
    public void PerformGrapplingMovement()
    {
        isGrapplingHooking = true;

        //Move the player with the speed of 8 towards the target position (Have GrapplingHookLine and redDot visible and Update the GrapplingHookLine to fit the playerPos)
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endDestination, 25 * Time.deltaTime);

        //Check if there is new blocks hovering over, to reduce the stepCount by this amount
        RaycastDown_New();
        if (blockUnderPlayer_Old != blockUnderPlayer_New && blockUnderPlayer_New.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock) != endDestination)
        {
            PlayerStats.Instance.stats.steps_Current -= blockUnderPlayer_New.GetComponent<BlockInfo>().movementCost;
            Player_Movement.Instance.Action_ResetBlockColorInvoke();

            //If steps is < 0
            if (PlayerStats.Instance.stats.steps_Current < 0)
            {
                PlayerStats.Instance.stats.steps_Current = 0;

                StopRaycastGrappling();
                isGrapplingHooking = false;
                Player_Movement.Instance.movementStates = MovementStates.Still;

                PlayerStats.Instance.RespawnPlayer();
            }
        }
        RaycastDown_Old();

        //Move the GrapplingLine with the player
        UngoingRaycastGrappling();

        //Snap into place when close enough     
        if (Vector3.Distance(PlayerManager.Instance.player.transform.position, endDestination) <= 0.05f)
        {
            StopRaycastGrappling();

            PlayerManager.Instance.player.transform.position = endDestination;
            Player_Movement.Instance.movementStates = MovementStates.Still;

            Player_Movement.Instance.Action_StepTakenInvoke();
            Player_Movement.Instance.Action_ResetBlockColorInvoke();

            isGrapplingHooking = false;
        }
    }
    void RaycastDown_Old()
    {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    blockUnderPlayer_Old = hit.transform.gameObject;
                    print("1. Hit.name: " + hit.transform.gameObject.name);
                }
            }
        }
    }
    void RaycastDown_New()
    {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    blockUnderPlayer_New = hit.transform.gameObject;
                    print("2. Hit.name: " + hit.transform.gameObject.name);
                }
            }
        }
    }
    void ResetRaycastBlockUnder()
    {
        blockUnderPlayer_Old = null;
        blockUnderPlayer_New = null;
    }
}