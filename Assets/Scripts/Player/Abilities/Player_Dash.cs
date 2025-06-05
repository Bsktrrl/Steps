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

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        //if (PlayerManager.Instance.isTransportingPlayer) { return; }

        if (PlayerManager.Instance.forward_isPressed)
            Dash_Forward();
        else if (PlayerManager.Instance.back_isPressed)
            Dash_Backward();
        else if (PlayerManager.Instance.left_isPressed)
            Dash_Left();
        else if (PlayerManager.Instance.right_isPressed)
            Dash_Right();

        OnElevator();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetDashDirections;
        Movement.Action_StepTaken += SetDashDirections;
        PlayerStats.Action_RespawnPlayerLate += SetDashDirections;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetDashDirections;
        Movement.Action_StepTaken -= SetDashDirections;
        PlayerStats.Action_RespawnPlayerLate -= SetDashDirections;
    }


    //--------------------


    bool DashChecks()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return false; }

        if (isDashing) { return false; }

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }

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


    void OnElevator()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator>()
            || PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator_StepOn>())
            {
                SetDashDirections();
            }
        }
    }
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
        if (Physics.Raycast(gameObject.transform.position, dir, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>() == null)
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

        //Raycast target +2
        if (Physics.Raycast(gameObject.transform.position + dir, dir, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }

        //Raycast down from target +2
        if (Physics.Raycast(gameObject.transform.position + (dir * 2), Vector3.down, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                target = hit.transform.gameObject;

                //Darken color in target block
                if (target)
                {
                    if (target.GetComponent<BlockInfo>())
                    {
                        target.GetComponent<BlockInfo>().SetDarkenColors();
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
        //Cannot Dash if having 0 movement and targetBlock has a MovementCost
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


    private IEnumerator DashRoutine(GameObject target)
    {
        isDashing = true;
        dashStartPos = gameObject.transform.position;

        Movement.Instance.SetMovementState(MovementStates.Moving);
        PlayerManager.Instance.pauseGame = true;
        //PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (target)
            endPosition = target.transform.position + (Vector3.up * (Movement.Instance.heightOverBlock));
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

        isDashing = false;
        Movement.Instance.SetMovementState(MovementStates.Still);
        PlayerManager.Instance.pauseGame = false;

        Movement.Instance.Action_StepTaken_Invoke();
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
