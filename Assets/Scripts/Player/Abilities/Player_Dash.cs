using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player_Dash : Singleton<Player_Dash>
{
    [Header("Dashing")]
    [SerializeField] Vector3 dashStartPos;

    public bool canDash_Forward;
    public bool canDash_Back;
    public bool canDash_Left;
    public bool canDash_Right;

    public GameObject dashTarget_Forward;
    public GameObject dashTarget_Back;
    public GameObject dashTarget_Left;
    public GameObject dashTarget_Right;

    public bool isDashing;

    [Header("Other")]
    RaycastHit hit;

    float dashDuration = 0.2f;


    //--------------------


    private void Update()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (isDashing) { return; }

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //SetDashDirections();

        if (PlayerManager.Instance.forward_isPressed /*&& !PlayerManager.Instance.canMove_Forward*/)
            Dash_Forward();
        else if (PlayerManager.Instance.back_isPressed /*&& !PlayerManager.Instance.canMove_Back*/)
            Dash_Backward();
        else if (PlayerManager.Instance.left_isPressed /*&& !PlayerManager.Instance.canMove_Left*/)
            Dash_Left();
        else if (PlayerManager.Instance.right_isPressed /*&& !PlayerManager.Instance.canMove_Right*/)
            Dash_Right();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetDashDirections;
        Player_Movement.Action_StepTaken += SetDashDirections;
        Player_Movement.Action_BodyRotated += SetDashDirections;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetDashDirections;
        Player_Movement.Action_StepTaken -= SetDashDirections;
        Player_Movement.Action_BodyRotated -= SetDashDirections;
    }


    //--------------------


    bool DashChecks()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return false; }

        if (isDashing) { return false; }

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.isTransportingPlayer) { return false; }

        return true;
    }
    public void Dash_Forward()
    {
        if (!DashChecks()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                //canDash_Forward = CheckIfCanDash(ref dashTarget_Forward, Vector3.forward);
                if (canDash_Forward && dashTarget_Forward)
                    StartCoroutine(DashRoutine(dashTarget_Forward));
                break;
            case CameraRotationState.Backward:
                //canDash_Back = CheckIfCanDash(ref dashTarget_Back, Vector3.back);
                if (canDash_Back && dashTarget_Back)
                    StartCoroutine(DashRoutine(dashTarget_Back));
                break;
            case CameraRotationState.Left:
                //canDash_Right = CheckIfCanDash(ref dashTarget_Right, Vector3.right);
                if (canDash_Right && dashTarget_Right)
                    StartCoroutine(DashRoutine(dashTarget_Right));
                break;
            case CameraRotationState.Right:
                //canDash_Left = CheckIfCanDash(ref dashTarget_Left, Vector3.left);
                if (canDash_Left && dashTarget_Left)
                    StartCoroutine(DashRoutine(dashTarget_Left));
                break;
            default:
                break;
        }
    }
    public void Dash_Backward()
    {
        if (!DashChecks()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                //canDash_Back = CheckIfCanDash(ref dashTarget_Back, Vector3.back);
                if (canDash_Back && dashTarget_Back)
                    StartCoroutine(DashRoutine(dashTarget_Back));
                break;
            case CameraRotationState.Backward:
                //canDash_Forward = CheckIfCanDash(ref dashTarget_Forward, Vector3.forward);
                if (canDash_Forward && dashTarget_Forward)
                    StartCoroutine(DashRoutine(dashTarget_Forward));
                break;
            case CameraRotationState.Left:
                //canDash_Left = CheckIfCanDash(ref dashTarget_Left, Vector3.left);
                if (canDash_Left && dashTarget_Left)
                    StartCoroutine(DashRoutine(dashTarget_Left));
                break;
            case CameraRotationState.Right:
                //canDash_Right = CheckIfCanDash(ref dashTarget_Right, Vector3.right);
                if (canDash_Right && dashTarget_Right)
                    StartCoroutine(DashRoutine(dashTarget_Right));
                break;
            default:
                break;
        }
    }
    public void Dash_Left()
    {
        if (!DashChecks()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                //canDash_Left = CheckIfCanDash(ref dashTarget_Left, Vector3.left);
                if (canDash_Left && dashTarget_Left)
                    StartCoroutine(DashRoutine(dashTarget_Left));
                break;
            case CameraRotationState.Backward:
                //canDash_Right = CheckIfCanDash(ref dashTarget_Right, Vector3.right);
                if (canDash_Right && dashTarget_Right)
                    StartCoroutine(DashRoutine(dashTarget_Right));
                break;
            case CameraRotationState.Left:
                //canDash_Forward = CheckIfCanDash(ref dashTarget_Forward, Vector3.forward);
                if (canDash_Forward && dashTarget_Forward)
                    StartCoroutine(DashRoutine(dashTarget_Forward));
                break;
            case CameraRotationState.Right:
                //canDash_Back = CheckIfCanDash(ref dashTarget_Back, Vector3.back);
                if (canDash_Back && dashTarget_Back)
                    StartCoroutine(DashRoutine(dashTarget_Back));
                break;
            default:
                break;
        }
    }
    public void Dash_Right()
    {
        if (!DashChecks()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                //canDash_Right = CheckIfCanDash(ref dashTarget_Right, Vector3.right);
                if (canDash_Right && dashTarget_Right)
                    StartCoroutine(DashRoutine(dashTarget_Right));
                break;
            case CameraRotationState.Backward:
                //canDash_Left = CheckIfCanDash(ref dashTarget_Left, Vector3.left);
                if (canDash_Left && dashTarget_Left)
                    StartCoroutine(DashRoutine(dashTarget_Left));
                break;
            case CameraRotationState.Left:
                //canDash_Back = CheckIfCanDash(ref dashTarget_Back, Vector3.back);
                if (canDash_Back && dashTarget_Back)
                    StartCoroutine(DashRoutine(dashTarget_Back));
                break;
            case CameraRotationState.Right:
                //canDash_Forward = CheckIfCanDash(ref dashTarget_Forward, Vector3.forward);
                if (canDash_Forward && dashTarget_Forward)
                    StartCoroutine(DashRoutine(dashTarget_Forward));
                break;
            default:
                break;
        }
    }


    //--------------------


    void SetDashDirections()
    {
        canDash_Forward = false;
        canDash_Back = false;
        canDash_Left = false;
        canDash_Right = false;

        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash) { return; }

        ResetTargetBlock(ref dashTarget_Forward);
        ResetTargetBlock(ref dashTarget_Back);
        ResetTargetBlock(ref dashTarget_Left);
        ResetTargetBlock(ref dashTarget_Right);

        //Check if can Dash and get DashTarget
        canDash_Forward = CheckIfCanDash(ref dashTarget_Forward, Vector3.forward);
        canDash_Back = CheckIfCanDash(ref dashTarget_Back, Vector3.back);
        canDash_Left = CheckIfCanDash(ref dashTarget_Left, Vector3.left);
        canDash_Right = CheckIfCanDash(ref dashTarget_Right, Vector3.right);
    }
    bool CheckIfCanDash(ref GameObject target, Vector3 dir)
    {
        ResetDarkenColorIfStepsIsGone(ref target);

        //Raycast target +1
        if (Physics.Raycast(gameObject.transform.position, dir, out hit, 1))
        {
            //if (/*hit.transform.gameObject.GetComponent<Block_Water>() ||*/ hit.transform.gameObject.GetComponent<Block_Moveable>())
            //{
            //    ResetTargetBlock(ref target);
            //    target = null;
            //    return false;
            //}
        }
        else
        {
            ResetTargetBlock(ref target);
            target = null;
            return false;
        }

        //Raycast target +2
        if (Physics.Raycast(gameObject.transform.position + dir, dir, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }

        //Raycast down from target +2
        if (Physics.Raycast(gameObject.transform.position + (dir * 2), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                target = hit.transform.gameObject;

                //Darken color in target block
                if (target)
                {
                    if (target.GetComponent<BlockInfo>())
                    {
                        target.GetComponent<BlockInfo>().DarkenColors();
                    }
                }

                ResetDarkenColorIfStepsIsGone(ref target);

                return true;
            }
            else
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }
        else
        {
            ResetTargetBlock(ref target);
            target = null;
            return false; 
        }
    }
    void ResetDarkenColorIfStepsIsGone(ref GameObject target)
    {
        //Cannot Jump if having 0 movement and targetBlock has a MovementCost
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        {
            if (target)
            {
                if (target.GetComponent<BlockInfo>().movementCost > 0)
                {
                    target.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
        }
    }
    void ResetTargetBlock(ref GameObject target)
    {
        //Reset Darken Color
        if (target)
        {
            target.GetComponent<BlockInfo>().ResetDarkenColor();
            target = null;
        }
    }


    //--------------------


    #region Old
    //void CheckHitDirection()
    //{
    //    if (PlayerManager.Instance.block_LookingAt_Horizontal)
    //    {
    //        if (Player_BlockDetector.Instance.lookDir == Vector3.forward)
    //            hitDir = HitDirection.Forward;
    //        else if (Player_BlockDetector.Instance.lookDir == Vector3.back)
    //            hitDir = HitDirection.Backward;
    //        else if (Player_BlockDetector.Instance.lookDir == Vector3.left)
    //            hitDir = HitDirection.Left;
    //        else if (Player_BlockDetector.Instance.lookDir == Vector3.right)
    //            hitDir = HitDirection.Right;
    //    }
    //    else
    //    {
    //        hitDir = HitDirection.None;
    //    }
    //}
    //void CheckForDash()
    //{
    //    if (PlayerManager.Instance.block_LookingAt_Horizontal)
    //    {
    //        if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>())
    //        {
    //            dashBlock_Previous = dashBlock_Current;

    //            switch (hitDir)
    //            {
    //                case HitDirection.None:
    //                    dashBlock_Current = null;
    //                    break;

    //                case HitDirection.Forward:
    //                    dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Front;
    //                    dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Front;
    //                    break;
    //                case HitDirection.Backward:
    //                    dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Back;
    //                    dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Back;
    //                    break;
    //                case HitDirection.Left:
    //                    dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Center_Left;
    //                    dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Center_Left;
    //                    break;
    //                case HitDirection.Right:
    //                    dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Center_Right;
    //                    dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Center_Right;
    //                    break;

    //                default:
    //                    dashBlock_Current = null;
    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            dashBlock_Current = null;
    //        }
    //    }
    //    else
    //    {
    //        dashBlock_Current = null;
    //    }

    //    if (dashBlock_Current)
    //    {
    //        if (dashBlock_Current.GetComponent<BlockInfo>())
    //        {
    //            if (PlayerStats.Instance.stats.abilitiesGot_Permanent != null || PlayerStats.Instance.stats.abilitiesGot_Temporary != null)
    //            {
    //                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash)
    //                {
    //                    if ((dashBlock_Current && !dashBlockOver_Current && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash)
    //                    || (dashBlock_Current && !dashBlockOver_Current.activeInHierarchy && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash))))
    //                    {
    //                        if (dashBlock_Current.GetComponent<BlockInfo>())
    //                        {
    //                            if (dashBlock_Current != dashBlock_Previous)
    //                            {
    //                                if (dashBlock_Previous)
    //                                {
    //                                    if (dashBlock_Previous.GetComponent<BlockInfo>())
    //                                    {
    //                                        dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
    //                                    }
    //                                }
    //                            }

    //                            dashBlock_Current.GetComponent<BlockInfo>().DarkenColors();
    //                            playerCanDash = true;
    //                        }
    //                    }
    //                    else if (((PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit) && dashBlock_Current && dashBlockOver_Current && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash))
    //                        || ((PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit) && !dashBlock_Current.activeInHierarchy && dashBlockOver_Current && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash)))
    //                    {
    //                        if (dashBlock_Current.GetComponent<BlockInfo>() && dashBlock_Current.GetComponent<Block_Water>())
    //                        {
    //                            if (dashBlock_Current != dashBlock_Previous)
    //                            {
    //                                if (dashBlock_Previous)
    //                                {
    //                                    if (dashBlock_Previous.GetComponent<BlockInfo>())
    //                                    {
    //                                        dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
    //                                    }
    //                                }
    //                            }

    //                            dashBlock_Current.GetComponent<BlockInfo>().DarkenColors();
    //                            playerCanDash = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (dashBlock_Previous)
    //                        {
    //                            if (dashBlock_Previous.GetComponent<BlockInfo>())
    //                            {
    //                                dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
    //                                dashBlock_Previous = null;
    //                                playerCanDash = false;
    //                            }
    //                        }
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                ResetPreviousColor();
    //            }
    //        }
    //        else
    //        {
    //            ResetPreviousColor();
    //        }
    //    }
    //    else
    //    {
    //        ResetPreviousColor();
    //    }
    //}
    //void ResetPreviousColor()
    //{
    //    if (dashBlock_Previous)
    //    {
    //        if (dashBlock_Previous.GetComponent<BlockInfo>())
    //        {
    //            dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
    //            dashBlock_Previous = null;
    //            playerCanDash = false;
    //        }
    //    }
    //}

    //public void Dash()
    //{
    //    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Dash || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash)
    //    {
    //        PlayerManager.Instance.pauseGame = true;
    //        PlayerManager.Instance.isTransportingPlayer = true;
    //        isDashing = true;
    //        Player_Movement.Instance.movementStates = MovementStates.Moving;

    //        dashBlock_Target = dashBlock_Current;

    //        Player_Movement.Instance.Action_ResetBlockColorInvoke();
    //    }
    //}

    //public void PerformDashMovement()
    //{
    //    if (isDashing && dashBlock_Target)
    //    {
    //        Vector3 targetPos = dashBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

    //        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, targetPos, dashSpeed * Time.deltaTime);

    //        //Snap into place when close enough
    //        if (Vector3.Distance(PlayerManager.Instance.player.transform.position, targetPos) <= 0.03f)
    //        {
    //            PlayerManager.Instance.player.transform.position = targetPos;

    //            Player_Movement.Instance.movementStates = MovementStates.Still;
    //            PlayerManager.Instance.pauseGame = false;
    //            PlayerManager.Instance.isTransportingPlayer = false;
    //            isDashing = false;

    //            Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

    //            if (PlayerManager.Instance.block_StandingOn_Current.block)
    //            {
    //                if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
    //                {
    //                    Player_Movement.Instance.currentMovementCost = PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
    //                }
    //            }

    //            dashBlock_Target = null;

    //            Player_Movement.Instance.Action_StepTaken_Invoke();
    //            Player_Movement.Instance.Action_ResetBlockColorInvoke();
    //        }
    //    }
    //}
    #endregion


    //--------------------


    private IEnumerator DashRoutine(GameObject target)
    {
        isDashing = true;
        dashStartPos = gameObject.transform.position;

        Player_Movement.Instance.movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (target)
            endPosition = target.transform.position + (Vector3.up * (Player_Movement.Instance.heightOverBlock));
        else
            endPosition = dashStartPos;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / dashDuration;

            // Interpolate the forward position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Calculate the vertical position to create a parabolic motion
            //float height = 4 * progress * (1 - progress); // Parabola equation
            //currentPosition.y += height;

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        Player_BlockDetector.Instance.RaycastSetup();

        isDashing = false;
        Player_Movement.Instance.movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        Player_Movement.Instance.Action_ResetBlockColorInvoke();
        Player_Movement.Instance.Action_StepTaken_Invoke();
    }
}

public enum HitDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right,
}
