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

    private void Update()
    {
        CheckHitDirection();

        CheckForDash();

        PerformDashMovement();
    }


    //--------------------


    void CheckHitDirection()
    {
        if (MainManager.Instance.block_LookingAt)
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
        if (MainManager.Instance.block_LookingAt)
        {
            if (MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>())
            {
                dashBlock_Previous = dashBlock_Current;

                switch (hitDir)
                {
                    case HitDirection.None:
                        dashBlock_Current = null;
                        break;

                    case HitDirection.Forward:
                        dashBlock_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().lower_Front;
                        dashBlockOver_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().center_Front;
                        break;
                    case HitDirection.Backward:
                        dashBlock_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().lower_Back;
                        dashBlockOver_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().center_Back;
                        break;
                    case HitDirection.Left:
                        dashBlock_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().lower_Center_Left;
                        dashBlockOver_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().center_Center_Left;
                        break;
                    case HitDirection.Right:
                        dashBlock_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().lower_Center_Right;
                        dashBlockOver_Current = MainManager.Instance.block_LookingAt.GetComponent<BlockInfo>().center_Center_Right;
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
        
        if (dashBlock_Current && !dashBlockOver_Current && Player_Stats.Instance.stats.abilities.Dash)
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
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Dash)
        {
            MainManager.Instance.pauseGame = true;
            gameObject.GetComponent<Player_Teleport>().isTeleporting = true;
            isDashing = true;

            dashBlock_Target = dashBlock_Current;

            Player_Movement.Instance.Action_ResetBlockColorInvoke();
        }
    }

    public void PerformDashMovement()
    {
        if (isDashing && dashBlock_Target)
        {
            Vector3 targetPos = dashBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, targetPos, dashSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(MainManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                MainManager.Instance.player.transform.position = targetPos;

                MainManager.Instance.pauseGame = false;
                gameObject.GetComponent<Player_Teleport>().isTeleporting = false;
                isDashing = false;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                if (MainManager.Instance.block_StandingOn.block)
                {
                    Player_Movement.Instance.currentMovementCost = MainManager.Instance.block_StandingOn.block.GetComponent<BlockInfo>().movementCost;
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
