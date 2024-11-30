using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player_Dash : MonoBehaviour
{
    [Header("Dashing")]
    public bool playerCanDash;
    public GameObject dashBlock_Previous;
    public GameObject dashBlock_Current;
    public GameObject dashBlockOver_Current;
    public GameObject dashBlock_Target;
    public float dashSpeed = 8;
    public bool isDashing;

    [SerializeField] HitDirection hitDir;

    bool canRun;


    //--------------------


    private void Update()
    {
        if (!canRun) { return; }

        CheckHitDirection();

        CheckForDash();

        PerformDashMovement();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
    }
    void StartRunningObject()
    {
        canRun = true;
    }


    //--------------------


    void CheckHitDirection()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal)
        {
            if (Player_BlockDetector.Instance.lookDir == Vector3.forward)
                hitDir = HitDirection.Forward;
            else if (Player_BlockDetector.Instance.lookDir == Vector3.back)
                hitDir = HitDirection.Backward;
            else if (Player_BlockDetector.Instance.lookDir == Vector3.left)
                hitDir = HitDirection.Left;
            else if (Player_BlockDetector.Instance.lookDir == Vector3.right)
                hitDir = HitDirection.Right;
        }
        else
        {
            hitDir = HitDirection.None;
        }
    }
    void CheckForDash()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal)
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>())
            {
                dashBlock_Previous = dashBlock_Current;

                switch (hitDir)
                {
                    case HitDirection.None:
                        dashBlock_Current = null;
                        break;

                    case HitDirection.Forward:
                        dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Front;
                        dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Front;
                        break;
                    case HitDirection.Backward:
                        dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Back;
                        dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Back;
                        break;
                    case HitDirection.Left:
                        dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Center_Left;
                        dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Center_Left;
                        break;
                    case HitDirection.Right:
                        dashBlock_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().lower_Center_Right;
                        dashBlockOver_Current = PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<BlockInfo>().center_Center_Right;
                        break;

                    default:
                        dashBlock_Current = null;
                        break;
                }
            }
            else
            {
                dashBlock_Current = null;
            }
        }
        else
        {
            dashBlock_Current = null;
        }

        if (PlayerStats.Instance.stats.abilitiesGot_Permanent != null || PlayerStats.Instance.stats.abilitiesGot_Temporary != null)
        {
            if (dashBlock_Current && !dashBlockOver_Current && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash))
            {
                if (dashBlock_Current.GetComponent<BlockInfo>())
                {
                    if (dashBlock_Current != dashBlock_Previous)
                    {
                        if (dashBlock_Previous)
                        {
                            if (dashBlock_Previous.GetComponent<BlockInfo>())
                            {
                                dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                            }
                        }
                    }

                    dashBlock_Current.GetComponent<BlockInfo>().DarkenColors();
                    playerCanDash = true;
                }
            }
            else if ((PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit) && dashBlock_Current && dashBlockOver_Current && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash || PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash))
            {
                if (dashBlock_Current.GetComponent<BlockInfo>() && dashBlock_Current.GetComponent<Block_Water>())
                {
                    if (dashBlock_Current != dashBlock_Previous)
                    {
                        if (dashBlock_Previous)
                        {
                            if (dashBlock_Previous.GetComponent<BlockInfo>())
                            {
                                dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                            }
                        }
                    }

                    dashBlock_Current.GetComponent<BlockInfo>().DarkenColors();
                    playerCanDash = true;
                }
            }
            else
            {
                if (dashBlock_Previous)
                {
                    if (dashBlock_Previous.GetComponent<BlockInfo>())
                    {
                        dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                        dashBlock_Previous = null;
                        playerCanDash = false;
                    }
                }
            }
        }
        else
        {
            if (dashBlock_Previous)
            {
                if (dashBlock_Previous.GetComponent<BlockInfo>())
                {
                    dashBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                    dashBlock_Previous = null;
                    playerCanDash = false;
                }
            }
        }
    }

    public void Dash()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Dash || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash)
        {
            PlayerManager.Instance.pauseGame = true;
            PlayerManager.Instance.isTransportingPlayer = true;
            isDashing = true;
            Player_Movement.Instance.movementStates = MovementStates.Moving;

            dashBlock_Target = dashBlock_Current;

            Player_Movement.Instance.Action_ResetBlockColorInvoke();
        }
    }

    public void PerformDashMovement()
    {
        if (isDashing && dashBlock_Target)
        {
            Vector3 targetPos = dashBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, targetPos, dashSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(PlayerManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                PlayerManager.Instance.player.transform.position = targetPos;

                Player_Movement.Instance.movementStates = MovementStates.Still;
                PlayerManager.Instance.pauseGame = false;
                PlayerManager.Instance.isTransportingPlayer = false;
                isDashing = false;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                if (PlayerManager.Instance.block_StandingOn_Current.block)
                {
                    if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
                    {
                        Player_Movement.Instance.currentMovementCost = PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().GetMovementCost();
                    }
                }

                dashBlock_Target = null;

                Player_Movement.Instance.Action_StepTakenInvoke();
                Player_Movement.Instance.Action_ResetBlockColorInvoke();
            }
        }
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
